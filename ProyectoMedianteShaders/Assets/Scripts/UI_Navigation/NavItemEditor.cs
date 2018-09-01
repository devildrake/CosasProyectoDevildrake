using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


/*
 * Editor personalizado para los Nav Items
 * 
 * Se ocultan y desocultan elementos dependiendo del tipo de navegación que se necesita.
 */
[CustomEditor (typeof (NavItem))]
public class NavItemEditor : Editor {

	public override void OnInspectorGUI() {
        NavItem i = (NavItem)target;

        i.navType = (NavItem.NAV_TYPE)EditorGUILayout.EnumPopup("Navigation type",i.navType);

        switch (i.navType) {
            case NavItem.NAV_TYPE.SIMPLE:
                Simple(i);
                break;

            case NavItem.NAV_TYPE.MAIN_MENU:
                MainMenu(i);
                break;

            case NavItem.NAV_TYPE.OPTIONS:
                Options(i);
                break;
        }
    }

    private void Simple(NavItem i) {
        EditorGUI.indentLevel++;
        i.upItem = (NavItem)EditorGUILayout.ObjectField("Up item", i.upItem, typeof(NavItem), true);
        i.downItem = (NavItem)EditorGUILayout.ObjectField("Down item", i.downItem, typeof(NavItem), true);
        i.rightItem = (NavItem)EditorGUILayout.ObjectField("Right item", i.rightItem, typeof(NavItem), true);
        i.leftItem = (NavItem)EditorGUILayout.ObjectField("Left item", i.leftItem, typeof(NavItem), true);
        EditorGUI.indentLevel--;
    }

    private void MainMenu(NavItem i) {

    }

    private void Options(NavItem i) {
        EditorGUI.indentLevel++;
        i.myType = (NavItem.MENU_ITEM_TYPE)EditorGUILayout.EnumPopup("Selectable type", i.myType);
        i.optionsPart = (NavItem.OPTION_TYPE)EditorGUILayout.EnumPopup("Options part", i.optionsPart);
        i.upItem = (NavItem)EditorGUILayout.ObjectField("Up item", i.upItem, typeof(NavItem), true);
        i.upItem2 = (NavItem)EditorGUILayout.ObjectField("Up item 2", i.upItem2, typeof(NavItem), true);
        i.downItem = (NavItem)EditorGUILayout.ObjectField("Down item", i.downItem, typeof(NavItem), true);
        i.downItem2 = (NavItem)EditorGUILayout.ObjectField("Down item 2", i.downItem2, typeof(NavItem), true);
        i.rightItem = (NavItem)EditorGUILayout.ObjectField("Right item", i.rightItem, typeof(NavItem), true);
        i.leftItem = (NavItem)EditorGUILayout.ObjectField("Left item", i.leftItem, typeof(NavItem), true);
        i.selectableElement1 = (Selectable)EditorGUILayout.ObjectField("Selectable 1", i.selectableElement1, typeof(Selectable), true);
        i.selectableElement2 = (Selectable)EditorGUILayout.ObjectField("Selectable 2", i.selectableElement2, typeof(Selectable), true);
        EditorGUI.indentLevel--;
    }
}
