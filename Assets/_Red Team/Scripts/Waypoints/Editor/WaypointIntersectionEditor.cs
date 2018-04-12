using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RedTeam {

	[CustomEditor(typeof(WaypointIntersection))]
	public class WaypointIntersectionEditor : Editor {

		WaypointIntersection _target;

		public override void OnInspectorGUI() {
			_target = target as WaypointIntersection;

			DrawDefaultInspector();

			if(GUILayout.Button("Generate")) {
				_target.Generate();
			}
		}
	}
}