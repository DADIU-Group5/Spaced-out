using UnityEngine;
//using UnityEditor;

public class ObjectCreator /* : Editor */ {

	//[MenuItem ("GameObject/Create Simple Prefab")]
	static void DoCreateSimplePrefab()
	{
		//Object prefab = EditorUtility.CreateEmptyPrefab ("Assets/Prototypes/Mia/" + "genericName" + ".prefab");
		//EditorUtility.ReplacePrefab (t.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
	}

	/*public override void OnInspectorGUI()
	{
		LevelScript myTarget = (LevelScript)target;

		myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
		EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
	}*/
}