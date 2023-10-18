using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Navigation;

namespace Fish
{
    public class GuidFishControl : MonoBehaviour
    {
        public enum GuideState
        {
            isGuiding,
            isWaiting
        }


      //  public Transform[] pathPoints;
        private GuideState _state;

        [SerializeField] private FishSwim _fishSwim;
        [SerializeField] private GuidePath _guidePath;
        [SerializeField] private float _guidSpeed;
        [SerializeField] private float _rotSpeed;

        private Transform _currentPathPoint;
        private Transform _nextPoint;
        private int _pathIndex = 0;

        float _pathPointDistance;
        float _playerDistance;
        Vector3 _pathPointDirection;
        Vector3 _playerDirection;

        private Transform _mainCamera;
        

        // Start is called before the first frame update
        void Start()
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();

            _fishSwim = GetComponent<FishSwim>();
            _fishSwim.enabled = false;
            _state = GuideState.isGuiding;
            NewPathPoint();
        }

        // Update is called once per frame
        void Update()
        {
            if (_state == GuideState.isGuiding)
            {
                MoveAlongPath();
            }

            if(_state == GuideState.isWaiting)
            {
                WaitForPlayer();
            }
        }

        private void NewPathPoint()
        {
            _nextPoint = _guidePath.path[_pathIndex];
        }
     

        private void MoveAlongPath()
        {
            CalculateDirectionAndDistance(_nextPoint.transform.position, _mainCamera.transform.position);

            if (_pathPointDistance < 1)
            {
                _pathIndex++;
                NewPathPoint();
            }

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_pathPointDirection), _rotSpeed * Time.deltaTime);
            transform.Translate(0, 0, _guidSpeed * Time.deltaTime);

            if (_playerDistance > 5)
            {
                _state = GuideState.isWaiting;
            }
        }

        private void WaitForPlayer()
        {
            Debug.Log("wait for player");
            CalculateDirectionAndDistance(_nextPoint.transform.position, _mainCamera.transform.position);

            _playerDirection.y = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_playerDirection),  _rotSpeed * Time.deltaTime);

            if (_playerDistance <= 3)
            {
                Debug.Log("new path point");
                NewPathPoint();
                _state = GuideState.isGuiding;
            }
        }

        private void CalculateDirectionAndDistance(Vector3 pathPoint, Vector3 player)
        {
            _pathPointDistance = Vector3.Distance(this.transform.position, pathPoint);
            _playerDistance = Vector3.Distance(this.transform.position, player);
            _pathPointDirection = pathPoint - this.transform.position;
            _playerDirection = player - this.transform.position;
        }
    }
}
