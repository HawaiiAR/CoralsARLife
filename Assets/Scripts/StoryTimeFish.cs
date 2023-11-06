using System.Collections;
using System.Collections.Generic;
using Bleaching;
using UnityEngine;
using Info;

namespace Fish
{
    public class StoryTimeFish : FishSwim
    {
        public Transform storyPoint;
        private GameObject _presentationPoint;
        private GameObject _player;

        [SerializeField] private MoveCanvasToPosition _moveCanvas;

        bool isFirstStoryPoint;
        Vector3 _startSize;
        Vector3 _maxSize;

        protected override void Start()
        {

            BleachingExperienceControl.BleachingStarted += PresentFish;
            //base.Start();  

        }

        private void OnEnable()
        {
            _presentationPoint = GameObject.FindGameObjectWithTag("PresentationPoint");
            _player = GameObject.FindGameObjectWithTag("MainCamera");
            state = FishState.isLookingForFood;
            isFirstStoryPoint = true;
        }

        protected override void OnDisable()
        {
            BleachingExperienceControl.BleachingStarted -= PresentFish;
            // base.OnDisable();
        }

        public void StartStory()
        {
            // PresentFish();
            state = FishState.isPresenting;
        }

        void Update()
        {

            if (state == FishState.isSwimming)
            {
                //  Debug.Log("Story fish active");
                Swim(storyPoint.transform.position);
                //moved this out of the swim function so it doesn't get inherited
                if (_distance < 1f)
                {
                    _rotSpeed = UnityEngine.Random.Range(.1f, .25f);
                    state = FishState.isLookingForFood;
                    //
                    isFirstStoryPoint = false;
                }
            }

            if (state == FishState.isLookingForFood)
            {
                // FloatTimer(FishState.isSwimming);
                Vector3 _dir = _presentationPoint.transform.position - this.transform.position;
                _dir.y = 0;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_dir), _rotSpeed * Time.deltaTime);
            }
            if (state == FishState.isPresenting)
            {

                PresentFish();

            }
        }

        protected override void PresentFish()
        {
            Vector3 _midPointDirection = (_player.transform.position - _presentationPoint.transform.position);
            Vector3 target = _player.transform.position - (_midPointDirection *.9f);

            float _presentationDistance = Vector3.Distance(this.transform.position, target);
            Vector3 _dir = target - this.transform.position;

            if (_presentationDistance > .2f)
            {
                this.transform.position = Vector3.Lerp(transform.position, target, 2 * Time.deltaTime);

            }
            if (_presentationDistance > 1)
            {
                this.transform.rotation = Quaternion.LookRotation(_dir);
            }

            if (_presentationDistance < 1)
            {
                _collider.enabled = true;
                //  this.transform.rotation = Quaternion.Slerp(transform.rotation, _presentationPoint.transform.rotation, 2 * Time.deltaTime);
            }

            if (_presentationDistance <= .2)
            {

                state = FishState.isFloating;
                //this allows the fish to display its first message to the playerbefore moving on to the other points
                if (isFirstStoryPoint)
                {
                    _fishInfo.StoryToTell(0);
                    //  isFirstStoryPoint = false;
                }

                if (!isFirstStoryPoint)
                {
                    _fishInfo.StoryToTell(1);
                }
            }

        }

        protected override void Chasingfood(Transform food)
        {


        }
        protected override void FoodGone()
        {

        }

        public void HideCanvas()
        {
            _moveCanvas.ReleaseFish();
        }

        protected override void OnCollisionStay(Collision collision)
        {
            // base.OnCollisionStay(collision);
        }

        public void SetGuidFishFree()
        {
            HideCanvas();
            NewTarget();
            state = FishState.isSwimming;
        }

        private void OnTriggerStay(Collider other)
        {

            //placeholder to override base
        }
    }
}
