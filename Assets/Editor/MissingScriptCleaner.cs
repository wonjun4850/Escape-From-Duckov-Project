using UnityEditor;
using UnityEngine;

public class MissingScriptCleaner
{
    [MenuItem("Tools/Clean Missing Scripts")]
    public static void Clean()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        int count = 0;

        foreach (GameObject obj in allObjects)
        {
            int removedCount = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
            count += removedCount;
        }

    }
}