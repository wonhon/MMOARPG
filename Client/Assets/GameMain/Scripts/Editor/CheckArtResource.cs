using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;

//===================================================
//作    者：WonHon
//创建时间：2019-08-03-20:46:57
//备    注：检查美术资源
//===================================================

public class CheckArtResource
{
    public class AddScriptOnGameObject
    {
        [MenuItem("SuperBiomass/Remove SceneOject Animator And Animation")]
        public static void Run()
        {
            Animator[] animators = Selection.activeGameObject.GetComponentsInChildren<Animator>(true);
            for (int i = 0; i < animators.Length; i++)
            {
                Object.DestroyImmediate(animators[i]);
            }

            Animation[] animations = Selection.activeGameObject.GetComponentsInChildren<Animation>(true);
            for (int i = 0; i < animations.Length; i++)
            {
                Object.DestroyImmediate(animations[i]);
            }
        }
        
        public static void Run1()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeContext);
            Log.Info("Path: {0}", path);

            if (Directory.Exists(path))
            {
                DirectoryInfo direction = new DirectoryInfo(path);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

                Debug.Log(files.Length);

                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.EndsWith(".meta"))
                    {
                        continue;
                    }
                }
            }
        }
    }
}
