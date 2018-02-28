// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine.States.SurfaceSticking
{
    public class SurfaceStickingActionStateInfo
        : ActionStateInfo
    {
        public readonly GameObject Surface;

        public SurfaceStickingActionStateInfo()
            : this(null, null)
        {
        }

        public SurfaceStickingActionStateInfo(GameObject inOwner, GameObject inSurface)
            : base(inOwner)
        {
            Surface = inSurface;
        }
    }
}
