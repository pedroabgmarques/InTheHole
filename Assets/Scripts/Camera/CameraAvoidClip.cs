using System;
using UnityEngine;
using System.Collections;

public class CameraAvoidClip : MonoBehaviour
{
    #region Variáveis

    /* Variáveis Públicas */
    public float ClipMoveTime = 0.05f;              // time taken to move when avoiding cliping (low value = fast, which it should be)
    public float ReturnTime = 0.4f;                 // time taken to move back towards desired position, when not clipping (typically should be a higher value than clipMoveTime)
    public float SphereCastRadius = 0.1f;           // the radius of the sphere used to test for object between camera and target
    public bool VisualiseInEditor;                  // toggle for visualising the algorithm through lines for the raycast in the editor
    public float ClosestDistance = 0.5f;            // the closest distance the camera can be from the target
    public bool Protecting { get; private set; }    // used for determining if there is an object between the target and the camera
    public string DontClipTag = "Player";           // don't clip against objects with this tag (useful for not clipping against the targeted object)


    /* Variáveis Privadas */

    // Camera
    private Transform cameraTransform;
    private Transform pivotTransform;
    private float originalDist;
    private float currentDist;
    // Velocidade actual do movimento da câmara
    private float moveVelocity;

    // Raycast
    private Ray ray;
    private RaycastHit[] hits;
    private RayHitComparer rayHitComparer;

    #endregion

#region Métodos MonoBehaviour

    // Start
    public void Start()
    {
        // find the camera in the object hierarchy
        cameraTransform = GetComponentInChildren<Camera>().transform;
        pivotTransform = cameraTransform.parent;

        originalDist = cameraTransform.localPosition.magnitude;
        currentDist = originalDist;

        rayHitComparer = new RayHitComparer();
    }

    // Late Update
    public void LateUpdate()
    {
        // initially set the target distance
        float targetDist = originalDist;

        ray.origin = pivotTransform.position + pivotTransform.forward * SphereCastRadius;
        ray.direction = -pivotTransform.forward;

        // initial check to see if start of spherecast intersects anything
        var cols = Physics.OverlapSphere(ray.origin, SphereCastRadius);

        bool initialIntersect = false;
        bool hitSomething = false;

        // loop through all the collisions to check if something we care about
        for (int i = 0; i < cols.Length; i++)
        {
            if ((!cols[i].isTrigger) &&
                !(cols[i].attachedRigidbody != null && cols[i].attachedRigidbody.CompareTag(DontClipTag)))
            {
                initialIntersect = true;
                break;
            }
        }

        // if there is a collision
        if (initialIntersect)
        {
            ray.origin += pivotTransform.forward * SphereCastRadius;

            // do a raycast and gather all the intersections
            hits = Physics.RaycastAll(ray, originalDist - SphereCastRadius);
        }
        else
        {
            // if there was no collision do a sphere cast to see if there were any other collisions
            hits = Physics.SphereCastAll(ray, SphereCastRadius, originalDist + SphereCastRadius);
        }

        // sort the collisions by distance
        Array.Sort(hits, rayHitComparer);

        // set the variable used for storing the closest to be as far as possible
        float nearest = Mathf.Infinity;

        // loop through all the collisions
        for (int i = 0; i < hits.Length; i++)
        {
            // only deal with the collision if it was closer than the previous one, not a trigger, and not attached to a rigidbody tagged with the dontClipTag
            if (hits[i].distance < nearest && (!hits[i].collider.isTrigger) &&
                !(hits[i].collider.attachedRigidbody != null &&
                  hits[i].collider.attachedRigidbody.CompareTag(DontClipTag)))
            {
                // change the nearest collision to latest
                nearest = hits[i].distance;
                targetDist = -pivotTransform.InverseTransformPoint(hits[i].point).z;
                hitSomething = true;
            }
        }

        // visualise the cam clip effect in the editor
        if (hitSomething)
        {
            Debug.DrawRay(ray.origin, -pivotTransform.forward * (targetDist + SphereCastRadius), Color.red);
        }

        // hit something so move the camera to a better position
        Protecting = hitSomething;
        currentDist = Mathf.SmoothDamp(currentDist, targetDist, ref moveVelocity,
                                       currentDist > targetDist ? ClipMoveTime : ReturnTime);
        currentDist = Mathf.Clamp(currentDist, ClosestDistance, originalDist);
        cameraTransform.localPosition = -Vector3.forward * currentDist;
    }

    #endregion

    // comparer for check distances in ray cast hits
    public class RayHitComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
        }
    }
}
