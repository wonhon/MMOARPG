using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime.UI;

//===================================================
//作    者：WonHon
//创建时间：2019-08-18-17:15:28
//备    注：根据预制件生成UI引用脚本
// 预制件名规则：
// UI_'Name'
// 引用物体命名规则：
// go__'Name'<GameObject>, 
// img_'Name'<Image>,
// txt_'Name'<Text>,
// img_'Name'<Image>,
// btn_'Name'<Button>,
// tog_'Name'<Toggle>,
// slider_'Name'<Slider>,
// input_'Name'<InputFiled>
//===================================================

namespace UnityGameFramework.Editor.UI
{
    class GeneralUIScript
    {
        struct UIItemInfo
        {
            public bool IsArray;
            public List<string> ItemPath;

            public UIItemInfo(bool isArray)
            {
                IsArray = isArray;
                ItemPath = new List<string>();
            }

            public void AddPath(string path)
            {
                ItemPath.Add(path);
            }

            public override string ToString()
            {
                return string.Format("ItemPath:{0}", ItemPath);
            }
        }

        private static readonly Dictionary<string, string> m_Regular = new Dictionary<string, string>
        {
            {"go","GameObject"},
            {"tsf", "Transform"},
            {"rectTsf", "RectTransform"},
            {"img","ImageT"},
            {"txt","TextT"},
            {"btn","ButtonT"},
            {"tog","ToggleT"},
            {"slider","SliderT" },
            {"input","InputFieldT"},
            {"togg", "ToggleGroupT"},
        };
        private static readonly string s_ASSET_PATH = "GameframeworkExtension/Config/";
        private static readonly string s_NAMESPACE = "SuperBiomass";
        private static readonly string s_AUTHOR = "WonHon";

        private static Dictionary<string, Dictionary<string, UIItemInfo>> m_ItemInfos = new Dictionary<string, Dictionary<string, UIItemInfo>>();
        private static StringBuilder m_FileConent;

        [MenuItem("Game Framework/UITools/General UI Reference Script", false, 0)]
        public static void Run()
        {
            if (m_FileConent == null)
                m_FileConent = new StringBuilder(4 * 1024 * 1024);

            m_ItemInfos.Clear();
            foreach (var data in m_Regular)
            {
                m_ItemInfos.Add(data.Key, new Dictionary<string, UIItemInfo>());
            }

            ForeachTransform(Selection.activeGameObject.transform);
            GeneralViewFile(string.Format("{0}View", Selection.activeGameObject.name));
            GeneralViewReferenceFile(string.Format("{0}View", Selection.activeGameObject.name));
            GeneralFile(Selection.activeGameObject.name);

            m_FileConent.Clear();
        }

        private static void ForeachTransform(Transform transform)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                CheckType(transform.GetChild(i));
            }
        }

        private static void CheckType(Transform child)
        {
            GameObject childGo = child.gameObject;
            string[] names = childGo.name.Split('_');
            if (names.Length >= 2)
            {
                string key = IsKey(names[0]);
                bool isArray = IsArray(names[0]);

                if (!string.IsNullOrEmpty(key))
                {
                    SetPath(key, isArray, childGo);
                }
            }

            ForeachTransform(child);
        }

        private static string IsKey(string str)
        {
            foreach (var key in m_ItemInfos.Keys)
            {
                if (str.Contains(key))
                    return key;
            }

            return string.Empty;
        }

        private static bool IsArray(string str)
        {
            foreach (var key in m_ItemInfos.Keys)
            {
                if (str.Contains(key))
                {
                    return !str.Equals(key);
                }
            }

            return true;
        }

        private static string SetFirstCharUpper(string str)
        {
            return string.Format("{0}{1}", str.Substring(0, 1).ToUpper(), str.Substring(1));
        }

        private static void SetPath(string key, bool isArray, GameObject gameObject)
        {
            string name = gameObject.name;
            string suffix = SetFirstCharUpper(key);

            StringBuilder sb = new StringBuilder();
            sb.Append(name);
            Transform parent = gameObject.transform.parent;
            while (parent != null && parent.parent != null)
            {
                sb.Insert(0, string.Format("{0}/", parent.gameObject.name));
                parent = parent.parent;
            }

            //去除标识部分,然后把首字母大写
            name = name.Split('_')[1];
            name = SetFirstCharUpper(name);
            string itemName = string.Format("m_{0}{1}", name, suffix);
            if (m_ItemInfos.ContainsKey(key))
            {
                if (!m_ItemInfos[key].ContainsKey(itemName))
                {
                    m_ItemInfos[key][itemName] = new UIItemInfo(isArray);
                }

                var info = m_ItemInfos[key][itemName];
                info.AddPath(sb.ToString());
                Debug.Log(info.ToString());
            }
        }

        private static string GetAttributeName(string fullName)
        {
            string flag = fullName.Split('_')[0];
            string attrName = fullName.Substring(flag.Length + 1);

            return attrName;
        }

        #region ViewScript
        private static void GeneralViewFile(string className)
        {
            string text = ReadUIScriptTemplate("UIScriptViewTemplate.txt");
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            text = text.Replace("#AUTHOR#", s_AUTHOR);
            text = text.Replace("#DATATIME#", System.DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));
            text = text.Replace("#NAMESPACE#", s_NAMESPACE);
            text = text.Replace("#CLASSNAME#", className);

            CreateUIScriptAsset(text, className, false);
        }
        #endregion

        #region View Reference Script
        private static void GeneralViewReferenceFile(string className)
        {
            string text = ReadUIScriptTemplate("UIScriptReferenceTemplate.txt");
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            text = text.Replace("#DATATIME#", System.DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));
            text = text.Replace("#NAMESPACE#", s_NAMESPACE);
            text = text.Replace("#CLASSNAME#", className);

            // Property
            m_FileConent.Clear();
            int index = 0;
            foreach (var data in m_ItemInfos)
            {
                index++;
                GeneralProperty(m_Regular[data.Key], data.Value, m_FileConent, index < m_ItemInfos.Count);
            }
            text = text.Replace("#CONTENT#", m_FileConent.ToString().Trim());

            // Init Property
            m_FileConent.Clear();
            foreach (var data in m_ItemInfos)
            {
                GeneralPropertyInInit(m_Regular[data.Key], data.Value, m_FileConent);
            }
            text = text.Replace("#INIT_REFERENCE#", m_FileConent.ToString().Trim());

            // Create Script
            CreateUIScriptAsset(text, string.Format("{0}.Reference", className), true);
        }

        private static void GeneralProperty(string componentName, Dictionary<string, UIItemInfo> infos, StringBuilder sb, bool isAppendLinebreak)
        {
            int index = 0;
            foreach (var info in infos)
            {
                string itemName = info.Key;
                string cn = componentName;
                if (info.Value.IsArray)
                {
                    cn = string.Format("{0}[]", componentName);
                }
                AppendLineEx(sb, "        [SerializeField]");
                AppendLineEx(sb, "        private {0} {1};", cn, itemName);
                if (isAppendLinebreak || index < infos.Count - 1)
                {
                    sb.AppendLine();
                }

                index++;
            }
        }

        private static void GeneralPropertyInInit(string componentName, Dictionary<string, UIItemInfo> infos, StringBuilder sb)
        {
            foreach (var info in infos)
            {
                string itemName = info.Key;
                if (info.Value.IsArray)
                {
                    for (int i = 0; i < info.Value.ItemPath.Count; i++)
                    {
                        if (i == 0)
                        {
                            AppendLineEx(sb, "            {0} = new {1}[{2}];", itemName, componentName, info.Value.ItemPath.Count);
                        }

                        if (componentName.Equals("GameObject"))
                        {
                            AppendLineEx(sb, "            {0}[{1}] = transform.Find(\"{2}\").gameObject;", itemName, i, info.Value.ItemPath[i]);
                        }
                        else if (componentName.Equals("Transform"))
                        {
                            AppendLineEx(sb, "            {0}[{1}] = transform.Find(\"{2}\");", itemName, i, info.Value.ItemPath[i]);
                        }
                        else
                        {
                            AppendLineEx(sb, "            {0}[{1}] = transform.Find(\"{2}\").GetComponent<{3}>();", itemName, i, info.Value.ItemPath[i], componentName);
                        }
                    }
                }
                else
                {
                    if (componentName.Equals("GameObject"))
                    {
                        AppendLineEx(sb, "            {0} = transform.Find(\"{1}\").gameObject;", itemName, info.Value.ItemPath[0]);
                    }
                    else if (componentName.Equals("Transform"))
                    {
                        AppendLineEx(sb, "            {0} = transform.Find(\"{1}\");", itemName, info.Value.ItemPath[0]);
                    }
                    else
                    {
                        AppendLineEx(sb, "            {0} = transform.Find(\"{1}\").GetComponent<{2}>();", itemName, info.Value.ItemPath[0], componentName);
                    }
                }
            }
        }
        #endregion

        #region Script
        private static void GeneralFile(string className)
        {
            string text = ReadUIScriptTemplate("UIScript.txt");
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            text = text.Replace("#AUTHOR#", s_AUTHOR);
            text = text.Replace("#DATATIME#", System.DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));
            text = text.Replace("#NAMESPACE#", s_NAMESPACE);
            text = text.Replace("#CLASSNAME#", className);

            CreateUIScriptAsset(text, className, false);
        }
        #endregion

        private static void AppendLineEx(StringBuilder sb, string format, params object[] args)
        {
            sb.AppendLine(string.Format(format, args));
        }

        private static string ReadUIScriptTemplate(string fileName)
        {
            string templatePath = Path.Combine(Application.dataPath, s_ASSET_PATH, fileName);
            string text = string.Empty;
            using (StreamReader streamReader = new StreamReader(templatePath))
            {
                text = streamReader.ReadToEnd();
            }
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return text;
        }

        private static void CreateUIScriptAsset(string content, string className, bool force)
        {
            string fileName = string.Format("{0}.cs", className);
            string savePath = Path.Combine(Application.dataPath, string.Format("GameMain/Scripts/UI/{0}", fileName));

            if (!force)
            {
                if (File.Exists(savePath))
                {
                    return;
                }
            }

            bool encoderShouldEmitUTF8Identifier = true; //参数指定是否提供 Unicode 字节顺序标记
            bool throwOnInvalidBytes = false;//是否在检测到无效的编码时引发异常
            UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
            bool append = false;
            //写入文件
            using (StreamWriter streamWriter = new StreamWriter(savePath, append, encoding))
            {
                streamWriter.Write(content);
            }
            //刷新资源管理器
            AssetDatabase.ImportAsset(savePath);
            AssetDatabase.Refresh();
        }

        private void OnDestroy()
        {
            m_ItemInfos.Clear();
        }
    }

    class ReplaceUnityUIComponent
    {
        [MenuItem("Game Framework/UITools/New Unity UI Component", false, 1)]
        public static void Run()
        {
            GameObject ui = new GameObject();
            GameObject ui_t = new GameObject();

            Replace<Text, TextT>(ui.transform, ui_t.transform);
            Replace<Image, ImageT>(ui.transform, ui_t.transform);
            Replace<RawImage, RawImageT>(ui.transform, ui_t.transform);
            Replace<Mask, MaskT>(ui.transform, ui_t.transform);
            Replace<RectMask2D, RectMask2DT>(ui.transform, ui_t.transform);
            Replace<Button, ButtonT>(ui.transform, ui_t.transform);
            Replace<InputField, InputFieldT>(ui.transform, ui_t.transform);
            Replace<Toggle, ToggleT>(ui.transform, ui_t.transform);
            Replace<ToggleGroup, ToggleGroupT>(ui.transform, ui_t.transform);
            Replace<Slider, SliderT>(ui.transform, ui_t.transform);
            Replace<Scrollbar, ScrollbarT>(ui.transform, ui_t.transform);
            Replace<Dropdown, DropdownT>(ui.transform, ui_t.transform);
            Replace<ScrollRect, ScrollRectT>(ui.transform, ui_t.transform);
            Replace<Selectable, SelectableT>(ui.transform, ui_t.transform);
            Replace<Outline, OutLineT>(ui.transform, ui_t.transform);
            Replace<Shadow, ShadowT>(ui.transform, ui_t.transform);

            PrefabUtility.SaveAsPrefabAsset(ui, Path.Combine(Application.dataPath, "GameframeworkExtension/Prefab/Template.prefab"));
            PrefabUtility.SaveAsPrefabAsset(ui_t, Path.Combine(Application.dataPath, "GameframeworkExtension/Prefab/Template_T.prefab"));
            Object.DestroyImmediate(ui);
            Object.DestroyImmediate(ui_t);
            AssetDatabase.Refresh();
        }

        private static void Replace<T, T1>(Transform parent, Transform parent_t) where T1 : T where T : UIBehaviour
        {
            GameObject go = new GameObject(typeof(T).Name);
            GameObject go_t = new GameObject(typeof(T).Name);
            go.AddComponent<T>();
            go.transform.SetParent(parent);
            go_t.AddComponent<T1>();
            go_t.transform.SetParent(parent_t);
        }
    }

    class ReplaceUnityUIComponentInFiles
    {
        private static Dictionary<string, string> m_PrefabDic;
        private static Dictionary<string, string> m_Prefab_T_Dic;

        private static int m_Step;
        private static string m_Key;
        private static List<string> content = new List<string>(1024 * 4);

        [MenuItem("Game Framework/UITools/Replace Unity UI Component In Files", false, 2)]
        public static void Run()
        {
            if (m_PrefabDic == null)
            {
                m_PrefabDic = new Dictionary<string, string>();
            }
            else
            {
                m_PrefabDic.Clear();
            }

            if (m_Prefab_T_Dic == null)
            {
                m_Prefab_T_Dic = new Dictionary<string, string>();
            }
            else
            {
                m_Prefab_T_Dic.Clear();
            }

            CollectPrefabInfo(m_PrefabDic, Path.Combine(Application.dataPath, "GameframeworkExtension/Prefab/Template.prefab"));
            CollectPrefabInfo(m_Prefab_T_Dic, Path.Combine(Application.dataPath, "GameframeworkExtension/Prefab/Template_T.prefab"));
            ReplaceAllUIPrefab(Path.Combine(Application.dataPath, "GameMain/Download/UI/UIForms"));

            AssetDatabase.Refresh();
        }

        private static void CollectPrefabInfo(Dictionary<string, string> dic, string path)
        {
            m_Step = 0;

            bool encoderShouldEmitUTF8Identifier = true; //参数指定是否提供 Unicode 字节顺序标记
            bool throwOnInvalidBytes = false;//是否在检测到无效的编码时引发异常
            UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
            using (StreamReader sr = new StreamReader(path, encoding))
            {
                string line = string.Empty;
                while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                {
                    Readline(dic, line);
                }
            }
        }

        private static void Readline(Dictionary<string, string> dic, string line)
        {
            //GameObject:
            if (m_Step == 0)
            {
                if (line.Equals("GameObject:"))
                {
                    m_Step = 1;
                }
            }

            if (m_Step == 1)
            {
                //m_Name: ScrollRect
                if (line.Contains("m_Name: "))
                {
                    if (line.Contains("m_Name: Template") || line.Contains("m_Name: Template_T"))
                    {
                        m_Step = 0;
                        m_Key = string.Empty;
                    }
                    else
                    {
                        m_Step = 2;
                        m_Key = line;
                    }
                }
            }

            if (m_Step == 2)
            {
                //m_Script: {fileID: 11500000, guid: cd46f7417599a5a47a6dca93e891bfd6, type: 3}
                if (line.Contains("m_Script: "))
                {
                    m_Step = 0;
                    if (!string.IsNullOrEmpty(m_Key))
                    {
                        dic[m_Key] = line;
                    }
                }
            }
        }

        private static void ReplaceAllUIPrefab(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] files = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i];
                if (file.Name.EndsWith(".prefab"))
                {
                    int lineCount = 0;
                    //读取文件
                    using (FileStream fileStream = file.OpenRead())
                    {
                        using (StreamReader sr = new StreamReader(fileStream))
                        {
                            content.Clear();
                            string line = string.Empty;
                            do
                            {
                                line = sr.ReadLine();
                                if (!string.IsNullOrEmpty(line))
                                {
                                    foreach (var data in m_PrefabDic)
                                    {
                                        if (data.Value.Equals(line))
                                        {
                                            line = m_Prefab_T_Dic[data.Key];
                                            break;
                                        }
                                    }
                                }
                                lineCount++;
                                content.Add(line);
                            }
                            while (!string.IsNullOrEmpty(line));
                        }
                    }

                    using (FileStream fileStream = file.Open(FileMode.Truncate, FileAccess.Write))
                    {
                        //写入文件
                        using (StreamWriter sw = new StreamWriter(fileStream))
                        {
                            for (int j = 0; j < lineCount; j++)
                            {
                                sw.WriteLine(content[j]);
                            }
                        }
                    }
                }
            }
        }
    }
}
