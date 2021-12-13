using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateScore : MonoBehaviour
{
    static Slider gnomeCamSlider;
    public static float value;

    static bool scoreUnlocked;
    
    void Start()
    {
        gnomeCamSlider = GetComponentInChildren<Slider>();
        value = .5f;
        gnomeCamSlider.value = value;
        scoreUnlocked = true;
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

        }

    }

    public static void SetScore(float term)
    {
        value = term;
        gnomeCamSlider.value = value;

        if (value <= 0f) ; //Gnome wins
        if (value >= 1f) ; //VR wins
    }
}