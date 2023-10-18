using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Navigation
{

    public class Navigation : MonoBehaviour
    {
        public static Action<Transform> UpdateDirection;

        [SerializeField] private Transform[] _locations;
        [SerializeField] private Button[] _buttons;

        // Start is called before the first frame update
        void Start()
        {
            _buttons[0].onClick.AddListener(delegate { NewDirection(0); });
            _buttons[1].onClick.AddListener(delegate { NewDirection(1); });
            _buttons[2].onClick.AddListener(delegate { NewDirection(2); });
            _buttons[3].onClick.AddListener(delegate { NewDirection(3); });
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected virtual void NewDirection(int location)
        {
            switch (location)
            {
                case 0:
                    UpdateDirection?.Invoke(_locations[0]);
                    break;
                case 1:
                    UpdateDirection?.Invoke(_locations[1]);
                    break;
                case 2:
                    UpdateDirection?.Invoke(_locations[2]);
                    break;
                case 3:
                    UpdateDirection?.Invoke(_locations[3]);
                    break;
            }
        }
    }
}
