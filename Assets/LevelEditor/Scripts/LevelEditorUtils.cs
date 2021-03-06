﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using Wooga.Foundation.Json;
using LitJson;
using System.IO;


namespace CommonLevelEditor
{
    public static class LevelEditorUtils
    {
        #region string extension
        public static string UppercaseFirst(this string value)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(value[0]) + value.Substring(1);
        }

        public static string LowercaseFirst(this string value)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToLower(value[0]) + value.Substring(1);
        }
        #endregion

        #region enum
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static bool IsDefined<T>(string value)
        {
            return Enum.IsDefined(typeof(T), value);
        }

        public static List<string> EnumToStringList<T>()
        {
            return new List<string>(Enum.GetNames(typeof(T)));
        }

        public static T[] GetValues<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        #endregion

        #region json
        //first load from Resources/ folder then load from absolute path
        //path is either short form under Resources/ folder or full system path
        public static JSONNode JSONNodeFromFileResourcesPath(string path)
        {
            UnityEngine.Object obj = UnityEngine.Resources.Load(path, typeof(UnityEngine.Object));
            if(obj == null)
            {
                if(obj == null){
                    Debug.LogError("fail to load Resources file, check if folder and file name are correct " + path);
                    return null;
                }       
            }
            

            var readString = assetToString(obj);

            JSONNode jsonNode = JSON.Deserialize(readString);

            return jsonNode;
        }

        public static JSONNode JSONNodeFromFileFullPath(string path)
        {
            

            if (!path.EndsWith(".json"))
            {
                path += ".json";
            }
            if (!File.Exists(path))
            {
                Debug.LogError("file don't exists, check if folder and file name are correct: " + path);
                return null;
            }


            JSONNode jsonNode = JSON.Deserialize(File.ReadAllText(path));

            return jsonNode;
        }
        #endregion

        #region assets
        static public string assetToString(UnityEngine.Object asset)
        {
            var readObject = asset as TextAsset;
            if (readObject == null)
            {
                return "";
            }
            return readObject.text;
        }    
        #endregion
    }
}
