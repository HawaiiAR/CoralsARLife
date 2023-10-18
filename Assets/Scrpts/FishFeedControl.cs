using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fish;
using System;

public class FishFeedControl : MonoBehaviour
{
    
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _food;
    GameObject _foodPellet;
    
    public List<GameObject> _fish = new List<GameObject>();
    public List<GameObject> _feedingFish = new List<GameObject>();
    int _hungryFish;
    
  
    public void InstantiateFood()
    {
        Vector3 _instantiationPos =
            new Vector3(_camera.transform.position.x,
                        _camera.transform.position.y,
                        _camera.transform.position.z + .5f
                         );

        _foodPellet = Instantiate(_food, _instantiationPos, _camera.transform.rotation);
        FindHungryFish();
    }

    private void FindHungryFish()
    {
        
        _hungryFish = UnityEngine.Random.Range(5, _fish.Count);
        Debug.Log("hungryFish" + _hungryFish);
        _fish.AddRange(GameObject.FindGameObjectsWithTag("Fish"));
        _fish.AddRange(GameObject.FindGameObjectsWithTag("SchoolingFish"));

        _feedingFish = GetRandomHungryFish(_fish, _hungryFish);

        Invoke(nameof(FeedTheHungryFish), 2);
    }

    private void FeedTheHungryFish()
    {
        foreach (GameObject fish in _feedingFish)
        {
            FishSwim swim = fish.GetComponent<FishSwim>();
            swim.food = _foodPellet;
            swim.state = FishSwim.FishState.isBeingFed;
        }
    }

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


    private void ClearList()
    {
        _feedingFish.Clear();
    }
}
