// Copyright (C) Threetee Gang All Rights Reserved 

using Assets.Scripts.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Core
{
    [TestFixture]
    public class TieredLockTestFiture
    {
        public enum ETestTieredLockEnum
        {
            TestReason = 1,
            OtherTestReason = 2
        }

        [Test]
        public void Creation_IsLocked_False()
        {
            var tieredLock = new TieredLock<ETestTieredLockEnum>();

            Assert.IsFalse(tieredLock.IsLocked());
        }

        [Test]
        public void Lock_IsLocked_True()
        {
            var tieredLock = new TieredLock<ETestTieredLockEnum>();
            tieredLock.Lock(ETestTieredLockEnum.TestReason);

            Assert.IsTrue(tieredLock.IsLocked());
        }

        [Test]
        public void Lock_UnlockAfterLocked_False()
        {
            var tieredLock = new TieredLock<ETestTieredLockEnum>();
            tieredLock.Lock(ETestTieredLockEnum.TestReason);
            tieredLock.Unlock(ETestTieredLockEnum.TestReason);

            Assert.IsFalse(tieredLock.IsLocked());
        }

        [Test]
        public void Lock_MultipleLocks_True()
        {
            var tieredLock = new TieredLock<ETestTieredLockEnum>();
            tieredLock.Lock(ETestTieredLockEnum.TestReason);
            tieredLock.Lock(ETestTieredLockEnum.OtherTestReason);

            Assert.IsTrue(tieredLock.IsLocked());
        }

        [Test]
        public void Lock_MultipleLockedAndUnlockedButOneRemains_True()
        {
            var tieredLock = new TieredLock<ETestTieredLockEnum>();
            tieredLock.Lock(ETestTieredLockEnum.TestReason);
            tieredLock.Lock(ETestTieredLockEnum.OtherTestReason);
            tieredLock.Unlock(ETestTieredLockEnum.OtherTestReason);

            Assert.IsTrue(tieredLock.IsLocked());
        }

        [Test]
        public void Lock_MultipleLockedAndAllUnlocked_False()
        {
            var tieredLock = new TieredLock<ETestTieredLockEnum>();
            tieredLock.Lock(ETestTieredLockEnum.TestReason);
            tieredLock.Lock(ETestTieredLockEnum.OtherTestReason);
            tieredLock.Unlock(ETestTieredLockEnum.OtherTestReason);
            tieredLock.Unlock(ETestTieredLockEnum.TestReason);

            Assert.IsFalse(tieredLock.IsLocked());
        }

        [Test]
        public void Lock_AlreadyLocked_ErrorAndStillLocked()
        {
            const ETestTieredLockEnum expectedReason = ETestTieredLockEnum.TestReason;

            LogAssert.Expect(LogType.Error, "Failed to lock! Already locked for reason " + expectedReason);

            var tieredLock = new TieredLock<ETestTieredLockEnum>();
            tieredLock.Lock(ETestTieredLockEnum.TestReason);
            tieredLock.Lock(ETestTieredLockEnum.TestReason);

            Assert.IsTrue(tieredLock.IsLocked());
        }

        [Test]
        public void Unlock_AlreadyUnlocked_ErrorAndStillUnlocked()
        {
            const ETestTieredLockEnum expectedReason = ETestTieredLockEnum.TestReason;

            LogAssert.Expect(LogType.Error, "Failed to unlock! Not locked with reason " + expectedReason);

            var tieredLock = new TieredLock<ETestTieredLockEnum>();
            tieredLock.Unlock(ETestTieredLockEnum.TestReason);

            Assert.IsFalse(tieredLock.IsLocked());
        }
    }
}
