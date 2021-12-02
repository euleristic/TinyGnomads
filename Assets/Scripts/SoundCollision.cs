using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCollision : MonoBehaviour
{
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

        if(collision.rigidbody == null && done == true)
        {
            // Low Sound

            float rand = Random.Range(0.8f, 1.2f);

            audioSource[0].pitch = rand;

            audioSource[0].Play();
        }

        if(collision.rigidbody != null && collision.relativeVelocity.sqrMagnitude > 0.01f && done == true)
        {
            // High Sound

            float rand = Random.Range(0.8f, 1.2f);

            audioSource[1].pitch = rand;
            audioSource[1].Play();
        }
    }
}
