using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RedTeam {

	[RequireComponent(typeof(NavMesh))]
	public class TestNavMeshWaypointAgent : MonoBehaviour {

		public Waypoint startWaypoint;

		NavMeshAgent agent;

		IWaypoint waypoint;

		void GoToNextPoint() {
			agent.SetDestination(waypoint.Position);
			agent.isStopped = false;
		}

		void Update() {

			if(agent.remainingDistance <= waypoint.ArrivedDistance) {
				
				IWaypoint nextWaypoint = waypoint.GetNext();
				if(nextWaypoint == null)
					agent.isStopped = true;
				else {
					waypoint = nextWaypoint;
					GoToNextPoint();
				}
			}
		}

		void Awake () {
			// this assignment implicitly converts the Waypoint 
			// object into an IWaypoint interface instance. 
			// This is important, because even though it's useful
			// to encapsulate waypoints in the IWaypoint interface, 
			// Unity does allow inspector fields for interfaces
			waypoint = startWaypoint;
			agent = GetComponent<NavMeshAgent>();
		}

		void Start() {
			GoToNextPoint();
		}
	}
}