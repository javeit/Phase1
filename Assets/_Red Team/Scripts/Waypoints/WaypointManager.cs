using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedTeam {

	public class WaypointManager : MonoBehaviour {

		Waypoint[] waypoints;
		Waypoint[] Waypoints {
			get {
				if (waypoints == null) {
					waypoints = GetComponentsInChildren<Waypoint> ();
				}
				return waypoints;
			}
		}

		public IWaypoint GetWaypointNear(Vector3 position) {
			if (Waypoints == null || Waypoints.Length == 0)
				return null;

			Waypoint nearestWaypoint = Waypoints [0];

			foreach (Waypoint waypoint in Waypoints) {
				if ((waypoint.Position - position).magnitude < (nearestWaypoint.Position - position).magnitude)
					nearestWaypoint = waypoint;
			}

			return nearestWaypoint;
		}
	}
}