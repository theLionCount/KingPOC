using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RouteProvider
{
	MapModule map;

	List<Vector2Int> route;
	RouteResponse response;

	public RouteProvider()
	{
		map = GameObject.Find("World").GetComponent<MapModule>();
		route = new List<Vector2Int>();
		response = new RouteResponse();
	}
	
	public Vector2Int nextTarget(Vector2 pos, Vector2 target, bool pause = false)
	{
		Vector2Int s = new Vector2Int((int)pos.x, (int)pos.y);
		Vector2Int t = new Vector2Int((int)target.x, (int)target.y);
		if (s == t) return Vector2Int.zero;

		if (route.Count > 1)
		{
			for (int i = 1; i < route.Count; i++)
			{
				Debug.DrawLine(new Vector3(route[i - 1].x + 0.5f, route[i - 1].y + 0.5f, 5), new Vector3(route[i].x + 0.5f, route[i].y + 0.5f, 5), Color.green);
			}
		}

		if (route.Contains(s) && route.Contains(t))
		{
			int si = route.IndexOf(s);
			int ti = route.IndexOf(t);
			if (ti > si) return route[si + 1];
			else return route[si - 1];
		}
		else
		{
			if (response.start == s && response.end == t)
			{
				lock (response.lck)
				{
					if (response.rdy)
					{
						System.Diagnostics.Debug.WriteLine("using route from provider");
						route = response.route.Select(v => v).ToList();
						return nextTarget(pos, target, pause);
					}
					else if (pause) return s;
					else if (route.Contains(s) && route.Last() != s) return route[route.IndexOf(s) + 1];
					else return s;
				}
			}
			else lock (response.lck)
				{
					if (!response.started)
					{
						System.Diagnostics.Debug.WriteLine("modifying route from provider");
						response.start = s;
						response.end = t;
						map.queRouteRequest(response);
					}
				}
			
			if (pause) return s;
			else if (route.Contains(s) && route.Last() != s) return route[route.IndexOf(s) + 1];
		}
		return s;
	}
}
