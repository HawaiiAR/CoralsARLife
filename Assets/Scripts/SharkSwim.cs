using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish {

    public class SharkSwim : FishSwim
    {

        protected override void Awake()
        {
            // base.Awake();
            _fishInfo = this.GetComponent<FishInfo>();
            _collider = this.GetComponent<Collider>();          
        }

        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {
            if (state == FishState.isSwimming)
            {
                Swim(target);
                if (_distance < .1f)
                {
                    NewTarget();
                }
            }

            if(state == FishState.isFloating)
            {
                _collider.enabled = true;
            }
        
            if (state == FishState.isPresenting)
            {
                PresentFish();
            }

            if (state == FishState.isEscaping)
            {
                NewTarget();
                state = FishState.isSwimming;
            }

        }

        protected override void NewTarget()
        {
            //randomizes the circle width the shark swims to make it a little less mechanical
            float _randomRadius = Random.Range(_tankWidth - .5f, _tankWidth);
            Vector3 centerPoint = new Vector3(_tankCenter.transform.position.x,
                _tankCenter.transform.position.y + Random.Range(-3, 3),
                _tankCenter.transform.position.z
                );
            target = RandomPointOnXZCircle(_tankCenter.transform.position, _randomRadius);
            
        }

        //this picks a random point on a circle to keep the shark swimming in somewhat of a circular pattern
       protected virtual Vector3 RandomPointOnXZCircle(Vector3 center, float radius)
        {
            
            float angle = Random.Range(0, 2f * Mathf.PI);
            return center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
        }

        //called when a shark is swimming
        protected override void Swim(Vector3 _target)
        {
            CalculateDistanceAndDirection(_target);
            averageSwimSpeed = _speed / 2;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_direction), _rotSpeed);
            this.transform.Translate(0, 0, _speed * Time.deltaTime);
           
            if (_distance < 3f)
            {    
                NewTarget();
            }
        
        }

        // adding this to enable animator speed adjustment
        protected override void TurnFromTarget(GameObject otherFish)
        {
            averageSwimSpeed = _speed;
            base.TurnFromTarget(otherFish);
        }

        public override void FishSelected()
        {
            base.FishSelected();
        }

        protected override void PresentFish()
        {
            base.PresentFish();
        }


        protected override void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<RockOrCoral>(out RockOrCoral rock))
            {
                //  Debug.Log("hit rock");
                TurnFromTarget(collision.gameObject);
            }

            if (collision.gameObject.CompareTag("Shark"))
            {
                //   Debug.Log("turnFromFish");

                TurnFromTarget(collision.gameObject);

            }
        }

        private void OnTriggerStay(Collider other)
        {
            //  Debug.Log("hit rock");
            if (other.gameObject.CompareTag("InfoPannel"))
            {

                //  UpdateCondition();
                TurnFromTarget(other.gameObject);
            }

        }
    }
}
