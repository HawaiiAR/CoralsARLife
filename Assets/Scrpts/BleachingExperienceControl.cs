using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Fish;

namespace Bleaching {

    public class BleachingExperienceControl : MonoBehaviour
    {
        public static Action ReefBleached;

        public List<GameObject> corals = new List<GameObject>();
        public List<GameObject> apexPreditors = new List<GameObject>();
        public List<GameObject> midLevelFish = new List<GameObject>();
        public List<GameObject> bottomLevelFish = new List<GameObject>();
        // Start is called before the first frame update

        [SerializeField] private float _colorFadeTime;
        private int _fishFoodChainLevel;


        void Start()
        {
            FishSelector.CoralRestored += BringFishBack;
        }

        private void OnDisable()
        {
            FishSelector.CoralRestored -= BringFishBack;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown("b"))
            {
                StartBleachingSequence();
            }
        }

        public void AddFishToList(int level, GameObject fish)
        {
            switch (level)
            {
                case 0:
                    corals.Add(fish);
                    break;
                case 1:
                    bottomLevelFish.Add(fish);
                    break;
                case 2:
                    midLevelFish.Add(fish);
                    break;
                case 3:
                    apexPreditors.Add(fish);
                    break;

            }
        }

        public void StartBleachingSequence()
        {
            StartCoroutine(BleachingSequence());
        }

        private IEnumerator BleachingSequence()
        {
            float delay = UnityEngine.Random.Range(.5f, 1);
            foreach (GameObject c in corals)
            {
               
             //   float delay = Random.Range(.5f, 1);
             //   Renderer rend = corals[i].GetComponent<Renderer>();
                Renderer rend = c.GetComponent<Renderer>();
                float _time = 0;
                _colorFadeTime = UnityEngine.Random.Range(1, 5);

                Color _currentColor = rend.material.color;

                while (_time < 1)
                {
                    _time += Time.deltaTime / _colorFadeTime;
                    rend.material.color = Color.Lerp(_currentColor, Color.white, _time);
                    yield return null;
                }

               // yield return new WaitForSeconds(delay);
            }

            yield return new WaitForSeconds(delay);

            _fishFoodChainLevel = 1;

            RemoveFish(_fishFoodChainLevel, false);
        }

        //by calling fish int and bool this can be recycled to bring fish back
        private void RemoveFish(int fishStrata, bool state)
        {  

            switch(fishStrata){
                case 1:
                    StartCoroutine(RemoveFish(bottomLevelFish, state));
                break;
                case 2:
                    StartCoroutine(RemoveFish(midLevelFish, state));
                    break;
                case 3:
                    StartCoroutine(RemoveFish(apexPreditors, state));
                    break;
                case 4:
                    ReefBleached?.Invoke();
                    break;

            }
        }

        IEnumerator RemoveFish(List<GameObject> fish, bool state)
        {
         
            for (int i = 0; i < fish.Count; i++)
            {
                float delay = UnityEngine.Random.Range(3, 5);
                fish[i].gameObject.SetActive(state);
                yield return new WaitForSeconds(delay);
                     
            }

            yield return new WaitForSeconds(1);

            _fishFoodChainLevel += 1;

            RemoveFish(_fishFoodChainLevel, state);
        }

        // this brings fish back when coral count reaches three in fish selector
        private void BringFishBack()
        {
            _fishFoodChainLevel = 1;
            RemoveFish(_fishFoodChainLevel, true);
        }



    }
   
}
