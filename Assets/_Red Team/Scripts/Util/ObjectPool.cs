using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedTeam.Util {

	/// <summary>
	/// The object pool interface
	/// </summary>
	public interface IObjectPool {
		/// <summary>
		/// Returns an object from the pool
		/// </summary>
		/// <returns>The object.</returns>
		/// <param name="position">Position.</param>
		/// <param name="rotation">Rotation.</param>
		IPoolableObject GetObject(Vector3 position, Quaternion rotation);

		/// <summary>
		/// Adds an object to the pool
		/// </summary>
		/// <param name="obj">Object.</param>
		void AddObject(IPoolableObject obj);

		/// <summary>
		/// Determines whether this instance is empty.
		/// </summary>
		/// <returns><c>true</c> if this instance is empty; otherwise, <c>false</c>.</returns>
		bool IsEmpty();
	}

	/// <summary>
	/// the poolable object interface
	/// </summary>
	public interface IPoolableObject {
		/// <summary>
		/// The default initializer for this poolable object
		/// </summary>
		void Init(Vector3 position, Quaternion rotation);

		/// <summary>
		/// Performs actions necessary when releasing an object back into the pool
		/// </summary>
		void Release();

		/// <summary>
		/// The GameObject being pooled
		/// </summary>
		/// <value>The game object.</value>
		GameObject PooledGameObject { get; }
	}

	public class ObjectPool : IObjectPool {

		private List<IPoolableObject> pool;

		/// <summary>
		/// Determines whether this object pool is empty.
		/// </summary>
		/// <returns><c>true</c> if this object pool is empty; otherwise, <c>false</c>.</returns>
		public bool IsEmpty() {
			if(pool == null || pool.Count == 0)
				return true;

			return false;
		}

		/// <summary>
		/// Returns an object from the pool initialized to the given position and rotation
		/// </summary>
		/// <returns>The object.</returns>
		/// <param name="position">Position.</param>
		/// <param name="rotation">Rotation.</param>
		public IPoolableObject GetObject(Vector3 position, Quaternion rotation) {
			if(IsEmpty()) {
				Debug.LogError("Cannot remove object from empty pool");
				return null;
			}

			IPoolableObject obj = pool[0];
			pool.RemoveAt(0);

			obj.Init(position, rotation);
			return obj;
		}

		/// <summary>
		/// Adds the object.
		/// </summary>
		/// <param name="obj">Object.</param>
		public void AddObject(IPoolableObject obj) {
			if(obj == null) {
				Debug.LogError("Cannot add null gameobject to pool");
			}

			if(pool == null)
				pool = new List<IPoolableObject>();

			obj.Release();
			pool.Add(obj);
		}
	}
}