// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.UnityLayer.Storage
{
    public static class GameDataStorageConstants
    {
        public const string SaveDataPath = "/PlayerSave.dat";
        public static byte[] AESKey = {0x2, 0x3, 0x43, 0x55, 0x2, 0x32, 0x43, 0x57, 0x22, 0x3, 0x13, 0x45, 0x2, 0x33, 0x43, 0x55};
        // Note: This would be terrible practice in proper cryptography, but we don't really care for our purposes (SALT should be unique for each session)
        public static byte[] AESIV = {0x33, 0x2, 0x98,0x12, 0x2, 0x3, 0x43, 0x55, 0x78, 0x13, 0x43, 0x55 ,0x2, 0x32, 0x43, 0x55};
    }
}
