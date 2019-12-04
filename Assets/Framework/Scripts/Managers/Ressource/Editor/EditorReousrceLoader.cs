using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ANFramework
{
    public class EditorReousrceLoader : BaseBehaviour, IReousrceLoader
    {
        public T LoadResource<T>(string path) where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
    }
}