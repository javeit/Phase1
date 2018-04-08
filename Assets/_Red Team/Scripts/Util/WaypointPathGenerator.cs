using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedTeam.Util {

	public class WaypointPathGenerator : MonoBehaviour {

		public Waypoint[] waypoints;

		public void GeneratePath() {
			for(int i = 0; i < waypoints.Length - 1; i++) {
				waypoints[i].nextWaypoints = new List<Waypoint>();
				waypoints[i].nextWaypoints.Add(waypoints[i + 1]);
				waypoints[i].nextWaypointProbabilities = new List<float>();
				waypoints[i].nextWaypointProbabilities.Add(1f);
			}
		}
	}
}