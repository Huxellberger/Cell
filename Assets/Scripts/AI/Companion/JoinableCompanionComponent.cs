// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Interaction;
using Assets.Scripts.UnityLayer.GameObjects;
using UnityEngine;

namespace Assets.Scripts.AI.Companion
{
    public class JoinableCompanionComponent 
        : InteractableComponent
    {
        public GameObject CompanionPrefab;

        protected override bool CanInteractImpl(GameObject inGameObject)
        {
            if (inGameObject != null)
            {
                return inGameObject.GetComponent<ICompanionSetInterface>() != null;
            }

            return false;
        }

        protected override void OnInteractImpl(GameObject inGameObject)
        {
            inGameObject.GetComponent<ICompanionSetInterface>().SetCompanion(Instantiate(CompanionPrefab).GetComponent<ICompanionInterface>(), ECompanionSlot.Primary);
            gameObject.SetActive(false);
            DestructionFunctions.DestroyGameObject(gameObject);
        }
    }
}
