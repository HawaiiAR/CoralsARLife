using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish
{
    public class SchoolingFishSelector : MonoBehaviour
    {
        [SerializeField] private GameObject[] _fish;
        [SerializeField] private FishInfo _fishInfo;


        // Start is called before the first frame update
        void Awake()
        {
            _fishInfo = GetComponent<FishInfo>();
            foreach(GameObject f in _fish)
            {
                f.SetActive(false);
            }
        }

        private void OnEnable()
        {
            int randFish = Random.Range(0, _fish.Length);
            _fish[randFish].SetActive(true);
            SetText(randFish);
        }

        private void SetText(int randFish)
        {
            switch (randFish)
            {
                case 0:
                    _fishInfo.fishName_txt = "Morish Idol";
                    _fishInfo.fishName_txt = "Renowned for its vibrant bands and graceful movements, the Moorish idol brings elegance to reefs.";
                    break;
                case 1:
                    _fishInfo.fishName_txt = "Long-nosed butterflyfish:";
                    _fishInfo.fishName_txt = "With its distinctive elongated snout, the long-nosed butterflyfish gracefully navigates coral reefs in search of nourishmen";
                    break;


            }
        }
    }
}
