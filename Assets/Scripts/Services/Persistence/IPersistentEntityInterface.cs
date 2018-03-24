// Copyright (C) Threetee Gang All Rights Reserved

using System.IO;

namespace Assets.Scripts.Services.Persistence
{
    public interface IPersistentEntityInterface
    {
        void WriteData(Stream stream);
        void ReadData(Stream stream);
    }
}
