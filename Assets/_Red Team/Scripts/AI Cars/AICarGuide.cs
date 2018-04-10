using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedTeam {

	public class AICarGuide : MonoBehaviour {

		public WaypointManager waypointManager;
		public float speed;
		public GameObject car;
		public float followDistance;

		IWaypoint waypoint;

		void Update() {

			// make sure the guide keeps within the given following distance of the car
			if((car.transform.position - transform.position).magnitude <= followDistance) {

				Vector3 remainingDistance = waypoint.Position - transform.position;

				if(remainingDistance.magnitude <= waypoint.ArrivedDistance) {
				
					if(waypoint.GetNext() != null)
						waypoint = waypoint.GetNext();
				
				} else {
				
					transform.Translate(remainingDistance.normalized * speed * Time.deltaTime);

				}
			}
		}

		void Awake() {
			waypoint = waypointManager.GetWaypointNear (transform.position);
		}
	}
}