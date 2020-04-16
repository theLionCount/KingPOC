using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    List<string> enemyTags = new List<string> { "MeeleEnemy" };
    List<string> playerTags = new List<string> { "Player" };
    public event EventHandler playerEntered;
    public event EventHandler playerExited;
    public event EventHandler areaClear;

    public bool playerHere;
    public int enemyNum;
    int coolDown;

    // Start is called before the first frame update
    void Start()
    {
        playerHere = false;
        enemyNum = 0;
        coolDown = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (coolDown > 0) coolDown--;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var tag = collision.gameObject.tag;
        if (playerTags.Contains(tag))
        {
            playerHere = true;
            if (coolDown<=0) playerEntered?.Invoke(this, null);
        }
        else if (enemyTags.Contains(tag)) enemyNum++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var tag = collision.gameObject.tag;
        if (playerTags.Contains(tag))
        {
            playerHere = false;
            if (coolDown <= 0) playerExited?.Invoke(this, null);
        }
        else if (enemyTags.Contains(tag))
        {
            enemyNum--;
            if (enemyNum <= 0 && coolDown<=0) areaClear?.Invoke(this, null);
        }
    }
}
