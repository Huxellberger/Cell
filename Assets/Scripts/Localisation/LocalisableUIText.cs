// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine.UI;

namespace Assets.Scripts.Localisation
{
    public class LocalisableUIText 
        : Text
    {
        public LocalisationKey LocalisedTextKey;

        private LocalisedTextRef _cachedLocalisedTextRef;
        private bool _canLoadText = false;

        protected override void Start()
        {
            _canLoadText = true;
            base.Start();

            UpdateLocalisedText();
        }

        // OnEnable so it's refreshed on language change
        protected override void OnEnable()
        {
            UpdateLocalisedText();

            base.OnEnable();
        }

        private void UpdateLocalisedText()
        {
            if (LocalisedTextKey != null && _canLoadText)
            {
                if (_cachedLocalisedTextRef == null)
                {
                    _cachedLocalisedTextRef = new LocalisedTextRef(LocalisedTextKey);
                }

                text = _cachedLocalisedTextRef.ToString();
            }
        }
    }
}
