using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/*
 * Editor personalizado para los Nav Items
 * 
 * Se ocultan y desocultan elementos dependiendo del tipo de navegación que se necesita.
 */
[CustomEditor (typeof (NavItem))]
public class NavItemEditor : Editor {

	public override void OnInspectorGUI() {
        NavItem i = (NavItem)target;

        EditorGUILayout.Toggle("Toggle", i.test);
    }
}
