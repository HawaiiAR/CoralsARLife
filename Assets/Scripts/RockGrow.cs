using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RocksnCoral {
    public class RockGrow : CoralGrow
    {
        [SerializeField] private GameObject[] _corals;
        // Start is called before the first frame update
        void Start()
        {
            ActivateCoral(false);
        }

        protected override void OnEnable()
        {
         /*   _startSize = Random.Range(.15f, .5f);
            _maxSize = Random.Range(1, 1.25f);
            _speed = Random.Range(.1f, .5f);
            this.transform.localScale = new Vector3(_startSize, _startSize, _startSize);
            StartCoroutine(Grow());*/
            base.OnEnable();
            Debug.Log("max size" + _maxSize);
          
        }

        private void ActivateCoral(bool state)
        {
            foreach (GameObject c in _corals)
            {
                if (c != null)
                {
                    c.SetActive(state);
                }
            }
        }

        protected override IEnumerator Grow()
        {
            float currentSize = _startSize;
            float targetSize = _maxSize;


            Debug.Log("Grow");
            while (currentSize < targetSize)
            {
                //  Debug.Log("Growspeed");
                this.transform.localScale = new Vector3(currentSize, currentSize, currentSize);
                currentSize += _speed * Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(.5f);
            StartCoroutine(StartGrowingCoral());
        }


        private IEnumerator StartGrowingCoral()
        {
            for (int i = 0; i < _corals.Length; i++)
            {
                float randDelay = Random.Range(.2f, .75f);
                if (_corals[i] != null)
                {
                    _corals[i].SetActive(true);
                }
                yield return new WaitForSeconds(randDelay);
            }
        }
    }
}
