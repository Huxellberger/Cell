// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Localisation;
using Assets.Scripts.Messaging;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.HUD.Conversation
{
    public class DialogueHUDComponent 
        : UIComponent
    {
        public Image PortraitImage;
        public Text NameText;
        public Text DisplayedDialogueText;
        public AudioSource SoundSource;
        public float AdvanceDelay = 3.0f;

        private RequestDialogueUIMessage _currentMessage;
        private int _currentLine = 0;
        private int _currentCharacter = 0;
        private string _currentText = "";
        private float _newCharactersToDisplay = 0.0f;
        private float _advanceDelayPassed = 0.0f;

        private UnityMessageEventHandle<RequestDialogueUIMessage> _requestDialogueHandle;

        protected override void OnStart()
        {
            ClearMessage();
            _requestDialogueHandle =
                Dispatcher.RegisterForMessageEvent<RequestDialogueUIMessage>(OnRequestDialogueUIMessage);
        }

        protected void Update()
        {
            if (_currentMessage != null)
            {
                var deltaTime = GetDeltaTime();
                var addedCharacters = false;

                if (_currentCharacter < _currentText.Length)
                {
                    addedCharacters = true;
                    _newCharactersToDisplay += _currentMessage.Lines[_currentLine].DialogueSpeed * deltaTime;

                    if (_newCharactersToDisplay >= 1.0f)
                    {
                        PlayChatterSound(_currentMessage.Lines[_currentLine].TalkNoise);
                        _currentCharacter += (int)_newCharactersToDisplay;
                        _newCharactersToDisplay = 0.0f;
                    }
                }

                UpdateCurrentLine(deltaTime, addedCharacters);
            }
        }

        private void UpdateCurrentLine(float deltaTime, bool addedCharacters)
        {
            _currentCharacter = Mathf.Clamp(_currentCharacter, 0, _currentText.Length);
            DisplayedDialogueText.text = _currentText.Substring(0, _currentCharacter);

            if (_currentCharacter >= _currentText.Length && !addedCharacters)
            {
                _advanceDelayPassed += deltaTime;
                if (_advanceDelayPassed > AdvanceDelay)
                {
                    _advanceDelayPassed = 0.0f;
                    _currentLine++;
                    
                    StartNewLine();
                }
            }
        }

        protected virtual float GetDeltaTime()
        {
            return Time.deltaTime;
        }

        protected override void OnEnd()
        {
            Dispatcher.UnregisterForMessageEvent(_requestDialogueHandle);
        }

        private void OnRequestDialogueUIMessage(RequestDialogueUIMessage message)
        {
            if (_currentMessage == null || message.Priority > _currentMessage.Priority)
            {
                ClearMessage();
                _currentMessage = message;

                StartNewDialogue();
            }
        }

        private void StartNewDialogue()
        {
            if (_currentMessage.Lines.Count > 0)
            {
                gameObject.SetActive(true);
                _currentLine = 0;
                StartNewLine();
            }
        }

        private void StartNewLine()
        {
            if (_currentLine >= _currentMessage.Lines.Count)
            {
                ClearMessage();
                return;
            }
            
            PortraitImage.sprite = _currentMessage.Lines[_currentLine].Portrait;
            NameText.text = LocalisedTextFunctions.GetTextFromLocalisationKey(_currentMessage.Lines[_currentLine].NameKey);
            _currentText = LocalisedTextFunctions.GetTextFromLocalisationKey(_currentMessage.Lines[_currentLine].DialogueKey);
            DisplayedDialogueText.text = "";
            _currentCharacter = 0;
        }

        protected virtual void PlayChatterSound(AudioClip clip)
        {
            SoundSource.PlayOneShot(clip);
        }

        protected virtual void StopChatterSound()
        {
            SoundSource.Stop();
        }

        private void ClearMessage()
        {
            if (_currentMessage != null)
            {
                if (_currentMessage.DialogueCompleteDelegate != null)
                {
                    _currentMessage.DialogueCompleteDelegate();
                }
            }
            
            _currentMessage = null;
            DisplayedDialogueText.text = "";
            NameText.text = "";
            PortraitImage.sprite = null;
            
            gameObject.SetActive(false);
        }
    }
}
