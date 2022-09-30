using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Funksoft.Barista
{
    public class Pointer : MonoBehaviour
    {
        private Vector2 _mousePos;
        private Camera _mainCamera;
        ClickableObject _clickableObject = null;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                //Find reference to clicked object
                RaycastHit2D result = Physics2D.Raycast(_mousePos, Vector2.zero);
                if (!result)
                    return;
                result.collider?.TryGetComponent<ClickableObject>(out _clickableObject);

                //Only activate if object exists and is not held activation object
                if (!_clickableObject)
                    return;
                if (!_clickableObject.GetRepeatIfHeld())
                    _clickableObject.Activate();
            }

            //Skip handling clicked object interactions if there is no clicked object.
            if (_clickableObject == null)
                return;

            //Continue activation every frame of input, if Hold Repeat is enabled
            if (Input.GetMouseButton(0))
            {
                if (!_clickableObject.GetRepeatIfHeld())
                    return;
                _clickableObject.Activate();
            }

            if (Input.GetMouseButtonUp(0))
            {
                _clickableObject = null;
            }

        }

        //Cursor follow mouse position
        private void LateUpdate()
        {
            _mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = _mousePos;
        }




        
    }
}
