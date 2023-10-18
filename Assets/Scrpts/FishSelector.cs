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

        bool _bleached;
        bool _canPlaceCoral;

        // Start is called before the first frame update
        void Start()
        {
            BleachingExperienceControl.ReefBleached += PlaceCoral;
            _fish = null;
        }

        private void OnDisable()
        {
            BleachingExperienceControl.ReefBleached -= PlaceCoral;
        }

        private void PlaceCoral()
        {
            _bleached = true;
            _canPlaceCoral = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            {
                //  Touch touch = Input.GetTouch(0);
                // Ray ray = _arCamera.ScreenPointToRay(touch.position);
                Ray ray = _arCamera.ScreenPointToRay(Input.mousePosition);
                // Debug.DrawRay()
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, _rayDistance))
                {
                    if (!_bleached)
                    {
                        if (hit.collider.TryGetComponent(out FishInfo _fishInfo))
                        {
                            _fishInfo.PresentFish();
                           // _fish = hit.collider.gameObject;
                            
                        }
                    }
                    if (_canPlaceCoral && hit.collider.TryGetComponent<RockOrCoral>(out RockOrCoral rock))
                    {
                        var hitRot = Quaternion.LookRotation(hit.normal);
                        SeedCoral(hit.point, hitRot);
                    }
                }
            }

            if(_healthyCoral >= 3 && _bleached)
            {
                CoralRestored?.Invoke();
                _bleached = false;
            }
        }

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
