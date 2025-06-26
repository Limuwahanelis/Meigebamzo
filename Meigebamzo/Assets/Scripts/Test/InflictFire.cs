using UnityEngine;

public class InflictFire : MonoBehaviour
{
    [SerializeField] ElementalSpell _basicElement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IDamagable affliciton;
        Rigidbody2D rb;
        rb = collision.attachedRigidbody;
        if (rb)
        {
            affliciton = rb.GetComponentInParent<IDamagable>();
            if (affliciton != null)
            {
                Logger.Log("Inflict");
                affliciton.TakeDamage(new DamageInfo(0, transform.position, _basicElement));
            }
        }
    }
}
