// Copyright (C) Threetee Gang All Rights Reserved

using System;

namespace Assets.Scripts.Input
{
    public class InvalidInputHandlerException : Exception
    {
        public InvalidInputHandlerException(InputHandler inInputHandler)
            : base("Could not find an input mapped to" + inInputHandler)
        {
        }
    }
}
