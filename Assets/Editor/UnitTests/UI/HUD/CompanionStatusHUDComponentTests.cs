// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.AI.Companion;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.UI.HUD;
using Assets.Scripts.UI.HUD;
using Castle.Core.Internal;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Editor.UnitTests.UI.HUD
{
    [TestFixture]
    public class CompanionStatusHUDComponentTestFixture
    {
        public const string SpritePath = "Test/Sprites/TestSprite";

        private TestCompanionStatusHUDComponent _status;
        private CompanionStatusHUDComponent.CompanionStatus _primaryStatusElement;
        private CompanionStatusHUDComponent.CompanionStatus _secondaryStatusElement;

        private Sprite _setSprite;

        [SetUp]
        public void BeforeTest()
        {
            _status = new GameObject().AddComponent<TestCompanionStatusHUDComponent>();
            _status.TestDispatcher = new UnityMessageEventDispatcher();
            _status.IconDisableAlpha = 0.3f;

            _primaryStatusElement = new CompanionStatusHUDComponent.CompanionStatus
            {
                CompanionSlider = new GameObject().AddComponent<Slider>(),
                CompanionSliderFill = new GameObject().AddComponent<Image>(),
                CompanionUseCountText = new GameObject().AddComponent<Text>()
            };

            _secondaryStatusElement = new CompanionStatusHUDComponent.CompanionStatus
            {
                CompanionSlider = new GameObject().AddComponent<Slider>(),
                CompanionSliderFill = new GameObject().AddComponent<Image>(),
                CompanionUseCountText = new GameObject().AddComponent<Text>()
            };

            _status.CompanionStates.Add(new CompanionStatusHUDComponent.CompanionStatusPairing{Slot = ECompanionSlot.Primary, Status = _primaryStatusElement});
            _status.CompanionStates.Add(new CompanionStatusHUDComponent.CompanionStatusPairing { Slot = ECompanionSlot.Secondary, Status = _secondaryStatusElement });

            _setSprite = Resources.Load<Sprite>(SpritePath);

            _status.TestStart();
        }
	
        [TearDown]
        public void AfterTest()
        {
            _status.TestDestroy();

            _setSprite = null;

            _secondaryStatusElement = null;
            _primaryStatusElement = null;
            _status = null;
        }
	
        [Test]
        public void ReceiveSlotsUpdatedUIMessage_UpdatesSlotsWithNewData() 
        {
            var primaryMessageData = new PriorCompanionSlotState{PriorCompanion = null, PriorActive = false, PriorUseCount = 2, PriorUIIcon = _setSprite, PriorCooldown = 1.0f};
            var secondaryMessageData = new PriorCompanionSlotState { PriorCompanion = null, PriorActive = true, PriorUseCount = 1, PriorUIIcon = _setSprite, PriorCooldown = 0.0f }; 

            _status.TestDispatcher.InvokeMessageEvent(new CompanionSlotsUpdatedUIMessage(new Dictionary<ECompanionSlot, PriorCompanionSlotState>
                {
                {ECompanionSlot.Primary, primaryMessageData },
                {ECompanionSlot.Secondary, secondaryMessageData }
                }
            ));

            AssertSlotUpdated(_primaryStatusElement, primaryMessageData);
            AssertSlotUpdated(_secondaryStatusElement, secondaryMessageData);
        }

        [Test]
        public void ReceiveSlotsUpdatedUIMessage_CountNonZero_SetTextColorToWhite()
        {
            var primaryMessageData = new PriorCompanionSlotState { PriorCompanion = null, PriorActive = false, PriorUseCount = 1, PriorUIIcon = _setSprite, PriorCooldown = 1.0f };
            var secondaryMessageData = new PriorCompanionSlotState { PriorCompanion = null, PriorActive = true, PriorUseCount = 1, PriorUIIcon = _setSprite, PriorCooldown = 0.0f };

            _status.TestDispatcher.InvokeMessageEvent(new CompanionSlotsUpdatedUIMessage(new Dictionary<ECompanionSlot, PriorCompanionSlotState>
                {
                    {ECompanionSlot.Primary, primaryMessageData },
                    {ECompanionSlot.Secondary, secondaryMessageData }
                }
            ));

            Assert.AreEqual(Color.white, _primaryStatusElement.CompanionUseCountText.color);
        }

        [Test]
        public void ReceiveSlotsUpdatedUIMessage_CountZero_SetTextColorToRed()
        {
            var primaryMessageData = new PriorCompanionSlotState { PriorCompanion = null, PriorActive = false, PriorUseCount = 0, PriorUIIcon = _setSprite, PriorCooldown = 1.0f };
            var secondaryMessageData = new PriorCompanionSlotState { PriorCompanion = null, PriorActive = true, PriorUseCount = 1, PriorUIIcon = _setSprite, PriorCooldown = 0.0f };

            _status.TestDispatcher.InvokeMessageEvent(new CompanionSlotsUpdatedUIMessage(new Dictionary<ECompanionSlot, PriorCompanionSlotState>
                {
                    {ECompanionSlot.Primary, primaryMessageData },
                    {ECompanionSlot.Secondary, secondaryMessageData }
                }
            ));

            Assert.AreEqual(Color.red, _primaryStatusElement.CompanionUseCountText.color);
        }

        [Test]
        public void ReceiveSlotsUpdatedUIMessage_CountUnlimited_SetTextColorToEmpty()
        {
            var primaryMessageData = new PriorCompanionSlotState { PriorCompanion = null, PriorActive = false, PriorUseCount = CompanionConstants.UnlimitedCharges, PriorUIIcon = _setSprite, PriorCooldown = 1.0f };
            var secondaryMessageData = new PriorCompanionSlotState { PriorCompanion = null, PriorActive = true, PriorUseCount = 1, PriorUIIcon = _setSprite, PriorCooldown = 0.0f };

            _status.TestDispatcher.InvokeMessageEvent(new CompanionSlotsUpdatedUIMessage(new Dictionary<ECompanionSlot, PriorCompanionSlotState>
                {
                    {ECompanionSlot.Primary, primaryMessageData },
                    {ECompanionSlot.Secondary, secondaryMessageData }
                }
            ));

            Assert.IsTrue(_primaryStatusElement.CompanionUseCountText.text.IsNullOrEmpty());
        }

        [Test]
        public void ReceiveSlotsUpdatedUIMessage_ImagePropertiesAreAsExpected()
        {
            var primaryMessageData = new PriorCompanionSlotState { PriorCompanion = null, PriorActive = false, PriorUseCount = CompanionConstants.UnlimitedCharges, PriorUIIcon = _setSprite, PriorCooldown = 1.0f };
            var secondaryMessageData = new PriorCompanionSlotState { PriorCompanion = null, PriorActive = true, PriorUseCount = 1, PriorUIIcon = _setSprite, PriorCooldown = 0.0f };

            _status.TestDispatcher.InvokeMessageEvent(new CompanionSlotsUpdatedUIMessage(new Dictionary<ECompanionSlot, PriorCompanionSlotState>
                {
                    {ECompanionSlot.Primary, primaryMessageData },
                    {ECompanionSlot.Secondary, secondaryMessageData }
                }
            ));

            Assert.AreEqual(Image.FillMethod.Vertical, _primaryStatusElement.CompanionSliderFill.fillMethod);
            Assert.AreEqual(Image.Type.Filled, _primaryStatusElement.CompanionSliderFill.type);
        }

        private void AssertSlotUpdated(CompanionStatusHUDComponent.CompanionStatus status,
            PriorCompanionSlotState state)
        {
            Assert.AreEqual(status.CompanionUseCountText.text, state.PriorUseCount.ToString());
            Assert.AreSame(status.CompanionSliderFill.sprite, state.PriorUIIcon);
            Assert.AreEqual(status.CompanionSlider.value, state.PriorCooldown);
        }
    }
}
