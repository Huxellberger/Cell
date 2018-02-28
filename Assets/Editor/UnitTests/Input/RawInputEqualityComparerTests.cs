// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Input;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Input
{
    [TestFixture]
    public class RawInputEqualityComparerTestFixture {

        [Test]
        public void BothNull_True()
        {
            var comparer = new RawInputEqualityComparer();

            Assert.IsTrue(comparer.Equals(null, null));
        }

        [Test]
        public void OneNull_False()
        {
            var comparer = new RawInputEqualityComparer();
            var firstInput = new RawInput("Test", EInputType.Analog);

            Assert.IsFalse(comparer.Equals(firstInput, null));
        }

        [Test]
        public void SameObject_True()
        {
            var comparer = new RawInputEqualityComparer();
            var firstInput = new RawInput("Test", EInputType.Analog);
            var secondInput = firstInput;

            Assert.IsTrue(comparer.Equals(firstInput, secondInput));
        }

        [Test]
        public void DifferentObject_MatchingTypeAndName_True()
        {
            var comparer = new RawInputEqualityComparer();
            const string inputName = "TestName";
            const EInputType inputType = EInputType.Analog;

            var firstInput = new RawInput(inputName, inputType);
            var secondInput = new RawInput(inputName, inputType);

            Assert.IsTrue(comparer.Equals(firstInput, secondInput));
        }

        [Test]
        public void DifferentObject_MatchingTypeButDifferentName_False()
        {
            var comparer = new RawInputEqualityComparer();
            const string inputName = "TestName";
            const string otherName = "BadTimes";
            const EInputType inputType = EInputType.Analog;

            var firstInput = new RawInput(inputName, inputType);
            var secondInput = new RawInput(otherName, inputType);

            Assert.IsFalse(comparer.Equals(firstInput, secondInput));
        }

        [Test]
        public void DifferentObject_MatchingNameButDifferentType_False()
        {
            var comparer = new RawInputEqualityComparer();
            const string inputName = "TestName";
            const EInputType inputType = EInputType.Analog;
            const EInputType otherType = EInputType.Button;

            var firstInput = new RawInput(inputName, inputType);
            var secondInput = new RawInput(inputName, otherType);

            Assert.IsFalse(comparer.Equals(firstInput, secondInput));
        }

        [Test]
        public void GetHashCode_ReturnsNamePlusType()
        {
            var comparer = new RawInputEqualityComparer();
            const string inputName = "TestName";
            const EInputType inputType = EInputType.Analog;

            var firstInput = new RawInput(inputName, inputType);

            Assert.AreEqual(inputName.GetHashCode() + inputType.GetHashCode(), comparer.GetHashCode(firstInput));
        }
    }
}

#endif
