using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace RedTeam.Util {

	[CustomEditor(typeof(WaypointPathGenerator))]
	public class WaypointPathGeneratorEditor : Editor {

		WaypointPathGenerator generator;

		public override void OnInspectorGUI() {
			generator = target as WaypointPathGenerator;

			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("waypoints"), true);
			serializedObject.ApplyModifiedProperties();


			serializedObject.ApplyModifiedProperties();

			if(GUILayout.Button("Generate Path")) {
				generator.GeneratePath();

				foreach(Waypoint waypoint in generator.waypoints) {
					EditorUtility.SetDirty(waypoint);
				}
			}
		}
	}
}