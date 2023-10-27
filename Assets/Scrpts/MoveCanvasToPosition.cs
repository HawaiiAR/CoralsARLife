using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Fish;

namespace Info
{
    public class MoveCanvasToPosition : MonoBehaviour
    {

        public TMP_Text fishName;
        public TMP_Text fishInfo;
        
        [SerializeField] protected FishSwim fishSwim;

        [SerializeField] protected float _speed;
        Vector3 _startSize = Vector3.zero;
        Vector3 _EndSize = Vector3.one;
        protected float _currentSize;
        protected float _maxSize = 1;
        protected float _minSize = 0;

       public bool isStoryFish = false;

        protected virtual void Start()
        {
            fishSwim = GetComponentInParent<FishSwim>();
            this.transform.localScale = _startSize;

        }

        //sets the canvas size to zero on start
        protected virtual void OnEnable()
        {
            this.transform.localScale = _startSize;
            GrowCanvas();
        }

        //when a canvas is disable sets its size back to zero
        protected virtual void OnDisable()
        {
            this.transform.localScale = _startSize;
        }
        
        void Update()
        {
            // for testing purposes in scene need to figure out a way to pick a new fish
            if (Input.GetKeyDown("r"))
            {
                ReleaseFish();
            }
        }


        protected virtual void GrowCanvas()
        {
            _currentSize = _minSize;
            StartCoroutine(GrowShrink(_currentSize, _maxSize, "grow"));

        }

        protected virtual void ShrinkCanvas()
        {
            _currentSize = _maxSize;
            StartCoroutine(GrowShrink(_currentSize, _minSize, "shrink"));
        }

        //is there a way to pass an operand as a peramiter, would be better than a switch to change -= to +=
        protected virtual IEnumerator GrowShrink(float currentSize, float targetSize, string growShrink)
        {
           // Vector3 newSize = new Vector3(targetSize, targetSize, targetSize);

            switch (growShrink)
            {
                case "grow":
               
                    while (currentSize < targetSize)
                    {
                        this.transform.localScale = new Vector3(currentSize, currentSize, currentSize);
                        currentSize += _speed * Time.deltaTime;
                        yield return null;
                    }
                    break;

                case "shrink":
             
                    while (currentSize > targetSize)
                    {
                        this.transform.localScale = new Vector3(currentSize, currentSize, currentSize);
                        currentSize -= _speed * Time.deltaTime;
                        yield return null;
                    }
                    this.gameObject.SetActive(false);

                    break;
            }
       
        }

     
        // this is triggered from a button on the canvas to let the fish go. Would be nice to have the fish released if another fish is selected
        public void ReleaseFish()
        {
            //ban practice but running out of time
            if (!isStoryFish)
            {
                fishSwim.state = FishSwim.FishState.isEscaping;
            }

            ShrinkCanvas();

        }
    }
}

