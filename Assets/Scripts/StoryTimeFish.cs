using System.Collections;
using System.Collections.Generic;
using Bleaching;
using UnityEngine;
using Info;
using System;

namespace Fish
{
    public class StoryTimeFish : FishSwim
    {
        public Transform storyPoint;

        [SerializeField] private ParticleSystem _bubbles;
        [SerializeField] private MoveCanvasToPosition _moveCanvas;

        private FishSelector _fishSelector;
        private float _bubbleAmount;
        private GameObject _presentationPoint;
        private GameObject _player;

        bool isFirstStoryPoint;
        bool storyOver;
        Vector3 _startSize;
        Vector3 _maxSize;

        protected override void Start()
        {

            BleachingExperienceControl.BleachingStarted += PresentFish;
            FishSelector.SetStoryFishFree += LookForCoral;
        }

        private void OnEnable()
        {
            _presentationPoint = GameObject.FindGameObjectWithTag("PresentationPoint");
            _player = GameObject.FindGameObjectWithTag("MainCamera");
            _fishSelector = FindObjectOfType<FishSelector>();

            state = FishState.isLookingForFood;

            isFirstStoryPoint = true;
            storyOver = false;

            _bubbleAmount = 0;
        }

        protected override void OnDisable()
        {
            BleachingExperienceControl.BleachingStarted -= PresentFish;
            FishSelector.SetStoryFishFree -= LookForCoral;

        }

    

            public void StartStory()
        {

            state = FishState.isPresenting;
        }

        void Update()
        {

            if (state == FishState.isSwimming)
            {
                if (!storyOver)
                {
                    Swim(storyPoint.transform.position);
                }

                if (storyOver)
                {
                    Swim(_fishSelector.newCoralPos);
                }

                var em = _bubbles.emission;
                if (_bubbleAmount < 5)
                {
                    _bubbleAmount++;
                    em.rateOverTime = _bubbleAmount;
                }

                if (_distance < 1f)
                {
                    _rotSpeed = UnityEngine.Random.Range(.1f, .25f);
                    state = FishState.isLookingForFood;
                    isFirstStoryPoint = false;
                }
            }

            if (state == FishState.isLookingForFood)
            {
                // FloatTimer(FishState.isSwimming);
                Vector3 _dir = _presentationPoint.transform.position - this.transform.position;
                _dir.y = 0;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_dir), _rotSpeed * Time.deltaTime);

                var em = _bubbles.emission;
                if (_bubbleAmount > 0)
                {
                    _bubbleAmount--;
                    em.rateOverTime = _bubbleAmount;
                }
            }
            if (state == FishState.isPresenting)
            {
               
                PresentFish();

            }
        }

        private void SetParticleEmission(float amount)
        {
            var em = _bubbles.emission;

            if(amount < 5)
            {
                amount++;
                em.rateOverTime = amount;
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
            //placeholder to override base
        }
        protected override void FoodGone()
        {
            //placeholder to override base
        }

        public void HideCanvas()
        {
            _moveCanvas.ReleaseFish();
        }

        protected override void OnCollisionStay(Collision collision)
        {
            //placeholder to override base
        }

        public void SetGuidFishFree()
        {
            HideCanvas();
            storyOver = true;
         
        }

        private void LookForCoral()
        {
            state = FishState.isSwimming;
        }

        private void OnTriggerStay(Collider other)
        {

            //placeholder to override base
        }
    }
}
