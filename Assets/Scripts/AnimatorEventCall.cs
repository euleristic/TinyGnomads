using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEventCall : MonoBehaviour
{
    public ParticleSystem[] part;
    

    public void PlayParticleSystem(int number)
    {
        switch (number)
        {
            case 0:
                part[0].Play();
                break;

            case 1:
                part[1].Play();
                break;

            case 2:
                part[2].Play();
                break;

            case 3:
                part[3].Play();
                break;

            case 4:
                part[4].Play();
                break;

            case 5:
                part[5].Play();
                break;

            case 6:
                part[6].Play();
                break;
        }
    }

}
