using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;

namespace SuperBiomass
{
    public class SetVersion
    {
        [MenuItem("SuperBiomass/Set Version")]
        public static void Run()
        {
            VersionInfo versionInfo = new VersionInfo();
            versionInfo.ForceGameUpdate = false;
            versionInfo.LatestGameVersion = "0.1.0";
            versionInfo.GameUpdateUrl = ".apk";
            
            string data = LitJson.JsonMapper.ToJson(versionInfo);

            string path = Utility.Path.GetCombinePath(Application.dataPath, "GameMain/Configs/Version.txt");
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                byte[] bytes = Utility.Converter.GetBytes(data);
                fs.Write(bytes, 0, bytes.Length);
                fs.Flush();
                fs.Close();
            }   
        }
    }
}
