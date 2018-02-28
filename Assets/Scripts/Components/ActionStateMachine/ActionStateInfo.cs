// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine
{
    public class ActionStateInfo
    {
        public ActionStateInfo()
        {
            Owner = null;
        }

        public ActionStateInfo(GameObject inOwner)
        {
            Owner = inOwner;
        }

        public GameObject Owner { get; set; }
    }
}
