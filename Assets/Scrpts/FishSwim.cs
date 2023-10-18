using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Fish
{
    [RequireComponent(typeof(FishInfo))]
    public class FishSwim : MonoBehaviour
    {
        [SerializeField]
        public enum FishState
        {
            isSwimming,
            isEating,
            isFloating,
            isLookingForFood,
            isBeingFed,
            isPresenting,
         
        }
        public GameObject food;

        protected Vector3 target;

        [Header("Fish")]      
        [SerializeField] public FishState state;
        [SerializeField] public float _speed;      
        [SerializeField] protected GameObject _fish;
        [SerializeField] protected float _rotSpeed;
        [SerializeField] protected float _waitTime;
        [SerializeField] protected float _distanceToOtherFish;
       

        protected float _timePassed;

        [Header("tank")]
        [SerializeField] protected Transform _tankCenter;
        [SerializeField] protected float _tankWidth;
    
        
        protected float _distance;
        protected Vector3 _direction;
        protected Vector3 _avoidanceRatio = Vector3.zero;

        Rigidbody _rb;

        protected virtual void Awake()
        {
            _rb = this.GetComponent<Rigidbody>();
            _tankCenter = GameObject.Find("TankCenter").GetComponent<Transform>();
            FoodPellet.FoodGone += FoodGone;
        }


        // Start is called before the first frame update
        protected virtual void Start()
        {   
            NewTarget();
            
        }


        protected virtual void OnDisable()
        {
            FoodPellet.FoodGone -= FoodGone;
        }

        // Update is called once per frame
        void Update()
        {

            if (state == FishState.isSwimming)
            {
                Swim(target);
                //moved this out of the swim function so it doesn't get inherited
                if (_distance < .1f)
                {
                    _rotSpeed = UnityEngine.Random.Range(.1f, .25f);
                    NewTarget();
                    state = FishState.isLookingForFood;

                }
            }

            if (state == FishState.isLookingForFood)
            {
               // Debug.Log("floating");
                FloatTimer(FishState.isSwimming);
                TurnToTarget(target);
            }

            if(state == FishState.isFloating)
            {
               // _state = FishState.isSwimming;
            }

            if(state == FishState.isPresenting)
            {
                PresentFish();
            }
            if(state == FishState.isBeingFed)
            {
                Chasingfood(food.transform);
            }
      
        }

        protected virtual void NewTarget()
        {
            
            _timePassed = _waitTime;
            target = new Vector3(
            UnityEngine.Random.Range(_tankCenter.transform.position.x - _tankWidth, _tankCenter.transform.position.x + _tankWidth),
            UnityEngine.Random.Range(_tankCenter.transform.position.y - _tankWidth, _tankCenter.transform.position.y + _tankWidth),
            UnityEngine.Random.Range(_tankCenter.transform.position.z - _tankWidth, _tankCenter.transform.position.z + _tankWidth)
            );
            // _state = FishState.isSwimming;

        }

        private void TurnToTarget(Vector3 _target)
        {
          //  Debug.Log("trun");
            CalculateDistanceAndDirection(_target);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_direction), _rotSpeed * Time.deltaTime);
        }

        protected virtual void TurnFromTarget(GameObject otherFish)
        {
            if (otherFish != this.gameObject)
            {
                _distance = Vector3.Distance(otherFish.transform.position, this.transform.position);
                if (_distance <= _distanceToOtherFish)
                {
  
                        _avoidanceRatio = _avoidanceRatio + (this.transform.position - otherFish.transform.position);

                    Vector3 _midPoint = (this.transform.forward + otherFish.transform.forward).normalized;
                    Vector3 _direction = (_midPoint + _avoidanceRatio) - this.transform.position;

                    if (_direction != Vector3.zero)
                    {
                        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_direction), _rotSpeed * Time.deltaTime);
                    }
                }
            }
         

        }

        protected virtual void Swim(Vector3 _target)
        {
            CalculateDistanceAndDirection(_target);        
               
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_direction), _rotSpeed);
            //this.transform.position = Vector3.Lerp(this.transform.position, _target, _speed);
            this.transform.Translate(0, 0, _speed * Time.deltaTime);
         
        }


        protected virtual void Chasingfood(Transform food)
        {
            if (food == null)
            {
                state = FishState.isSwimming;
            }
            else
            {

                CalculateDistanceAndDirection(food.transform.position);

                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_direction), _rotSpeed);
                //this.transform.position = Vector3.Lerp(this.transform.position, _target, _speed);
                this.transform.Translate(0, 0, _speed * 2 * Time.deltaTime);
            }

        }

        protected virtual void FoodGone()
        {
            state = FishState.isSwimming;
            food = null;
            NewTarget();
        }

        protected virtual void CalculateDistanceAndDirection(Vector3 _target)
        {
            _distance = Vector3.Distance(this.transform.position, _target);
            _direction = _target - this.transform.position;
        }

        protected virtual void FloatTimer(FishState _fishState)
        {
           
                _timePassed -= Time.deltaTime;

                if (_timePassed <= 0)
                {
                    state = _fishState;
                    //  _timePassed = _waitTime;
                  //  Debug.Log("fishState" + state);
                }
            
        }

     

        protected virtual void PresentFish()
        {
            GameObject _presentationPoint = GameObject.FindGameObjectWithTag("PresentationPoint");
            target = _presentationPoint.transform.position;
            float _presentationDistance = Vector3.Distance(this.transform.position, target);

            if (_presentationDistance > .1f)
            {
                this.transform.position = Vector3.Lerp(transform.position, target, 2 * Time.deltaTime);

            }

            if (_presentationDistance < 1)
            {
                this.transform.rotation = Quaternion.Slerp(transform.rotation, _presentationPoint.transform.rotation, 2 * Time.deltaTime);
            }
        }


        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("collision" + collision.gameObject.name);

            if (collision.gameObject.CompareTag("Coral"))     
              {
                Debug.Log("hit rock");
                state = FishState.isSwimming;
                NewTarget();
            }

            if (collision.gameObject.TryGetComponent<FishSwim>(out FishSwim fish))
            {
                Debug.Log("turnFromFish");
                TurnFromTarget(collision.gameObject);


            }

        }

       /* private void OnTriggerEnter(Collider other)
        {
            Debug.Log("collision" + other.gameObject.name);

            if (other.gameObject.CompareTag("Coral"))
            {
                Debug.Log("hit rock");
                state = FishState.isLookingForFood;
                NewTarget();
            }

            if (other.gameObject.CompareTag("Fish"))
            {

               state = FishState.isSwimming;
                TurnFromTarget(other.gameObject);
            }

        }*/


    }

}
