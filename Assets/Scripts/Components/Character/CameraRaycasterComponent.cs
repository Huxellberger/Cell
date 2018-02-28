// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Components.Character
{
    public class CameraRaycasterComponent 
        : MonoBehaviour
    {
        public class OccludingObjectEntry
        {
            public readonly Material ActiveMaterial;
            public readonly Color InitialColor;

            public bool LiveEntry;
            
            public OccludingObjectEntry(Material inMaterial, Color inColor)
            {
                ActiveMaterial = inMaterial;
                InitialColor = inColor;

                LiveEntry = true;
            }
        }

        public Shader RenderShader;
        public float AlphaValue = 0.4f;
        public float DistanceCutoff = 2.0f;

        private readonly List<OccludingObjectEntry> _shaderEntries = new List<OccludingObjectEntry>(8);

        private Vector3 _raycastDirection;
        private float _raycastDistance;
        
        protected void Start()
        {
            _raycastDistance = Vector3.Distance(transform.position, transform.parent.position) - DistanceCutoff;
        }
	
        protected void Update ()
        {
            ResetEntries();

            _raycastDirection = transform.parent.position - transform.position;
            var hits = Physics.RaycastAll(transform.position, _raycastDirection, _raycastDistance);

            foreach (var hit in hits)
            {
                var rend = hit.transform.GetComponent<Renderer>();

                var matchingEntry = _shaderEntries.FirstOrDefault(entry => rend.material == entry.ActiveMaterial);

                if (matchingEntry != null)
                {
                    matchingEntry.LiveEntry = true;
                }
                else
                {
                    _shaderEntries.Add(new OccludingObjectEntry(rend.material, rend.material.color));
                }

                var alteredColor = rend.material.color;
                alteredColor.a = AlphaValue;

                rend.material.color = alteredColor;
                rend.material.shader = RenderShader;
            }

            RemoveInvalidEntries();
        }

        private void ResetEntries()
        {
            foreach (var shaderEntry in _shaderEntries)
            {
                shaderEntry.LiveEntry = false;
            }
        }

        private void RemoveInvalidEntries()
        {
            foreach (var shaderEntry in _shaderEntries)
            {
                if (!shaderEntry.LiveEntry)
                {
                    shaderEntry.ActiveMaterial.color = shaderEntry.InitialColor;
                }
            }

            _shaderEntries.RemoveAll(entry => !entry.LiveEntry);
        }
    }
}
