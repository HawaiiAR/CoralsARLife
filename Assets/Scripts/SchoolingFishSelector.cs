using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish
{
    public class SchoolingFishSelector : MonoBehaviour
    {
        [SerializeField] private GameObject[] _fish;


        // Start is called before the first frame update
        void Awake()
        {
            foreach(GameObject f in _fish)
            {
                f.SetActive(false);
            }
        }

        private void OnEnable()
        {
            int randFish = Random.Range(0, _fish.Length);
            _fish[randFish].SetActive(true);
            
        }
    }
}
