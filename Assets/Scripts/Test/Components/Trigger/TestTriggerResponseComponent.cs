// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Trigger;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Trigger
{
    public class TestTriggerResponseComponent
        : TriggerResponseComponent
    {
        public GameObject OnTriggerGameObject { get; private set; }
        public GameObject OnCancelTriggerGameObject { get; private set; }

        public void TestStart()
        {
            Start();
        }

        public void TestDestroy()
        {
            OnDestroy();
        }

        protected override void OnTriggerImpl(TriggerMessage inMessage)
        {
            OnTriggerGameObject = inMessage.TriggeringObject;
        }

        protected override void OnCancelTriggerImpl(CancelTriggerMessage inMessage)
        {
            OnCancelTriggerGameObject = inMessage.TriggeringObject;
        }
    }
}

#endif // UNITY_EDITOR
