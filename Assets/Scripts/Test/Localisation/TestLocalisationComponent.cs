// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Localisation;
using Assets.Scripts.Test.Input;

namespace Assets.Scripts.Test.Localisation
{
    public class TestLocalisationComponent 
        : LocalisationComponent
    {
        public MockPlayerPrefsRepository PlayerPrefsMock;

        public void TestAwake ()
        {
            if (PlayerPrefsMock == null)
            {
                PlayerPrefsMock = new MockPlayerPrefsRepository();
            }
            
		    Awake();
        }

        protected override void SetPlayerPrefsRepo()
        {
            PlayerPrefsRepo = PlayerPrefsMock;
        }

        public void TestDestroy()
        {
            OnDestroy();
        }
    }
}

#endif // UNITY_EDITOR
