//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;

namespace SuperBiomass
{
    public static class AssetUtility
    {
        private static string m_Root = "Assets/GameMain/Down";

        public static string GetConfigAsset(string assetName, LoadType loadType)
        {
            return Utility.Text.Format("{0}/Configs/{1}.{2}", m_Root, assetName, loadType == LoadType.Text ? "txt" : "bytes");
        }

        public static string GetDataTableAsset(string assetName, LoadType loadType)
        {
            return Utility.Text.Format("{0}/DataTables/{1}.{2}", m_Root, assetName, loadType == LoadType.Text ? "txt" : "bytes");
        }

        public static string GetDictionaryAsset(string assetName, LoadType loadType)
        {
            return Utility.Text.Format("{0}/Localization/{1}/Dictionaries/{2}.{3}", m_Root, GameEntry.Localization.Language.ToString(), assetName, loadType == LoadType.Text ? "xml" : "bytes");
        }

        public static string GetFontAsset(string assetName)
        {
            return Utility.Text.Format("{0}/Fonts/{1}.ttf", m_Root, assetName);
        }

        public static string GetSceneAsset(string assetName)
        {
            return Utility.Text.Format("{0}/Scenes/{1}.unity", m_Root, assetName);
        }

        public static string GetMusicAsset(string assetName)
        {
            return Utility.Text.Format("{0}/Music/{1}.mp3", m_Root, assetName);
        }

        public static string GetSoundAsset(string assetName)
        {
            return Utility.Text.Format("{0}/Sounds/{1}.wav", m_Root, assetName);
        }

        public static string GetEntityAsset(string assetName)
        {
            return Utility.Text.Format("{0}/Entities/{1}.prefab", m_Root, assetName);
        }

        public static string GetUIFormAsset(string assetName)
        {
            return Utility.Text.Format("{0}/UI/UIForms/{1}.prefab", m_Root, assetName);
        }

        public static string GetUISoundAsset(string assetName)
        {
            return Utility.Text.Format("{0}/UI/UISounds/{1}.wav", m_Root, assetName);
        }
    }
}
