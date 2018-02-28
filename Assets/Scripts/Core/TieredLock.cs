// Copyright (C) Threetee Gang All Rights Reserved 

using System;

namespace Assets.Scripts.Core
{
    public class TieredLock<TTierType>
        where TTierType : struct, IConvertible
    {
        private int LockStatus { get; set; }

        public TieredLock()
        {
            LockStatus = 0;
        }

        public void Lock(TTierType inTierType)
        {
            var lockValue = Convert.ToInt32(inTierType);

            lockValue = 1 << lockValue;

            if (!IsBitSet(lockValue))
            {
                LockStatus |= lockValue;
            }
            else
            {
                UnityEngine.Debug.LogError("Failed to lock! Already locked for reason " + inTierType);
            }
        }

        public void Unlock(TTierType inTierType)
        {
            var lockValue = Convert.ToInt32(inTierType);

            lockValue = 1 << lockValue;

            if (IsBitSet(lockValue))
            {
                LockStatus &= ~lockValue;
            }
            else
            {
                UnityEngine.Debug.LogError("Failed to unlock! Not locked with reason " + inTierType);
            }
        }

        public bool IsLocked()
        {
            return LockStatus != 0;
        }

        bool IsBitSet(int newLockValue)
        {
            return (LockStatus & newLockValue) != 0;
        }
    }
}
