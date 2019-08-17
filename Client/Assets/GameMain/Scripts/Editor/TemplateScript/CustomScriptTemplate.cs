using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;
using UnityGameFramework;

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


    [MenuItem("Assets/Create/My UI Script", false, 60)]
    public static void CreateUI()
    {
        GeneralUIScript.Run();
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

/// <summary>
/// 根据预制件生成UI引用脚本
/// 预制件名规则：UI_'Name'
/// 引用物体命名规则：
/// go__'Name'<GameObject>, 
/// img_'Name'<Image>,
/// text_'Name'<Text>,
/// img_'Name'<Image>,
/// btn_'Name'<Button>,
/// tog_'Name'<Toggle>,
/// slider_'Name'<Slider>,
/// input_'Name'<InputFiled>
/// </summary>
class GeneralUIScript
{
    private static Dictionary<string, StringBuilder> m_Gameobjects;
    private static Dictionary<string, StringBuilder> m_Images;
    private static Dictionary<string, StringBuilder> m_Texts;
    private static Dictionary<string, StringBuilder> m_Buttons;
    private static Dictionary<string, StringBuilder> m_Toggles;
    private static Dictionary<string, StringBuilder> m_Sliders;
    private static Dictionary<string, StringBuilder> m_InputFields;

    private static StringBuilder m_FileConent;

    private static readonly string m_Linebreak = System.Environment.NewLine;

    public static void Run()
    {
        if (m_FileConent == null)
            m_FileConent = new StringBuilder(4 * 1024 * 1024);

        Init(ref m_Gameobjects);
        Init(ref m_Images);
        Init(ref m_Texts);
        Init(ref m_Buttons);
        Init(ref m_Toggles);
        Init(ref m_Sliders);
        Init(ref m_InputFields);

        ForeachTransform(Selection.activeGameObject.transform);
        GeneralFile(Selection.activeGameObject.name);
        CreateUIScriptAsset();

        m_FileConent.Clear();
    }

    private static void Init(ref Dictionary<string, StringBuilder> dic)
    {
        if (dic == null)
        {
            dic = new Dictionary<string, StringBuilder>();
        }
        else
        {
            dic.Clear();
        }
    }

    private static void ForeachTransform(Transform transform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            CheckType(transform.GetChild(i));
        }
    }

    private static void GeneralFile(string className)
    {
        m_FileConent.AppendFormat("//------------------------------------------------------------{0}", m_Linebreak);
        m_FileConent.AppendFormat("// 此文件由工具自动生成，请勿直接修改{0}", m_Linebreak);
        m_FileConent.AppendFormat("// 生成时间：{0}{1}", System.DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"), m_Linebreak);
        m_FileConent.AppendFormat("//------------------------------------------------------------{0}", m_Linebreak);
        m_FileConent.AppendFormat("{0}", m_Linebreak);
        m_FileConent.AppendFormat("using UnityEngine;{0}", m_Linebreak);
        m_FileConent.AppendFormat("using UnityEngine.UI;{0}", m_Linebreak);
        m_FileConent.AppendFormat("{0}", m_Linebreak);
        m_FileConent.AppendFormat("namespace SuperBiomass{0}", m_Linebreak);
        m_FileConent.AppendLine("{");

        className = className.Split('_')[1];
        m_FileConent.AppendFormat("   public partial class {0}{1}Form{2}", className.Substring(0, 1).ToUpper(), className.Substring(1), m_Linebreak);
        m_FileConent.AppendLine("   {");

        GeneralProperty(m_Gameobjects, m_FileConent, "GameObject");
        GeneralProperty(m_Images, m_FileConent, "Image");
        GeneralProperty(m_Texts, m_FileConent, "Text");
        GeneralProperty(m_Buttons, m_FileConent, "Button");
        GeneralProperty(m_Toggles, m_FileConent, "Toggle");
        GeneralProperty(m_Sliders, m_FileConent, "Slider");
        GeneralProperty(m_InputFields, m_FileConent, "InputField");

        m_FileConent.AppendLine("   }");
        m_FileConent.AppendLine("}");
    }

    private static void CreateUIScriptAsset()
    {
        string conent = m_FileConent.ToString();
        string gameobjectName = Selection.activeObject.name.Split('_')[1];
        string fileName = string.Format("{0}FormRef.cs", gameobjectName);
        string savePath = Path.Combine(Application.dataPath, string.Format("GameMain/Scripts/UI/{0}", fileName));

        bool encoderShouldEmitUTF8Identifier = true; //参数指定是否提供 Unicode 字节顺序标记
        bool throwOnInvalidBytes = false;//是否在检测到无效的编码时引发异常
        UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
        bool append = false;
        //写入文件
        StreamWriter streamWriter = new StreamWriter(savePath, append, encoding);
        streamWriter.Write(conent);
        streamWriter.Close();
        //刷新资源管理器
        AssetDatabase.ImportAsset(savePath);
        AssetDatabase.Refresh();
    }

    private static void GeneralProperty(Dictionary<string, StringBuilder> dic, StringBuilder sb, string flagName)
    {
        foreach (var fullName in dic.Keys)
        {
            string name = fullName.Split('_')[1];
            sb.AppendFormat("       [SerializeField]{0}", m_Linebreak);
            sb.AppendFormat("       private {0} m_{1}{2};{3}", flagName, name.Substring(0, 1).ToUpper(), name.Substring(1), m_Linebreak);
            sb.AppendFormat("{0}", m_Linebreak);
        }
    }

    private static void CheckType(Transform child)
    {
        GameObject childGo = child.gameObject;
        string[] names = childGo.name.Split('_');
        if (names.Length == 2)
        {
            string flag = names[0];

            if (flag.Equals("go"))
            {
                SetPath(m_Gameobjects, childGo);
            }
            else if (flag.Equals("img"))
            {
                SetPath(m_Images, childGo);
            }
            else if (flag.Equals("text"))
            {
                SetPath(m_Texts, childGo);
            }
            else if (flag.Equals("btn"))
            {
                SetPath(m_Buttons, childGo);
            }
            else if (flag.Equals("tog"))
            {
                SetPath(m_Toggles, childGo);
            }
            else if (flag.Equals("slider"))
            {
                SetPath(m_Sliders, childGo);
            }
            else if (flag.Equals("input"))
            {
                SetPath(m_InputFields, childGo);
            }
        }

        ForeachTransform(child);
    }

    private static void SetPath(Dictionary<string, StringBuilder> dic, GameObject gameObject)
    {
        string name = gameObject.name;
        if (!dic.ContainsKey(name))
        {
            dic[name] = new StringBuilder();
            dic[name].Append(name);
            Transform parent = gameObject.transform.parent;
            while (parent != null)
            {
                dic[name].Insert(0, string.Format("{0}\\", parent.gameObject.name));
                parent = parent.parent;
            }
        }

        Debug.Log(string.Format("key: {0}, value:{1}", name, dic[name]));
    }

    private void OnDestroy()
    {
        m_Gameobjects.Clear();
        m_Gameobjects = null;
        m_Images.Clear();
        m_Images = null;
        m_Texts.Clear();
        m_Texts = null;
        m_Buttons.Clear();
        m_Buttons = null;
        m_Toggles.Clear();
        m_Toggles = null;
        m_Sliders.Clear();
        m_Sliders = null;
        m_InputFields.Clear();
        m_InputFields = null;
    }
}


