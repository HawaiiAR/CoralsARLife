using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bleaching {

    public class BleachingExperienceControl : MonoBehaviour
    {

        public List<GameObject> corals = new List<GameObject>();
        public List<GameObject> apexPreditors = new List<GameObject>();
        public List<GameObject> midLevelFish = new List<GameObject>();
        public List<GameObject> bottomLevelFish = new List<GameObject>();
        // Start is called before the first frame update

        [SerializeField] private float _colorFadeTime;
        private int _fishFoodChainLevel;


        void Start()
        {

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
            float delay = Random.Range(.5f, 1);
            foreach (GameObject c in corals)
            {
               
             //   float delay = Random.Range(.5f, 1);
             //   Renderer rend = corals[i].GetComponent<Renderer>();
                Renderer rend = c.GetComponent<Renderer>();
                float _time = 0;
                _colorFadeTime = Random.Range(1, 5);

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

            RemoveFish(_fishFoodChainLevel);
        }

        private void RemoveFish(int fishStrata)
        {  

            switch(fishStrata){
                case 1:
                    StartCoroutine(RemoveFish(bottomLevelFish));
                break;
                case 2:
                    StartCoroutine(RemoveFish(midLevelFish));
                    break;
                case 3:
                    StartCoroutine(RemoveFish(apexPreditors));
                    break;

            }
        }

        IEnumerator RemoveFish(List<GameObject> fish)
        {
            Debug.Log("RemoveFishStarted");
            for (int i = 0; i < fish.Count; i++)
            {
                float delay = Random.Range(3, 5);
                fish[i].gameObject.SetActive(false);
                yield return new WaitForSeconds(delay);
                     
            }

            yield return new WaitForSeconds(1);

            _fishFoodChainLevel += 1;

            RemoveFish(_fishFoodChainLevel);
        }
    }



   
}
