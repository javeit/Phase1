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
		public int numWaypointsPerDirection;

		public Waypoint[] waypoints;

		public RoadDirection direction;

		#region endpoints

		public Waypoint NorthEntrance { 
			get {
				return waypoints [0];
			}
		}

		public Waypoint NorthExit {
			get {
				return waypoints [numWaypointsPerDirection - 1];
			}
		}

		public Waypoint SouthEntrance {
			get {
				return waypoints [numWaypointsPerDirection];
			}
		}

		public Waypoint SouthExit {
			get {
				return waypoints [2 * numWaypointsPerDirection - 1];
			}
		}

		public Waypoint WestEntrance { 
			get {
				return waypoints [0];
			}
		}

		public Waypoint WestExit {
			get {
				return waypoints [numWaypointsPerDirection - 1];
			}
		}

		public Waypoint EastEntrance {
			get {
				return waypoints [numWaypointsPerDirection];
			}
		}

		public Waypoint EastExit {
			get {
				return waypoints [2 * numWaypointsPerDirection - 1];
			}
		}

		#endregion

		public void Generate() {
			if(waypoints == null || waypoints.Length == 0) {
				waypoints = new Waypoint[2 * numWaypointsPerDirection];
				for(int i = 0; i < 2 * numWaypointsPerDirection; i++) {
					waypoints[i] = GameObject.Instantiate<Waypoint>(waypointPrefab, this.transform);
				}
			}

			if(direction == RoadDirection.NS) {

				GenerateNorthSouth ();

				//				float x = roadWidth / 4f;
				//				float z = blockLength / 2f - 1f;
				//
				//				waypoints[0].transform.localPosition = new Vector3(		x,	 0f,	-1 * z);
				//				waypoints[1].transform.localPosition = new Vector3(		x,	 0f,		 z);
				//				waypoints[2].transform.localPosition = new Vector3(-1 * x,	 0f,		 z);
				//				waypoints[3].transform.localPosition = new Vector3(-1 * x,	 0f,	-1 * z);
			} else {

				GenerateWestEast ();

				//				float x = blockLength / 2f - 1f;
				//				float z = roadWidth / 4f;
				//
				//				waypoints[0].transform.localPosition = new Vector3(		x,	 0f,		 z);
				//				waypoints[1].transform.localPosition = new Vector3(-1 * x,	 0f,		 z);
				//				waypoints[2].transform.localPosition = new Vector3(-1 * x,	 0f,	-1 * z);
				//				waypoints[3].transform.localPosition = new Vector3(		x,	 0f,	-1 * z);
			}

			//			waypoints[0].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[1] });
			//			waypoints[0].nextWaypointProbabilities = new List<float>(new float[] { 1f });
			//
			//			waypoints[2].nextWaypoints = new List<Waypoint>(new Waypoint[] { waypoints[3] });
			//			waypoints[2].nextWaypointProbabilities = new List<float>(new float[] { 1f });
		}

		void GenerateNorthSouth() {

			float x = roadWidth / 4f;
			float z = blockLength / 2f - 1f;

			float segmentLength = (blockLength - 2f) / (numWaypointsPerDirection - 1);

			// North
			for (int i = 0; i < numWaypointsPerDirection; i++) {
				waypoints[i].transform.localPosition = new Vector3(x, 0f, (segmentLength * i) - z);

				if (i < numWaypointsPerDirection - 1) {
					waypoints [i].nextWaypoints = new List<Waypoint> (new Waypoint[] { waypoints [i + 1] });
					waypoints [i].nextWaypointProbabilities = new List<float> (new float[] { 1f });
				}
			}

			// South
			for (int i = numWaypointsPerDirection; i < 2 * numWaypointsPerDirection; i++) {
				waypoints[i].transform.localPosition = new Vector3(-1 * x, 0f, -1 * (segmentLength * (i - numWaypointsPerDirection) - z));

				if (i < 2 * numWaypointsPerDirection - 1) {
					waypoints [i].nextWaypoints = new List<Waypoint> (new Waypoint[] { waypoints [i + 1] });
					waypoints [i].nextWaypointProbabilities = new List<float> (new float[] { 1f });
				}
			}
		}

		void GenerateWestEast() {
			float x = blockLength / 2f - 1f;
			float z = roadWidth / 4f;

			float segmentLength = (blockLength - 2f) / (numWaypointsPerDirection - 1);

			// West
			for (int i = 0; i < numWaypointsPerDirection; i++) {
				waypoints[i].transform.localPosition = new Vector3( -1 * (segmentLength * i - x), 0f, z);

				if (i < numWaypointsPerDirection - 1) {
					waypoints [i].nextWaypoints = new List<Waypoint> (new Waypoint[] { waypoints [i + 1] });
					waypoints [i].nextWaypointProbabilities = new List<float> (new float[] { 1f });
				}
			}

			// East
			for (int i = numWaypointsPerDirection; i < 2 * numWaypointsPerDirection; i++) {
				waypoints[i].transform.localPosition = new Vector3(segmentLength * (i - numWaypointsPerDirection) - x, 0f, -1 * z);

				if (i < 2 * numWaypointsPerDirection - 1) {
					waypoints [i].nextWaypoints = new List<Waypoint> (new Waypoint[] { waypoints [i + 1] });
					waypoints [i].nextWaypointProbabilities = new List<float> (new float[] { 1f });
				}
			}
		}
	}
}