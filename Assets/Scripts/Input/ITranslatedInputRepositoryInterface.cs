// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;

namespace Assets.Scripts.Input
{
    public interface ITranslatedInputRepositoryInterface
    {
        IDictionary<RawInput, TranslatedInput> RetrieveMappingsForRawInputs(IEnumerable<RawInput> inRawInputs);
    }
}
