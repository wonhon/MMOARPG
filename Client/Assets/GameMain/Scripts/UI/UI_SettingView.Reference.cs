//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改
// 生成时间：2019-09-03-23:28:28
//------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime.UI;

namespace SuperBiomass
{
    sealed partial class UI_SettingView
    {
        [SerializeField]
        private ButtonT m_ConfirmBtn;

        [SerializeField]
        private ButtonT m_CancelBtn;

        [SerializeField]
        private ToggleT m_MusicMuteTog;

        [SerializeField]
        private ToggleT m_SoundMuteTog;

        [SerializeField]
        private ToggleT m_UISoundMuteTog;

        [SerializeField]
        private ToggleT m_EnglishTog;

        [SerializeField]
        private ToggleT m_ChineseSimplifiedTog;

        [SerializeField]
        private ToggleT m_ChineseTraditionalTog;

        [SerializeField]
        private ToggleT m_KoreanTog;

        [SerializeField]
        private SliderT m_MusicVolumeSlider;

        [SerializeField]
        private SliderT m_SoundVolumeSlider;

        [SerializeField]
        private SliderT m_UISoundVolumeSlider;

#if UNITY_EDITOR
        [ContextMenu("添加引用")]
        public void Run()
        {
            InitReference();
        }
#endif

		private void InitReference()
        {
            m_ConfirmBtn = transform.Find("Mask/Background/Buttons/btn_Confirm").GetComponent<ButtonT>();
            m_CancelBtn = transform.Find("Mask/Background/Buttons/btn_Cancel").GetComponent<ButtonT>();
            m_MusicMuteTog = transform.Find("Mask/Background/Music/tog_MusicMute").GetComponent<ToggleT>();
            m_SoundMuteTog = transform.Find("Mask/Background/Sound/tog_SoundMute").GetComponent<ToggleT>();
            m_UISoundMuteTog = transform.Find("Mask/Background/UISound/tog_UISoundMute").GetComponent<ToggleT>();
            m_EnglishTog = transform.Find("Mask/Background/Languages/tog_English").GetComponent<ToggleT>();
            m_ChineseSimplifiedTog = transform.Find("Mask/Background/Languages/tog_ChineseSimplified").GetComponent<ToggleT>();
            m_ChineseTraditionalTog = transform.Find("Mask/Background/Languages/tog_ChineseTraditional").GetComponent<ToggleT>();
            m_KoreanTog = transform.Find("Mask/Background/Languages/tog_Korean").GetComponent<ToggleT>();
            m_MusicVolumeSlider = transform.Find("Mask/Background/Music/slider_MusicVolume").GetComponent<SliderT>();
            m_SoundVolumeSlider = transform.Find("Mask/Background/Sound/slider_SoundVolume").GetComponent<SliderT>();
            m_UISoundVolumeSlider = transform.Find("Mask/Background/UISound/slider_UISoundVolume").GetComponent<SliderT>();
        }
    }
}