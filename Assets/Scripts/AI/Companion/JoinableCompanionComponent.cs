// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Interaction;
using UnityEngine;

namespace Assets.Scripts.AI.Companion
{
    [RequireComponent(typeof(ICompanionInterface), typeof(Collider2D), typeof(SpriteRenderer))]
    public class JoinableCompanionComponent 
        : InteractableComponent
    {
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
            inGameObject.GetComponent<ICompanionSetInterface>().SetCompanion(gameObject.GetComponent<ICompanionInterface>(), ECompanionSlot.Primary);
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
