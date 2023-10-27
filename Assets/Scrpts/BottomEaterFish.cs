using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Fish
{
    [RequireComponent(typeof(FishInfo))]
    public class BottomEaterFish : FishSwim
    {
        //bottom feeders go from rock to rock pecking at the rock looking for food
        [SerializeField] private Transform[] _rocks;
        [SerializeField] private int _feedAttempts;
        [SerializeField] private int _full;
        [SerializeField] private float _floatSpeed;
        [SerializeField] protected float _minFeedtime;
        [SerializeField] protected float _maxFeedtime;

        private float _fishStuckTimer;
        private float _time;
        bool _fishStuck;

        protected override void Awake()
        {
            base.Awake();
        }
      
        protected override void Start()
        { 
            GetRock();
            _fishStuckTimer = 3;
        }

        protected override void OnDisable()
        {
            FoodPellet.FoodGone -= FoodGone;
        }


        void Update()
        {
            if (state == FishState.isSwimming)
            {
                Swim(target);
           
            }

            //sends the fish back to the rock it game from if there are still feed attempts
            if (state == FishState.isEating)
            {
                Feed(target);
                FloatTimer(FishState.isSwimming);

            }
            if (state == FishState.isFloating)
            {
              //  _collider.enabled = true;
                // GetRock();
            }
            if(state == FishState.isLookingForFood)
            {
                GetRock();
            }

            if (state == FishState.isPresenting)
            {
               
                PresentFish();
            }

            if (state == FishState.isBeingFed)
            {
                Debug.Log("bottom feeder feeding");
                Chasingfood(food.transform);
            }
            if (state == FishState.isEscaping)
            {
                GetRock();
            }

            //this timer gets triggered every time the fish hits a rock, if it gets stuck it waits and then triggers an escape function
            if (_fishStuck)
            {
                
                _time += Time.deltaTime;
               
                if (_time >= _fishStuckTimer)
                {
                    StartCoroutine(Escape());
                    _fishStuck = false;
                }

            }
        }


        protected override void Swim(Vector3 _target)
        {
            base.Swim(_target);
        }

        //after a bottom feeder is full it will get a new rock
        private void GetRock()
        {
          //  Debug.Log("GetRock");
            ResetWaitTime();
            _feedAttempts = Random.Range(1, 3);
            int _randomRock = UnityEngine.Random.Range(0, _rocks.Length);
            target = _rocks[_randomRock].GetComponent<Transform>().position;
            state = FishState.isSwimming;
        }

   

        //this is the fish presentation state
        public override void FishSelected()
        {
            ResetWaitTime();
            base.FishSelected();
           
        }

        //resets the float timer when a fish hits a rock
        private void LetTheFishEat()
        {
            state = FishState.isEating;
            ResetWaitTime();
            _feedAttempts += 1;
          
        }

        //this is triggered when the fish 
        protected override void FloatTimer(FishState _fishState)
        {

            base.FloatTimer(_fishState);
        }

        //the fish will float backward away from the rock it is feeding on until the float timer is up, then it will go back to the rock if there are still feedattempts
        private void Feed(/*int feedAttempts,*/ Vector3 target)
        {
         //   Debug.Log("feed me");
            CalculateDistanceAndDirection(target);
            _floatSpeed = Random.Range(.1f, .45f);
           this.transform.Translate(0, _floatSpeed * Time.deltaTime, -_speed * Time.deltaTime);
             /* this.transform.localPosition = new Vector3(this.transform.localPosition.x,
                  transform.localPosition.y - _floatSpeed * Time.deltaTime,
                  transform.localPosition.z - _speed );*/


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), _rotSpeed);
           
            //sends the fish to a new rock if the fish is full
            if (_feedAttempts >= _full)
            {
                //changed from floating
                state = FishState.isLookingForFood;
            
            }

        }

        //if a fish has been selected to go to a players feed pellet it will go back to feeding when the food is gone
        protected override void FoodGone()
        {
            food = null;
            isFeeding = false;
            GetRock();

        }

        //resets the timer of float timer
        private void ResetWaitTime()
        {
            _waitTime = UnityEngine.Random.Range(_minFeedtime, _maxFeedtime);
            _timePassed = _waitTime;
         
        }

        //I dont think this need to be here but it doesn't seem to be triggered if it's not
        protected override void PresentFish()
        {
            base.PresentFish();
        }

        //when a fish hits a rock if it still has feed attempts it will go back to the rock after the float timer is up
        private void OnCollisionEnter(Collision collision)
        {
            if (!isFeeding)
            {
                if (collision.gameObject.TryGetComponent<RockOrCoral>(out RockOrCoral rock))
                {
                    LetTheFishEat();
                    StartTimer();
                }
            }
        }

        //rediculus work around beccause for some reason every once in awhile a fish gets stuck in a rock
        private void OnCollisionExit(Collision collision)
        {
            if (!isFeeding)
            {
                if (collision.gameObject.TryGetComponent<RockOrCoral>(out RockOrCoral rock))
                {
                    _fishStuck = false;
                }
            }
        }

        private void StartTimer()
        {
            _time = 0;
            _fishStuck = true;
        }

        private IEnumerator Escape()
        {
            transform.Translate(0, 0, 0);
            float _escapeTime = 0;
           
            while (_escapeTime <= 1)
            {
                _escapeTime += Time.deltaTime;
                transform.Translate(0, 0, 5 * Time.deltaTime);
                yield return null;
            }

            state = FishState.isLookingForFood;
                    
        }


        protected override void OnCollisionStay(Collision collision)
        {
           //this has to be here to override the base functionality
        }
    }
}
