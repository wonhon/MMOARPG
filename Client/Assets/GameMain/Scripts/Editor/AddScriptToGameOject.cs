using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;

//===================================================
//作    者：WonHon
//创建时间：2019-08-03-20:21:47
//备    注：添加目标脚本到MeshRender物体上，包括子物体
//===================================================

namespace SuperBiomass
{
    public class AddScriptOnGameObject
    {
        [MenuItem("SuperBiomass/Add <MeshRenderer> To MeshRender")]
        public static void Run()
        {
            MeshRenderer[] rds = Selection.activeGameObject.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < rds.Length; i++)
            {
                if (rds[i].enabled)
                {
                    rds[i].gameObject.GetOrAddComponent<MeshLightmapSetting>();
                }
                else
                {
                    Log.Warning(" MeshRenderer: '{0}', enable is false", rds[i].gameObject);
                }
            }

            Transform[] tsfs = Selection.activeGameObject.GetComponentsInChildren<Transform>();
            {
                for (int i = 0; i < tsfs.Length; i++)
                {
                    MeshRenderer msrd = tsfs[i].GetComponent<MeshRenderer>();
                    MeshLightmapSetting mls = tsfs[i].GetComponent<MeshLightmapSetting>();
                    if (mls && (msrd == null || !msrd.enabled))
                    {
                        Object.DestroyImmediate(mls);   
                    }
                }
            }
        }
    }
}
