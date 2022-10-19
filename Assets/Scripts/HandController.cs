using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public float SearchRadius;
    public Transform PlayerHead;
    public Transform Hand;
    public Transform Target;

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, PlayerHead.transform.position) <= SearchRadius)
        {
            Vector3 fromTo = Hand.transform.position - PlayerHead.transform.position;

            Vector3 newpos = PlayerHead.transform.position - fromTo * -0.5f;

            Target.transform.position = Vector3.Lerp(Target.transform.position, newpos, Time.deltaTime * 10);

            Target.rotation = Quaternion.LookRotation(-fromTo, Vector3.up) * Quaternion.Euler(0,-90,-90);
        }
    }
}
