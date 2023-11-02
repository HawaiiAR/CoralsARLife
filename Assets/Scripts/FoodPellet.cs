using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Fish
{
    public class FoodPellet : MonoBehaviour
    {
        public delegate void FoodDestoyed();
        public static event FoodDestoyed FoodGone;

        [SerializeField] private float _speed;
        Rigidbody _rb;

       
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
                Debug.Log("Food Gone");
                FoodGone();
                Destroy(this.gameObject);
            }
        }
    }
}
