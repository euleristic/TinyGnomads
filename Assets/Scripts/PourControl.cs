using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourControl : MonoBehaviour
{
    [SerializeField] Transform top, bottom;
    [SerializeField] ParticleSystem system;
    bool active; //I'm scared that SetActive() might be expensive, so just to be safe...

    void Update()
    {
        if (!active && top.position.y < bottom.position.y)
        {
            active = true;
            system.gameObject.SetActive(active);
        }
        else if (active && bottom.position.y < top.position.y)
        {
            active = false;
            system.gameObject.SetActive(active);
        }
    }
}
