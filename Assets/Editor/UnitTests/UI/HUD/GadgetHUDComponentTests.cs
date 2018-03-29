// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Gadget;
using Assets.Scripts.Test.UI.HUD;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Editor.UnitTests.UI.HUD
{
    [TestFixture]
    public class GadgetHUDComponentTestFixture
    {
        public const string SpritePath = "Test/Sprites/TestSprite";

        private Image _image;
        private Text _text;
        private TestGadgetHUDComponent _gadget;
        private Sprite _sprite;

        [SetUp]
        public void BeforeTest()
        {
            _image = new GameObject().AddComponent<Image>();
            _text = new GameObject().AddComponent<Text>();

            _gadget = _image.gameObject.AddComponent<TestGadgetHUDComponent>();

            _gadget.GadgetCount = _text;
            _gadget.GadgetImage = _image;

            _sprite = Resources.Load<Sprite>(SpritePath);
        }

        [TearDown]
        public void AfterTest()
        {
            _sprite = null;

            _gadget = null;

            _text = null;
            _image = null;
        }

        [Test]
        public void OnStart_ImageIsNullSpriteAndTextIsZero()
        {
            _gadget.TestStart();

            Assert.IsNull(_image.sprite);
            Assert.AreEqual("0", _text.text);

            _gadget.TestDestroy();
        }

        [Test]
        public void ReceiveGadgetUIMessage_ElementsTakeMessageProperties()
        {
            const int expectedCount = 3;

            _gadget.TestStart();

            _gadget.TestDispatcher.InvokeMessageEvent(new GadgetUpdatedUIMessage(_sprite, expectedCount));

            Assert.AreSame(_sprite, _image.sprite);
            Assert.AreEqual(expectedCount.ToString(), _text.text);

            _gadget.TestDestroy();
        }
    }
}
