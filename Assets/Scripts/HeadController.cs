using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    public float SearchRadius;
    public Transform PlayerHead;
    public Transform Target;
    public Transform Head;

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, PlayerHead.position) <= SearchRadius)
        {
            Vector3 fromTo = Head.position - PlayerHead.position;

            Vector3 newpos = PlayerHead.position - fromTo * -0.5f;

            Target.position = Vector3.Lerp(Target.position, newpos, Time.deltaTime * 10);
        }
    }
}
