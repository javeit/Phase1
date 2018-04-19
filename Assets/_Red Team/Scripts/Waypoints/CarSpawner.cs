using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RedTeam {

	public class CarSpawner : MonoBehaviour {

		public Transform carParentObject;
		public GameObject[] carPrefabs;
		public int numCars;

		public GameObject grid;

		public float minSpeed;
		public float maxSpeed;

		List<Waypoint> startWaypoints;

		void GetStartWaypoints() {

			WaypointRoadSegment[] segments = grid.GetComponentsInChildren<WaypointRoadSegment>();

			if(segments == null || segments.Length == 0)
				return;

			startWaypoints = new List<Waypoint>();

			foreach(WaypointRoadSegment roadSegment in segments) {
				startWaypoints.Add(roadSegment.waypoints[1]);
				startWaypoints.Add(roadSegment.waypoints[3]);
			}
		}

		public void SpawnCars() {

			GetStartWaypoints();

			if(numCars <= 0)
				return;

			for(int i = 0; i < numCars; i++) {

				GameObject carPrefab = carPrefabs[Random.Range(0, carPrefabs.Length - 1)];
				GameObject car = GameObject.Instantiate(carPrefab, carParentObject);

				car.GetComponentInChildren<NavMeshAgent>().speed = Random.Range(minSpeed, maxSpeed);

				Waypoint waypoint = startWaypoints[Random.Range(0, startWaypoints.Count - 1)];

				car.transform.position = waypoint.transform.parent.position;
				car.GetComponentInChildren<AICarGuide>().startWaypoint = waypoint;
			}
		}
	}
}