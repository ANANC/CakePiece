using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANFramework
{
    public interface IReousrceLoader
    {
        T LoadResource<T>(string path) where T : Object;
    }
}