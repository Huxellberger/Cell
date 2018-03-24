// Copyright (C) Threetee Gang All Rights Reserved

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Core;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Core
{
    [TestFixture]
    public class PersistantDataOperationFunctionsTestFixture
    {
        [Serializable]
        public class DataBlockA
        {
            public int CurrentIntValue;
            public string CurrentStringValue;
        }

        [Serializable]
        public class DataBlockB
        {
            public string SomeStringValue;
        }

        private const string _filePath = "/Test/Crypto/TestCryptoFile.dat";

        private readonly byte[] _iV = {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        private readonly byte[] _key = {0x01, 0x03, 0x04, 0x03, 0x04, 0x32, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x22, 0x0E, 0x0F };

        private FileStream _file;

        private DataBlockA _firstData;
        private DataBlockB _secondData;

        [SetUp]
        public void BeforeTest()
        {
            _firstData = new DataBlockA{CurrentIntValue = 1, CurrentStringValue = "Whatever"};
            _secondData = new DataBlockB { SomeStringValue = "SomethingElse" };

            _file = File.Create(Application.dataPath + _filePath);
            var bf = new BinaryFormatter();
            bf.Serialize(_file, _firstData);
            bf.Serialize(_file, _secondData);
            _file.Close();
        }
	
        [TearDown]
        public void AfterTest()
        {
            _file.Close();
        }
	
        [Test]
        public void CryptoFunctionsEncryptAndDecryptAsExpected()
        {
            _file = File.Open(Application.dataPath + _filePath, FileMode.Open);

            var encryptedResult = PersistantDataOperationFunctions.EncryptFileStream(_file, _key, _iV);
            var decryptedResult = PersistantDataOperationFunctions.DecryptFileStream(new MemoryStream(encryptedResult), _key, _iV);
            var stream = new MemoryStream(decryptedResult);
            var bf = new BinaryFormatter();

            var actualFirst = (DataBlockA)bf.Deserialize(stream);
            Assert.AreEqual(_firstData.CurrentIntValue, actualFirst.CurrentIntValue);
            Assert.AreEqual(_firstData.CurrentStringValue, actualFirst.CurrentStringValue);

            var actualSecond = (DataBlockB)bf.Deserialize(stream);
            Assert.AreEqual(_secondData.SomeStringValue, actualSecond.SomeStringValue);

            _file.Close();
        }
    }
}
