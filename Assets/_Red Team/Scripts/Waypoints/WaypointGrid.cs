using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedTeam {

	public class WaypointGrid : MonoBehaviour {

		public WaypointRoadSegment[,] northSouthRoadSegments;
		public WaypointRoadSegment[,] westEastRoadSegments;
		public WaypointIntersection[,] intersections;

		public float roadWidth;
		public float blockLength;

		public WaypointRoadSegment northSouthRoadSegmentPrefab;
		public WaypointRoadSegment westEastRoadSegmentPrefab;
		public WaypointIntersection waypointIntersectionPrefab;

		public GameObject[] sidewalkPrefabs;

		/// <summary>
		/// The size of the grid in number of blocks
		/// along the width(x) and length(z)
		/// </summary>
		public Vector2Int gridSize;

		public void Generate() {
			if(gridSize.x == 0 || gridSize.y == 0) {
				Debug.LogError("Cannot create grid of width 0 or length 0");
				return;
			}

			CreateRoadsAndIntersections();

			PositionRoadsAndIntersections();

			LinkIntersections();

			CreateAndPositionSidewalks();
		}

		/// <summary>
		/// Creates the roads and intersections from the prefabs
		/// </summary>
		void CreateRoadsAndIntersections() {
			
			northSouthRoadSegments = new WaypointRoadSegment[gridSize.x + 1, gridSize.y];
			westEastRoadSegments = new WaypointRoadSegment[gridSize.x, gridSize.y + 1];
			intersections = new WaypointIntersection[gridSize.x + 1,gridSize.y + 1];

			for(int i = 0; i < gridSize.x + 1; i++) {
				for(int j = 0; j < gridSize.y + 1; j++) {

					if(j < gridSize.y)
						northSouthRoadSegments[i, j] = GameObject.Instantiate<WaypointRoadSegment>(northSouthRoadSegmentPrefab, transform);

					if(i < gridSize.x)
						westEastRoadSegments[i, j] = GameObject.Instantiate<WaypointRoadSegment>(westEastRoadSegmentPrefab, transform);

					intersections[i, j] = GameObject.Instantiate<WaypointIntersection>(waypointIntersectionPrefab, transform);
				}
			}
		}

		/// <summary>
		/// Positions the roads and intersections.
		/// </summary>
		void PositionRoadsAndIntersections() {
			
			for(int i = 0; i < gridSize.x + 1; i++) {
				for(int j = 0; j < gridSize.y + 1; j++) {

					if(j < gridSize.y) {

						northSouthRoadSegments[i, j].transform.localPosition = new Vector3(
							i * (roadWidth + blockLength) + (roadWidth / 2f), 
							0f, 
							j * (roadWidth + blockLength) + (blockLength / 2f) + roadWidth
						);

					}

					if(i < gridSize.x) {

						westEastRoadSegments[i, j].transform.localPosition = new Vector3(
							i * (roadWidth + blockLength) + (blockLength / 2f) + roadWidth,
							0f,
							j * (roadWidth + blockLength) + (roadWidth / 2f)
						);
					}

					intersections[i, j].transform.localPosition = new Vector3(
						i * (roadWidth + blockLength) + (roadWidth / 2f),
						0f,
						j * (roadWidth + blockLength) + (roadWidth / 2f)
					);
				}
			}
		}

		/// <summary>
		/// Links the intersections to the roads
		/// </summary>
		void LinkIntersections() {
			
			for(int i = 0; i < gridSize.x + 1; i++) {
				for(int j = 0; j < gridSize.y + 1; j++) {
					if(i == 0) {

						intersections[i, j].westBoundRoad = westEastRoadSegments[i, j];

					} else if(i == gridSize.x) {

						intersections[i, j].eastBoundRoad = westEastRoadSegments[i - 1, j];

					} else {

						intersections[i, j].eastBoundRoad = westEastRoadSegments[i - 1, j];
						intersections[i, j].westBoundRoad = westEastRoadSegments[i, j];

					}

					if(j == 0) {

						intersections[i, j].southBoundRoad = northSouthRoadSegments[i, j];

					} else if(j == gridSize.y) {

						intersections[i, j].northBoundRoad = northSouthRoadSegments[i, j - 1];

					} else {

						intersections[i, j].northBoundRoad = northSouthRoadSegments[i, j - 1];
						intersections[i, j].southBoundRoad = northSouthRoadSegments[i, j];

					}

					intersections[i, j].Generate();
				}
			}
		}

		void CreateAndPositionSidewalks() {
			for(int i = 0; i < gridSize.x; i++) {
				for(int j = 0; j < gridSize.y; j++) {

					GameObject sidewalk = GameObject.Instantiate(sidewalkPrefabs[Random.Range(0, sidewalkPrefabs.Length)], transform);
					sidewalk.transform.localPosition = new Vector3(
						i * (roadWidth + blockLength) + roadWidth + (blockLength / 2f), 
						0f, 
						j * (roadWidth + blockLength) + roadWidth + (blockLength / 2f)
					);
				}
			}
		}
	}
}