//===================================================
//作    者：WonHon
//创建时间：2019-08-31-18:08:37
//备    注：
//===================================================

using GameFramework.Localization;
using UnityEngine;
using UnityGameFramework.Runtime.UI;

namespace SuperBiomass
{
    sealed partial class UI_SettingView : View
    {
        [SerializeField]
        private CanvasGroup m_LanguageTipsCanvasGroup = null;

        internal const string m_MUSIC_MUTE_BOOL = "Music Mute Toggle";
        internal const string m_MUSIC_VOLUME_FLOAT = "Music Volume Slider";

        internal const string m_SOUND_MUTE_BOOL = "Sound Mute Toggle";
        internal const string m_SOUND_VOLUME_FLOAT = "Sound Volume Slider";

        internal const string m_UISOUND_MUTE_BOOL = "UISound Mute Toggle";
        internal const string m_UISOUND_VOLUME_FLOAT = "UISound Volume Slider";

        internal const string m_LANGUAGE_CHANGE = "Languae Change";

        internal const string m_SUBMIT_BTN_CLICK = "Submit Button Click";

        public void Init()
        {
            m_MusicMuteTog.onValueChanged.AddListener(OnMusicMuteChanged);
            m_MusicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

            m_SoundMuteTog.onValueChanged.AddListener(OnSoundMuteChanged);
            m_SoundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeChanged);

            m_UISoundMuteTog.onValueChanged.AddListener(OnUISoundMuteChanged);
            m_UISoundVolumeSlider.onValueChanged.AddListener(OnUISoundVolumeChanged);

            m_EnglishTog.onValueChanged.AddListener(OnEnglishSelected);
            m_ChineseSimplifiedTog.onValueChanged.AddListener(OnChineseSimplifiedSelected);
            m_ChineseTraditionalTog.onValueChanged.AddListener(OnChineseTraditionalSelected);
            m_KoreanTog.onValueChanged.AddListener(OnKoreanSelected);

            m_CancelBtn.onClick.AddListener(OnClose);
            m_ConfirmBtn.onClick.AddListener(OnSubmitButtonClick);
        }

        public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (m_LanguageTipsCanvasGroup.gameObject.activeSelf)
            {
                m_LanguageTipsCanvasGroup.alpha = 0.5f + 0.5f * Mathf.Sin(Mathf.PI * Time.time);
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
        }

        private void OnSubmitButtonClick()
        {
            Fire(m_SUBMIT_BTN_CLICK);
        }

        private void OnMusicMuteChanged(bool isOn)
        {
            Fire(m_MUSIC_MUTE_BOOL, isOn);
            SetActive(m_MusicVolumeSlider.gameObject, isOn);
        }

        private void OnMusicVolumeChanged(float volume)
        {
            Fire(m_MUSIC_VOLUME_FLOAT, volume);
        }

        private void OnSoundMuteChanged(bool isOn)
        {
            Fire(m_SOUND_MUTE_BOOL, isOn);
            SetActive(m_SoundVolumeSlider.gameObject, isOn);
        }

        private void OnSoundVolumeChanged(float volume)
        {
            Fire(m_SOUND_VOLUME_FLOAT, volume);
        }

        private void OnUISoundMuteChanged(bool isOn)
        {
            Fire(m_UISOUND_MUTE_BOOL, isOn);
            SetActive(m_UISoundVolumeSlider.gameObject, isOn);
        }

        private void OnUISoundVolumeChanged(float volume)
        {
            Fire(m_UISOUND_VOLUME_FLOAT, volume);
        }

        private void OnEnglishSelected(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            Fire(m_LANGUAGE_CHANGE, Language.English);
            RefreshLanguageTips(Language.English);
        }

        private void OnChineseSimplifiedSelected(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            Fire(m_LANGUAGE_CHANGE, Language.ChineseSimplified);
            RefreshLanguageTips(Language.ChineseSimplified);
        }

        private void OnChineseTraditionalSelected(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            Fire(m_LANGUAGE_CHANGE, Language.ChineseTraditional);
            RefreshLanguageTips(Language.ChineseTraditional);
        }

        private void OnKoreanSelected(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            Fire(m_LANGUAGE_CHANGE, Language.Korean);
            RefreshLanguageTips(Language.Korean);
        }

        private void RefreshLanguageTips(Language language)
        {
            SetActive(m_LanguageTipsCanvasGroup.gameObject, language != GameEntry.Localization.Language);
        }

        #region 刷新
        public void OnRefreshMusicMute(bool isOn)
        {
            m_MusicMuteTog.isOn = isOn;
        }

        public void OnRefreshMusicVolume(float value)
        {
            m_MusicVolumeSlider.value = value;
        }

        public void OnRefreshSoundMute(bool isOn)
        {
            m_SoundMuteTog.isOn = isOn;
        }

        public void OnRefreshSoundVolume(float value)
        {
            m_SoundVolumeSlider.value = value;
        }

        public void OnRefreshUISoundMute(bool isOn)
        {
            m_UISoundMuteTog.isOn = isOn;
        }

        public void OnRefreshUISoundVolume(float value)
        {
            m_UISoundVolumeSlider.value = value;
        }

        public void OnRefreshLanguage(Language language)
        {
            switch (language)
            {
                case Language.English:
                    m_EnglishTog.isOn = true;
                    break;
                case Language.ChineseSimplified:
                    m_ChineseSimplifiedTog.isOn = true;
                    break;
                case Language.ChineseTraditional:
                    m_ChineseTraditionalTog.isOn = true;
                    break;
                case Language.Korean:
                    m_KoreanTog.isOn = true;
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
