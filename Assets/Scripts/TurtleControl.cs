using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish
{
    public class TurtleControl : SharkSwim
    {
        private Animator _anim;

        bool newAnimState;

        protected override void Awake()
        {
            _anim = this.GetComponentInChildren<Animator>();
            _fishInfo = this.GetComponent<FishInfo>();
            _collider = this.GetComponent<Collider>();

        }

        protected override void Start()
        {
            base.Start();
            AnimationState("swim");
            newAnimState = false;
            _timePassed = _waitTime;
        }

       
        void Update()
        {
            if (state == FishState.isSwimming)
            {
             
                Swim(target);
                if (_distance < 1f)
                {
                    _rotSpeed = UnityEngine.Random.Range(.1f, .25f);
                    NewTarget();
                    AnimationState("float");
                    state = FishState.isLookingForFood;
                  
                }
            }

            if (state == FishState.isFloating)
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

            if (state == FishState.isLookingForFood)
            {
             
                FloatTimer(FishState.isSwimming);
                TurnToTarget(target);
            }

        }

        protected override void NewTarget()
        {
            _timePassed = _waitTime;
            base.NewTarget();
        }

        protected override Vector3 RandomPointOnXZCircle(Vector3 center, float radius)
        {

            float angle = Random.Range(0, 2f * Mathf.PI);
            return center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
        }


        protected override void FloatTimer(FishState _fishState)
        {
           
            _timePassed -= Time.deltaTime;

            if (_timePassed <= 0)
            {
                AnimationState("swim"); 
                state = _fishState;

            }
        }


        private void AnimationState(string state)
        {
            switch(state)
            {
                case "swim":
                    _anim.SetBool("TurtleSwim", true);
                    _anim.SetBool("TurtleFloat", false);
                    break;
                case "float":
                    _anim.SetBool("TurtleSwim", false);
                    _anim.SetBool("TurtleFloat", true);
                    break;

            }
        }

       
    }
}
