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
        [SerializeField] public string fishName_txt;
        [TextArea(2, 5)]
        [SerializeField] public string fishInformation_txt;
        [TextArea(2, 5)]
        [SerializeField] protected string _storyTwoInfo_txt;

        [SerializeField] protected GameObject _infoCanvas;
        MoveCanvasToPosition _moveCanvas;

        public bool isStoryFish;
     
        // Start is called before the first frame update
        protected virtual void Start()
        {
            _infoCanvas.SetActive(false);
            _fishSwim = this.GetComponent<FishSwim>();
            _moveCanvas = _infoCanvas.GetComponent<MoveCanvasToPosition>();
          
        }

        //gets the fish this is attatched to and makes it move to the presentation point
        public virtual void PresentFish()
        {
            _fishSwim.FishSelected();
        }

        public void StoryToTell(int storyNum)
        {
            switch(storyNum)
            {
                case 0:
                    _moveCanvas.fishName.text = fishName_txt;
                    _moveCanvas.fishInfo.text = fishInformation_txt;
                    
                    break;
                case 1:
                    _moveCanvas.fishName.text = fishName_txt;
                    _moveCanvas.fishInfo.text = _storyTwoInfo_txt;
                   
                    break;
            }
            _infoCanvas.SetActive(true);
        }

        //gets called when fish reaches display position
        public virtual void DisplayFishInformation()
        {

            _infoCanvas.SetActive(true); 
         
         

        }

    }
}
