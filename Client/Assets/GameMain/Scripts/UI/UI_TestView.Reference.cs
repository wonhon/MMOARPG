//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改
// 生成时间：2019-09-03-23:28:20
//------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime.UI;

namespace SuperBiomass
{
    sealed partial class UI_TestView
    {
        [SerializeField]
        private GameObject m_ContainerGo;

        [SerializeField]
        private ImageT m_ImageImg;

        [SerializeField]
        private TextT[] m_TextTxt;

        [SerializeField]
        private ButtonT[] m_ButtonBtn;

        [SerializeField]
        private ToggleT m_ToggleTog;

        [SerializeField]
        private SliderT m_SliderSlider;

        [SerializeField]
        private InputFieldT m_InputFieldInput;

#if UNITY_EDITOR
        [ContextMenu("添加引用")]
        public void Run()
        {
            InitReference();
        }
#endif

		private void InitReference()
        {
            m_ContainerGo = transform.Find("go_container").gameObject;
            m_ImageImg = transform.Find("img_image").GetComponent<ImageT>();
            m_TextTxt = new TextT[2];
            m_TextTxt[0] = transform.Find("txt[0]_text").GetComponent<TextT>();
            m_TextTxt[1] = transform.Find("txt[1]_text").GetComponent<TextT>();
            m_ButtonBtn = new ButtonT[2];
            m_ButtonBtn[0] = transform.Find("btn[0]_button").GetComponent<ButtonT>();
            m_ButtonBtn[1] = transform.Find("btn[1]_button").GetComponent<ButtonT>();
            m_ToggleTog = transform.Find("tog_Toggle").GetComponent<ToggleT>();
            m_SliderSlider = transform.Find("slider_Slider").GetComponent<SliderT>();
            m_InputFieldInput = transform.Find("input_InputField").GetComponent<InputFieldT>();
        }
    }
}