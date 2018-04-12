using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RedTeam {

	[CustomEditor(typeof(WaypointRoadSegment))]
	public class WaypointRoadSegmentEditor : Editor {

		WaypointRoadSegment _target;

		public override void OnInspectorGUI() {
			_target = target as WaypointRoadSegment;

			DrawDefaultInspector();

			if(GUILayout.Button("Generate")) {
				_target.Generate();
			}
		}
	}
}