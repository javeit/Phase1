using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RedTeam {

	[RequireComponent(typeof(NavMeshAgent))]
	public class AICarController : MonoBehaviour {

		public GameObject guide;

		NavMeshAgent agent;

		void Update() {
			agent.SetDestination(guide.transform.position);
		}

		void Awake() {
			agent = GetComponent<NavMeshAgent>();
		}
	}
}