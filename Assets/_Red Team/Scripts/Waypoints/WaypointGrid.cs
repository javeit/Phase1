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

		public GameObject[] blockPrefabs;

		/// <summary>
		/// The size of the grid in number of blocks
		/// along the width(x) and length(z)
		/// </summary>
		public Vector2Int gridSize;

		public bool createBlocks;

        Transform roadParent;
        Transform RoadParent {
            get {
                if (roadParent == null) {
                    roadParent = new GameObject("Roads").transform;
                    roadParent.parent = transform;
                    roadParent.localPosition = Vector3.zero;
                }
                return roadParent;
            }
        }

        Transform intersectionParent;
        Transform IntersectionParent {
            get {
                if(intersectionParent == null) {
                    intersectionParent = new GameObject("Intersections").transform;
                    intersectionParent.parent = transform;
                    intersectionParent.localPosition = Vector3.zero;
                }
                return intersectionParent;
            }
        }

        Transform blockParent;
        Transform BlockParent {
            get {
                if(blockParent == null) {
                    blockParent = new GameObject("Blocks").transform;
                    blockParent.parent = transform;
                    blockParent.localPosition = Vector3.zero;
                }
                return blockParent;
            }
        }

        Transform northSouthRoadParent;
        Transform NorthSouthRoadParent {
            get {
                if(northSouthRoadParent == null) {
                    northSouthRoadParent = new GameObject("North-South Roads").transform;
                    northSouthRoadParent.parent = RoadParent;
                    northSouthRoadParent.localPosition = Vector3.zero;
                }
                return northSouthRoadParent;
            }
        }

        Transform westEastRoadParent;
        Transform WestEastRoadParent {
            get {
                if(westEastRoadParent == null) {
                    westEastRoadParent = new GameObject("West-East Roads").transform;
                    westEastRoadParent.parent = RoadParent;
                    westEastRoadParent.localPosition = Vector3.zero;
                }
                return westEastRoadParent;
            }
        }

        public void Generate() {
			if(gridSize.x == 0 || gridSize.y == 0) {
				Debug.LogError("Cannot create grid of width 0 or length 0");
				return;
			}

			CreateRoadsAndIntersections();

			PositionRoadsAndIntersections();

			LinkIntersections();

			if(createBlocks)
				CreateAndPositionBlocks();
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

                    if (j < gridSize.y) {
                        northSouthRoadSegments[i, j] = GameObject.Instantiate<WaypointRoadSegment>(northSouthRoadSegmentPrefab, NorthSouthRoadParent);
                        northSouthRoadSegments[i, j].name = string.Format("North-South Road ({0}, {1})", i, j);
                    }

                    if (i < gridSize.x) {
                        westEastRoadSegments[i, j] = GameObject.Instantiate<WaypointRoadSegment>(westEastRoadSegmentPrefab, WestEastRoadParent);
                        westEastRoadSegments[i, j].name = string.Format("West-East Road ({0}, {1})", i, j);
                    }

					intersections[i, j] = GameObject.Instantiate<WaypointIntersection>(waypointIntersectionPrefab, IntersectionParent);
                    intersections[i, j].name = string.Format("Intersection ({0}, {1})", i, j);
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

		void CreateAndPositionBlocks() {
			for(int i = 0; i < gridSize.x; i++) {
				for(int j = 0; j < gridSize.y; j++) {

					GameObject block = GameObject.Instantiate(blockPrefabs[Random.Range(0, blockPrefabs.Length)], BlockParent);
					block.transform.localPosition = new Vector3(
						i * (roadWidth + blockLength) + roadWidth + (blockLength / 2f), 
						0f, 
						j * (roadWidth + blockLength) + roadWidth + (blockLength / 2f)
					);
                    block.name = string.Format("Block ({0}, {1})", i, j);
				}
			}
		}
	}
}