using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedTeam {

	public class AICarGuide : MonoBehaviour
	{

		public float speed;
		public GameObject car;
		public float followDistance;
		public bool redStopNS = false;
		public bool redStopWE = false;
		public bool collideValNS = false;
		public bool collideValWE = false;

		public Waypoint startWaypoint;

		IWaypoint waypoint;

		public bool getRedNS()
		{
			LightControlReverse lcr = new LightControlReverse();
			redStopNS = lcr.getRedStatus();
			return redStopNS;
		}
		public bool getRedWE()
		{
			LightControl lc = new LightControl();
			redStopWE = lc.getRedStatus();
			return redStopWE;
		}
		public void OnTriggerEnter(Collider col)
		{

			if (col.gameObject.tag == "TrafficNS")
			{
				//Debug.Log("NS colliede");
				collideValNS = true;

			}
			else if (col.gameObject.tag == "TrafficWE")
			{
				//Debug.Log("WE collided");
				collideValWE = true;

			}
		}

		public void OnTriggerExit(Collider col)
		{
			if (col.gameObject.tag == "TrafficNS")
			{
				//Debug.Log("NS exit");
				collideValNS = false;

			}
			else if (col.gameObject.tag == "TrafficWE")
			{
				//Debug.Log("WE exit");
				collideValWE = false;

			}


		}

		void Update()
		{

			// make sure the guide keeps within the given following distance of the car
			if ((car.transform.position - transform.position).magnitude <= followDistance)
			{

				Vector3 remainingDistance = waypoint.Position - transform.position;

				if (remainingDistance.magnitude <= waypoint.ArrivedDistance)
				{
					if(!(getRedNS() && collideValNS) && !(getRedWE() && collideValWE)) {
						
						IWaypoint nextWaypoint = waypoint.GetNext();
						if(nextWaypoint != null) {

							waypoint.SetCar(null);

							waypoint = nextWaypoint;

							waypoint.SetCar(gameObject);
						}
					}

				}
				else
				{

					transform.Translate(remainingDistance.normalized * speed * Time.deltaTime);

				}

			}
		}

		void Awake()
		{
			waypoint = startWaypoint as IWaypoint;

		}

	}
}