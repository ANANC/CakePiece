using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ANFramework
{
    public class EditorReousrceLoader : IReousrceLoader
    {
        public void Destroy()
        {

        }

        public void Init()
        {

        }

        public void Start()
        {

        }

        public void Update()
        {

        }

        public T LoadResource<T>(string path) where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
    }
}