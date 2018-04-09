using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RedTeam.Util;
using System;

namespace RedTeam {

	[RequireComponent(typeof(Rigidbody))]
	public class PooledBullet : MonoBehaviour, IPoolableObject {

		float age = 0f;

		Rigidbody RigidBody {
			get {
				return GetComponent<Rigidbody>();
			}
		}

		public IObjectPool pool;

		public void Fire(float fireForce) {
			RigidBody.velocity = transform.forward * fireForce;
		}

		void Update() {
			age += Time.deltaTime;
			if(age >= 3f)
				pool.AddObject(this);
		}

		#region PooledObject stuff

		public GameObject PooledGameObject {
			get {
				return gameObject;
			}
		}

		public void Init(Vector3 position, Quaternion rotation) {
			transform.position = position;
			transform.rotation = rotation;
			age = 0f;
			gameObject.SetActive(true);
		}

		public void Release() {
			gameObject.SetActive(false);
			RigidBody.velocity = new Vector3(0f, 0f, 0f);
		}

		#endregion
	}
}