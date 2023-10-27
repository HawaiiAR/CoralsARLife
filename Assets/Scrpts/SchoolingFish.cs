using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Fish
{
    [RequireComponent(typeof(FishInfo))]
    public class SchoolingFish : FishSwim
    {
        [Header("Fish")]   
        private Vector3 _averageHeading;
        private Vector3 _averagePosition;


        [Header("school")]
        GlobalSchoolingControl schoolControl;

        GameObject[] _school;     

        Vector3 _schoolCenter = Vector3.zero;
        Vector3 _schoolTarget;

        int _schoolSize = 0;

        private bool _returnToCenter = false;

        protected override void Awake()
        {
            base.Awake();
            schoolControl = FindAnyObjectByType<GlobalSchoolingControl>();
           
        }

        //sets a random speed and rotation speed for the fish
        protected override void Start()
        {
           
            _speed = Random.Range(.5f, 1);
            _rotSpeed = Random.Range(.5f, 1);

            state = FishState.isSwimming;

        }

        protected override void OnDisable()
        {
            FoodPellet.FoodGone -= FoodGone;
        }

        // Update is called once per frame
        void Update()
        {
            if (state == FishState.isSwimming)
            {
                if (Vector3.Distance(this.transform.position, schoolControl.tankCenter.transform.position) >= schoolControl.tankSize)
                {
                    _returnToCenter = true;
                   //  Debug.Log("return to center");
                }

                if (_returnToCenter)
                {
                    Vector3 _direction = schoolControl.tankCenter.transform.position - this.transform.position;
                    UpdateRotation(_direction);
                    _speed = Random.Range(.5f, 1);
                    _rotSpeed = Random.Range(.5f, 1);
                    _returnToCenter = false;
                }

                else
                {
                    if (Random.Range(0, 5) < 1)
                    {
                        UpdateCondition();
                    }
                }

                transform.Translate(0, 0, _speed * Time.deltaTime);
            }

            if (state == FishState.isPresenting)
            {
               PresentFish();
            }

            if(state == FishState.isFloating)
            {
                _collider.enabled = true;
            }

            if(state == FishState.isEscaping)
            {
                UpdateCondition();
                state = FishState.isSwimming;
            }

            if (state == FishState.isBeingFed)
            {
                Chasingfood(food.transform);
            }
        }

        private void UpdateCondition()
        {
            _schoolTarget = schoolControl.fishTarget;
            _school = schoolControl.fishSchool;
            float _schoolSpeed = Random.Range(.1f, .35f);
            float _distance;

            foreach (GameObject f in _school)
            {
                if (f != this.gameObject)
                {
                    _distance = Vector3.Distance( f.transform.position, this.transform.position);
                    if (_distance <= _distanceToOtherFish)
                    {
                        _schoolCenter += f.transform.position;
                        _schoolSize++;

                        if (_distance < 1)
                        {
                            _avoidanceRatio = _avoidanceRatio + (this.transform.position - f.transform.position);
                        }

                        SchoolingFish _newSchool = f.GetComponent<SchoolingFish>();

                        _schoolSpeed = _schoolSpeed + _newSchool._speed;
                    }
                }
            }

            if (_schoolSize > 0)
            {
                _schoolCenter = _schoolCenter / _schoolSize + (_schoolTarget - this.transform.position);
                _speed = _schoolSpeed / _schoolSize;

                Vector3 _direction = (_schoolCenter + _avoidanceRatio) - this.transform.position;

                if (_direction != Vector3.zero)
                {
                    UpdateRotation(_direction);
                }

            }
        }

        private void UpdateRotation(Vector3 _direction)
        {
           // Debug.Log("turn motherfucker");

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_direction), _rotSpeed * Time.deltaTime);

        }

        protected override void FoodGone()
        {
            food = null;
            isFeeding = false;
            state = FishState.isSwimming;
           
        }

        public override void FishSelected()
        {
            base.FishSelected();
        }

        protected override void PresentFish()
        {
            base.PresentFish();
        }

       /* private void OnTriggerEnter(Collider other)
        {
          //  Debug.Log("hit rock");
            if (other.gameObject.CompareTag("Rock"))
            {
            
                UpdateCondition();
            }

            if (other.gameObject.TryGetComponent<FishSwim>(out FishSwim fish))
            {
               // Debug.Log("turnFromFish");
                TurnFromTarget(other.gameObject);
               

            }
          
        }*/

        protected override void OnCollisionStay(Collision collision)
        {
            if (!isFeeding)
            {
                if (collision.gameObject.TryGetComponent<RockOrCoral>(out RockOrCoral Rock))
                {
                    UpdateCondition();
                    TurnFromTarget(collision.gameObject);
                }

                if (collision.gameObject.TryGetComponent<FishSwim>(out FishSwim fish))
                {
                    // Debug.Log("turnFromFish");
                    TurnFromTarget(collision.gameObject);


                }
            }
        }
    }

}
