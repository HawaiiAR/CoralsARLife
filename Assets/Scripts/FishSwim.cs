using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//this cluster is the base for all fish types, come here when something doesn't work
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
            isEscaping

        }

        public GameObject food;
        public float averageSwimSpeed;

        [Header("Fish")]      
        [SerializeField] public FishState state;
        [SerializeField] public float _speed;      
        [SerializeField] protected GameObject _fish;
        [SerializeField] protected float _rotSpeed;
        [SerializeField] protected float _waitTime;
        [SerializeField] protected float _distanceToOtherFish;

        protected FishInfo _fishInfo;
        protected float _timePassed;
        protected Vector3 target;

        [Header("tank")]
        [SerializeField] protected Transform _tankCenter;
        [SerializeField] protected float _tankWidth;
    
        
        protected float _distance;
        protected Vector3 _direction;
        protected Vector3 _avoidanceRatio = Vector3.zero;

        // each fish needs a sigle collider, this get turned off when a fish is taged to present to avoid it hitting another fish
        //and changing direction
        public Collider _collider;
        public bool isFeeding;
      
       protected Rigidbody _rb;

        protected virtual void Awake()
        {
          
            _tankCenter = GameObject.Find("--TANKCENTER--").GetComponent<Transform>();
            
            _fishInfo = this.GetComponent<FishInfo>();
            _collider = this.GetComponent<Collider>();
            _rb = this.GetComponent<Rigidbody>();
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            FoodPellet.FoodGone += FoodGone;
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
                FloatTimer(FishState.isSwimming);
                TurnToTarget(target);
            }

            if(state == FishState.isPresenting)
            {
               
                PresentFish();

            }
            if(state == FishState.isBeingFed)
            {
                Chasingfood(food.transform);
            }

            if(state == FishState.isEscaping)
            {
               // Debug.Log("fish trying to escape");
                NewTarget();
                state = FishState.isSwimming;
            }
      
        }

        //provides a target for the fish to swim too used by sharks and regular fish, not bottomfeeders or schoolers
        protected virtual void NewTarget()
        {
            _timePassed = _waitTime;
            target = new Vector3(
            UnityEngine.Random.Range(_tankCenter.transform.position.x - _tankWidth, _tankCenter.transform.position.x + _tankWidth),
            UnityEngine.Random.Range(_tankCenter.transform.position.y - _tankWidth / 2, _tankCenter.transform.position.y + _tankWidth / 2),
            UnityEngine.Random.Range(_tankCenter.transform.position.z - _tankWidth, _tankCenter.transform.position.z + _tankWidth)
            );

        }

        //turns the fish towards its new target so it has a nice smooth motion
        protected virtual void TurnToTarget(Vector3 _target)
        {
            CalculateDistanceAndDirection(_target);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_direction), _rotSpeed * Time.deltaTime);
        }

        //this is supposed to test whether a fish is close to another fish and turn it. it doesn't quite work
        protected virtual void TurnFromTarget(GameObject otherFish)
        {
            if (otherFish != this.gameObject)
            {
              
           //     Debug.Log("turn away");
                /*  _distance = Vector3.Distance(otherFish.transform.position, this.transform.position);
                  if (_distance <= _distanceToOtherFish)
                  {

                     _avoidanceRatio = _avoidanceRatio + (this.transform.position - otherFish.transform.position);

                      Vector3 _midPoint = (this.transform.forward + otherFish.transform.forward).normalized;
                      Vector3 _direction = (_midPoint + _avoidanceRatio) - this.transform.position;

                      if (_direction != Vector3.zero)
                      {
                          this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(-_direction), _rotSpeed * 5 * Time.deltaTime);
                      }
                  }*/

                Vector3 avoidanceTarget = this.transform.position - otherFish.transform.position;
                transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(avoidanceTarget), _rotSpeed  * Time.deltaTime);
                averageSwimSpeed = _speed * 3;
                this.transform.Translate(0, 0, averageSwimSpeed * Time.deltaTime);

       
            }

        }

        //this is the base mostion for sharks and middle fish
        protected virtual void Swim(Vector3 _target)
        {
            CalculateDistanceAndDirection(_target);
            averageSwimSpeed = _speed;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_direction), _rotSpeed);
            this.transform.Translate(0, 0, _speed * Time.deltaTime);
         
        }

        // when a food pellet is released a random assortmente of fish will be slected and move towards the pellet until
        //one reaches and consumes it
        protected virtual void Chasingfood(Transform food)
        {
            if (food == null)
            {
                state = FishState.isSwimming;
                isFeeding = false;
            }
            else
            {
                isFeeding = true;
                CalculateDistanceAndDirection(food.transform.position);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_direction), _rotSpeed);
                this.transform.Translate(0, 0, _speed * 5 * Time.deltaTime);
            }

        }

        //when a pellet is gone the fish are released to find a new target, this changes on a per fish basis
        protected virtual void FoodGone()
        {
           
            state = FishState.isSwimming;
            isFeeding = false;
            food = null;
            NewTarget();
        }

        //called to update the direction and distance to the next target
        protected virtual void CalculateDistanceAndDirection(Vector3 _target)
        {
            _distance = Vector3.Distance(this.transform.position, _target);
            _direction = _target - this.transform.position;
        }


        // when certain fish reach a target they will wait and then pick another target
        protected virtual void FloatTimer(FishState _fishState)
        {         
                _timePassed -= Time.deltaTime;

                if (_timePassed <= 0)
                {
                    state = _fishState;

                }           
        }

        // when a fish has been selected it will swim towards the presentation point which is a few meteres infront of the main camera
        public virtual void FishSelected()
        {

            _collider.enabled = false;
            //   StopAllCoroutines();
            state = FishState.isPresenting;
        }

        //this has the fish swim towards the camera and then rotate 90 degress so they are side on to the player before displaying their information
        protected virtual void PresentFish()
        {
            GameObject _presentationPoint = GameObject.FindGameObjectWithTag("PresentationPoint");
            Vector3 target = _presentationPoint.transform.position;     

            float _presentationDistance = Vector3.Distance(this.transform.position, target);
            Vector3 _dir = target - this.transform.position;
            averageSwimSpeed = _speed * 2;

            if (_presentationDistance > .2f)
            {
                this.transform.position = Vector3.Lerp(transform.position, target, averageSwimSpeed * Time.deltaTime);
               
            }
            if(_presentationDistance > 1)
            {
                this.transform.rotation = Quaternion.LookRotation(_dir);
            }

            if (_presentationDistance < 1)
            {
                _collider.enabled = true;
                this.transform.rotation = Quaternion.Slerp(transform.rotation, _presentationPoint.transform.rotation, 2 * Time.deltaTime);
            }

            if(_presentationDistance <= .2)
            {
                
                state = FishState.isFloating;
                _fishInfo.StoryToTell(0);
            }
        }

        //a clumsy attemt at phyisics based avoidance. Each fish has a substantially larger collider that should make it turn before the fish mesh reaches another fish mesh
        /* private void OnCollisionEnter(Collision collision)
         {
 //            Debug.Log("collision" + collision.gameObject.name);

             if (collision.gameObject.CompareTag("Coral"))     
               {
               //  Debug.Log("hit rock");
                 state = FishState.isSwimming;
                 NewTarget();
             }

             if (collision.gameObject.TryGetComponent<FishSwim>(out FishSwim fish))
             {
                 //   Debug.Log("turnFromFish");

                 TurnFromTarget(collision.gameObject);


             }

         }*/


      /*  private void OnCollisionEnter(Collision collision)
        {
            if (!isFeeding)
            {
                if (collision.gameObject.TryGetComponent<RockOrCoral>(out RockOrCoral rock))
                {
                    Debug.Log("hit rock");
                    state = FishState.isSwimming;
                    NewTarget();
                }
            }
        }*/

        // need to add a check in here if the fish is feeding
        protected virtual void OnCollisionStay(Collision collision)
        {

            if (!isFeeding)
            {
                if (collision.gameObject.TryGetComponent<RockOrCoral>(out RockOrCoral rock))
                {
                   // Debug.Log("hit rock");
                  //  state = FishState.isSwimming;
                   // NewTarget();
                    TurnFromTarget(collision.gameObject);
                }

                if (collision.gameObject.TryGetComponent<FishSwim>(out FishSwim fish))
                {
                   
                     //   Debug.Log("turnFromFish");

                        TurnFromTarget(collision.gameObject);
                     
                    

                }
            }
        }
    }

}
