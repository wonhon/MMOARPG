﻿using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace SuperBiomass.Editor.GeneralScrptTools
{
    public class CustomScriptTemplate
    {
        //菜单路径，是否为验证方法，菜单项优先级
        [MenuItem("Assets/Create/My Lua Script", false, 80)]
        //方法必须为静态方法
        public static void CreateLua()
        {
            CreateEvent("/NewLuaScript.lua", "Assets/GameMain/Scripts/Editor/TemplateScript/My LuaScript.txt");
        }

        [MenuItem("Assets/Create/My c#Script", false, 70)]
        public static void CreateEventCS()
        {
            CreateEvent("/NewCSharpScript.cs", "Assets/GameMain/Scripts/Editor/TemplateScript/My C#Script.txt");
        }

        private static void CreateEvent(string defaultFileName, string templateFilePath)
        {
            //将设置焦点到某文件并进入重命名
            //参数为传递给CreateEventCSScriptAsset类action方法的参数
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
               ScriptableObject.CreateInstance<CreateEventCSScriptAsset>(),
               GetSelectPathOrFallback() + defaultFileName, null, templateFilePath);
        }

        //取得要创建文件的路径
        public static string GetSelectPathOrFallback()
        {
            string path = "Assets";
            //遍历选中的资源以获得路径
            //Selection.GetFiltered是过滤选择文件或文件夹下的物体，assets表示只返回选择对象本身
            foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                    break;
                }
            }
            return path;
        }
    }

    //要创建模板文件必须继承EndNameEditAction，重写action方法
    class CreateEventCSScriptAsset : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            //创建资源
            Object obj = CreateScriptAssetFromTemplate(pathName, resourceFile);
            ProjectWindowUtil.ShowCreatedAsset(obj);//高亮显示资源
        }

        internal static Object CreateScriptAssetFromTemplate(string pathName, string resourceFile)
        {
            //获取要创建资源的绝对路径
            string fullPath = Path.GetFullPath(pathName);
            //读取本地的模板文件
            StreamReader streamReader = new StreamReader(resourceFile);
            string text = streamReader.ReadToEnd();
            streamReader.Close();
            //获取文件名，不含扩展名
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);
            Debug.Log("text===" + text);

            //将模板类中的类名替换成你创建的文件名
            text = text.Replace("#SCRIPTNAME#", fileNameWithoutExtension);
            text = text.Replace("#Author#", "WonHon");
            text = text.Replace("#CreateTime#", System.DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));
            bool encoderShouldEmitUTF8Identifier = true; //参数指定是否提供 Unicode 字节顺序标记
            bool throwOnInvalidBytes = false;//是否在检测到无效的编码时引发异常
            UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
            bool append = false;
            //写入文件
            StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
            streamWriter.Write(text);
            streamWriter.Close();
            //刷新资源管理器
            AssetDatabase.ImportAsset(pathName);
            AssetDatabase.Refresh();
            return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
        }
    }

    class CreateLuaScriptAsset : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            Object obj = CreateScriptAssetFromTemplate(pathName, resourceFile);
            ProjectWindowUtil.ShowCreatedAsset(obj);
        }

        internal static Object CreateScriptAssetFromTemplate(string pathName, string resourceFile)
        {
            string fullPath = Path.GetFullPath(pathName);
            StreamReader streamReader = new StreamReader(resourceFile);
            string text = streamReader.ReadToEnd();
            streamReader.Close();
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);
            Debug.Log("text===" + text);
            text = Regex.Replace(text, "LuaClass", fileNameWithoutExtension);
            bool encoderShouldEmitUTF8Identifier = true;
            bool throwOnInvalidBytes = false;
            UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
            bool append = false;
            StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
            streamWriter.Write(text);
            streamWriter.Close();
            AssetDatabase.ImportAsset(pathName);//导入指定路径下的资源
            return AssetDatabase.LoadAssetAtPath(pathName, typeof(Object));//返回指定路径下的所有Object对象
        }
    }
}


