using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Navigation
{
    public class GuidePath : MonoBehaviour
    {

        public List<Transform> path = new List<Transform>();


        // Start is called before the first frame update
        void Start()
        {

        }


        public void ReverseGuideRoute()
        {
            path.Reverse();
        }

    
    }

}
