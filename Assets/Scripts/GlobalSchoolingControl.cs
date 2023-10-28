using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish
{
    //this controls schooling fish behaviour should be used as a base for different schools of fish if we get that far
    public class GlobalSchoolingControl : MonoBehaviour
    {

        public GameObject[] fishSchool;
        public Vector3 fishTarget;
        public Transform tankCenter;
        public int tankSize;

        [SerializeField] private GameObject _fish;
        [SerializeField] private int _fishCount;
      

        private void Awake()
        {  
            fishSchool = new GameObject[_fishCount];
        }

        // Start is called before the first frame update
        void Start()
        {
            SetNewTarget();

            //instantiates a group of fish based on the _fish gameobject
            for (int i = 0; i < _fishCount; i++)
            {

                Vector3 startPos = new Vector3(
                    Random.Range(tankCenter.transform.position.x - tankSize, tankCenter.transform.position.x + tankSize),
                   Random.Range(tankCenter.transform.position.y - tankSize, tankCenter.transform.position.y + tankSize),
                   Random.Range(tankCenter.transform.position.z - tankSize, tankCenter.transform.position.z + tankSize));

                Vector3 offset = new Vector3(
                    Random.Range(tankCenter.transform.position.x - tankSize, tankCenter.transform.position.x + tankSize),
                   Random.Range(tankCenter.transform.position.y - tankSize, tankCenter.transform.position.y + tankSize),
                   Random.Range(tankCenter.transform.position.z - tankSize, tankCenter.transform.position.z + tankSize));

                fishSchool[i] = Instantiate(_fish, startPos, Quaternion.identity);
                fishSchool[i].transform.parent = tankCenter;
              
            }

        }

        // Update is called once per frame
        void Update()
        {
            //timer to set a new position every once in a while so the fish swim in different directions
            if (Random.Range(500, 1000) < 10)
            {
                SetNewTarget();

            }
        }

        //new target for fish to follow
        private void SetNewTarget()
        {
            Debug.Log("new target");
            fishTarget = new Vector3(
                 Random.Range(tankCenter.transform.position.x - tankSize, tankCenter.transform.position.x + tankSize),
                Random.Range(tankCenter.transform.position.y - tankSize, tankCenter.transform.position.y + tankSize),
                Random.Range(tankCenter.transform.position.z - tankSize, tankCenter.transform.position.z + tankSize));
        }
    }
}
