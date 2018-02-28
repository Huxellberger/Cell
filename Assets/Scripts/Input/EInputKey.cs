// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Input
{
    public enum EInputKey
    {
        // Movement
        SprintButton,
        JumpButton,
        VerticalAnalog,
        HorizontalAnalog,

        // Camera
        CameraHorizontal,
        CameraVertical,
        CameraZoom,
        CameraZoomReset,
        CameraToggle,

        // Functions
        TogglePause,
        Interact,
        PrimaryHeldAction,
        SecondaryHeldAction,
        DropHeldItem,

        // Animal
        PositiveAnimalCry,
        NegativeAnimalCry,

        // MouseSim
        VirtualLeftClick,
        VirtualRightClick,
        VirtualMiddleClick
    }
}
