using System.Collections;
using UnityEngine;

public class SimplePush : MonoBehaviour, IPushable
{
    [SerializeField] float _forceMultiplier=1;
    public void Push(PushInfo pushInfo)
    {
        GetComponent<Rigidbody2D>().AddForce((transform.position - pushInfo.pushPosition ).normalized * pushInfo.pushForce*_forceMultiplier,ForceMode2D.Impulse);
    }
}
