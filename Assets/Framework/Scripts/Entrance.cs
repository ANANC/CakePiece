using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject framework = new GameObject("FrameworkEntrance");
        framework.AddComponent<FrameworkEntrance>();
        
        GameObject.Destroy(this.gameObject);
    }
}
