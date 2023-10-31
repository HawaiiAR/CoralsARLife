using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RocksnCoral {
    public class CoralGrow : MonoBehaviour
    {
        protected float _speed;
        protected float _maxSize;
        protected float _startSize;

        //when a coral is intantiated it will randomly grow to a new size
        protected virtual void OnEnable()
        {
            _startSize = Random.Range(.15f, .5f);
            _maxSize = Random.Range(.95f, 1.1f);
            _speed = Random.Range(.25f, 1f);

            this.transform.localScale = new Vector3(_startSize, _startSize, _startSize);
            StartCoroutine(Grow());
        }

        protected virtual IEnumerator Grow()
        {
            float currentSize = _startSize;
            float targetSize = _maxSize;


            Debug.Log("Grow");
            while (currentSize < targetSize)
            {
                //  Debug.Log("Growspeed");
                this.transform.localScale = new Vector3(currentSize, currentSize, currentSize);
                currentSize += _speed * Time.deltaTime;
                yield return null;
            }
        }
    }
}
