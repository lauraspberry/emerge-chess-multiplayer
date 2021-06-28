using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomManagerihg))]
public class RoomManagerEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.HelpBox("This script is responsible for creating and joining rooms",MessageType.Info);

        RoomManagerihg roomManager = (RoomManagerihg)target;
        

        if (GUILayout.Button("Join School Room"))
        {
            roomManager.OnEnterRoomButtonClicked_School();
        }

        if (GUILayout.Button("Join Outdoor Room"))
        {
            roomManager.OnEnterRoomButtonClicked_Outdoor();
        }
    }
}
