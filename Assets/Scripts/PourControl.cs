using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourControl : MonoBehaviour
{
    [SerializeField] Transform top, bottom;
    [SerializeField] ParticleSystem system;
    bool active;

    void Update()
    {
        if (!active && top.position.y < bottom.position.y)
        {
            active = true;
            system.Play();
        }
        else if (active && bottom.position.y < top.position.y)
        {
            active = false;
            system.Stop();
        }
    }
}
