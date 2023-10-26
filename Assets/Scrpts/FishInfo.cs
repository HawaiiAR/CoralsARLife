using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Info;

namespace Fish
{
    //this is a required component of all fish and needs to be filled out on a prefab basis so when a fish is
    //selected the proper info shows
    public class FishInfo : MonoBehaviour
    {
        public delegate void DisplayFishInfo(string name, string information);
        public static event DisplayFishInfo DisplayInfo;

        //gets the fishcontrol of the prefab so the fish can be told to swim to the player
        [SerializeField] FishSwim _fishSwim;

        //these should be filled in in the inspector with proper info
        [TextArea(1, 1)]
        [SerializeField] private string _fishName_txt;
        [TextArea(2, 5)]
        [SerializeField] private string _fishInformation_txt;

        [SerializeField] private GameObject _infoCanvas;
        MoveCanvasToPosition _moveCanvas;

     
        // Start is called before the first frame update
        void Start()
        {
            _infoCanvas.SetActive(false);
            _fishSwim = this.GetComponent<FishSwim>();
            _moveCanvas = _infoCanvas.GetComponent<MoveCanvasToPosition>();
          
        }

        //gets the fish this is attatched to and makes it move to the presentation point
        public void PresentFish()
        {
            _fishSwim.FishSelected();
        }

        //gets called when fish reaches display position
        public void DisplayFishInformation()
        {

            _infoCanvas.SetActive(true); 
            _moveCanvas.fishName.text = _fishName_txt;
            _moveCanvas.fishInfo.text = _fishInformation_txt;        
            _moveCanvas.fishName.text = _fishName_txt;
            _moveCanvas.fishInfo.text = _fishInformation_txt;

        }

    }
}
