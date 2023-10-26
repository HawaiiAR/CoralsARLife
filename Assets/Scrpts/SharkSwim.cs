using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish {

    public class SharkSwim : FishSwim
    {

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
            target = RandomPointOnXZCircle(_tankCenter.transform.position, _randomRadius);
            
        }

        //this picks a random point on a circle to keep the shark swimming in somewhat of a circular pattern
        Vector3 RandomPointOnXZCircle(Vector3 center, float radius)
        {
            float angle = Random.Range(0, 2f * Mathf.PI);
            return center + new Vector3(Mathf.Cos(angle),
                UnityEngine.Random.Range(0, 5),
                Mathf.Sin(angle)) * radius;
        }

        //called when a shark is swimming
        protected override void Swim(Vector3 _target)
        {
            CalculateDistanceAndDirection(_target);

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_direction), _rotSpeed);
            this.transform.Translate(0, 0, _speed * Time.deltaTime);
           
            if (_distance < 1f)
            {    
                NewTarget();
            }
        
        }

        public override void FishSelected()
        {
            base.FishSelected();
        }

        protected override void PresentFish()
        {
            base.PresentFish();
        }

      /*  private void OnCollisionEnter(Collision collision)
        {

            if (collision.gameObject.TryGetComponent<FishSwim>(out FishSwim fish))
            {
               // Debug.Log("turnFromFish");
                TurnFromTarget(collision.gameObject);

              //  NewTarget();
              //  state = FishState.isSwimming;


            }
        }*/

        protected override void OnCollisionStay(Collision collision)
        {
            base.OnCollisionStay(collision);
        }
    }
}
