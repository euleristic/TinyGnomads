using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateScore : MonoBehaviour
{
    [SerializeField] static Slider gnomeCamSlider;
    static float value;
    
    void Start()
    {
        value = .5f;
        gnomeCamSlider.value = value;
    }

    
    void Update()
    {
        if (value <= 0f); //Gnome wins
        if (value >= 1f); //VR wins
    }

    ///<summary>
    ///A positive term benefits the VR-player
    ///</summary>
    public static void ChangeScore(float term)
    {
        value += term;
        gnomeCamSlider.value = value;
    }
}