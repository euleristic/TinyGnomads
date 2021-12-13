using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] GameObject breakObject;
    [SerializeField] float breakVelocity;
    [SerializeField] float dateReaction;
    float breakVelocitySqr;

    private void Start()
    {
        breakVelocitySqr = breakVelocity * breakVelocity;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.sqrMagnitude > breakVelocitySqr)
        {
            DateScore.ChangeScore(dateReaction);
            Destroy(Instantiate(breakObject, transform.position, Quaternion.identity), 2f);
            Destroy(gameObject);

        }
    }
}
