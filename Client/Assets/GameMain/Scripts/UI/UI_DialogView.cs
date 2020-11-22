//===================================================
//作    者：WonHon
//创建时间：2019-09-01-20:13:23
//备    注：
//===================================================

using UnityEngine;

namespace SuperBiomass
{
    sealed partial class UI_DialogView : View
    {
        internal const string m_CONFIRM_BTN_CLICK = "Confirm Button Click";
        internal const string m_CANCEL_BTN_CLICK = "Cancel Button Click";
        internal const string m_OTHER_BTN_CLICK = "Other Button Click";

        public void Init()
        {
            //InitReference();

            for (int i = 0; i < m_ConfirmBtn.Length; i++)
            {
                m_ConfirmBtn[i].onClick.AddListener(OnConfirmButtonClick);
            }

            for (int i = 0; i < m_CancelBtn.Length; i++)
            {
                m_CancelBtn[i].onClick.AddListener(OnCancelButtonClick);
            }

            for (int i = 0; i < m_OtherBtn.Length; i++)
            {
                m_OtherBtn[i].onClick.AddListener(OnOtherButtonClick);
            }
        }

        #region event
        private void OnConfirmButtonClick()
        {
            Fire(m_CONFIRM_BTN_CLICK);
        }

        private void OnCancelButtonClick()
        {
            Fire(m_CANCEL_BTN_CLICK);
        }

        private void OnOtherButtonClick()
        {
            Fire(m_OTHER_BTN_CLICK);
        }
        #endregion

        #region refresh ui
        public void OnRefreshTitle(string text)
        {
            m_TitleTxt.text = text;
        }

        public void OnRefreshMessage(string text)
        {
            m_MessageTxt.text = text;
        }

        public void RefreshDialogMode(int dialogMode)
        {
            for (int i = 1; i <= m_ButtonGroupGo.Length; i++)
            {
                SetActive(m_ButtonGroupGo[i - 1], i == dialogMode);
            }
        }

        public void RefreshConfirmText(string confirmText)
        {
            if (string.IsNullOrEmpty(confirmText))
            {
                confirmText = GameEntry.Localization.GetString("Dialog.ConfirmButton");
            }

            for (int i = 0; i < m_ConfirmTextTxt.Length; i++)
            {
                m_ConfirmTextTxt[i].text = confirmText;
            }
        }

        public void RefreshCancelText(string cancelText)
        {
            if (string.IsNullOrEmpty(cancelText))
            {
                cancelText = GameEntry.Localization.GetString("Dialog.CancelButton");
            }

            for (int i = 0; i < m_CancelTextTxt.Length; i++)
            {
                m_CancelTextTxt[i].text = cancelText;
            }
        }

        public void RefreshOtherText(string otherText)
        {
            if (string.IsNullOrEmpty(otherText))
            {
                otherText = GameEntry.Localization.GetString("Dialog.OtherButton");
            }

            for (int i = 0; i < m_OtherTextTxt.Length; i++)
            {
                m_OtherTextTxt[i].text = otherText;
            }
        }
        #endregion
    }
}
