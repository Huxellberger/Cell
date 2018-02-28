// Copyright (C) Threetee Gang All Rights Reserved 

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.VirtualMouse
{
    public class VirtualMouseComponent 
        : UIBehaviour
        , IVirtualMouseInterface
    {
        public Image MouseGraphic;
        public float MouseSpeed = 500.0f;

        private float _mouseMoveX = 0.0f;
        private float _mouseMoveY = 0.0f;

        private float _priorTime = 0.0f;

        private bool ? _priorMouseVisibilitySetting;

        private readonly VirtualMouseData _data = new VirtualMouseData();

        protected override void Awake()
        {
            base.Awake();
            SetMouseVisibile(false);
            VirtualMouseInstance.CurrentVirtualMouse = this;
            _priorTime = 0.0f;
            MouseGraphic.raycastTarget = false;
        }

        protected override void OnDestroy()
        {
            VirtualMouseInstance.CurrentVirtualMouse = null;
            base.OnDestroy();
        }

        protected void Update()
        {
            var deltaTime = GetDeltaTime();

            var movementVector = new Vector3(_mouseMoveX, _mouseMoveY, 0.0f);

            if (_mouseMoveX > 0.0f && _mouseMoveY > 0.0f)
            {
                movementVector = movementVector.normalized;
            }
            
            movementVector *= MouseSpeed * deltaTime;

            SetVirtualMousePosition(GetVirtualMousePosition() + movementVector);

            _mouseMoveX = 0.0f;
            _mouseMoveY = 0.0f;

            _data.Reset();
        }

        protected virtual float GetDeltaTime()
        {
            var deltaTime = Time.realtimeSinceStartup - _priorTime;
            _priorTime = Time.realtimeSinceStartup;
            return deltaTime;
        }

        // IVirtualMouseInterface
        public Vector3 GetVirtualMousePosition()
        {
            return MouseGraphic.rectTransform.position;
        }

        public void SetVirtualMousePosition(Vector3 inPosition)
        {
            MouseGraphic.rectTransform.position = new Vector3(inPosition.x, inPosition.y, gameObject.transform.position.z);
        }

        public void SetMouseVisibile(bool isVisible)
        {
            if (isVisible)
            {
                _priorMouseVisibilitySetting = Cursor.visible;
                Cursor.visible = false;
            }
            else if (_priorMouseVisibilitySetting != null)
            {
                Cursor.visible = _priorMouseVisibilitySetting.Value;
            }

            MouseGraphic.enabled = isVisible;
        }

        public bool IsMouseVisible()
        {
            return MouseGraphic.enabled;
        }

        public VirtualMouseData GetVirtualMouseData()
        {
            return _data;
        }

        public void ApplyHorizontalMovement(float inValue)
        {
            _mouseMoveX = Mathf.Clamp(inValue, -1.0f, 1.0f);
        }

        public void ApplyVerticalMovement(float inValue)
        {
            _mouseMoveY = Mathf.Clamp(inValue, -1.0f, 1.0f);
        }

        public void SetButtonState(PointerEventData.InputButton inButton, bool isPressed)
        {
            _data.ButtonEntries[inButton].UpdateButton(isPressed);
        }
        // ~IVirtualMouseInterface
    }
}
