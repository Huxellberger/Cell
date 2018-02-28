// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;

namespace Assets.Scripts.Input
{
    public class DefaultInputMappingProvider
        : IInputMappingProviderInterface
    {
        private readonly IEnumerable<RawInput> _rawInputs;
        private readonly IDictionary<RawInput, TranslatedInput> _inputMappings;

        public DefaultInputMappingProvider(IEnumerable<RawInput> inRawInputs, ITranslatedInputRepositoryInterface inTranslatedInputRepositoryInterface)
        {
            _rawInputs = inRawInputs;
            _inputMappings = inTranslatedInputRepositoryInterface.RetrieveMappingsForRawInputs(_rawInputs);
        }

        public IEnumerable<RawInput> GetRawInputs()
        {
            return _rawInputs;
        }

        // Note: Causing performance problems. Need to examine reason (possible expensive key generation?)
        public TranslatedInput GetTranslatedInput(RawInput inRawInput)
        {
            if (_inputMappings.ContainsKey(inRawInput))
            {
                return _inputMappings[inRawInput];
            }
            
            throw new UnassignedInputMappingException(inRawInput);
        }
    }
}
