using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Fish
{
    public class FoodPellet : MonoBehaviour
    {
        public static Action FoodGone;

        [SerializeField] private float _speed;
        Rigidbody _rb;

        // Start is called before the first frame update
        void Start()
        {
            _rb = this.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _rb.AddRelativeForce(Vector3.forward * _speed, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision collision)
        {
       
                if (collision.gameObject.TryGetComponent<FishSwim>(out FishSwim fish))
                {
                 
                FoodGone?.Invoke();
                Destroy(this.gameObject);
            }
        }
    }
}
