using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bleaching {

    public class AddToBleachingLists : MonoBehaviour
    {
        [SerializeField] private int _fishLevel;

        BleachingExperienceControl _bleachingControl;

        // Start is called before the first frame update
        void Awake()
        {
            _bleachingControl = FindObjectOfType<BleachingExperienceControl>();
        }

        private void OnEnable()
        {
            _bleachingControl.AddFishToList(_fishLevel, this.gameObject);
        }
    }
}
