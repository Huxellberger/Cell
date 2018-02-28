// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Localisation;

namespace Assets.Scripts.Test.Localisation
{
    public class TestLocalisableUIText 
        : LocalisableUIText
    {
        public bool BlockStart = true;

        public void TestStart()
        {
            Start();
        }

        protected override void Start()
        {
            if (!BlockStart)
            {
                base.Start();
            }
        }
    }
}

#endif // UNITY_EDITOR
