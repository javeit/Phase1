using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RedTeam {

	public interface IWaypoint {
		IWaypoint GetNext();
		Vector3 Position { get; }
		float ArrivedDistance { get; }
		void SetCar(GameObject car);
	}


	public class Waypoint : MonoBehaviour, IWaypoint {

		/// <summary>
		/// A list of the next waypoints
		/// </summary>
		public List<Waypoint> nextWaypoints;

		/// <summary>
		/// A list of the probability of the corresponding
		/// waypoints in nextWaypoints to be chosen from
		/// 0 to 1
		/// </summary>
		public List<float> nextWaypointProbabilities;

		public float arrivedDistance;

		public float ArrivedDistance {
			get {
				return arrivedDistance;
			}
		}

		public Vector3 Position {
			get {
				return transform.position;
			}
		}

		public GameObject car;

		void OnValidate() {

			if (nextWaypoints != null) {
				if (nextWaypointProbabilities == null)
					nextWaypointProbabilities = new List<float>();

				while (nextWaypointProbabilities.Count != nextWaypoints.Count) {
					if (nextWaypointProbabilities.Count > nextWaypoints.Count)
						nextWaypointProbabilities.RemoveAt(nextWaypointProbabilities.Count - 1);
					else
						nextWaypointProbabilities.Add(0f);
				}

				for (int i = 0; i < nextWaypointProbabilities.Count; i++) {
					if (nextWaypointProbabilities[i] < 0f)
						nextWaypointProbabilities[i] = 0f;

					if (nextWaypointProbabilities[i] > 1f)
						nextWaypointProbabilities[i] = 1f;
				}
			}
		}

		/// <summary>
		/// Gets the next waypoint the car should travel to.
		/// Returns null if there are no next waypoints
		/// </summary>
		/// <returns>The next waypoint</returns>
		public IWaypoint GetNext() {
			if(nextWaypoints == null || nextWaypoints.Count == 0)
				return null;

			float rand = UnityEngine.Random.Range(0f, 1f);

			// this number represents the chance that the waypoint
			// being checked is the selected path. It is incremented
			// by the probabilities of each waypoint until one is
			// selected
			float currentProbability = 0f;

			for(int i = 0; i < nextWaypoints.Count; i++) {
				currentProbability += nextWaypointProbabilities[i];

				if(rand <= currentProbability) {
					if(nextWaypoints[i].car == null)
						return nextWaypoints[i];
					else
						return null;
				}
			}

			// default - return the last element in the list of waypoints
			if(nextWaypoints[nextWaypoints.Count - 1].car == null)
				return nextWaypoints[nextWaypoints.Count - 1];
			else
				return null;
		}

		public void SetCar(GameObject car) {
			this.car = car;
		}

		/// <summary>
		/// Taken from the waypoints example, draws gizmos to visually 
		/// indicate the waypoints location and connection to other waypoints
		/// </summary>
		void OnDrawGizmos() {

			// Draw waypoints and connections in the scene view for easier debugging.
			if(car == null)
				Gizmos.color = new Color(1, 0, 0, 0.3f);
			else
				Gizmos.color = new Color(0, 1, 0, 0.3f);

			Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));

			if(nextWaypoints != null && nextWaypoints.Count != 0) {
				Gizmos.color = new Color(0, 1, 0, 1f);

				foreach(IWaypoint waypoint in nextWaypoints) {
					if(waypoint != null)
						Gizmos.DrawLine(transform.position, waypoint.Position);
				}
			}
		}
	}
}