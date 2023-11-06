using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish {
    public class AnimationControl : MonoBehaviour
    {
        private Animator _anim;
        private float _swimSpeed;
        private FishSwim _fishSwimControl;


        private void OnEnable()
        {
            _anim = GetComponentInChildren<Animator>();
            _fishSwimControl = GetComponent<FishSwim>();
        }

        // Update is called once per frame
        void Update()
        {
            _swimSpeed = _fishSwimControl.averageSwimSpeed;
            _anim.SetFloat("SwimSpeed", _swimSpeed);
        }
    }
}
