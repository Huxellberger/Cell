// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine.States.Transforming
{
    public class TransformingActionStateInfo 
        : ActionStateInfo
    {
        public readonly GameObject TransformTypePrefab;

        public TransformingActionStateInfo()
            : this(null, null)
        {
        }

        public TransformingActionStateInfo(GameObject inOwner, GameObject inTransformTypePrefab)
            : base(inOwner)
        {
            TransformTypePrefab = inTransformTypePrefab;
        }
    }
}
