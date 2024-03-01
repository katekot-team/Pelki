using System;
using UnityEngine;

namespace Pelki.Gameplay.Inputs
{
    public class PuzzleInput : MonoBehaviour, IPuzzleInput
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float deadZone = 80;
        
        private Vector2 _tapPosition;
        private Vector2 _swipeDelta;
        
        private bool _isSwiping;
        private bool _isEditor = false;
        
        public event Action<Vector2, GameObject> Swiped;

        private void Start()
        {
#if UNITY_EDITOR
            _isEditor = true;
#endif
        }

        private void Update()
        {
            if (_isEditor)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _isSwiping = true;
                    _tapPosition = Input.mousePosition;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    ResetSwipe();
                }
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        _isSwiping = true;
                        _tapPosition = Input.GetTouch(0).position;
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Canceled ||
                             Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        ResetSwipe();
                    }
                }
            }
            
            CheckSwipe();
        }

        private void CheckSwipe()
        {
            _swipeDelta = Vector2.zero;

            if (_isSwiping)
            {
                if (_isEditor && Input.GetMouseButton(0))
                {
                    _swipeDelta = (Vector2)Input.mousePosition - _tapPosition;
                } else if (Input.touchCount > 0)
                {
                    _swipeDelta = Input.GetTouch(0).position - _tapPosition;
                }
            }

            if (_swipeDelta.magnitude > deadZone)
            {
                if (Swiped == null)
                {
                    return;
                }
                
                var pos = UnityEngine.Camera.main.ScreenToWorldPoint(_tapPosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
                if (hit != null && Mathf.Abs(_swipeDelta.x) > Mathf.Abs(_swipeDelta.y))
                {
                    GameObject touchedObject = hit.transform.gameObject;
                    if ((layerMask & (1 << touchedObject.gameObject.layer)) != 0)
                    {
                        Vector2 direction = _swipeDelta.x > 0 ? Vector2.right : Vector2.left;
                        Swiped.Invoke(direction, touchedObject);
                    }
                }
                
                ResetSwipe();
            }
        }

        private void ResetSwipe()
        {
            _isSwiping = false;
            _tapPosition = Vector2.zero;
            _swipeDelta = Vector2.zero;
        }
    }
}