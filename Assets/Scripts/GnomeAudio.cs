using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeAudio : MonoBehaviour
{
    private Animator gnomeAnim;

    public AudioSource[] audios;
    public AudioSource[] stepSound;
    void Start()
    {
        gnomeAnim = GetComponent<Animator>();
    }


    public void playAudio(int num)
    {
        if(audios[num] != null)
        {
        audios[num].Play();
        }
    }

    public void stepAudio()
    {
        //if (gnomeAnim.GetBool("Grounded"))
        //{
            float ran = Random.Range(0.9f, 1.1f);

            int stepNum = Random.Range(0, 2);

            stepSound[stepNum].pitch = ran;

            stepSound[stepNum].Play();
        //}
    }
}
