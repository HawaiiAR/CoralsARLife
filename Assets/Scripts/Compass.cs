using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    public class Compass : MonoBehaviour
    {
        [SerializeField] private float _rotSpeed;
        [SerializeField] private Transform _compassPos;
        private Transform _destination;

        // Start is called before the first frame update
        void Start()
        {
            Navigation.UpdateDirection += LeadToDestination;
            _compassPos = GameObject.Find("CompassPoint").GetComponent<Transform>();
            
        }
        private void OnDisable()
        {
            Navigation.UpdateDirection -= LeadToDestination;
        }

        private void LeadToDestination(Transform _newDestination)
        {
            _destination = _newDestination;
        }

        // Update is called once per frame
        void Update()
        {
            this.transform.position = _compassPos.transform.position;

            if (_destination != null)
            {
                Vector3 _direction = this.transform.position - _destination.transform.position;

                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_direction), _rotSpeed * Time.deltaTime);
                
            }
        }
    }
}
