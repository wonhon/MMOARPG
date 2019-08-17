using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//===================================================
//作    者：WonHon
//创建时间：2019-08-11-21:39:43
//备    注：删除场景无效的脚本
//===================================================

namespace SuperBiomass
{
    public class RemoveInvalidScript
    {
        [MenuItem("SuperBiomass/Remove Scene InvalidScript")]
        public static void Run()
        {
            List<GameObject> sceneRoot = new List<GameObject>();
            List<GameObject> obj = new List<GameObject>();
            //得到场景的根节点
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects(sceneRoot);

            int dex = 0;
            for (int i = 0; i < sceneRoot.Count; i++)
            {
                Transform[] a = sceneRoot[i].GetComponentsInChildren<Transform>();
                for (int j = 0; j < a.Length; j++)
                {
                    GameObject go = a[j].gameObject; //得到数组中的单个object
                    SerializedObject so = new SerializedObject(go); // 将object序列化
                    var soProperties = so.FindProperty("m_Component");//得到object的Component序列化数组
                    var components = go.GetComponents<Component>();//得到物体的component数组
                    int propertyIndex = 0;
                    foreach (var c in components)//遍历component数组
                    {
                        if (c == null)
                        {
                            Debug.Log(string.Format("remove invalid script: {0}", propertyIndex));
                            soProperties.DeleteArrayElementAtIndex(propertyIndex);
                            dex++;
                        }
                        ++propertyIndex;
                    }
                    so.ApplyModifiedProperties();//这句话好重要 开始没加上 怎么都不成功 坑死我了 
                }
            }
            //将场景设置为dert
            if (dex > 0)
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
    }
}

