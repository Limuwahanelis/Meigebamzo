using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastFromCamera2D : MonoBehaviour
{
    public static Vector3 MouseInWorldPos => _cameraInWorldPos;
    [SerializeField] Camera _cam;
    private static Vector3 _cameraInWorldPos;
    // Start is called before the first frame update
    void Start()
    {
        if (_cam == null) _cam = Camera.main;
    }

    private void Update()
    {
        _cameraInWorldPos = _cam.ScreenPointToRay(HelperClass.MousePos).origin;
        _cameraInWorldPos.z = 0;
    }
}
