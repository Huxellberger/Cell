﻿// Copyright (C) Threetee Gang All Rights Reserved 

// NOTE: Lifted from https://bitbucket.org/Unity-Technologies/ui/src/0155c39e05ca5d7dcc97d9974256ef83bc122586/UnityEngine.UI/EventSystem/InputModules/StandaloneInputModule.cs?at=5.2&fileviewer=file-view-default
// This the StandaloneInputCode refitted for our benefit since Unity made it open source kindly

using System;
using Assets.Scripts.UI.VirtualMouse;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Assets.Scripts.Input.Modules
{
    public enum EMouseCursorType
    {
        Hardware,
        Virtual
    }

    [AddComponentMenu("Event/Virtual Mouse Input Module")]
    public class VirtualMouseInputModule 
        : ExposedPointerInputModule
    {
        private float m_PrevActionTime;
        Vector2 m_LastMoveVector;
        int m_ConsecutiveMoveCount = 0;

        private Vector2 m_LastMousePosition;
        private Vector2 m_MousePosition;

        // Sifaka START: Support Virtual Mouse Input
        private readonly MouseState m_VirtualMouseState = new MouseState();
        private Vector2 m_LastVirtualMousePosition;
        private Vector2 m_VirtualMousePosition;

        private EMouseCursorType m_ActiveMouseType;

        private IVirtualMouseInterface m_CachedVirtualMouse;

        private bool _initialised = false;

        private IVirtualMouseInterface CachedVirtualMouse
        {
            get
            {
                if (m_CachedVirtualMouse == null)
                {
                    CachedVirtualMouse = VirtualMouseInstance.CurrentVirtualMouse;
                }

                return m_CachedVirtualMouse;
            }
            set { m_CachedVirtualMouse = value; }
        }
        // Sifaka END: Support Virtual Mouse Input

        protected VirtualMouseInputModule()
        { }

        [Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
        public enum InputMode
        {
            Mouse,
            Buttons
        }

        [Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
        public InputMode inputMode
        {
            get { return InputMode.Mouse; }
        }

        [SerializeField]
        private string m_HorizontalAxis = "Horizontal_Analog";

        /// <summary>
        /// Name of the vertical axis for movement (if axis events are used).
        /// </summary>
        [SerializeField]
        private string m_VerticalAxis = "Vertical_Analog";

        /// <summary>
        /// Name of the submit button.
        /// </summary>
        [SerializeField]
        private string m_SubmitButton = "Submit_Button";

        /// <summary>
        /// Name of the submit button.
        /// </summary>
        [SerializeField]
        private string m_CancelButton = "Cancel_Button";

        [SerializeField]
        private float m_InputActionsPerSecond = 10;

        [SerializeField]
        private float m_RepeatDelay = 0.5f;

        [SerializeField]
        [FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
        private bool m_ForceModuleActive;

        [Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead (UnityUpgradable) -> forceModuleActive")]
        public bool allowActivationOnMobileDevice
        {
            get { return m_ForceModuleActive; }
            set { m_ForceModuleActive = value; }
        }

        public bool forceModuleActive
        {
            get { return m_ForceModuleActive; }
            set { m_ForceModuleActive = value; }
        }

        public float inputActionsPerSecond
        {
            get { return m_InputActionsPerSecond; }
            set { m_InputActionsPerSecond = value; }
        }

        public float repeatDelay
        {
            get { return m_RepeatDelay; }
            set { m_RepeatDelay = value; }
        }

        /// <summary>
        /// Name of the horizontal axis for movement (if axis events are used).
        /// </summary>
        public string horizontalAxis
        {
            get { return m_HorizontalAxis; }
            set { m_HorizontalAxis = value; }
        }

        /// <summary>
        /// Name of the vertical axis for movement (if axis events are used).
        /// </summary>
        public string verticalAxis
        {
            get { return m_VerticalAxis; }
            set { m_VerticalAxis = value; }
        }

        public string submitButton
        {
            get { return m_SubmitButton; }
            set { m_SubmitButton = value; }
        }

        public string cancelButton
        {
            get { return m_CancelButton; }
            set { m_CancelButton = value; }
        }

        // Sifaka START: Support Virtual Mouse Input
        private void UpdateActiveMouse()
        {
            var hardwareMouseMoved = (m_MousePosition - m_LastMousePosition).sqrMagnitude > 0.0f;
            var virtualMouseMoved = (m_VirtualMousePosition - m_LastVirtualMousePosition).sqrMagnitude > 0.0f;

            if (CachedVirtualMouse == null || (hardwareMouseMoved && !virtualMouseMoved) || (!virtualMouseMoved && m_ActiveMouseType == EMouseCursorType.Hardware))
            {
                if (m_ActiveMouseType != EMouseCursorType.Hardware && CachedVirtualMouse != null)
                {
                    if (CachedVirtualMouse.IsMouseVisible())
                    {
                        CachedVirtualMouse.SetMouseVisibile(false);
                    }
                }
                m_ActiveMouseType = EMouseCursorType.Hardware;
            }
            else if (CachedVirtualMouse != null && virtualMouseMoved)
            {
                // Update virtual mouse to match hardware mouse
                if (m_ActiveMouseType != EMouseCursorType.Virtual)
                {
                    CachedVirtualMouse.SetMouseVisibile(true);
                    CachedVirtualMouse.SetVirtualMousePosition(m_MousePosition);
                }
                m_ActiveMouseType = EMouseCursorType.Virtual;
            }
        }

        private void InitialiseMousePosition()
        {    
            m_MousePosition = UnityEngine.Input.mousePosition;
            m_LastMousePosition = UnityEngine.Input.mousePosition;

            if (CachedVirtualMouse != null)
            {
                m_VirtualMousePosition = CachedVirtualMouse.GetVirtualMousePosition();
                m_LastVirtualMousePosition = CachedVirtualMouse.GetVirtualMousePosition();
            }

            m_ActiveMouseType = EMouseCursorType.Hardware;

            _initialised = true;
        }

        private void UpdateMousePosition()
        {
            m_LastMousePosition = m_MousePosition;
            m_MousePosition = UnityEngine.Input.mousePosition;

            if (CachedVirtualMouse != null)
            {
                m_LastVirtualMousePosition = m_VirtualMousePosition;
                m_VirtualMousePosition = CachedVirtualMouse.GetVirtualMousePosition();
            }
        }
        // Sifaka END: Support Virtual Mouse Input

        public override void UpdateModule()
        {
            // Sifaka START: Support Virtual Mouse Input
            if (_initialised)
            {
                UpdateMousePosition();
                UpdateActiveMouse();
            }
            // Sifaka END: Support Virtual Mouse Input
        }

        public override bool IsModuleSupported()
        {
            // Check for mouse presence instead of whether touch is supported,
            // as you can connect mouse to a tablet and in that case we'd want
            // to use StandaloneInputModule for non-touch input events.
            return m_ForceModuleActive || UnityEngine.Input.mousePresent;
        }

        public override bool ShouldActivateModule()
        {
            if (!base.ShouldActivateModule())
                return false;

            var shouldActivate = m_ForceModuleActive;
            UnityEngine.Input.GetButtonDown(m_SubmitButton);
            shouldActivate |= UnityEngine.Input.GetButtonDown(m_CancelButton);
            shouldActivate |= !Mathf.Approximately(UnityEngine.Input.GetAxisRaw(m_HorizontalAxis), 0.0f);
            shouldActivate |= !Mathf.Approximately(UnityEngine.Input.GetAxisRaw(m_VerticalAxis), 0.0f);
            shouldActivate |= (m_MousePosition - m_LastMousePosition).sqrMagnitude > 0.0f;
            shouldActivate |= UnityEngine.Input.GetMouseButtonDown(0);
            // Sifaka START: Support Virtual Mouse Input
            shouldActivate |= (m_VirtualMousePosition - m_LastVirtualMousePosition).sqrMagnitude > 0.0f;
            // Sifaka END: Support Virtual Mouse Input
            return shouldActivate;
        }

        public override void ActivateModule()
        {
            base.ActivateModule();

            // Sifaka START: Support Virtual Mouse Input
            InitialiseMousePosition();
            // Sifaka END: Support Virtual Mouse Input

            var toSelect = eventSystem.currentSelectedGameObject;
            if (toSelect == null)
                toSelect = eventSystem.firstSelectedGameObject;

            eventSystem.SetSelectedGameObject(toSelect, GetBaseEventData());
        }

        public override void DeactivateModule()
        {
            base.DeactivateModule();
            ClearSelection();
        }

        public override void Process()
        {
            bool usedEvent = SendUpdateEventToSelectedObject();

            if (eventSystem.sendNavigationEvents)
            {
                if (!usedEvent)
                    usedEvent |= SendMoveEventToSelectedObject();

                if (!usedEvent)
                    SendSubmitEventToSelectedObject();
            }

            ProcessMouseEvent();
        }

        /// <summary>
        /// Process submit keys.
        /// </summary>
        protected bool SendSubmitEventToSelectedObject()
        {
            if (eventSystem.currentSelectedGameObject == null)
                return false;

            var data = GetBaseEventData();
            if (UnityEngine.Input.GetButtonDown(m_SubmitButton))
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.submitHandler);

            if (UnityEngine.Input.GetButtonDown(m_CancelButton))
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.cancelHandler);
            return data.used;
        }

        private Vector2 GetRawMoveVector()
        {
            Vector2 move = Vector2.zero;
            move.x = UnityEngine.Input.GetAxisRaw(m_HorizontalAxis);
            move.y = UnityEngine.Input.GetAxisRaw(m_VerticalAxis);

            if (UnityEngine.Input.GetButtonDown(m_HorizontalAxis))
            {
                if (move.x < 0)
                    move.x = -1f;
                if (move.x > 0)
                    move.x = 1f;
            }
            if (UnityEngine.Input.GetButtonDown(m_VerticalAxis))
            {
                if (move.y < 0)
                    move.y = -1f;
                if (move.y > 0)
                    move.y = 1f;
            }
            return move;
        }

        /// <summary>
        /// Process keyboard events.
        /// </summary>
        protected bool SendMoveEventToSelectedObject()
        {
            float time = Time.unscaledTime;

            Vector2 movement = GetRawMoveVector();
            if (Mathf.Approximately(movement.x, 0f) && Mathf.Approximately(movement.y, 0f))
            {
                m_ConsecutiveMoveCount = 0;
                return false;
            }

            // If user pressed key again, always allow event
            bool allow = UnityEngine.Input.GetButtonDown(m_HorizontalAxis) || UnityEngine.Input.GetButtonDown(m_VerticalAxis);
            bool similarDir = (Vector2.Dot(movement, m_LastMoveVector) > 0);
            if (!allow)
            {
                // Otherwise, user held down key or axis.
                // If direction didn't change at least 90 degrees, wait for delay before allowing consequtive event.
                if (similarDir && m_ConsecutiveMoveCount == 1)
                    allow = (time > m_PrevActionTime + m_RepeatDelay);
                // If direction changed at least 90 degree, or we already had the delay, repeat at repeat rate.
                else
                    allow = (time > m_PrevActionTime + 1f / m_InputActionsPerSecond);
            }
            if (!allow)
                return false;

            // Debug.Log(m_ProcessingEvent.rawType + " axis:" + m_AllowAxisEvents + " value:" + "(" + x + "," + y + ")");
            var axisEventData = GetAxisEventData(movement.x, movement.y, 0.6f);
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
            if (!similarDir)
                m_ConsecutiveMoveCount = 0;
            m_ConsecutiveMoveCount++;
            m_PrevActionTime = time;
            m_LastMoveVector = movement;
            return axisEventData.used;
        }

        protected void ProcessMouseEvent()
        {
            ProcessMouseEvent(0);
        }

        /// <summary>
        /// Process all mouse events.
        /// </summary>
        protected void ProcessMouseEvent(int id)
        {
            var mouseData = GetMousePointerEventData(id);
            var leftButtonData = mouseData.GetButtonState(PointerEventData.InputButton.Left).eventData;

            // Process the first mouse button fully
            ProcessMousePress(leftButtonData);
            ProcessMove(leftButtonData.buttonData);
            ProcessDrag(leftButtonData.buttonData);

            // Now process right / middle clicks
            ProcessMousePress(mouseData.GetButtonState(PointerEventData.InputButton.Right).eventData);
            ProcessDrag(mouseData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
            ProcessMousePress(mouseData.GetButtonState(PointerEventData.InputButton.Middle).eventData);
            ProcessDrag(mouseData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);

            if (!Mathf.Approximately(leftButtonData.buttonData.scrollDelta.sqrMagnitude, 0.0f))
            {
                var scrollHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(leftButtonData.buttonData.pointerCurrentRaycast.gameObject);
                ExecuteEvents.ExecuteHierarchy(scrollHandler, leftButtonData.buttonData, ExecuteEvents.scrollHandler);
            }
        }

        // Sifaka START: Support Virtual Mouse Input
        protected override MouseState GetMousePointerEventData(int id)
        {
            // Normal flow if no virtual mouse
            if (m_ActiveMouseType == EMouseCursorType.Hardware)
            {
                return base.GetMousePointerEventData(id);
            }

            // Our shiny flow for virtual mouse
            PointerEventData leftData;
            var created = GetPointerData(kMouseLeftId, out leftData, true);

            leftData.Reset();

            if (created)
            {
                leftData.position = m_VirtualMousePosition;
            }

            Vector2 pos = m_VirtualMousePosition;
            leftData.delta = pos - leftData.position;
            leftData.position = pos;
            leftData.scrollDelta = new Vector2();
            leftData.button = PointerEventData.InputButton.Left;
            eventSystem.RaycastAll(leftData, m_RaycastResultCache);
            var raycast = FindFirstRaycast(m_RaycastResultCache);
            leftData.pointerCurrentRaycast = raycast;
            m_RaycastResultCache.Clear();

            // copy the apropriate data into right and middle slots
            PointerEventData rightData;
            GetPointerData(kMouseRightId, out rightData, true);
            CopyFromTo(leftData, rightData);
            rightData.button = PointerEventData.InputButton.Right;

            PointerEventData middleData;
            GetPointerData(kMouseMiddleId, out middleData, true);
            CopyFromTo(leftData, middleData);
            middleData.button = PointerEventData.InputButton.Middle;

            var data = CachedVirtualMouse.GetVirtualMouseData();

            m_VirtualMouseState.SetButtonState(PointerEventData.InputButton.Left, data.ButtonEntries[PointerEventData.InputButton.Left].StateForMouseButton(), leftData);
            m_VirtualMouseState.SetButtonState(PointerEventData.InputButton.Right, data.ButtonEntries[PointerEventData.InputButton.Right].StateForMouseButton(), rightData);
            m_VirtualMouseState.SetButtonState(PointerEventData.InputButton.Middle, data.ButtonEntries[PointerEventData.InputButton.Middle].StateForMouseButton(), middleData);
            // data.Reset();
            return m_VirtualMouseState;
        }
        // Sifaka END: Support Virtual Mouse Input

        protected bool SendUpdateEventToSelectedObject()
        {
            if (eventSystem.currentSelectedGameObject == null)
                return false;

            var data = GetBaseEventData();
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);
            return data.used;
        }

        /// <summary>
        /// Process the current mouse press.
        /// </summary>
        protected void ProcessMousePress(MouseButtonEventData data)
        {
            var pointerEvent = data.buttonData;
            var currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;

            // PointerDown notification
            if (data.PressedThisFrame())
            {
                pointerEvent.eligibleForClick = true;
                pointerEvent.delta = Vector2.zero;
                pointerEvent.dragging = false;
                pointerEvent.useDragThreshold = true;
                pointerEvent.pressPosition = pointerEvent.position;
                pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;

                DeselectIfSelectionChanged(currentOverGo, pointerEvent);

                // search for the control that will receive the press
                // if we can't find a press handler set the press
                // handler to be what would receive a click.
                var newPressed = ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.pointerDownHandler);

                // didnt find a press handler... search for a click handler
                if (newPressed == null)
                    newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

                // Debug.Log("Pressed: " + newPressed);

                float time = Time.unscaledTime;

                if (newPressed == pointerEvent.lastPress)
                {
                    var diffTime = time - pointerEvent.clickTime;
                    if (diffTime < 0.3f)
                        ++pointerEvent.clickCount;
                    else
                        pointerEvent.clickCount = 1;

                    pointerEvent.clickTime = time;
                }
                else
                {
                    pointerEvent.clickCount = 1;
                }

                pointerEvent.pointerPress = newPressed;
                pointerEvent.rawPointerPress = currentOverGo;

                pointerEvent.clickTime = time;

                // Save the drag handler as well
                pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(currentOverGo);

                if (pointerEvent.pointerDrag != null)
                    ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
            }

            // PointerUp notification
            if (data.ReleasedThisFrame())
            {
                // Debug.Log("Executing pressup on: " + pointer.pointerPress);
                ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

                // Debug.Log("KeyCode: " + pointer.eventData.keyCode);

                // see if we mouse up on the same element that we clicked on...
                var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

                // PointerClick and Drop events
                if (pointerEvent.pointerPress == pointerUpHandler && pointerEvent.eligibleForClick)
                {
                    ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
                }
                else if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
                {
                    ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.dropHandler);
                }

                pointerEvent.eligibleForClick = false;
                pointerEvent.pointerPress = null;
                pointerEvent.rawPointerPress = null;

                if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
                    ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);

                pointerEvent.dragging = false;
                pointerEvent.pointerDrag = null;

                // redo pointer enter / exit to refresh state
                // so that if we moused over somethign that ignored it before
                // due to having pressed on something else
                // it now gets it.
                if (currentOverGo != pointerEvent.pointerEnter)
                {
                    HandlePointerExitAndEnter(pointerEvent, null);
                    HandlePointerExitAndEnter(pointerEvent, currentOverGo);
                }
            }
        }
    }
}