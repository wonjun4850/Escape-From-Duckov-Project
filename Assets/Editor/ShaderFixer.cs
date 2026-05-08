using UnityEngine;
using UnityEditor;

public class ShaderFixer : EditorWindow
{
    [MenuItem("Tools/Fix Shaders for WonJun")]
    public static void Fix()
    {
        foreach (Object obj in Selection.objects)
        {
            if (obj is Material mat)
            {
                Texture oldTex = mat.GetTexture("_MainTex");

                mat.shader = Shader.Find("Universal Render Pipeline/Lit");

                if (oldTex != null)
                {
                    mat.SetTexture("_BaseMap", oldTex);
                }
            }
        }
    }
}