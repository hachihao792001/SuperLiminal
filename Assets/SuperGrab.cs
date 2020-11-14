using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperGrab : MonoBehaviour
{
    public Transform Grabbing;
    public float originalDist;
    public Vector3 originalScale;
    public LayerMask IgnoreGrabbable;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Grabbing == null)
            {
                RaycastHit rch;
                if (Physics.Raycast(transform.position, transform.forward, out rch))
                {
                    if (rch.collider.tag == "Grabbable")
                    {
                        Grabbing = rch.collider.transform;
                        Grabbing.GetComponent<Rigidbody>().isKinematic = true;
                        //Grabbing.GetComponent<Collider>().enabled = false;
                        Grabbing.parent = transform;

                        originalScale = Grabbing.localScale;
                        originalDist = Vector3.Distance(transform.position, Grabbing.position);
                    }
                }
            }
            else
            {
                Grabbing.GetComponent<Rigidbody>().isKinematic = false;
                //Grabbing.GetComponent<Collider>().enabled = true;
                Grabbing.parent = null;
                Grabbing = null;
            }
        }

        if (Grabbing != null)
        {
            RaycastHit wallRch;
            if (Physics.Raycast(transform.position, transform.forward, out wallRch, 10000, IgnoreGrabbable))
            {
                Grabbing.position = wallRch.point;
                Vector3 outDir;
                float outDist;
                Physics.ComputePenetration(Grabbing.GetComponent<Collider>(), Grabbing.position, Grabbing.rotation,
                    wallRch.collider, wallRch.collider.transform.position, wallRch.transform.rotation,
                    out outDir, out outDist);

                Grabbing.position += (transform.position-wallRch.point).normalized* outDist;
            }

            Grabbing.localScale = originalScale * (Vector3.Distance(transform.position, Grabbing.position) / originalDist);
        }
    }
}
