using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabThrowBehaviour : MonoBehaviour
{
    private MovementBehavior movement_behavior;

    private bool isGrabbing;
    private GameObject heldObject;

    private Transform held_grab_transform;
    private Vector3 gnome_grab_point;

    private ConfigurableJoint joint1, joint2;

    [SerializeField] LayerMask grabbableLayer;


    private float gnome_height, gnome_lenght, gnome_width;
    private Vector3 grab_center;
    private Vector3 grab_collider_half_extents;
    Quaternion grab_collider_rotation;
    private GameObject gnome_body;
    private float object_body_mass;

    private bool game_started;

    //grab gnome's grab
    [SerializeField] MeshCollider new_object_mesh_collider;
    private MeshCollider grabbed_object_mesh_collider;

    [SerializeField] PhysicsHand left_hand, right_hand;
    PhysicsHand[] hands;

    //throw
    float throw_force = 1.5f;

    //animation
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        grab_center = gnome_body.transform.position + gnome_body.transform.forward * gnome_lenght + new Vector3(0.0f, gnome_height / 6, 0.0f);
        grab_collider_half_extents = new Vector3(gnome_width, gnome_height / 1.5f, gnome_lenght / 2);
        grab_collider_rotation = gnome_body.transform.rotation;

        grabbed_object_mesh_collider = null;
        hands = new PhysicsHand[2] { left_hand, right_hand };

        game_started = true;
    }
    private void Awake()
    {
        movement_behavior = gameObject.GetComponent<MovementBehavior>();
        gnome_body = movement_behavior.body;

        gnome_lenght = movement_behavior.gnome_lenght;
        gnome_height = movement_behavior.gnome_height;
        gnome_width = movement_behavior.gnome_width;

        //animation
        animator = movement_behavior.animator;
    }

    // Update is called once per frame
    void Update()
    {
        grab_center = gnome_body.transform.position + gnome_body.transform.forward * gnome_lenght + new Vector3(0.0f, gnome_height / 3, 0.0f);
        grab_collider_half_extents = new Vector3(gnome_width, gnome_height / 1.5f, gnome_lenght / 2);
        grab_collider_rotation = gnome_body.transform.rotation;

        gnome_grab_point = movement_behavior.gnome_collider.transform.position + gnome_body.transform.rotation * new Vector3(0, gnome_height * 1.1f, 0);
    }

    public void Throw(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (movement_behavior.is_grabbed)
            {
                Release();
                return;
            }

            if (heldObject != null)
            {
                var objectBody = heldObject.GetComponent<Rigidbody>();
                Release();
                objectBody.gameObject.transform.position = gnome_grab_point;
                objectBody.velocity += (gnome_body.transform.up * 0.8f + gnome_body.transform.forward) * throw_force;
            }
        }
    }

    public void Grab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrabbing || heldObject)
            {
                Release();
                return;
            }

            Collider[] grabbableColliders = Physics.OverlapSphere(grab_center, gnome_width, grabbableLayer);

            if (grabbableColliders.Length < 1) return;

            //print("miiiiiiiiiiiiiiiisstaaaa");

            var objectToGrab = grabbableColliders[0].transform.gameObject;

            foreach(Collider grabbable_collider in grabbableColliders)
            {
                if(Vector3.Distance(grabbable_collider.transform.position, grab_center) < Vector3.Distance(objectToGrab.transform.position, grab_center))
                {
                    objectToGrab = grabbable_collider.transform.gameObject;
                }
            }

            var objectBody = objectToGrab.GetComponent<Rigidbody>();

            if (objectBody != null)
            {
                heldObject = objectBody.gameObject;
            }

            GrabObject(objectBody);
        }
    }

    private void GrabObject(Rigidbody objectBody)
    {
        isGrabbing = true;
        animator.SetLayerWeight(1, 1);

        //objectBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        objectBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

        gnome_grab_point = movement_behavior.gnome_collider.transform.position + new Vector3(0, gnome_height * 1.1f, 0);
        objectBody.transform.parent = gnome_body.transform;

        objectBody.transform.position = gnome_grab_point;
        objectBody.transform.localRotation = Quaternion.identity;

        //objectBody.MovePosition(gnome_grab_point);

        objectBody.useGravity = false;
        object_body_mass = objectBody.mass;
        objectBody.mass = 0.0f;
        objectBody.isKinematic = true;

        //collider stuff
        System.Type collider_type;
        collider_type = objectBody.gameObject.GetComponent<Collider>().GetType();
        new_object_mesh_collider.transform.position = gnome_grab_point;
        if (collider_type == new_object_mesh_collider.GetType())
        {
            new_object_mesh_collider.transform.localScale = objectBody.transform.lossyScale;


            grabbed_object_mesh_collider = objectBody.gameObject.GetComponent<MeshCollider>();
            grabbed_object_mesh_collider.enabled = false;
            new_object_mesh_collider.sharedMesh = grabbed_object_mesh_collider.sharedMesh;
            new_object_mesh_collider.enabled = true;
        }
    }

    private void Release()
    {
        if (heldObject != null)
        {
            var objectBody = heldObject.GetComponent<Rigidbody>();
            objectBody.gameObject.transform.parent = null;
            objectBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            heldObject = null;
            objectBody.useGravity = true;
            objectBody.mass = object_body_mass;
            objectBody.isKinematic = false;

            objectBody.gameObject.transform.position = grab_center + (gnome_body.transform.forward * objectBody.GetComponent<MeshRenderer>().bounds.extents.z * 0.8f);

            //collider stuff
            new_object_mesh_collider.enabled = false;
            grabbed_object_mesh_collider.enabled = true;

            if(movement_behavior.is_grabbed)
            {
                foreach(PhysicsHand hand in hands)
                {
                    if(hand.heldCollider == new_object_mesh_collider)
                    {
                        objectBody.transform.position = gnome_grab_point;
                        hand.ReleaseObject();
                        hand.heldObject = objectBody.gameObject;
                        StartCoroutine(hand.GrabObject(grabbed_object_mesh_collider, objectBody));
                    }
                }
            }
        }

        isGrabbing = false;
        animator.SetLayerWeight(1, 0);
    }

    void OnDrawGizmos()
    { 
        Gizmos.color = Color.green;
        if (game_started)
        {
            Gizmos.DrawSphere(grab_center, gnome_width);
        }
    }
}
