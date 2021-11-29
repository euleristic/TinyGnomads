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

    private bool game_started;

    // Start is called before the first frame update
    void Start()
    {
        grab_center = gnome_body.transform.position + gnome_body.transform.forward * gnome_lenght + new Vector3(0.0f, gnome_height / 6, 0.0f);
        grab_collider_half_extents = new Vector3(gnome_width, gnome_height / 1.5f, gnome_lenght / 2);
        grab_collider_rotation = gnome_body.transform.rotation;

        game_started = true;
    }
    private void Awake()
    {
        movement_behavior = gameObject.GetComponent<MovementBehavior>();
        gnome_body = movement_behavior.body;

        gnome_lenght = movement_behavior.gnome_lenght;
        gnome_height = movement_behavior.gnome_height;
        gnome_width = movement_behavior.gnome_width;
    }

    // Update is called once per frame
    void Update()
    {
        grab_center = gnome_body.transform.position + gnome_body.transform.forward * gnome_lenght + new Vector3(0.0f, gnome_height / 6, 0.0f);
        grab_collider_half_extents = new Vector3(gnome_width, gnome_height / 1.5f, gnome_lenght / 2);
        grab_collider_rotation = gnome_body.transform.rotation;

        //gnome_grab_point = movement_behavior.gnome_collider.transform.position + Vector3.up / 3f;
        gnome_grab_point = movement_behavior.gnome_collider.transform.position + new Vector3(0, gnome_height * 1.1f, 0);

        foreach (Transform child in gnome_body.transform)
        {
            Rigidbody child_rb = child.GetComponent<Rigidbody>();
            if (child_rb != null)
            {
                child.position = gnome_grab_point;
                child_rb.velocity = Vector3.zero;
                child.transform.rotation = gnome_body.transform.rotation;
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

            print("miiiiiiiiiiiiiiiisstaaaa");

            var objectToGrab = grabbableColliders[0].transform.gameObject;
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

        ////create a grab point
        //held_grab_transform = new GameObject().transform;

        //MeshFilter mf = objectBody.GetComponent<MeshFilter>();
        //Vector3 object_size = mf.sharedMesh.bounds.size;
        //Vector3 object_scale = objectBody.transform.localScale;
        //float object_height = object_size.y * object_scale.y;

        //held_grab_transform.position = objectBody.position + new Vector3(0.0f, object_height, 0.0f);

        //gnome_grab_point = movement_behavior.gnome_collider.transform.position + new Vector3(0, gnome_height * 1.1f, 0);

        //held_grab_transform.parent = heldObject.transform;

        ////objectBody.velocity = Vector3.zero;
        ////objectBody.angularVelocity = Vector3.zero;

        objectBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        //objectBody.gameObject.transform.parent = gameObject.transform;
        //held_grab_transform.position = gnome_grab_point;



        //gnome_grab_point = movement_behavior.gnome_collider.transform.position + Vector3.up/3f;
        gnome_grab_point = movement_behavior.gnome_collider.transform.position + new Vector3(0, gnome_height * 1.1f, 0);
        objectBody.gameObject.transform.parent = movement_behavior.body.transform;

        objectBody.gameObject.transform.position = gnome_grab_point;
        //objectBody.MovePosition(gnome_grab_point);
        objectBody.useGravity = false;



        //joint1 = gameObject.AddComponent<ConfigurableJoint>();
        //joint1.connectedBody = objectBody;
        //joint1.breakForce = float.PositiveInfinity;
        //joint1.breakTorque = float.PositiveInfinity;
        //joint1.xMotion = ConfigurableJointMotion.Locked;
        //joint1.yMotion = ConfigurableJointMotion.Locked;
        //joint1.zMotion = ConfigurableJointMotion.Locked;

        ////joint1.connectedMassScale = 1;
        ////joint1.massScale = 1;
        //joint1.enableCollision = false;
        //joint1.enablePreprocessing = false;

        //joint2 = heldObject.AddComponent<ConfigurableJoint>();
        //joint2.connectedBody = movement_behavior.rb;
        //joint2.breakForce = float.PositiveInfinity;
        //joint2.breakTorque = float.PositiveInfinity;
        //joint2.xMotion = ConfigurableJointMotion.Locked;
        //joint2.yMotion = ConfigurableJointMotion.Locked;
        //joint2.zMotion = ConfigurableJointMotion.Locked;

        ////joint2.connectedMassScale = 1;
        ////joint2.massScale = 1;
        //joint2.enableCollision = false;
        //joint2.enablePreprocessing = false;
    }

    private void Release()
    {
        //if (joint1 != null)
        //    Destroy(joint1);

        //if (joint2 != null)
        //    Destroy(joint2);

        //if (grabPoint != null)
        //    Destroy(grabPoint.gameObject);

        if (heldObject != null)
        {
            var objectBody = heldObject.GetComponent<Rigidbody>();
            objectBody.gameObject.transform.parent = null;
            objectBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            heldObject = null;
            objectBody.useGravity = true;
        }

        isGrabbing = false;
    }

    void OnDrawGizmos()
    { 
        Gizmos.color = Color.green;
        if (game_started)
        {
            Gizmos.DrawSphere(grab_center, gnome_width);
        }
        //var rotationMatrix = Matrix4x4.Translate(grab_center) * Matrix4x4.Rotate(grab_collider_rotation);
        //Gizmos.matrix = rotationMatrix;
        //Gizmos.DrawCube(Vector3.zero, grab_collider_half_extents);
    }
}
