// Copyright (C) Threetee Gang All Rights Reserved 

using Assets.Scripts.Messaging;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Menu;

public class InGameMenuComponent 
    : UIComponent
{
    private UnityMessageEventHandle<RequestInGameMenuActivationMessage> _requestInGameMenuActivationHandle;
    private UnityMessageEventHandle<RequestInGameMenuDeactivationMessage> _requestInGameMenuDeactivationHandle;

    protected override void OnStart()
    {
        gameObject.SetActive(false);

        RegisterMessages();
    }

    protected override void OnEnd()
    {
        UnregisterMessages();
    }

    private void RegisterMessages()
    {
        _requestInGameMenuActivationHandle = Dispatcher.RegisterForMessageEvent<RequestInGameMenuActivationMessage>(OnRequestInGameMenuActivation);
        _requestInGameMenuDeactivationHandle = Dispatcher.RegisterForMessageEvent<RequestInGameMenuDeactivationMessage>(OnRequestInGameMenuDeactivation);
    }

    private void UnregisterMessages()
    {
        Dispatcher.UnregisterForMessageEvent(_requestInGameMenuDeactivationHandle);
        Dispatcher.UnregisterForMessageEvent(_requestInGameMenuActivationHandle);
    }

    private void OnRequestInGameMenuActivation(RequestInGameMenuActivationMessage inMessage)
    {
        gameObject.SetActive(true);
    }

    private void OnRequestInGameMenuDeactivation(RequestInGameMenuDeactivationMessage inMessage)
    {
        gameObject.SetActive(false);
    }
}
