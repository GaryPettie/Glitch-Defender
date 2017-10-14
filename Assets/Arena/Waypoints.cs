using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Waypoints : MonoBehaviour {
	static List<Waypoint> points;

	void Start () {
		points = GetComponentsInChildren<Waypoint>().ToList();
	}

	public static List<Waypoint> GetPath () {
		return points;
	}

	public static Transform GetNextWaypoint (int currentWaypointIndex) {
		return points[currentWaypointIndex + 1].transform;
	}

	public static Transform GetFirstWaypoint () {
		return points[0].transform;
	}

	public static Transform GetLastWaypoint () {
		return points[points.Count].transform;
	}

	public static bool IsLastWaypoint(int currentIndex) {
		return points.Count == currentIndex;
	}

	//TODO This will be used to alter the maze path once breakable blocks open up a new path. 
	public static void AlterPath () {
		//FIXME This whole method is demo code.

		//Creates the L01 shortcut.
		//FIXME Using this method causes odd behavior for enemies already on the map and only works for newly instantiated enemies.
		//It appears that the next 3 nodes in the path being traversed are removed, rather than the 3 being specified. 
		points.RemoveAt(4);
		points.RemoveAt(3);
		points.RemoveAt(2);
	}
}
