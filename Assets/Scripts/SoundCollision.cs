using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCollision : MonoBehaviour
{
    [Header("Audio Source 1 = Rigidbody Collsion")]
    [Header("Audio Source 0 = Non Rigidbody Collsion")]


    [Tooltip("Audio Source 0 = Non Rigidbody Collsion & Audio Source 1 = Rigidbody Collsion")]
    public AudioSource[] audioSource;
    private bool done;

    void Start()
    {
        done = false;
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(1);
        done = true;
    }

    void OnCollisionEnter(Collision collision)
    {

        if(collision.rigidbody == null && done == true && audioSource[0] != null)
        {
            // Low Sound

            float rand = Random.Range(0.9f, 1.1f);

            audioSource[0].pitch = rand;

            audioSource[0].Play();
        }

        if(collision.rigidbody != null && collision.relativeVelocity.sqrMagnitude > 0.01f && done == true && audioSource[1] != null)
        {
            // High Sound

            float rand = Random.Range(0.9f, 1.1f);

            audioSource[1].pitch = rand;
            audioSource[1].Play();
        }
    }
}
