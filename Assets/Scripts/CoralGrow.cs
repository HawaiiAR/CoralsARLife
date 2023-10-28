using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoralGrow : MonoBehaviour
{
    private float _speed;
    private float _maxSize;
    private float _startSize;

    //when a coral is intantiated it will randomly grow to a new size
    private void OnEnable()
    {
        _startSize = Random.Range(.15f, .5f);
        _maxSize = Random.Range(.5f, 1);
        _speed = Random.Range(.1f, .5f);

        this.transform.localScale = new Vector3(_startSize, _startSize, _startSize);
        StartCoroutine(Grow());
    }

    private IEnumerator Grow()
    {
        float currentSize = _startSize;
        float targetSize = _maxSize;


        Debug.Log("Grow"); 
        while (currentSize < targetSize)
        {
            Debug.Log("Growspeed");
            this.transform.localScale = new Vector3(currentSize, currentSize, currentSize);
            currentSize += _speed * Time.deltaTime;
            yield return null;
        }
    }
}
