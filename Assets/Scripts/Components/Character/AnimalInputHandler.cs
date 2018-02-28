// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Species;
using Assets.Scripts.Input;

namespace Assets.Scripts.Components.Character
{
    public class AnimalInputHandler 
        : InputHandler
    {
        private readonly ISpeciesInterface _species;

        public AnimalInputHandler(ISpeciesInterface inSpeciesInterface)
            : base()
        {
            _species = inSpeciesInterface;

            ButtonResponses.Add(EInputKey.PositiveAnimalCry, OnPositiveAnimalCry);
            ButtonResponses.Add(EInputKey.NegativeAnimalCry, OnNegativeAnimalCry);
        }

        private EInputHandlerResult OnPositiveAnimalCry(bool pressed)
        {
            if (_species != null)
            {
                if (pressed)
                {
                    _species.SpeciesCry(ECryType.Positive);
                }

                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnNegativeAnimalCry(bool pressed)
        {
            if (_species != null)
            {
                if (pressed)
                {
                    _species.SpeciesCry(ECryType.Negative);
                }

                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }
    }
}
