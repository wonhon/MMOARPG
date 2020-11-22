using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

//===================================================
//作    者：WonHon
//创建时间：2019-09-01-23:19:36
//备    注：
//===================================================

namespace SuperBiomass
{
    public class UIRebuildMonitor : MonoBehaviour
    {
        private IList<ICanvasElement> m_LayoutRebuildQueue;
        private IList<ICanvasElement> m_GraphicRebuildQueue;

        private void Awake()
        {
            System.Type type = typeof(CanvasUpdateRegistry);
            FieldInfo fieldInfo = type.GetField("m_LayoutRebuildQueue", BindingFlags.NonPublic | BindingFlags.Instance);
            m_LayoutRebuildQueue = fieldInfo.GetValue(CanvasUpdateRegistry.instance) as IList<ICanvasElement>;
            fieldInfo = type.GetField("m_GraphicRebuildQueue", BindingFlags.NonPublic | BindingFlags.Instance);
            m_GraphicRebuildQueue = fieldInfo.GetValue(CanvasUpdateRegistry.instance) as IList<ICanvasElement>;
        }

        private void Update()
        {
            for (int i = 0; i < m_LayoutRebuildQueue.Count; i++)
            {
                var rebuild = m_LayoutRebuildQueue[i];
                if (ObjectValidForUpdate(rebuild))
                {
                    Log.Warning("【UI Rebuild】 '{0}' 引起网格重建", rebuild.transform.gameObject);
                }
            }

            for (int i = 0; i < m_GraphicRebuildQueue.Count; i++)
            {
                var element = m_GraphicRebuildQueue[i];
                if (ObjectValidForUpdate(element))
                {
                    Log.Warning("【UI Rebuild】 '{0}' 引起网格 '{1}' 重建", element.transform.gameObject, element.transform.GetComponent<Graphic>().canvas.name);
                }
            }
        }

        private bool ObjectValidForUpdate(ICanvasElement element)
        {
            var valid = element != null;

            var isUnityObject = element is Object;
            //Here we make use of the overloaded UnityEngine.Object == null, that checks if the native object is alive.
            if (isUnityObject)
                valid = (element as Object) != null;

            return valid;
        }
    }
}

