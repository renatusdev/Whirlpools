using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlpool : MonoBehaviour
{
    [Range(0,10)] public float pull;
    [Range(0,40)] public float rotate;

    BoxCollider c;
    List<Rigidbody> objsPulled;

    Material shader;
    float drop;
    float maxDistance;

    void Start()
    {
        objsPulled = new List<Rigidbody>();
        c = GetComponent<BoxCollider>();

        shader = GetComponent<MeshRenderer>().sharedMaterial;
        drop = shader.GetFloat("_Drop") * transform.localScale.y;

        maxDistance = transform.localScale.x * 10;
    }

    private void FixedUpdate()
    {
        if (HasObjectsNear())
            foreach (Rigidbody r in objsPulled)
                Pull(r);
    }






    private void Pull(Rigidbody r)
    {
        Vector3 center = transform.position;
        Vector3 obj = r.position;
        center.y -= drop;
        Vector3 direction = center - obj;
        float distance = direction.magnitude;        
        direction.Normalize();

        distance = Mathf.Clamp(distance, 0, maxDistance);
        distance /= maxDistance;

        Vector3 force = direction * distance * pull;
        Vector3 torque = direction * distance * rotate;

        r.AddForce(force, ForceMode.VelocityChange);
        r.AddTorque(torque);
    }

    private void OnDrawGizmos()
    {
        Vector3 center = transform.position;
        Gizmos.color = Color.blue;
        center.y -= drop;
        Gizmos.DrawSphere(center, 0.6f);

        Gizmos.color = Color.red;

        if (Application.isPlaying)
        {
            if (!HasObjectsNear()) { return; }

            foreach(Rigidbody rb in objsPulled)
            {
                Vector3 obj = rb.position;
                Vector3 dir = center - obj;
                dir.Normalize();

                float distance = Vector3.Distance(center, obj);

                Vector3 force = dir * (distance);

                Gizmos.DrawRay(obj, force);
            }
        }
    }

    private void OnTriggerEnter(Collider o)
    {
        objsPulled.Add(o.GetComponent<Rigidbody>());
    }

    private void OnTriggerExit(Collider o)
    {
        objsPulled.Remove(o.GetComponent<Rigidbody>());

        if(o.transform.position.y <= drop)
            Destroy(o.gameObject);
    }

    private bool HasObjectsNear() { return objsPulled.Count != 0; }
}