// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Objects.Pushable;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Objects.Pushable
{
    public class MockPushableObjectComponent
        : MonoBehaviour
        , IPushableObjectInterface
    {
        public Vector2 ? PushResult { get; private set; }

        public void Push(Vector2 inVector)
        {
            PushResult = inVector;
        }
    }
}

#endif // UNITY_EDITOR
