using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedTeam {

	public enum RoadDirection {
		NS,
		WE
	}

	public class WaypointRoadSegment : MonoBehaviour {

		public Waypoint waypointPrefab;
		public float roadWidth;
		public float blockLength;

		/// <summary>
		/// The waypoints
		/// 
		/// for NS roads
		/// 	0 - NorthBound entry,
		/// 	1 - NorthBound exit,
		/// 	2 - SouthBound entry,
		/// 	3 - SouthBound exit,
		/// 
		/// for WE roads
		/// 	0 - WestBound entry,
		/// 	1 - WestBound exit,
		/// 	2 - EastBound entry,
		/// 	3 - EastBound exit,
		/// 
		/// </summary>
		public Waypoint[] waypoints;

		public RoadDirection direction;

		public void Generate() {
			if(waypoints == null || waypoints.Length == 0) {
				waypoints = new Waypoint[4];
				for(int i = 0; i < 4; i++) {
					waypoints[i] = GameObject.Instantiate<Waypoint>(waypointPrefab, this.transform);
				}
			}

			if(direction == RoadDirection.NS) {
				float x = roadWidth / 4f;
				float z = blockLength / 2f - 1f;

				waypoints[0].transform.localPosition = new Vector3(		x,	 0f,	-1 * z);
				waypoints[1].transform.localPosition = new Vector3(		x,	 0f,		 z);
				waypoints[2].transform.localPosition = new Vector3(-1 * x,	 0f,		 z);
				waypoints[3].transform.localPosition = new Vector3(-1 * x,	 0f,	-1 * z);
			} else {
				float x = blockLength / 2f - 1f;
				float z = roadWidth / 4f;

				waypoints[0].transform.localPosition = new Vector3(		x,	 0f,		 z);
				waypoints[1].transform.localPosition = new Vector3(-1 * x,	 0f,		 z);
				waypoints[2].transform.localPosition = new Vector3(-1 * x,	 0f,	-1 * z);
				waypoints[3].transform.localPosition = new Vector3(		x,	 0f,	-1 * z);
			}

			waypoints[0].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[1] });
			waypoints[0].nextWaypointProbabilities = new List<float>(new float[] { 1f });

			waypoints[2].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[3] });
			waypoints[2].nextWaypointProbabilities = new List<float>(new float[] { 1f });
		}
	}
}