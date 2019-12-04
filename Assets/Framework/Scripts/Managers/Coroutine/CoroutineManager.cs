using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANFramework
{
    public class CoroutineManager : MonoBehaviour
    {
        public Coroutine RunCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }
    }
}