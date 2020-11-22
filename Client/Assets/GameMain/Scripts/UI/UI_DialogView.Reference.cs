//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改
// 生成时间：2019-09-03-23:28:52
//------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime.UI;

namespace SuperBiomass
{
    sealed partial class UI_DialogView
    {
        [SerializeField]
        private GameObject[] m_ButtonGroupGo;

        [SerializeField]
        private TextT m_TitleTxt;

        [SerializeField]
        private TextT m_MessageTxt;

        [SerializeField]
        private TextT[] m_ConfirmTextTxt;

        [SerializeField]
        private TextT[] m_CancelTextTxt;

        [SerializeField]
        private TextT[] m_OtherTextTxt;

        [SerializeField]
        private ButtonT[] m_ConfirmBtn;

        [SerializeField]
        private ButtonT[] m_CancelBtn;

        [SerializeField]
        private ButtonT[] m_OtherBtn;

#if UNITY_EDITOR
        [ContextMenu("添加引用")]
        public void Run()
        {
            InitReference();
        }
#endif

		private void InitReference()
        {
            m_ButtonGroupGo = new GameObject[3];
            m_ButtonGroupGo[0] = transform.Find("Mask/Background/go[0]_ButtonGroup").gameObject;
            m_ButtonGroupGo[1] = transform.Find("Mask/Background/go[1]_ButtonGroup").gameObject;
            m_ButtonGroupGo[2] = transform.Find("Mask/Background/go[2]_ButtonGroup").gameObject;
            m_TitleTxt = transform.Find("Mask/Background/TitleBackground/txt_Title").GetComponent<TextT>();
            m_MessageTxt = transform.Find("Mask/Background/txt_Message").GetComponent<TextT>();
            m_ConfirmTextTxt = new TextT[3];
            m_ConfirmTextTxt[0] = transform.Find("Mask/Background/go[0]_ButtonGroup/btn[0]_Confirm/txt[0]_ConfirmText").GetComponent<TextT>();
            m_ConfirmTextTxt[1] = transform.Find("Mask/Background/go[1]_ButtonGroup/btn[1]_Confirm/txt[1]_ConfirmText").GetComponent<TextT>();
            m_ConfirmTextTxt[2] = transform.Find("Mask/Background/go[2]_ButtonGroup/btn[2]_Confirm/txt[2]_ConfirmText").GetComponent<TextT>();
            m_CancelTextTxt = new TextT[2];
            m_CancelTextTxt[0] = transform.Find("Mask/Background/go[1]_ButtonGroup/btn[0]_Cancel/txt[0]_CancelText").GetComponent<TextT>();
            m_CancelTextTxt[1] = transform.Find("Mask/Background/go[2]_ButtonGroup/btn[1]_Cancel/txt[1]_CancelText").GetComponent<TextT>();
            m_OtherTextTxt = new TextT[1];
            m_OtherTextTxt[0] = transform.Find("Mask/Background/go[2]_ButtonGroup/btn[0]_Other/txt[0]_OtherText").GetComponent<TextT>();
            m_ConfirmBtn = new ButtonT[3];
            m_ConfirmBtn[0] = transform.Find("Mask/Background/go[0]_ButtonGroup/btn[0]_Confirm").GetComponent<ButtonT>();
            m_ConfirmBtn[1] = transform.Find("Mask/Background/go[1]_ButtonGroup/btn[1]_Confirm").GetComponent<ButtonT>();
            m_ConfirmBtn[2] = transform.Find("Mask/Background/go[2]_ButtonGroup/btn[2]_Confirm").GetComponent<ButtonT>();
            m_CancelBtn = new ButtonT[2];
            m_CancelBtn[0] = transform.Find("Mask/Background/go[1]_ButtonGroup/btn[0]_Cancel").GetComponent<ButtonT>();
            m_CancelBtn[1] = transform.Find("Mask/Background/go[2]_ButtonGroup/btn[1]_Cancel").GetComponent<ButtonT>();
            m_OtherBtn = new ButtonT[1];
            m_OtherBtn[0] = transform.Find("Mask/Background/go[2]_ButtonGroup/btn[0]_Other").GetComponent<ButtonT>();
        }
    }
}