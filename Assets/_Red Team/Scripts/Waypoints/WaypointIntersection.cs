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

		public Waypoint[] waypoints;

		#region Endpoints

		Waypoint  NorthBoundEntrance {
			get {
				return waypoints[0];
			}
		}

		Waypoint NorthBoundExit {
			get {
				return waypoints[1];
			}
		}

		Waypoint EastBoundEntrance {
			get {
				return waypoints[2];
			}
		}

		Waypoint EastBoundExit {
			get {
				return waypoints[3];
			}
		}

		Waypoint SouthBoundEntrance {
			get {
				return waypoints[4];
			}
		}

		Waypoint SouthBoundExit {
			get {
				return waypoints[5];
			}
		}

		Waypoint WestBoundEntrance {
			get {
				return waypoints[6];
			}
		}

		Waypoint WestBoundExit {
			get {
				return waypoints[7];
			}
		}

		#endregion

		public void Generate() {
			if(waypoints == null || waypoints.Length == 0) {
				waypoints = new Waypoint[8];
				for(int i = 0; i < 8; i++) {
					waypoints[i] = GameObject.Instantiate<Waypoint>(waypointPrefab, this.transform);
				}
			}

			float x = (roadWidth / 4f);

			NorthBoundEntrance.transform.localPosition = new Vector3(		x, 0f, -1 * x);
			NorthBoundExit.transform.localPosition = new Vector3(		x, 0f, 		x);
			EastBoundEntrance.transform.localPosition = new Vector3( -1 * x, 0f, -1 * x);
			EastBoundExit.transform.localPosition = new Vector3(		x, 0f, -1 * x);
			SouthBoundEntrance.transform.localPosition = new Vector3( -1 * x, 0f,		x);
			SouthBoundExit.transform.localPosition = new Vector3( -1 * x, 0f, -1 * x);
			WestBoundEntrance.transform.localPosition = new Vector3(		x, 0f,		x);
			WestBoundExit.transform.localPosition = new Vector3( -1 * x, 0f,		x);

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
			northBoundRoad.NorthExit.nextWaypoints = new List<Waypoint>(new Waypoint[] {   NorthBoundEntrance });
			northBoundRoad.NorthExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

			NorthBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { NorthBoundExit, westBoundRoad.EastEntrance });
			NorthBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 0.70f, 0.30f });

			NorthBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { southBoundRoad.NorthEntrance, eastBoundRoad.WestEntrance });
			NorthBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

			// East Bound
			eastBoundRoad.EastExit.nextWaypoints = new List<Waypoint>(new Waypoint[] {  EastBoundEntrance });
			eastBoundRoad.EastExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

			EastBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { EastBoundExit, northBoundRoad.SouthEntrance });
			EastBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 0.70f, 0.30f });

			EastBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { westBoundRoad.EastEntrance, southBoundRoad.NorthEntrance });
			EastBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

			// South Bound
			southBoundRoad.SouthExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { SouthBoundEntrance });
			southBoundRoad.SouthExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

			SouthBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { SouthBoundExit, eastBoundRoad.WestEntrance });
			SouthBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 0.70f, 0.30f });

			SouthBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { northBoundRoad.SouthEntrance, westBoundRoad.EastEntrance });
			SouthBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

			// West Bound
			westBoundRoad.WestExit.nextWaypoints = new List<Waypoint>(new Waypoint[] {  WestBoundEntrance });
			westBoundRoad.WestExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

			WestBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { WestBoundExit, southBoundRoad.NorthEntrance });
			WestBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 0.70f, 0.30f });

			WestBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { eastBoundRoad.WestEntrance, northBoundRoad.SouthEntrance });
			WestBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });
		}

		void LinkT() {
			if(northBoundRoad == null) {

				// East Bound
				eastBoundRoad.EastExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { EastBoundEntrance });
				eastBoundRoad.EastExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				EastBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { EastBoundExit });
				EastBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				EastBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { westBoundRoad.EastEntrance, southBoundRoad.NorthEntrance });
				EastBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

				// South Bound
				southBoundRoad.SouthExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { SouthBoundEntrance });
				southBoundRoad.SouthExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				SouthBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { SouthBoundExit, eastBoundRoad.WestEntrance });
				SouthBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 0.50f, 0.50f });

				SouthBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { westBoundRoad.EastEntrance });
				SouthBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// West Bound
				westBoundRoad.WestExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { WestBoundEntrance });
				westBoundRoad.WestExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				WestBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { WestBoundExit, southBoundRoad.NorthEntrance });
				WestBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

				WestBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { eastBoundRoad.WestEntrance });
				WestBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

			} else if(eastBoundRoad == null) {

				// North Bound
				northBoundRoad.NorthExit.nextWaypoints = new List<Waypoint>(new Waypoint[] {  NorthBoundEntrance });
				northBoundRoad.NorthExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				NorthBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] {  NorthBoundExit, westBoundRoad.EastEntrance });
				NorthBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

				NorthBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { southBoundRoad.NorthEntrance });
				NorthBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// South Bound
				southBoundRoad.SouthExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { SouthBoundEntrance });
				southBoundRoad.SouthExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				SouthBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { SouthBoundExit });
				SouthBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				SouthBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { northBoundRoad.SouthEntrance, westBoundRoad.EastEntrance });
				SouthBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

				// West Bound
				westBoundRoad.WestExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { WestBoundEntrance });
				westBoundRoad.WestExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				WestBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { WestBoundExit, southBoundRoad.NorthEntrance });
				WestBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 0.50f, 0.50f });

				WestBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { northBoundRoad.SouthEntrance });
				WestBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

			} else if(southBoundRoad == null) {

				// North Bound
				northBoundRoad.NorthExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { NorthBoundEntrance });
				northBoundRoad.NorthExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				NorthBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { NorthBoundExit, westBoundRoad.EastEntrance });
				NorthBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 0.50f, 0.50f });

				NorthBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { eastBoundRoad.WestEntrance });
				NorthBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// East Bound
				eastBoundRoad.EastExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { EastBoundEntrance });
				eastBoundRoad.EastExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				EastBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { EastBoundExit, northBoundRoad.SouthEntrance });
				EastBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

				EastBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { westBoundRoad.EastEntrance });
				EastBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// West Bound
				westBoundRoad.WestExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { WestBoundEntrance });
				westBoundRoad.WestExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				WestBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { WestBoundExit });
				WestBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				WestBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { eastBoundRoad.WestEntrance, northBoundRoad.SouthEntrance });
				WestBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });
			} else {
				// North Bound
				northBoundRoad.NorthExit.nextWaypoints = new List<Waypoint>(new Waypoint[] {  NorthBoundEntrance });
				northBoundRoad.NorthExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				NorthBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { NorthBoundExit});
				NorthBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				NorthBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { southBoundRoad.NorthEntrance, eastBoundRoad.WestEntrance });
				NorthBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

				// East Bound
				eastBoundRoad.EastExit.nextWaypoints = new List<Waypoint>(new Waypoint[] {  EastBoundEntrance });
				eastBoundRoad.EastExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				EastBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { EastBoundExit, northBoundRoad.SouthEntrance });
				EastBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 0.50f, 0.50f });

				EastBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { southBoundRoad.NorthEntrance });
				EastBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// South Bound
				southBoundRoad.SouthExit.nextWaypoints = new List<Waypoint>(new Waypoint[] {  SouthBoundEntrance });
				southBoundRoad.SouthExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				SouthBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { SouthBoundExit, eastBoundRoad.WestEntrance });
				SouthBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 0.66f, 0.34f });

				SouthBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { northBoundRoad.SouthEntrance});
				SouthBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });
			}
		}

		void LinkTwoWay() {

			if(northBoundRoad == null && eastBoundRoad == null) {

				// South Bound
				southBoundRoad.SouthExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { SouthBoundEntrance });
				southBoundRoad.SouthExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				SouthBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { SouthBoundExit });
				SouthBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				SouthBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { westBoundRoad.EastEntrance });
				SouthBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// West Bound
				westBoundRoad.WestExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { WestBoundEntrance });
				westBoundRoad.WestExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				WestBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { southBoundRoad.NorthEntrance });
				WestBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

			} else if(northBoundRoad == null && southBoundRoad == null) {

				// East Bound
				eastBoundRoad.EastExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { EastBoundEntrance });
				eastBoundRoad.EastExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				EastBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { EastBoundExit });
				EastBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				EastBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { westBoundRoad.EastEntrance });
				EastBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// West Bound
				westBoundRoad.WestExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { WestBoundEntrance });
				westBoundRoad.WestExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				WestBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { WestBoundExit });
				WestBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				WestBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { eastBoundRoad.WestEntrance });
				WestBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

			} else if(northBoundRoad == null && westBoundRoad == null) {

				// East Bound
				eastBoundRoad.EastExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { EastBoundEntrance });
				eastBoundRoad.EastExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				EastBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { EastBoundExit });
				EastBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				EastBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { southBoundRoad.NorthEntrance });
				EastBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// South Bound
				southBoundRoad.SouthExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { SouthBoundEntrance });
				southBoundRoad.SouthExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				SouthBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { eastBoundRoad.WestEntrance });
				SouthBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

			} else if(eastBoundRoad == null && southBoundRoad == null) {

				// North Bound
				northBoundRoad.NorthExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { NorthBoundEntrance });
				northBoundRoad.NorthExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				NorthBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { westBoundRoad.EastEntrance });
				NorthBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// West Bound
				westBoundRoad.WestExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { WestBoundEntrance });
				westBoundRoad.WestExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				WestBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { WestBoundExit });
				WestBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				WestBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { northBoundRoad.SouthEntrance });
				WestBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

			} else if(eastBoundRoad == null && westBoundRoad == null) {

				// North Bound
				northBoundRoad.NorthExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { NorthBoundEntrance });
				northBoundRoad.NorthExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				NorthBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { NorthBoundExit });
				NorthBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				NorthBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { southBoundRoad.NorthEntrance });
				NorthBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// South Bound
				southBoundRoad.SouthExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { SouthBoundEntrance });
				southBoundRoad.SouthExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				SouthBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { SouthBoundExit });
				SouthBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				SouthBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { northBoundRoad.SouthEntrance });
				SouthBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

			} else {

				// North Bound
				northBoundRoad.NorthExit.nextWaypoints = new List<Waypoint>(new Waypoint[] {  NorthBoundEntrance });
				northBoundRoad.NorthExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				NorthBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { NorthBoundExit });
				NorthBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				NorthBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { eastBoundRoad.WestEntrance });
				NorthBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				// East Bound
				eastBoundRoad.EastExit.nextWaypoints = new List<Waypoint>(new Waypoint[] {  EastBoundEntrance });
				eastBoundRoad.EastExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				EastBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { northBoundRoad.SouthEntrance });
				EastBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

			}
		}

		void LinkDeadEnd() {
			if(northBoundRoad != null) {

				northBoundRoad.NorthExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { NorthBoundEntrance });
				northBoundRoad.NorthExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				NorthBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { NorthBoundExit });
				NorthBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				NorthBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { SouthBoundEntrance });
				NorthBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				SouthBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { SouthBoundExit });
				SouthBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				SouthBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { northBoundRoad.SouthEntrance });
				SouthBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

			} else if(eastBoundRoad != null) {

				eastBoundRoad.EastExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { EastBoundEntrance });
				eastBoundRoad.EastExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				EastBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { EastBoundExit });
				EastBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				EastBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { WestBoundEntrance });
				EastBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				WestBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { WestBoundExit });
				WestBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				WestBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { eastBoundRoad.WestEntrance });
				WestBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

			} else if(southBoundRoad != null) {

				southBoundRoad.SouthExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { SouthBoundEntrance });
				southBoundRoad.SouthExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				SouthBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { SouthBoundExit });
				SouthBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				SouthBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { NorthBoundEntrance });
				SouthBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				NorthBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { NorthBoundExit });
				NorthBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				NorthBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { southBoundRoad.NorthEntrance });
				NorthBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

			} else {

				westBoundRoad.WestExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { WestBoundEntrance });
				westBoundRoad.WestExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				WestBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { WestBoundExit });
				WestBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				WestBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { EastBoundEntrance });
				WestBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				EastBoundEntrance.nextWaypoints = new List<Waypoint>(new Waypoint[] { EastBoundExit });
				EastBoundEntrance.nextWaypointProbabilities = new List<float>(new float[] { 1f });

				EastBoundExit.nextWaypoints = new List<Waypoint>(new Waypoint[] { westBoundRoad.EastEntrance });
				EastBoundExit.nextWaypointProbabilities = new List<float>(new float[] { 1f });

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