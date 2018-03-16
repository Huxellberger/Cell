// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Character;
using UnityEngine;

namespace Assets.Scripts.AI.Vision
{
    public class GuardVisionComponent 
        : VisionComponent
    {
        protected override bool IsSuspicious(GameObject inDetectedObject)
        {
            return inDetectedObject.GetComponent<CharacterComponent>() != null;
        }
    }
}
