// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Input
{
    [System.Serializable]
    public class PlayerInput
    {
        private readonly EInputType _inputType;

        public PlayerInput(EInputType inInputType)
        {
            _inputType = inInputType;
        }

        public EInputType InputType { get { return _inputType; } }
    }
}
