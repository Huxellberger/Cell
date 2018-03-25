// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.Test.UI.HUD
{
    public class TestToastNotificationHUDComponent
        : ToastNotificationHUDComponent
    {
        public UnityMessageEventDispatcher TestDispatcher;
        public AudioClip LastPlayedClip { get; private set; }

        private float _deltaTime;

        public void TestStart()
        {
            TestDispatcher = new UnityMessageEventDispatcher();
            Start();
        }

        public void TestDestroy()
        {
            OnDestroy();
        }

        public void TestUpdate(float deltaTime)
        {
            _deltaTime = deltaTime;
            Update();
        }

        protected override float GetDeltaTime()
        {
            return _deltaTime;
        }

        protected override void PlayAudio(AudioClip inClip)
        {
            LastPlayedClip = inClip;
        }

        protected override UnityMessageEventDispatcher GetUIDispatcher()
        {
            return TestDispatcher;
        }
    }
}

#endif // UNITY_EDITOR
