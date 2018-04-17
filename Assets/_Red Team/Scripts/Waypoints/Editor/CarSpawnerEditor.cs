using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RedTeam {

	[CustomEditor(typeof(CarSpawner))]
	public class CarSpawnerEditor : Editor {

		CarSpawner _target;

		public override void OnInspectorGUI() {
			_target = target as CarSpawner;

			DrawDefaultInspector();

			if(GUILayout.Button("Spawn Cars"))
				_target.SpawnCars();
		}
	}
}