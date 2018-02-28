// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;

namespace Assets.Scripts.Input
{
    public interface IInputMappingProviderInterface
    {
        IEnumerable<RawInput> GetRawInputs();
        TranslatedInput GetTranslatedInput(RawInput inRawInput);
    }
}
