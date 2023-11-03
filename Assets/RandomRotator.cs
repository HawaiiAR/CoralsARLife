using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour
{
    private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        _speed = Random.Range(.5f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_speed, _speed, -_speed);
    }
}
