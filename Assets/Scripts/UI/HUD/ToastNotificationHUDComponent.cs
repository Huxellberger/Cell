// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Messaging;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class ToastNotificationHUDComponent 
	: UIComponent
{
    public Text DisplayText;
    public float TimePerNotification = 3.0f;

    private readonly List<DisplayToastUIMessage> _messageQueue = new List<DisplayToastUIMessage>();

    private AudioSource _audioSource;

    private DisplayToastUIMessage _displayedMessage;
    private UnityMessageEventHandle<DisplayToastUIMessage> _displayToastHandle;
    private float _currentTimePassedForNotification = 0.0f;

    protected override void OnStart()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        _displayToastHandle = Dispatcher.RegisterForMessageEvent<DisplayToastUIMessage>(OnDisplayToastUIMessage);
        gameObject.SetActive(false);
    }

    protected override void OnEnd()
    {
        Dispatcher.UnregisterForMessageEvent(_displayToastHandle);
        _audioSource = null;
    }

    protected void Update() 
	{
	    if (_displayedMessage != null)
	    {
	        var deltaTime = GetDeltaTime();

	        _currentTimePassedForNotification += deltaTime;

	        if (_currentTimePassedForNotification > TimePerNotification)
	        {
	            _displayedMessage = null;
	            if (_messageQueue.Count > 0)
	            {
	                UpdateDisplayedMessage(_messageQueue[0]);
                    _messageQueue.RemoveAt(0);
	            }
	            else
	            {
	                gameObject.SetActive(false);
	            }

	            _currentTimePassedForNotification = 0.0f;
	        }
	    }
	}

    protected virtual float GetDeltaTime()
    {
        return Time.deltaTime;
    }

    private void OnDisplayToastUIMessage(DisplayToastUIMessage inMessage)
    {
        if (_displayedMessage == null)
        {
            gameObject.SetActive(true);
            UpdateDisplayedMessage(inMessage);
        }
        else
        {
            _messageQueue.Add(inMessage);
        }
        
    }

    private void UpdateDisplayedMessage(DisplayToastUIMessage inMessage)
    {
        _displayedMessage = inMessage;
        DisplayText.text = inMessage.ToastText;
        PlayAudio(inMessage.ToastAudio);
    }

    protected virtual void PlayAudio(AudioClip inClip)
    {
        _audioSource.PlayOneShot(inClip);
    }
}
