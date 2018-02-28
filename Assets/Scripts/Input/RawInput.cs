// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Input
{
    [System.Serializable]
    public class RawInput
        : PlayerInput
    {
        private readonly string _inputName;

        public RawInput(string inInputName, EInputType inInputType)
            : base(inInputType)
        {
            _inputName = inInputName;
        }

        public string InputName { get { return _inputName; } }
    }
}
