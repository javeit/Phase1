using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedTeam {

	public enum IntersectionType {
		full,
		t,
		corner,
		deadend
	}

	public class WaypointIntersection : MonoBehaviour {

		public Waypoint waypointPrefab;
		public float roadWidth;
		public IntersectionType? type;

		public WaypointRoadSegment northBoundRoad;
		public WaypointRoadSegment eastBoundRoad;
		public WaypointRoadSegment southBoundRoad;
		public WaypointRoadSegment westBoundRoad;

		/// <summary>
		/// Waypoint meanings
		/// 	0 - NorthBound Entrance
		/// 	1 - NorthBound Exit
		/// 	2 - EastBound Entrance
		/// 	3 - EastBound Exit
		/// 	4 - SouthBound Entrance
		/// 	5 - SouthBound Exit
		/// 	6 - WestBound Entrance
		/// 	7 - WestBound Exit
		/// 
		/// Road names refer to their direction toward the intersection
		/// 
		/// </summary>
		public Waypoint[] waypoints;

		public void Generate() {
			if(waypoints == null || waypoints.Length == 0) {
				waypoints = new Waypoint[8];
				for(int i = 0; i < 8; i++) {
					waypoints[i] = GameObject.Instantiate<Waypoint>(waypointPrefab, this.transform);
				}
			}

			float x = (roadWidth / 4f);

			waypoints[0].transform.position = new Vector3(		x, 0f, -1 * x);
			waypoints[1].transform.position = new Vector3(		x, 0f, 		x);
			waypoints[2].transform.position = new Vector3( -1 * x, 0f, -1 * x);
			waypoints[3].transform.position = new Vector3(		x, 0f, -1 * x);
			waypoints[4].transform.position = new Vector3( -1 * x, 0f,		x);
			waypoints[5].transform.position = new Vector3( -1 * x, 0f, -1 * x);
			waypoints[6].transform.position = new Vector3(		x, 0f,		x);
			waypoints[7].transform.position = new Vector3( -1 * x, 0f,		x);

			type = GetIntersectionType();

			if(type == null)
				return;
			else if(type.Value == IntersectionType.deadend)
				LinkDeadEnd();
			else if(type.Value == IntersectionType.corner)
				LinkCorner();
			else if(type.Value == IntersectionType.t)
				LinkT();
			else
				LinkFull();
		}

		void LinkFull() {

			// North Bound
			northBoundRoad.waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] {  waypoints[0] });
			northBoundRoad.waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

			waypoints[0].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[1], westBoundRoad.waypoints[2] });
			waypoints[0].nextWaypointProbabilities = new List<float>(new float[] { 0.75f, 0.25f });

			waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { southBoundRoad.waypoints[0], eastBoundRoad.waypoints[0] });
			waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

			// East Bound
			eastBoundRoad.waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] {  waypoints[2] });
			eastBoundRoad.waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

			waypoints[2].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[3], northBoundRoad.waypoints[2] });
			waypoints[2].nextWaypointProbabilities = new List<float>(new float[] { 0.75f, 0.25f });

			waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] { westBoundRoad.waypoints[2], southBoundRoad.waypoints[0] });
			waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

			// South Bound
			southBoundRoad.waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] {  waypoints[4] });
			southBoundRoad.waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

			waypoints[4].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[5], eastBoundRoad.waypoints[0] });
			waypoints[4].nextWaypointProbabilities = new List<float>(new float[] { 0.75f, 0.25f });

			waypoints[5].nextWaypoints = new List<Waypoint>(new Waypoint[] { northBoundRoad.waypoints[2], westBoundRoad.waypoints[2] });
			waypoints[5].nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

			// West Bound
			westBoundRoad.waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] {  waypoints[6] });
			westBoundRoad.waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

			waypoints[6].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[7], southBoundRoad.waypoints[0] });
			waypoints[6].nextWaypointProbabilities = new List<float>(new float[] { 0.75f, 0.25f });

			waypoints[7].nextWaypoints = new List<Waypoint>(new Waypoint[] { eastBoundRoad.waypoints[0], northBoundRoad.waypoints[2] });
			waypoints[7].nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });
		}

		void LinkT() {
			Debug.Log("Haven't implemented this yet");
		}

		void LinkCorner() {
			Debug.Log("Haven't implemented this yet");
		}

		void LinkDeadEnd() {
			Debug.Log("Haven't implemented this yet");
		}

		IntersectionType? GetIntersectionType () {
			int numRoads = 0	;

			if(northBoundRoad != null)
				numRoads++;

			if(eastBoundRoad != null)
				numRoads++;

			if(southBoundRoad != null)
				numRoads++;

			if(westBoundRoad != null)
				numRoads++;

			if(numRoads == 0) {
				Debug.LogError("Cannot generate intersection without roads");
				return null;
			} else if(numRoads == 1) {
				return IntersectionType.deadend;
			} else if(numRoads == 2) {
				return IntersectionType.corner;
			} else if(numRoads == 3) {
				return IntersectionType.t;
			} else {
				return IntersectionType.full;
			}
		}
	}
}