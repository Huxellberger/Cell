// Copyright (C) Threetee Gang All Rights Reserved

using System.IO;

namespace Assets.Scripts.Services.Persistence
{
    public interface IPersistentBehaviourInterface 
    {
        void WriteData(Stream stream);
        void ReadData(Stream stream);
    }
}
