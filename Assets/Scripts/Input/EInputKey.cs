﻿// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Input
{
    public enum EInputKey
    {
        // Movement
        SprintButton,
        VerticalAnalog,
        HorizontalAnalog,

        // Camera
        CameraHorizontal,
        CameraZoom,
        CameraZoomReset,

        // Functions
        TogglePause,
        Interact,
        PrimaryHeldAction,
        SecondaryHeldAction,
        DropHeldItem,

        // Companions
        PrimaryPower,
        SecondaryPower,
        PrimaryDialogue,
        SecondaryDialogue,

        // Animal
        PositiveAnimalCry,
        NegativeAnimalCry,

        // Gadget
        UseActiveGadget,
        CycleGadgetPositive,
        CycleGadgetNegative,

        // MouseSim
        VirtualLeftClick,
        VirtualRightClick,
        VirtualMiddleClick
    }
}
