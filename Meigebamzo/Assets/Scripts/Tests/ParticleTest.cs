using UnityEngine;

public class ParticleTest : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] int _count;
    [SerializeField] float _startLifetime;
    [SerializeField] float _length;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ParticleSystem.EmitParams parameters = new ParticleSystem.EmitParams();
       
        parameters.startLifetime = _startLifetime;
        //_particleSystem.textureSheetAnimation.spriteCount
        Vector3 size = new Vector3( _particleSystem.main.startSizeX.constant,0, _particleSystem.main.startSizeZ.constant);
        size.y = _length;
        parameters.startSize3D = size;
        if (Input.GetKeyDown(KeyCode.P))
        {
            _particleSystem.Emit(parameters, _count);
           // _particleSystem.Play();
        }
    }
}
