using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Fish
{
   
    public class FishInfo : MonoBehaviour
    {
        public delegate void DisplayFishInfo(string name, string information);
        public static event DisplayFishInfo DisplayInfo;

        [SerializeField] FishSwim _fishSwim;

        [TextArea ( 1, 1)]
        [SerializeField] private string _fishName;
        [TextArea(2, 5)]
        [SerializeField] private string _fishInformation;

        // Start is called before the first frame update
        void Start()
        {
            _fishSwim = this.GetComponent<FishSwim>();
        }

       public void PresentFish() 
        {
            _fishSwim.state = FishSwim.FishState.isPresenting;
        }

        public void DisplayFishInformation()
        {
            DisplayInfo(_fishName, _fishInformation);
        }
    }
}
