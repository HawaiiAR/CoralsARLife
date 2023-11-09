using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Info;

namespace Fish
{
    public class BleachingGuideFishActivatore : MonoBehaviour
    {
       
        [SerializeField] private GameObject _pointOfInterestPanel;
        StoryTimeFish storyFish;
        // Start is called before the first frame update


        void Start()
        {
          
            _pointOfInterestPanel.SetActive(false);
        }

        private void OnEnable()
        {
            storyFish = GameObject.Find("BleachingGuideFish").GetComponent<StoryTimeFish>();
            storyFish.storyPoint = this.transform;
            storyFish.state = FishSwim.FishState.isSwimming;

        }
        private void ActivatePanel(bool state)
        {
            _pointOfInterestPanel.SetActive(state);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<StoryTimeFish>(out StoryTimeFish storyFish))
            {
                ActivatePanel(true);
            }
        }

        private void DeactivatePanel()
        {
            ActivatePanel(false);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<StoryTimeFish>(out StoryTimeFish storyFish))
            {
                Invoke(nameof(DeactivatePanel), 2);
            }
        }
    }
}
