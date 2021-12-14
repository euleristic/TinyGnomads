using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetchupMustard : MonoBehaviour
{
    public string type;
    public GameObject splashParticle;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<HeadManager>() != null)
        {
            GameObject splash = Instantiate(splashParticle, transform.position, transform.rotation);
            Destroy(splash, 2);

           switch (type)
        {
            case "ketchup":
                other.GetComponent<HeadManager>().splats[0].Play();

                break;
            case "mustard":
                other.GetComponent<HeadManager>().splats[1].Play();
                break;
        }
            DateScore.ChangeScore(-0.05f);
            Destroy(gameObject);
        }
    }

}
