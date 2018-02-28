// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Input;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Input
{
    [TestFixture]
    public class InputManagerParserTestFixture
    {
        [Test]
        public void GetRawInputsFromInputManager_ParsesFileSuccessfully()
        {
            Assert.AreEqual(InputManagerParser.GetNumberOfInputsRegistered(), InputManagerParser.GetRawInputsFromInputManager().Count);
        }

        [Test]
        public void SaveRawInputsToFile()
        {
            InputManagerParser.WriteInputManagerToFile();
        }
    }
}

#endif
