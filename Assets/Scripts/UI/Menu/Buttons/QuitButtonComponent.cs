// Copyright (C) Threetee Gang All Rights Reserved 

using Assets.Scripts.UnityLayer;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu.Buttons
{
    [RequireComponent(typeof(Button))]
    public class QuitButtonComponent 
        : MonoBehaviour
    {
        private void Start ()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(CoreUnityFunctions.CloseGame);
        }
    }
}
