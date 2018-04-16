using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedTeam {

	public class WaypointIntersection : MonoBehaviour {

		public Waypoint waypointPrefab;
		public float roadWidth;
		public WaypointRoadSegment northBoundRoad;
		public WaypointRoadSegment eastBoundRoad;
		public WaypointRoadSegment southBoundRoad;
		public WaypointRoadSegment westBoundRoad;

		/// <summary>
		/// Waypoint meanings
		/// 	0 - NorthBound Entrance,
		/// 	1 - NorthBound Exit,
		/// 	2 - EastBound Entrance,
		/// 	3 - EastBound Exit,
		/// 	4 - SouthBound Entrance,
		/// 	5 - SouthBound Exit,
		/// 	6 - WestBound Entrance,
		/// 	7 - WestBound Exit,
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

			waypoints[0].transform.localPosition = new Vector3(		x, 0f, -1 * x);
			waypoints[1].transform.localPosition = new Vector3(		x, 0f, 		x);
			waypoints[2].transform.localPosition = new Vector3( -1 * x, 0f, -1 * x);
			waypoints[3].transform.localPosition = new Vector3(		x, 0f, -1 * x);
			waypoints[4].transform.localPosition = new Vector3( -1 * x, 0f,		x);
			waypoints[5].transform.localPosition = new Vector3( -1 * x, 0f, -1 * x);
			waypoints[6].transform.localPosition = new Vector3(		x, 0f,		x);
			waypoints[7].transform.localPosition = new Vector3( -1 * x, 0f,		x);

			int numRoads = GetNumRoads();

			if(numRoads == 0) {
				Debug.LogError("Cannot create an intersection without roads");
				return;
			}

			switch(numRoads) {
			case 1:
				LinkDeadEnd();
				break;
			case 2:
				LinkTwoWay();
				break;
			case 3:
				LinkT();
				break;
			case 4:
				LinkFull();
				break;
			}
		}

		void LinkFull() {

			// North Bound
			northBoundRoad.waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] {  waypoints[0] });
			northBoundRoad.waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

			waypoints[0].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[1], westBoundRoad.waypoints[2] });
			waypoints[0].nextWaypointProbabilities = new List<float>(new float[] { 0.70f, 0.30f });

			waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { southBoundRoad.waypoints[0], eastBoundRoad.waypoints[0] });
			waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

			// East Bound
			eastBoundRoad.waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] {  waypoints[2] });
			eastBoundRoad.waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

			waypoints[2].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[3], northBoundRoad.waypoints[2] });
			waypoints[2].nextWaypointProbabilities = new List<float>(new float[] { 0.70f, 0.30f });

			waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] { westBoundRoad.waypoints[2], southBoundRoad.waypoints[0] });
			waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

			// South Bound
			southBoundRoad.waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] {  waypoints[4] });
			southBoundRoad.waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

			waypoints[4].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[5], eastBoundRoad.waypoints[0] });
			waypoints[4].nextWaypointProbabilities = new List<float>(new float[] { 0.70f, 0.30f });

			waypoints[5].nextWaypoints = new List<Waypoint>(new Waypoint[] { northBoundRoad.waypoints[2], westBoundRoad.waypoints[2] });
			waypoints[5].nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

			// West Bound
			westBoundRoad.waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] {  waypoints[6] });
			westBoundRoad.waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

			waypoints[6].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[7], southBoundRoad.waypoints[0] });
			waypoints[6].nextWaypointProbabilities = new List<float>(new float[] { 0.70f, 0.30f });

			waypoints[7].nextWaypoints = new List<Waypoint>(new Waypoint[] { eastBoundRoad.waypoints[0], northBoundRoad.waypoints[2] });
			waypoints[7].nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });
		}

		void LinkT() {
			if(northBoundRoad == null) {
				// East Bound
				eastBoundRoad.waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[2] });
				eastBoundRoad.waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[2].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[3] });
				waypoints[2].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] { westBoundRoad.waypoints[2], southBoundRoad.waypoints[0] });
				waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

				// South Bound
				southBoundRoad.waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[4] });
				southBoundRoad.waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[4].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[5], eastBoundRoad.waypoints[0] });
				waypoints[4].nextWaypointProbabilities = new List<float>(new float[] { 0.50f, 0.50f });

				waypoints[5].nextWaypoints = new List<Waypoint>(new Waypoint[] { westBoundRoad.waypoints[2] });
				waypoints[5].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// West Bound
				westBoundRoad.waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[6] });
				westBoundRoad.waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[6].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[7], southBoundRoad.waypoints[0] });
				waypoints[6].nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

				waypoints[7].nextWaypoints = new List<Waypoint>(new Waypoint[] { eastBoundRoad.waypoints[0] });
				waypoints[7].nextWaypointProbabilities = new List<float>(new float[] { 1f });
			} else if(eastBoundRoad == null) {
				// North Bound
				northBoundRoad.waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[0] });
				northBoundRoad.waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[0].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[1], westBoundRoad.waypoints[2] });
				waypoints[0].nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

				waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { southBoundRoad.waypoints[0] });
				waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// South Bound
				southBoundRoad.waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[4] });
				southBoundRoad.waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[4].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[5] });
				waypoints[4].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[5].nextWaypoints = new List<Waypoint>(new Waypoint[] { northBoundRoad.waypoints[2], westBoundRoad.waypoints[2] });
				waypoints[5].nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

				// West Bound
				westBoundRoad.waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[6] });
				westBoundRoad.waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[6].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[7], southBoundRoad.waypoints[0] });
				waypoints[6].nextWaypointProbabilities = new List<float>(new float[] { 0.50f, 0.50f });

				waypoints[7].nextWaypoints = new List<Waypoint>(new Waypoint[] { northBoundRoad.waypoints[2] });
				waypoints[7].nextWaypointProbabilities = new List<float>(new float[] { 1f });
			} else if(southBoundRoad == null) {
				// North Bound
				northBoundRoad.waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[0] });
				northBoundRoad.waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[0].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[1], westBoundRoad.waypoints[2] });
				waypoints[0].nextWaypointProbabilities = new List<float>(new float[] { 0.50f, 0.50f });

				waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { eastBoundRoad.waypoints[0] });
				waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// East Bound
				eastBoundRoad.waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[2] });
				eastBoundRoad.waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[2].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[3], northBoundRoad.waypoints[2] });
				waypoints[2].nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

				waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] { westBoundRoad.waypoints[2] });
				waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// West Bound
				westBoundRoad.waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[6] });
				westBoundRoad.waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[6].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[7] });
				waypoints[6].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[7].nextWaypoints = new List<Waypoint>(new Waypoint[] { eastBoundRoad.waypoints[0], northBoundRoad.waypoints[2] });
				waypoints[7].nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });
			} else {
				// North Bound
				northBoundRoad.waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] {  waypoints[0] });
				northBoundRoad.waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[0].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[1]});
				waypoints[0].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { southBoundRoad.waypoints[0], eastBoundRoad.waypoints[0] });
				waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

				// East Bound
				eastBoundRoad.waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] {  waypoints[2] });
				eastBoundRoad.waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[2].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[3], northBoundRoad.waypoints[2] });
				waypoints[2].nextWaypointProbabilities = new List<float>(new float[] { 0.50f, 0.50f });

				waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] { southBoundRoad.waypoints[0] });
				waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// South Bound
				southBoundRoad.waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] {  waypoints[4] });
				southBoundRoad.waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[4].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[5], eastBoundRoad.waypoints[0] });
				waypoints[4].nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

				waypoints[5].nextWaypoints = new List<Waypoint>(new Waypoint[] { northBoundRoad.waypoints[2]});
				waypoints[5].nextWaypointProbabilities = new List<float>(new float[] { 1f });
			}
		}

		void LinkTwoWay() {
			
			if(northBoundRoad == null && eastBoundRoad == null) {

				// South Bound
				southBoundRoad.waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[4] });
				southBoundRoad.waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[4].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[5] });
				waypoints[4].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[5].nextWaypoints = new List<Waypoint>(new Waypoint[] { westBoundRoad.waypoints[2] });
				waypoints[5].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// West Bound
				westBoundRoad.waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[6] });
				westBoundRoad.waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[6].nextWaypoints = new List<Waypoint>(new Waypoint[] { southBoundRoad.waypoints[0] });
				waypoints[6].nextWaypointProbabilities = new List<float>(new float[] { 1f });

			} else if(northBoundRoad == null && southBoundRoad == null) {
				
				// East Bound
				eastBoundRoad.waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[2] });
				eastBoundRoad.waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[2].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[3] });
				waypoints[2].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] { westBoundRoad.waypoints[2] });
				waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// West Bound
				westBoundRoad.waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[6] });
				westBoundRoad.waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[6].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[7] });
				waypoints[6].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[7].nextWaypoints = new List<Waypoint>(new Waypoint[] { eastBoundRoad.waypoints[0] });
				waypoints[7].nextWaypointProbabilities = new List<float>(new float[] { 1f });

			} else if(northBoundRoad == null && westBoundRoad == null) {
				
				// East Bound
				eastBoundRoad.waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[2] });
				eastBoundRoad.waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[2].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[3] });
				waypoints[2].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] { southBoundRoad.waypoints[0] });
				waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// South Bound
				southBoundRoad.waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[4] });
				southBoundRoad.waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[4].nextWaypoints = new List<Waypoint>(new Waypoint[] { eastBoundRoad.waypoints[0] });
				waypoints[4].nextWaypointProbabilities = new List<float>(new float[] { 1f });

			} else if(eastBoundRoad == null && southBoundRoad == null) {
				
				// North Bound
				northBoundRoad.waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[0] });
				northBoundRoad.waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[0].nextWaypoints = new List<Waypoint>(new Waypoint[] { westBoundRoad.waypoints[2] });
				waypoints[0].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// West Bound
				westBoundRoad.waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[6] });
				westBoundRoad.waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[6].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[7] });
				waypoints[6].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[7].nextWaypoints = new List<Waypoint>(new Waypoint[] { northBoundRoad.waypoints[2] });
				waypoints[7].nextWaypointProbabilities = new List<float>(new float[] { 1f });

			} else if(eastBoundRoad == null && westBoundRoad == null) {

				// North Bound
				northBoundRoad.waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[0] });
				northBoundRoad.waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[0].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[1] });
				waypoints[0].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { southBoundRoad.waypoints[0] });
				waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// South Bound
				southBoundRoad.waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[4] });
				southBoundRoad.waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[4].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[5] });
				waypoints[4].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[5].nextWaypoints = new List<Waypoint>(new Waypoint[] { northBoundRoad.waypoints[2] });
				waypoints[5].nextWaypointProbabilities = new List<float>(new float[] { 1f });

			} else {

				// North Bound
				northBoundRoad.waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] {  waypoints[0] });
				northBoundRoad.waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[0].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[1] });
				waypoints[0].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { eastBoundRoad.waypoints[0] });
				waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// East Bound
				eastBoundRoad.waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] {  waypoints[2] });
				eastBoundRoad.waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[2].nextWaypoints = new List<Waypoint>(new Waypoint[] { northBoundRoad.waypoints[2] });
				waypoints[2].nextWaypointProbabilities = new List<float>(new float[] { 1f });

			}
		}

		void LinkDeadEnd() {
			if(northBoundRoad != null) {

				northBoundRoad.waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[0] });
				northBoundRoad.waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[0].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[1] });
				waypoints[0].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[4] });
				waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[4].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[5] });
				waypoints[4].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[5].nextWaypoints = new List<Waypoint>(new Waypoint[] { northBoundRoad.waypoints[2] });
				waypoints[5].nextWaypointProbabilities = new List<float>(new float[] { 1f });
			
			} else if(eastBoundRoad != null) {

				eastBoundRoad.waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[2] });
				eastBoundRoad.waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[2].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[3] });
				waypoints[2].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[6] });
				waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[6].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[7] });
				waypoints[6].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[7].nextWaypoints = new List<Waypoint>(new Waypoint[] { eastBoundRoad.waypoints[0] });
				waypoints[7].nextWaypointProbabilities = new List<float>(new float[] { 1f });

			} else if(southBoundRoad != null) {

				southBoundRoad.waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[4] });
				southBoundRoad.waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[4].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[5] });
				waypoints[4].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[5].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[0] });
				waypoints[5].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[0].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[1] });
				waypoints[0].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { southBoundRoad.waypoints[0] });
				waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

			} else {

				westBoundRoad.waypoints[1].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[6] });
				westBoundRoad.waypoints[1].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[6].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[7] });
				waypoints[6].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[7].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[2] });
				waypoints[7].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[2].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[3] });
				waypoints[2].nextWaypointProbabilities = new List<float>(new float[] { 1f });

				waypoints[3].nextWaypoints = new List<Waypoint>(new Waypoint[] { westBoundRoad.waypoints[2] });
				waypoints[3].nextWaypointProbabilities = new List<float>(new float[] { 1f });

			}
		}

		int GetNumRoads () {
			int numRoads = 0;

			if(northBoundRoad != null)
				numRoads++;

			if(eastBoundRoad != null)
				numRoads++;

			if(southBoundRoad != null)
				numRoads++;

			if(westBoundRoad != null)
				numRoads++;

			return numRoads;
		}
	}
}