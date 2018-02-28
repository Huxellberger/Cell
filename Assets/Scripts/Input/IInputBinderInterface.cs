// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Input
{
    public interface IInputBinderInterface
    {
        void SetInputInterface(IInputInterface inInputInterface);
        void RegisterInputHandler(InputHandler inInputHandler);
        void UnregisterInputHandler(InputHandler inInputHandler);
    }
}
