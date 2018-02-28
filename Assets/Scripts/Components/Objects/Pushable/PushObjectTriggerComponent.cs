// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Components.Species;
using Assets.Scripts.Components.Trigger;
using UnityEngine;

namespace Assets.Scripts.Components.Objects.Pushable
{
    public class PushObjectTriggerComponent 
        : TriggerComponent
    {
        public List<ESpeciesType> TriggeringSpeciesTypes = new List<ESpeciesType>();

        private readonly List<GameObject> _triggeringObjects = new List<GameObject>();

        protected override bool CanTrigger(GameObject inGameObject)
        {
            if (inGameObject != null)
            {
                if (IsAbleToPushObject(inGameObject))
                {
                    _triggeringObjects.Add(inGameObject);
                    if (_triggeringObjects.Count == 1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsAbleToPushObject(GameObject inGameObject)
        {
            // if a pushable block
            if (inGameObject.GetComponent<IPushableObjectInterface>() != null)
            {
                return true;
            }

            // or a member of the correct species
            var species = inGameObject.GetComponent<ISpeciesInterface>();
            if (species != null)
            {
                return TriggeringSpeciesTypes.Contains(species.GetCurrentSpeciesType());
            }

            return false;
        }

        protected override bool CanCancelTrigger(GameObject inGameObject)
        {
            if (_triggeringObjects.Contains(inGameObject))
            {
                _triggeringObjects.Remove(inGameObject);

                return _triggeringObjects.Count == 0;
            }

            return false;
        }
    }
}
