﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedTeam {

	public class CarSpawner : MonoBehaviour {

		public Transform carParentObject;
		public GameObject carPrefab;
		public int numCars;

		public WaypointGrid grid;

		public List<Waypoint> startWaypoints;
		List<Waypoint> StartWaypoints {
			get {
				if(startWaypoints == null || startWaypoints.Count == 0)
					GetStartWaypoints();

				return startWaypoints;
			}
		}

		void GetStartWaypoints() {
			if(grid.northSouthRoadSegments == null || grid.northSouthRoadSegments.Length == 0 
				|| grid.westEastRoadSegments == null || grid.westEastRoadSegments.Length == 0)
				return;

			startWaypoints = new List<Waypoint>();

			foreach(WaypointRoadSegment roadSegment in grid.northSouthRoadSegments) {
				startWaypoints.Add(roadSegment.waypoints[1]);
				startWaypoints.Add(roadSegment.waypoints[3]);
			}

			foreach(WaypointRoadSegment roadSegment in grid.westEastRoadSegments) {
				startWaypoints.Add(roadSegment.waypoints[1]);
				startWaypoints.Add(roadSegment.waypoints[3]);
			}
		}

		public void SpawnCars() {

			if(numCars <= 0)
				return;

			for(int i = 0; i < numCars; i++) {
				GameObject car = GameObject.Instantiate(carPrefab, carParentObject);

				Waypoint waypoint = StartWaypoints[Random.Range(0, StartWaypoints.Count - 1)];

				car.transform.position = waypoint.transform.parent.position;
				car.GetComponentInChildren<AICarGuide>().startWaypoint = waypoint;
			}
		}
	}
}