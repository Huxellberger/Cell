// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core;
using Assets.Scripts.UnityLayer.Storage;

namespace Assets.Scripts.Input
{
    public class DefaultTranslatedInputRepository 
        : ITranslatedInputRepositoryInterface
    {
        public readonly Dictionary<RawInput, TranslatedInput> DefaultMappings;
        private readonly IPlayerPrefsRepositoryInterface _playerPlayerPrefsRepositoryInterface;

        public DefaultTranslatedInputRepository(IPlayerPrefsRepositoryInterface inPlayerPlayerPrefsRepositoryInterface)
        {
            _playerPlayerPrefsRepositoryInterface = inPlayerPlayerPrefsRepositoryInterface;

            DefaultMappings = GetDefaultMappings();
        }

        public IDictionary<RawInput, TranslatedInput> RetrieveMappingsForRawInputs(IEnumerable<RawInput> inRawInputs)
        {
            // Multiple enumerations is fine, will usually fail on first if not saved and this is easier to follow
            if (inRawInputs == null || !inRawInputs.Any() || MappingsAreNotCustomised(inRawInputs))
            {
                return DefaultMappings;
            }

            var inputTranslations = new Dictionary<RawInput, TranslatedInput>();

            foreach (var rawInput in inRawInputs)
            {
                var potentialEnum =
                    EnumExtensions.TryParse<EInputKey>(
                        _playerPlayerPrefsRepositoryInterface.GetValueForKey(rawInput.InputName));

                if (potentialEnum.IsSet())
                {
                    inputTranslations.Add(rawInput, new TranslatedInput(potentialEnum.Get(), rawInput.InputType));
                }
                else
                {
                    return DefaultMappings;
                }
            }

            return inputTranslations;
        }

        private bool MappingsAreNotCustomised(IEnumerable<RawInput> inRawInputs)
        {
            return inRawInputs.Any(rawInput => _playerPlayerPrefsRepositoryInterface.GetValueForKey(rawInput.InputName) == null);
        }

        // These are the fallback mappings, you should change these if you update the InputManager
        public static Dictionary<RawInput, TranslatedInput> GetDefaultMappings()
        {
            return new Dictionary<RawInput, TranslatedInput>(new RawInputEqualityComparer())
            {
                {
                    new RawInput("Vertical_Analog", EInputType.Analog),
                    new TranslatedInput(EInputKey.VerticalAnalog, EInputType.Analog)
                },
                {
                    new RawInput("Horizontal_Analog", EInputType.Analog),
                    new TranslatedInput(EInputKey.HorizontalAnalog, EInputType.Analog)
                },
                {
                    new RawInput("Mouse X_Analog", EInputType.Analog),
                    new TranslatedInput(EInputKey.CameraHorizontal, EInputType.Analog)
                },
                {
                    new RawInput("Mouse ScrollWheel_Analog", EInputType.Analog),
                    new TranslatedInput(EInputKey.CameraZoom, EInputType.Analog)
                },
                {
                    new RawInput("MiddleMouse_Button", EInputType.Button),
                    new TranslatedInput(EInputKey.CameraZoomReset, EInputType.Button)
                },
                {
                    new RawInput("LeftShift_Button", EInputType.Button),
                    new TranslatedInput(EInputKey.SprintButton, EInputType.Button)
                },
                {
                    new RawInput("Cancel_Button", EInputType.Button),
                    new TranslatedInput(EInputKey.TogglePause, EInputType.Button)
                },
                {
                    new RawInput("Z_Button", EInputType.Button),
                    new TranslatedInput(EInputKey.VirtualLeftClick, EInputType.Button)
                },
                {
                    new RawInput("F_Button", EInputType.Button),
                    new TranslatedInput(EInputKey.Interact, EInputType.Button)
                },
                {
                    new RawInput("Q_Button", EInputType.Button),
                    new TranslatedInput(EInputKey.PrimaryPower, EInputType.Button)
                },
                {
                    new RawInput("E_Button", EInputType.Button),
                    new TranslatedInput(EInputKey.SecondaryPower, EInputType.Button)
                },
                {
                    new RawInput("X_Button", EInputType.Button),
                    new TranslatedInput(EInputKey.PrimaryDialogue, EInputType.Button)
                },
                {
                    new RawInput("C_Button", EInputType.Button),
                    new TranslatedInput(EInputKey.SecondaryDialogue, EInputType.Button)
                },
                {
                    new RawInput("lmb_Button", EInputType.Button),
                    new TranslatedInput(EInputKey.PrimaryHeldAction, EInputType.Button)
                },
                {
                    new RawInput("rmb_Button", EInputType.Button),
                    new TranslatedInput(EInputKey.SecondaryHeldAction, EInputType.Button)
                },
                {
                    new RawInput("R_Button", EInputType.Button),
                    new TranslatedInput(EInputKey.DropHeldItem, EInputType.Button)
                },
                {
                    new RawInput("1_Button", EInputType.Button),
                    new TranslatedInput(EInputKey.CycleGadgetPositive, EInputType.Button)
                },
                {
                    new RawInput("2_Button", EInputType.Button),
                    new TranslatedInput(EInputKey.CycleGadgetNegative, EInputType.Button)
                },
                {
                    new RawInput("G_Button", EInputType.Button),
                    new TranslatedInput(EInputKey.UseActiveGadget, EInputType.Button)
                }
            };
        }
    }
}
