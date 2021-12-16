using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeStaticByTag : MonoBehaviour
{
    [Header("OBS! All other gameobjects with this tag will be destroyed")]
    [SerializeField] bool IUnderstand;
    void Awake()
    {
        if (!IUnderstand) return;
        GameObject[] objs = GameObject.FindGameObjectsWithTag(transform.tag);
            
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
