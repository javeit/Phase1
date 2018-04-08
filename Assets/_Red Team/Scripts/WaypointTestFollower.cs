using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedTeam {

	public class WaypointTestFollower : MonoBehaviour {

		public Waypoint startWaypoint;
		public float speed;

		IWaypoint waypoint;

		void Update() {
			Vector3 distance = waypoint.Position - transform.position;

			if(distance.magnitude <= waypoint.ArrivedDistance)
				waypoint = waypoint.GetNext();
			else {
				if(distance.magnitude < speed)
					transform.Translate(distance);
				else
					transform.Translate(distance.normalized * speed);
			}
		}

		void Awake() {
			waypoint = startWaypoint;
		}
	}
}