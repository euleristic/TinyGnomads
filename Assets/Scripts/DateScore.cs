using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateScore : MonoBehaviour
{
    static Slider gnomeCamSlider;
    public static float value;

    static bool scoreUnlocked;

    //animation and effects
    static Animator dateAnim;
    static ParticleSystem heartParticles;
    
    void Start()
    {
        gnomeCamSlider = GetComponentInChildren<Slider>();
        value = .5f;
        gnomeCamSlider.value = value;
        scoreUnlocked = true;

        dateAnim = GameObject.FindGameObjectWithTag("Date").GetComponent<Animator>();
        heartParticles = GameObject.Find("Heart Particles").GetComponent<ParticleSystem>();

        Debug.Log(dateAnim);
        Debug.Log(heartParticles);
        var heartPS = heartParticles.main;
        heartPS.startSize = value * 0.1f;
    }

    ///<summary>
    ///A positive term benefits the VR-player22
    ///</summary>
    public static void ChangeScore(float term)
    {

        if (scoreUnlocked)
        {

            value += term;
            gnomeCamSlider.value = value;


            if (value <= 0f); //Gnome wins
            if (value >= 1f); //VR wins


            var heartPS = heartParticles.main;
            heartPS.startSize = value * 0.1f;

            if(term < 0)
            {
                dateAnim.SetTrigger("Bad");
            }
            else
            {
                dateAnim.SetTrigger("Good");
            }

            Debug.Log(value);
        }

    }

    public static void SetScore(float term)
    {
        value = term;
        gnomeCamSlider.value = value;

        if (value <= 0f) ; //Gnome wins
        if (value >= 1f) ; //VR wins

        var heartPS = heartParticles.main;
        heartPS.startSize = value * 0.1f;

    }
}