// Copyright (C) Threetee Gang All Rights Reserved

using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Assets.Scripts.Localisation
{
    public class LocalisationDropdown 
        : Dropdown
    {
        private IDictionary<int, ELanguageOptions> _languageMappings;

        protected override void Start ()
        {
            options.Clear();

            base.Start();

            _languageMappings = new Dictionary<int, ELanguageOptions>();

            var currentOption = 0;
            foreach (ELanguageOptions languageOption in Enum.GetValues(typeof(ELanguageOptions)))
            {
                options.Add(new OptionData(languageOption.ToString()));
                _languageMappings.Add(currentOption, languageOption);
                currentOption++;
            }

            onValueChanged.AddListener(OnSelection);
        }

        public void OnSelection(int inSelection)
        {
            LocalisationManager.CurrentLocalisationInterface.SetCurrentLanguage(_languageMappings[inSelection]);
        }
    }
}
