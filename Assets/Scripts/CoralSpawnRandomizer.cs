using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoralSpawnRandomizer : MonoBehaviour
{

    [SerializeField] private GameObject[] _coralsToSpawn;


    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject c in _coralsToSpawn)
        {
            c.SetActive(false);
        }
    }

    private void OnEnable()
    {
        int randCoral = Random.Range(0, _coralsToSpawn.Length);
        _coralsToSpawn[randCoral].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
