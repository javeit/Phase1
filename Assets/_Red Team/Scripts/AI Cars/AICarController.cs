using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RedTeam {

	[RequireComponent(typeof(NavMeshAgent))]
	public class AICarController : MonoBehaviour {

		public GameObject guide;

		public float maxSpeed;
		public float acceleration;
		public float minGuideDistance;

		NavMeshAgent agent;

		void Update() {
			agent.SetDestination(guide.transform.position);
			if((guide.transform.position - transform.position).magnitude < minGuideDistance) {
				agent.speed -= acceleration;
			} else {
				agent.speed += acceleration;
				if(agent.speed >= maxSpeed)
					agent.speed = maxSpeed;
			}
		}

		void Awake() {
			agent = GetComponent<NavMeshAgent>();
		}
	}
}