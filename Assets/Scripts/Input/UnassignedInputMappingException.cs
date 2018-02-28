// Copyright (C) Threetee Gang All Rights Reserved

using System;

namespace Assets.Scripts.Input
{
    public class UnassignedInputMappingException : Exception
    {
        public UnassignedInputMappingException(RawInput unassignedInput)
            : base("Could not find an input mapped to" + unassignedInput.InputName)
        {
        }
    }
}
