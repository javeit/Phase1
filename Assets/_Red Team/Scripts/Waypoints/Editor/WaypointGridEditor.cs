using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RedTeam {

	[CustomEditor(typeof(WaypointGrid))]
	public class WaypointGridEditor : Editor {

		WaypointGrid _target;

		public override void OnInspectorGUI() {
			_target = target as WaypointGrid;

			DrawDefaultInspector();

			if(GUILayout.Button("Generate")) {
				_target.Generate();
			}
		}
	}
}