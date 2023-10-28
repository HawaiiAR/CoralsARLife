using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{

    [SerializeField] private GameObject _mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _direction = this.transform.position - _mainCamera.transform.position;
        _direction.y = 0;
        this.transform.rotation = Quaternion.LookRotation(_direction);
    }
}
