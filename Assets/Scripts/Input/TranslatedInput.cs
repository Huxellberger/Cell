// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Input
{
    public class TranslatedInput
        : PlayerInput
    {
        private readonly EInputKey _inputKey;

        public TranslatedInput(EInputKey inInputKey, EInputType inInputType)
            : base(inInputType)
        {
            _inputKey = inInputKey;
        }

        public EInputKey InputKey { get { return _inputKey; } }

        public bool Pressed { get; set; }
        public float AxisValue { get; set; }
        public Vector3 Coordinate { get; set; }
    }
}
