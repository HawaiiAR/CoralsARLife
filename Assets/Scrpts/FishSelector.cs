using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Bleaching;
using TMPro;

namespace Fish
{

    public class FishSelector : MonoBehaviour
    {
        public static Action CoralRestored;

        [SerializeField] private Camera _arCamera;
        [SerializeField] private float _rayDistance;
        [SerializeField] private GameObject[] _corals;
        [SerializeField] private int _healthyCoral;

         GameObject _fish;

        bool _bleachingStarted;
        bool _bleached;
        bool _canPlaceCoral;
       

        // Start is called before the first frame update
        void Start()
        {
            BleachingExperienceControl.ReefBleached += PlaceCoral;
            BleachingExperienceControl.BleachingStarted += BleachingStarted;
            _fish = null;
        }

        private void OnDisable()
        {
            BleachingExperienceControl.ReefBleached -= PlaceCoral;
            BleachingExperienceControl.BleachingStarted -= BleachingStarted;
        }

        //keeps player from being able to select fish once bleaching has started
        private void BleachingStarted()
        {
            _bleachingStarted = true;
        }
        //allows players to seed coral and replenish the reef
        private void PlaceCoral()
        {
            _bleachingStarted = false;
            _bleached = true;
            _canPlaceCoral = true;
        }

       
        void Update()
        {
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            {
                //this  needs to be switched out for touch to work with mobile//////////////////////////////////////////////
                // Touch touch = Input.GetTouch(0);
                // Ray ray = _arCamera.ScreenPointToRay(touch.position);

                Ray ray = _arCamera.ScreenPointToRay(Input.mousePosition);
           
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, _rayDistance))
                {
                    if (!_bleached)
                    {
                        //allows a player to select fish if bleaching hasn't started
                        if (!_bleachingStarted && hit.collider.TryGetComponent(out FishInfo _fishInfo))
                        {
                            _fishInfo.PresentFish();   
                            
                        }
                    }
                    //allows player to place coral where the raycast hits the target
                    if (_canPlaceCoral && hit.collider.TryGetComponent<RockOrCoral>(out RockOrCoral rock))
                    {
                        var hitRot = Quaternion.LookRotation(hit.normal);
                        SeedCoral(hit.point, hitRot);
                    }
                }
            }

            // totally forgot I added this, now if coral is gone you should be able to tap and add new coral back into the scene(red cubes)
            if(_healthyCoral >= 3 && _bleached)
            {
                //this will start to bring coral back
                CoralRestored?.Invoke();
                _bleached = false;
            }
        }

        //ads new coral on previous coral at intersection of raycast
        private void SeedCoral(Vector3 coralSpawnPoint, Quaternion rotation)
        {
            int randCoral = UnityEngine.Random.Range(0, _corals.Length);
            Instantiate(_corals[randCoral], coralSpawnPoint, rotation);
            _healthyCoral += 1;
        }

        public void ReleaseFish()
        {
            FishSwim fishSwim = _fish.GetComponent<FishSwim>();
            fishSwim.state = FishSwim.FishState.isLookingForFood;
            _fish = null;
        }
    }
}
