using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DynamicDungeonCreator : StaticDungeon
{
    public List<GameObject> roomPrefabs;
    public List<GameObject> corridorPrefabs;

    List<DungeonRoomLoader> roomLoaders;
    List<DungeonRoomLoader> corridorLoaders;
    List<DungeonRoomLoader> allRoom;

    DungeonDoorMarker marker;

    bool[,] taken;

    protected override void Start()
    {
        preSetup();

        taken = new bool[w, h];

        marker = GameObject.Find("DoorMarker").GetComponent<DungeonDoorMarker>();
        marker.pos = Vector2Int.FloorToInt(marker.transform.position);

        roomLoaders = new List<DungeonRoomLoader>();
        corridorLoaders = new List<DungeonRoomLoader>();
        foreach (var item in roomPrefabs)
        {
            roomLoaders.Add(new DungeonRoomLoader(item.name + ".room", item));
            //corridorLoaders.Add(new DungeonRoomLoader(item.name + ".room", item));
        }
        foreach (var item in corridorPrefabs)
        {
            corridorLoaders.Add(new DungeonRoomLoader(item.name + ".corridor", item));
           
        }

        allRoom = new List<DungeonRoomLoader>();
        allRoom.AddRange(roomLoaders);
        allRoom.AddRange(corridorLoaders);

        createDungeon(marker);


        postSetup();
    }

    void createDungeon(DungeonDoorMarker start)
    {
        var doors = new List<DungeonDoorMarker>() { start };

        int i = 0;
        while(i<8)
        {
            var r = addRoom(doors);
            doors.AddRange(r.Item1);
            doors.RemoveAll(t => !t.open);
            if (r.Item2) i++;
        }
    }

    Tuple<List<DungeonDoorMarker>, bool> addRoom(List<DungeonDoorMarker> openDoors)
    {
        if (openDoors == null || openDoors.Count <= 0) return null;
        

        bool okRoom = false;
        DungeonDoorMarker okdoor = null;
        Vector2Int pos = Vector2Int.zero;
        BoundsInt bounds = new BoundsInt();
        DungeonRoomLoader loader = null;
        List<DungeonDoorMarker> doors = null;
        DungeonDoorMarker door = null;
        int i = 0;
        bool wasRoom = false;
        while (!okRoom && i<1000)
        {
            door = openDoors[Random.Range(0, openDoors.Count)];
            int x;
            if (i > 90000)
                 x = 2 + 1;
            i++;
            do
            {
                if (Random.value < door.corridorChance)
                {
                    loader = corridorLoaders[Random.Range(0, corridorLoaders.Count)];
                    wasRoom = false;
                }
                else
                {
                    loader = roomLoaders[Random.Range(0, roomLoaders.Count)];
                    wasRoom = true;
                }
            } while (loader.rerollChance > Random.value);
            doors = loader.getDoors().Where(t => okDoor(t, door)).ToList();
            if (doors.Count > 0)
            {
                okdoor = doors[Random.Range(0, doors.Count)];
                pos = loader.getGlobalPosByDoor(okdoor, door.pos);

                bounds = loader.getWorldBounds(pos);
                okRoom = isRoomOk(bounds);
            }
        }

        if (wasRoom) loader.rerollChance += (1 - loader.rerollChance) / 2;


        return new Tuple<List<DungeonDoorMarker>, bool>(addRoom(loader, door, okdoor, pos, bounds), wasRoom);
    }

    bool isAnyRoomOk(DungeonDoorMarker door)
    {
        foreach (var loader in allRoom)
        {
            var doors = loader.getDoors().Where(t => okDoor(t, door)).ToList();
            foreach (var item in doors)
            {
                var pos = loader.getGlobalPosByDoor(item, door.pos);

                var bounds = loader.getWorldBounds(pos);
                if (isRoomOk(bounds)) return true;
            }
        }
        return false;
    }

    bool isRoomOk(BoundsInt bounds)
    {
        for (int i = bounds.min.x; i < bounds.max.x; i++)
        {
            for (int j = bounds.min.y; j < bounds.max.y; j++)
            {
                if (taken[i+w/2, j+h/2])
                {
                    return false;
                }
            }
        }

        return true;
    }

    bool okDoor(DungeonDoorMarker myDoor, DungeonDoorMarker outerDoor)
    {
        return myDoor.intoDir.x * outerDoor.intoDir.x == -1 || myDoor.intoDir.y * outerDoor.intoDir.y == -1;
    }

    List<DungeonDoorMarker> addRoom(DungeonRoomLoader loader, DungeonDoorMarker start, DungeonDoorMarker okdoor, Vector2Int pos, BoundsInt bounds)
    {
        start.open = false;
        okdoor.open = false;
        var room = Instantiate(loader.prefab);
        room.transform.position = new Vector3(pos.x, pos.y, 0);
        loader.paintConstruct(tilemap, FGTilemap, FullFGTilemap, new Vector3Int(pos.x, pos.y, 0));
        var doors = loader.getDoors().Where(t => t.pos != okdoor.pos).ToList();
        doors.ForEach(t => t.setToWorld(pos));
        foreach (var item in doors.Where(t => !isAnyRoomOk(t)))
        {
            item.open = false;
        }
        for (int i = bounds.min.x; i < bounds.max.x; i++)
        {
            for (int j = bounds.min.y; j < bounds.max.y; j++)
            {
                taken[i+w/2, j+h/2] = true;
            }
        }
        return doors.Where(t => t.open).ToList();

    }
}
