//===================================================
//作    者：WonHon
//创建时间：2019-09-01-20:13:24
//备    注：
//===================================================

using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework;
using GameFramework.Event;

namespace SuperBiomass
{
    [RequireComponent(typeof(UI_DialogView))]
    sealed class UI_Dialog : UGuiForm
    {
        private UI_DialogView m_View;

        private int m_DialogMode = 1;
        private bool m_PauseGame = false;
        private object m_UserData = null;
        private GameFrameworkAction<object> m_OnClickConfirm = null;
        private GameFrameworkAction<object> m_OnClickCancel = null;
        private GameFrameworkAction<object> m_OnClickOther = null;

        public int DialogMode
        {
            get
            {
                return m_DialogMode;
            }
        }

        public bool PauseGame
        {
            get
            {
                return m_PauseGame;
            }
        }

        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }

        public void OnConfirmButtonClick()
        {
            PlayUISound(10001);
            if (m_OnClickConfirm != null)
            {
                m_OnClickConfirm(m_UserData);
            }

            Close();
        }

        public void OnCancelButtonClick()
        {
            PlayUISound(10001);
            if (m_OnClickCancel != null)
            {
                m_OnClickCancel(m_UserData);
            }

            Close();
        }

        public void OnOtherButtonClick()
        {
            PlayUISound(10001);
            if (m_OnClickOther != null)
            {
                m_OnClickOther(m_UserData);
            }

            Close();
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_View = GetComponent<UI_DialogView>();
            m_View.Init();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_View.OnOpen(UIForm.SerialId);

            DialogParams dialogParams = (DialogParams)userData;
            if (dialogParams == null)
            {
                Log.Warning("DialogParams is invalid.");
                return;
            }

            m_DialogMode = dialogParams.Mode;
            m_View.RefreshDialogMode(dialogParams.Mode);

            m_View.OnRefreshTitle(dialogParams.Title);
            m_View.OnRefreshMessage(dialogParams.Message);

            m_PauseGame = dialogParams.PauseGame;
            RefreshPauseGame();

            m_UserData = dialogParams.UserData;

            m_View.RefreshConfirmText(dialogParams.ConfirmText);
            m_OnClickConfirm = dialogParams.OnClickConfirm;

            m_View.RefreshCancelText(dialogParams.CancelText);
            m_OnClickCancel = dialogParams.OnClickCancel;

            m_View.RefreshOtherText(dialogParams.OtherText);
            m_OnClickOther = dialogParams.OnClickOther;

            m_View.AddListener(OnInnerEvent);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            m_View.RemoveListener(OnInnerEvent);

            RefreshPauseGame();

            m_DialogMode = 1;

            m_View.OnRefreshTitle(string.Empty);
            m_View.OnRefreshMessage(string.Empty);
            m_PauseGame = false;
            m_UserData = null;

            m_View.RefreshConfirmText(string.Empty);
            m_OnClickConfirm = null;

            m_View.RefreshCancelText(string.Empty);
            m_OnClickCancel = null;

            m_View.RefreshOtherText(string.Empty);
            m_OnClickOther = null;

            base.OnClose(isShutdown, userData);
        }

        private void OnInnerEvent(object sender, GameEventArgs e)
        {
            UIEventArgs args = e as UIEventArgs;
            if (args.SerialId == UIForm.SerialId)
            {
                switch (args.MsgName)
                {
                    case UI_DialogView.m_CONFIRM_BTN_CLICK:
                        {
                            OnConfirmButtonClick();
                        }
                        break;
                    case UI_DialogView.m_CANCEL_BTN_CLICK:
                        {
                            OnCancelButtonClick();
                        }
                        break;
                    case UI_DialogView.m_OTHER_BTN_CLICK:
                        {
                            OnOtherButtonClick();
                        }
                        break;
                }
            }
        }

        private void RefreshPauseGame()
        {
            if (m_PauseGame)
            {
                GameEntry.Base.PauseGame();
            }
        }
    }
}
