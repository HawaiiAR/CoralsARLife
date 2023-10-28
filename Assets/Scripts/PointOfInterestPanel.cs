using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Info
{
    public class PointOfInterestPanel : MoveCanvasToPosition
    {
        [TextArea(1, 1)]
        [SerializeField] private string _infoTitle;
        [TextArea(10, 10)]
        [SerializeField] private string _info;



        protected override void Start()
        {
            base.Start();
            base.fishName.text = _infoTitle;
            base.fishInfo.text = _info;
        }

        protected override void GrowCanvas()
        {
            base.GrowCanvas();
        }

        protected override void ShrinkCanvas()
        {
            base.ShrinkCanvas();
        }

        private void OnTriggerEnter(Collider other)
        {
           // if(other.TryGetComponent<GuidFish>)
        }
    }
}
