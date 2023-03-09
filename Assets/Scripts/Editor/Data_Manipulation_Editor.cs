using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Base_Data))]
public class Data_Manipulation_Editor : Editor
{
    public override void OnInspectorGUI () {
        //Called whenever the inspector is drawn for this object.
        DrawDefaultInspector();
        //This draws the default screen. You don't need this if you want
        //to start from scratch, but I use this when I'm just adding a button or
        //some small addition and don't feel like recreating the whole inspector.
        Base_Data myTarget = (Base_Data)target;

        if (GUILayout.Button("Save JSON"))
        {
            Data_Manager.SaveJSON(myTarget);
        }

        if (GUILayout.Button("Load from JSON"))
        {
            Model_Entity newData = new Model_Entity();
            Data_Manager.TryLoadJSON(Application.persistentDataPath + "\\" + myTarget.entityName + ".json", newData);
            myTarget.properties = newData.properties;
        }
    }
}
