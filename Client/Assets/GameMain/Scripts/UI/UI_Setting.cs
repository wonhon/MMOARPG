//===================================================
//作    者：WonHon
//创建时间：2019-08-31-18:08:38
//备    注：
//===================================================

using System;
using GameFramework.Event;
using GameFramework.Localization;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SuperBiomass
{
    [RequireComponent(typeof(UI_SettingView))]
    public class UI_Setting : UGuiForm
    {
        private UI_SettingView m_View;
        private Language m_SelectedLanguage = Language.Unspecified;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_View = GetComponent<UI_SettingView>();
            m_View.Init();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_View.OnOpen(UIForm.SerialId);

            m_View.OnRefreshMusicMute(!GameEntry.Sound.IsMuted("Music"));
            m_View.OnRefreshMusicVolume(GameEntry.Sound.GetVolume("Music"));

            m_View.OnRefreshSoundMute(!GameEntry.Sound.IsMuted("Sound"));
            m_View.OnRefreshSoundVolume(GameEntry.Sound.GetVolume("Sound"));

            m_View.OnRefreshUISoundMute(!GameEntry.Sound.IsMuted("UISound"));
            m_View.OnRefreshUISoundVolume(GameEntry.Sound.GetVolume("UISound"));

            m_SelectedLanguage = GameEntry.Localization.Language;
            m_View.OnRefreshLanguage(m_SelectedLanguage);

            AddInnerEvent();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            m_View.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            RemoveInnerEvent();

            base.OnClose(isShutdown, userData);
        }

        #region internal event
        private void AddInnerEvent()
        {
            m_View.AddListener(OnInnerEvent);
            m_View.AddListener<bool>(OnInnerBoolEvent);
            m_View.AddListener<float>(OnInnerFloatEvent);
            m_View.AddListener<Language>(OnInnerLanguaeEvent);
        }

        private void RemoveInnerEvent()
        {
            m_View.RemoveListener(OnInnerEvent);
            m_View.RemoveListener<bool>(OnInnerBoolEvent);
            m_View.RemoveListener<float>(OnInnerFloatEvent);
            m_View.RemoveListener<Language>(OnInnerLanguaeEvent);
        }

        private void OnInnerEvent(object sender, GameEventArgs e)
        {
            UIEventArgs args = e as UIEventArgs;
            if (args.SerialId == UIForm.SerialId)
            {
                switch (args.MsgName)
                {
                    case UI_SettingView.m_SUBMIT_BTN_CLICK:
                        {
                            OnSubmitButtonClick();
                        }
                        break;
                    case View.CLOSE_BTN:
                        {
                            OnCancelButtonClick();
                        }
                        break;
                }
            }
        }

        private void OnInnerBoolEvent(object sender, GameEventArgs e)
        {
            UIEventArgs<bool> args = e as UIEventArgs<bool>;
            if (args.SerialId == UIForm.SerialId)
            {
                switch (args.MsgName)
                {
                    case UI_SettingView.m_MUSIC_MUTE_BOOL:
                        {
                            OnMusicMuteChanged(args.UserData);
                        }
                        break;
                    case UI_SettingView.m_SOUND_MUTE_BOOL:
                        {
                            OnSoundMuteChanged(args.UserData);
                        }
                        break;
                    case UI_SettingView.m_UISOUND_MUTE_BOOL:
                        {
                            OnUISoundMuteChanged(args.UserData);
                        }
                        break;
                }
            }
        }

        private void OnInnerFloatEvent(object sender, GameEventArgs e)
        {
            UIEventArgs<float> args = e as UIEventArgs<float>;
            if (args.SerialId == UIForm.SerialId)
            {
                switch (args.MsgName)
                {
                    case UI_SettingView.m_MUSIC_VOLUME_FLOAT:
                        {
                            OnMusicVolumeChanged(args.UserData);
                        }
                        break;
                    case UI_SettingView.m_SOUND_VOLUME_FLOAT:
                        {
                            OnSoundVolumeChanged(args.UserData);
                        }
                        break;
                    case UI_SettingView.m_UISOUND_VOLUME_FLOAT:
                        {
                            OnUISoundVolumeChanged(args.UserData);
                        }
                        break;
                }
            }
        }

        private void OnInnerLanguaeEvent(object sender, GameEventArgs e)
        {
            UIEventArgs<Language> args = e as UIEventArgs<Language>;
            if (args.SerialId == UIForm.SerialId)
            {
                switch (args.MsgName)
                {
                    case UI_SettingView.m_LANGUAGE_CHANGE:
                        {
                            OnLanguaeSelected(args.UserData);
                        }
                        break;
                }
            }
        }
        #endregion

        public void OnMusicMuteChanged(bool isOn)
        {
            PlayUISound(10001);
            GameEntry.Sound.Mute("Music", !isOn);
        }

        public void OnMusicVolumeChanged(float volume)
        {
            GameEntry.Sound.SetVolume("Music", volume);
        }

        public void OnSoundMuteChanged(bool isOn)
        {
            PlayUISound(10001);
            GameEntry.Sound.Mute("Sound", !isOn);
        }

        public void OnSoundVolumeChanged(float volume)
        {
            GameEntry.Sound.SetVolume("Sound", volume);
        }

        public void OnUISoundMuteChanged(bool isOn)
        {
            PlayUISound(10001);
            GameEntry.Sound.Mute("UISound", !isOn);
        }

        public void OnUISoundVolumeChanged(float volume)
        {
            GameEntry.Sound.SetVolume("UISound", volume);
        }

        public void OnLanguaeSelected(Language language)
        {
            if (m_SelectedLanguage != language)
            {
                PlayUISound(10001);
                m_SelectedLanguage = language;
            }
        }

        private void OnCancelButtonClick()
        {
            PlayUISound(10001);
            Close();
        }

        public void OnSubmitButtonClick()
        {
            PlayUISound(10001);
            if (m_SelectedLanguage == GameEntry.Localization.Language)
            {
                Close();
                return;
            }

            GameEntry.Setting.SetString(Constant.Setting.Language, m_SelectedLanguage.ToString());
            GameEntry.Setting.Save();

            GameEntry.Sound.StopMusic();
            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Restart);
        }
    }
}
