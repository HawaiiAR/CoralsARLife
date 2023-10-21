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
        // public Vector3 target;
        [SerializeField] private FishSwim fishSwim;

        [SerializeField] private float _speed;
        Vector3 _startSize = Vector3.zero;
        Vector3 _EndSize = Vector3.one;
        float _currentSize;
        float _maxSize = 1;
        float _minSize = 0;

        private void Start()
        {
            fishSwim = GetComponentInParent<FishSwim>();
            this.transform.localScale = _startSize;
        }

        private void OnEnable()
        {
            this.transform.localScale = _startSize;
            GrowCanvas();

        }

        private void OnDisable()
        {
            this.transform.localScale = _startSize;
        }


        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown("r"))
            {
                ReleaseFish();
            }
        }

        public void MoveCanvas()
        {
            //  StartCoroutine(MoveCanvasToFishPosition());
        }

        public void GrowCanvas()
        {
            _currentSize = _minSize;
            StartCoroutine(GrowShrink(_currentSize, _maxSize, "grow"));

        }

        public void ShrinkCanvas()
        {
            _currentSize = _maxSize;
            StartCoroutine(GrowShrink(_currentSize, _minSize, "shrink"));


        }

        private IEnumerator GrowShrink(float currentSize, float targetSize, string growShrink)
        {
            Vector3 newSize = new Vector3(targetSize, targetSize, targetSize);

            switch (growShrink)
            {
                case "grow":
                    Debug.Log(newSize);
                    
                    while (currentSize < targetSize)
                    {
                       // this.transform.localScale = Vector3.Lerp(this.transform.localScale, newSize, _speed * 2 * Time.deltaTime);
                        this.transform.localScale = new Vector3(currentSize, currentSize, currentSize);
                        currentSize += _speed * Time.deltaTime;
                        yield return null;
                    }
                    break;

                case "shrink":
                    Debug.Log("new" + newSize);
                    Debug.Log("currentSize" + _currentSize);
                    while (currentSize > targetSize)
                    {
                       // this.transform.localScale = Vector3.Lerp(this.transform.localScale, newSize, _speed * 5 * Time.deltaTime);
                        this.transform.localScale = new Vector3(currentSize, currentSize, currentSize);
                        currentSize -= _speed * Time.deltaTime;
                        yield return null;
                    }
                    this.gameObject.SetActive(false);

                    break;

            }
      
          
        }

        /* private IEnumerator MoveCanvasToFishPosition()
          {
              while (Vector3.Distance(this.transform.position, target) > .1f)
              {
                  this.transform.localScale = Vector3.Lerp(_startSize, _EndSize, _speed * Time.deltaTime);
                  this.transform.position = Vector3.Lerp(this.transform.position, target, Vector3.Distance(this.transform.position, target) * Time.deltaTime);

                  yield return null;
              }

              Debug.Log("canvas hit pos");
           //   StopAllCoroutines();
          }*/

        public void ReleaseFish()
        {
            fishSwim.state = FishSwim.FishState.isEscaping;
            ShrinkCanvas();

        }
    }
}

