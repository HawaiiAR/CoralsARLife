using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fish;
using System;

namespace Fish
{
    public class FishFeedControl : MonoBehaviour
    {

        [SerializeField] private GameObject _camera;
        [SerializeField] private GameObject _food;
        GameObject _foodPellet;

        public List<GameObject> _fish = new List<GameObject>();
        public List<GameObject> _feedingFish = new List<GameObject>();
        int _hungryFish;

        FishSwim swim;
        bool canFeedFish;

        private void Awake()
        {
            FoodPellet.FoodGone += ClearFeedingFish;
        }

        private void OnDisable()
        {
            FoodPellet.FoodGone -= ClearFeedingFish;
        }
        //releases a food pelet the pelet has a script that pushes it forward
        public void InstantiateFood()
        {
            if (canFeedFish)
            {
                Vector3 _instantiationPos =
                    new Vector3(_camera.transform.position.x,
                                _camera.transform.position.y,
                                _camera.transform.position.z + 1.5f
                                 );

                _foodPellet = Instantiate(_food, _instantiationPos, _camera.transform.rotation);

                FindHungryFish();
                canFeedFish = false;
            }
        }

        //gets a random selection from the fish in the scene then picks some of those fish to swim towards the fud
        private void FindHungryFish()
        {
            _hungryFish = UnityEngine.Random.Range(5, _fish.Count);
            _fish.AddRange(GameObject.FindGameObjectsWithTag("Fish"));
            _fish.AddRange(GameObject.FindGameObjectsWithTag("SchoolingFish"));

            //creates a list of random hungry fish to go get food
            _feedingFish = GetRandomHungryFish(_fish, _hungryFish);

            //feeds those hungry fish
            Invoke(nameof(FeedTheHungryFish), 1);
        }

        //sets the move target of the hungry fish to the fod pelet
        private void FeedTheHungryFish()
        {
            for (int i = 0; i < _feedingFish.Count; i++)
            {
                swim = _feedingFish[i].GetComponent<FishSwim>();
                Debug.Log(_feedingFish[i].name);
                swim.isFeeding = true;
                swim.food = _foodPellet;
                swim.state = FishSwim.FishState.isBeingFed;
            }

        }

        //gets a random number of fish that are hungry
        List<T> GetRandomHungryFish<T>(List<T> fish, int hungryFishCount)
        {
            List<T> _feedingFish = new List<T>();
            for (int i = 0; i < hungryFishCount; i++)
            {
                int index = UnityEngine.Random.Range(0, fish.Count);
                _feedingFish.Add(fish[index]);
            }
            Debug.Log("feeding fish");
            return _feedingFish;
        }

        //when a pelet is destroyed the hungry fish are released and sent back to feed
        private void ClearFeedingFish()
        {
            _feedingFish.Clear();
            canFeedFish = true;
        }
    }
}
