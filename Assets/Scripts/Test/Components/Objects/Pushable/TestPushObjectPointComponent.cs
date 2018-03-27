// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Objects.Pushable;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Objects.Pushable
{
    public class TestPushObjectPointComponent 
        : PushObjectPointComponent
    {
        public bool ExtendedPushConditionsValidResult = true;

        protected override bool ExtendedPushConditionsValid(GameObject inGameObject)
        {
            return ExtendedPushConditionsValidResult;
        }
    }
}

#endif // UNITY_EDITOR
