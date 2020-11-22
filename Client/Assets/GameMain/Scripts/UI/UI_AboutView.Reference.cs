//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改
// 生成时间：2019-09-03-23:28:57
//------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime.UI;

namespace SuperBiomass
{
    sealed partial class UI_AboutView
    {
        [SerializeField]
        private RectTransform m_ContentRectTsf;

        [SerializeField]
        private ButtonT m_BackButtonBtn;

#if UNITY_EDITOR
        [ContextMenu("添加引用")]
        public void Run()
        {
            InitReference();
        }
#endif

		private void InitReference()
        {
            m_ContentRectTsf = transform.Find("rectTsf_Content").GetComponent<RectTransform>();
            m_BackButtonBtn = transform.Find("btn_BackButton").GetComponent<ButtonT>();
        }
    }
}