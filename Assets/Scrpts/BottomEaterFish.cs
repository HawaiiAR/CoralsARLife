using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Fish
{
    [RequireComponent(typeof(FishInfo))]
    public class BottomEaterFish : FishSwim
    {

        [SerializeField] private Transform[] _rocks;
        [SerializeField] private int _feedAttempts;
        [SerializeField] private int _full;
        [SerializeField] private float _floatSpeed;
        [SerializeField] protected float _minFeedtime;
        [SerializeField] protected float _maxFeedtime;

        private Bounds _rockBounds;

        protected override void Awake()
        {
            base.Awake();
        }
        // Start is called before the first frame update
        protected override void Start()
        {
            GetRock();
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
                Swim(target);
           
            }

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
        }


        protected override void Swim(Vector3 _target)
        {
            base.Swim(_target);
        }

        protected override void NewTarget()
        {
            base.NewTarget();
        }

        private void GetRock()
        {
          //  Debug.Log("GetRock");
            ResetWaitTime();
            _feedAttempts = Random.Range(1, 3);
            int _randomRock = UnityEngine.Random.Range(0, _rocks.Length);
            target = _rocks[_randomRock].GetComponent<Transform>().position;
            state = FishState.isSwimming;
        }

        private void OnCollisionEnter(Collision collision)
        {

            if (collision.gameObject.CompareTag("Coral"))
            {
         
                LetTheFishEat();
            }

            if (collision.gameObject.TryGetComponent<FishSwim>(out FishSwim fish))
            {
                  GetRock();
           
            }
        }

        public override void FishSelected()
        {
            ResetWaitTime();
            base.FishSelected();
           
        }

        private void LetTheFishEat()
        {
            state = FishState.isEating;
            ResetWaitTime();
            _feedAttempts += 1;
          
        }

        protected override void FloatTimer(FishState _fishState)
        {

            base.FloatTimer(_fishState);
        }

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
           

            if (_feedAttempts >= _full)
            {
                //changed from floating
                state = FishState.isLookingForFood;
                // Debug.Log("full");
            }


        }

        protected override void FoodGone()
        {
            food = null;
            GetRock();

        }


        private void ResetWaitTime()
        {
            _waitTime = UnityEngine.Random.Range(_minFeedtime, _maxFeedtime);
            _timePassed = _waitTime;
            //   Debug.Log("timePassed" + _timePassed);

        }

        protected override void PresentFish()
        {
            base.PresentFish();
        }
    }
}
