// Copyright (C) Threetee Gang All Rights Reserved

using System;
using Assets.Scripts.Components.ActionStateMachine.States.Dead;
using Assets.Scripts.Components.Controller;
using Assets.Scripts.Messaging;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Spawn;
using Assets.Scripts.UnityLayer.GameObjects;
using UnityEngine;

namespace Assets.Scripts.Mode
{
    public class GameModeComponent : MonoBehaviour
    {
        public static GameModeComponent RegisteredGameMode { get; set; }

        public GameObject PlayerControllerType;
        public GameObject PlayerCharacterType;
        public GameObject HUDType;
        public Texture2D MouseTexture;

        public SpawnLocationComponent StartingSpawnLocation;

        public ControllerComponent ActiveController { get; protected set; }
        protected GameObject HUDInstance { get; set; }

        private UnityMessageEventHandle<RequestRespawnMessage> _requestRespawnHandle;
        private ISpawnServiceInterface _spawnService;

        protected void Awake()
        {
            RegisteredGameMode = this;

            Cursor.SetCursor(MouseTexture, Vector2.up, CursorMode.Auto);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

            _spawnService = GameServiceProvider.CurrentInstance.GetService<ISpawnServiceInterface>();

            HUDInstance = Instantiate(HUDType);
            InitialisePlayer();
            RegisterForMessages();
        }

        protected void OnDestroy()
        {
            DestructionFunctions.DestroyGameObject(HUDInstance);
            UnregisterForMessages();
            Cursor.visible = true;
        }

        private void InitialisePlayer()
        {
            var playerController = Instantiate(PlayerControllerType);
            ActiveController = playerController.GetComponent<ControllerComponent>();

            if (ActiveController == null)
            {
                throw new ApplicationException("Controller type was not valid! Needs a controller component!");
            }

            ActiveController.PawnInitialTransform = StartingSpawnLocation.GetSpawnLocation();
            ActiveController.CreatePawnOfType(PlayerCharacterType);
        }

        private void RegisterForMessages()
        {
            _requestRespawnHandle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<RequestRespawnMessage>(gameObject,
                    OnRequestRespawnMessage);
        }

        private void UnregisterForMessages()
        {
            UnityMessageEventFunctions.UnregisterActionWithDispatcher(gameObject, _requestRespawnHandle);
        }

        private void OnRequestRespawnMessage(RequestRespawnMessage inMessage)
        {
            if (ActiveController.PawnInstance == inMessage.RequestingPlayer)
            {
                ActiveController.PawnInitialTransform =
                    _spawnService.GetNearestSpawnLocation(inMessage.RequestingPlayer.transform.position);

                ActiveController.DestroyPawn();
                ActiveController.CreatePawnOfType(PlayerCharacterType);
            }
        }
    }
}
