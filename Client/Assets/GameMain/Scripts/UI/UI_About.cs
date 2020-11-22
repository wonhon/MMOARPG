//===================================================
//作    者：WonHon
//创建时间：2019-09-01-15:04:09
//备    注：
//===================================================

using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SuperBiomass
{
    [RequireComponent(typeof(UI_AboutView))]
    public class UI_About : UGuiForm
    {
        private UI_AboutView m_View;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_View = GetComponent<UI_AboutView>();
            m_View.Init();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_View.OnOpen(UIForm.SerialId);

            m_View.AddListener(OnInnerEvent);

            // 换个音乐
            GameEntry.Sound.PlayMusic(3);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            m_View.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            m_View.RemoveListener(OnInnerEvent);

            base.OnClose(isShutdown, userData);
        }

        #region 内部事件
        private void OnInnerEvent(object sender, GameEventArgs e)
        {
            UIEventArgs args = e as UIEventArgs;
            if (args.SerialId == UIForm.SerialId)
            {
                switch (args.MsgName)
                {
                    case View.CLOSE_BTN:
                        {
                            // 还原音乐
                            GameEntry.Sound.PlayMusic(1);
                            Close();
                        }
                        break;
                }
            }
        }
        #endregion
    }
}
