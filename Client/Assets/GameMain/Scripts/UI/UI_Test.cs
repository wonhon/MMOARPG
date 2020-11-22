//===================================================
//作    者：WonHon
//创建时间：2019-08-31-16:56:02
//备    注：
//===================================================

using UnityEngine;
using UnityGameFramework.Runtime;

namespace SuperBiomass
{
    [RequireComponent(typeof(UI_TestView))]
    public class UI_Test : UGuiForm
    {
        private UI_TestView m_View;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_View = GetComponent<UI_TestView>();
            m_View.Init();
        }
    }
}
