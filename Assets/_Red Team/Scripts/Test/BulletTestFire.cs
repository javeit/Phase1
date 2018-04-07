using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RedTeam.Util;

namespace RedTeam {

	public class BulletTestFire : MonoBehaviour {

		public PooledBullet prefab;
		public float bulletForce;

		IObjectPool pool;

		void Update() {
			if(Input.GetMouseButtonDown(0)) {
				Debug.Log("FIRE!!!");
				PooledBullet bullet = GetBullet();

				bullet.Fire(bulletForce);
			}
		}

		PooledBullet GetBullet() {
			PooledBullet bullet;

			Vector3 bulletPos = Camera.main.transform.position + Camera.main.ScreenPointToRay(Input.mousePosition).direction * 10f;
			Vector3 bulletDir = bulletPos - Camera.main.transform.position;

			if(!pool.IsEmpty())
				bullet = pool.GetObject(bulletPos, Quaternion.LookRotation(bulletDir)) as PooledBullet;
			else {
				bullet = GameObject.Instantiate(prefab, bulletPos, Quaternion.LookRotation(bulletDir));
				bullet.pool = pool;
			}

			return bullet;
		}

		void Awake() {
			pool = new ObjectPool();
		}
	}
}