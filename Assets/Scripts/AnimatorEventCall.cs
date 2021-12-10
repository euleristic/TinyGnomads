using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEventCall : MonoBehaviour
{
    public ParticleSystem[] part;

    public GameObject[] objects;

    public AudioSource[] audios;
    

    public void PlayParticleSystem(int number)
    {
        switch (number)
        {
            case 0:
                part[number].Play();
                break;

            case 1:
                part[number].Play();
                break;

            case 2:
                part[number].Play();
                break;

            case 3:
                part[number].Play();
                break;

            case 4:
                part[number].Play();
                break;

            case 5:
                part[number].Play();
                break;

            case 6:
                part[number].Play();
                break;
        }
    }

    public void SwitchObjectOnOff(int number)
    {

        switch (number)
        {
            case 0:

                if(!objects[number].activeInHierarchy)
                {
                    objects[number].SetActive(true);
                }
                else
                {
                    objects[number].SetActive(false);
                }

                break;
            case 1:

                if (!objects[number].activeInHierarchy)
                {
                    objects[number].SetActive(true);
                }
                else
                {
                    objects[number].SetActive(false);
                }

                break;
            case 2:

                if (!objects[number].activeInHierarchy)
                {
                    objects[number].SetActive(true);
                }
                else
                {
                    objects[number].SetActive(false);
                }

                break;
            case 3:

                if (!objects[number].activeInHierarchy)
                {
                    objects[number].SetActive(true);
                }
                else
                {
                    objects[number].SetActive(false);
                }

                break;
            case 4:

                if (!objects[number].activeInHierarchy)
                {
                    objects[number].SetActive(true);
                }
                else
                {
                    objects[number].SetActive(false);
                }

                break;
        }
    }

    public void PlayAudio(int number)
    {
        switch (number)
        {
            case 0:
                audios[number].Play();
                break;
            case 1:
                audios[number].Play();
                break;
            case 2:
                audios[number].Play();
                break;
            case 3:
                audios[number].Play();
                break;
            case 4:
                audios[number].Play();
                break;
        }
    }
}
