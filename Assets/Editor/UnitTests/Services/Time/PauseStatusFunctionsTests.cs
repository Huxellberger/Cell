// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Services.Time;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Services.Time
{
    [TestFixture]
    public class PauseStatusFunctionsTestFixture
    {
        [Test]
        public void Invert_Paused_Unpaused()
        {
            Assert.AreEqual(EPauseStatus.Paused, PauseStatusFunctions.Invert(EPauseStatus.Unpaused));
        }

        [Test]
        public void Invert_Unpaused_Paused()
        {
            Assert.AreEqual(EPauseStatus.Unpaused, PauseStatusFunctions.Invert(EPauseStatus.Paused));
        }
    }
}
