using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DOFController : MonoBehaviour
{
    // Start is called before the first frame update

    Ray raycast;
    RaycastHit hit;
    bool isHit;
    float hitDistance;

    public PostProcessVolume volume;
    DepthOfField depthOfField;

    void Start()
    {
        volume.profile.TryGetSettings(out depthOfField);
        
    }

    // Update is called once per frame
    void Update()
    {

        raycast = new Ray(transform.position, transform.forward * 100);
        isHit = false;
        if (Physics.Raycast(raycast, out hit, 100f))
        {
            isHit = true;
            hitDistance = Vector3.Distance(transform.position, hit.point);
            //Debug.Log("hit object");
        }
        else
        {
            if (hitDistance < 100f)
            {
                hitDistance++;
            }
        }
        SetFocus();

    }
    private void SetFocus()
    {
        depthOfField.focusDistance.value = hitDistance;
    }

    private void OnDrawGizmos()
    {
        if (isHit)
        {
            Gizmos.DrawSphere(hit.point, 0.1f);
            Debug.DrawRay(transform.position, transform.forward * Vector3.Distance(transform.position, hit.point));
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 100f);
        }

    }
}
