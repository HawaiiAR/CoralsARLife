using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fish;

public class FishSelector : MonoBehaviour
{
    [SerializeField] private Camera _arCamera;
    [SerializeField] private float _rayDistance; 

    // Start is called before the first frame update
    void Start()
    {
        
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
            if(Physics.Raycast(ray, out hit, _rayDistance))
            {
                if (hit.collider.TryGetComponent(out FishInfo _fishInfo))
                {
                    _fishInfo.PresentFish();
                    Debug.Log("fishname" + _fishInfo.name);
                }
            }
        }
    }
}
