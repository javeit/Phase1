using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedTeam {

	public class AICarGuide : MonoBehaviour {

		public Waypoint startWaypoint;
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
				
				} else if(remainingDistance.magnitude <= speed) {
				
					transform.Translate(remainingDistance);

				} else {
				
					transform.Translate(remainingDistance.normalized * speed);

				}
			}
		}

		void Awake() {
			waypoint = startWaypoint;
		}
	}
}