using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] Transform _transformToFollow;
    [SerializeField] bool _keepOffset;
    [SerializeField] Vector3 _offset;
    private void Start()
    {
        if(_offset==Vector3.zero && _keepOffset)
        {
            _offset = transform.position-_transformToFollow.position;
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = _transformToFollow.position+ _offset;
    }
}
