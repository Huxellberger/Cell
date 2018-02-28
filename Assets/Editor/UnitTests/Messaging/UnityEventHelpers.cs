// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;

namespace Assets.Editor.UnitTests.Messaging
{
    public class UnityTestMessagePayload
        : UnityMessagePayload
    {
        public UnityTestMessagePayload()
            : this(true)
        {
        }

        public UnityTestMessagePayload(bool inFlag)
            : base()
        {
            Flag = inFlag;
        }

        public bool Flag { get; private set; }
    }

    public class OtherUnityTestMessagePayload
        : UnityMessagePayload
    {
        public OtherUnityTestMessagePayload()
            : this(true)
        {
        }

        public OtherUnityTestMessagePayload(bool inFlag)
            : base()
        {
            Flag = inFlag;
        }

        public bool Flag { get; private set; }
    }

    public class UnityTestMessageHandleResponseObject<TMessageType>
        where TMessageType : UnityMessagePayload
    {
        public UnityTestMessageHandleResponseObject()
        {
            ActionCalled = false;
            MessagePayload = null;
        }

        public void OnResponse(TMessageType inPayload)
        {
            ActionCalled = true;
            MessagePayload = inPayload;
        }

        public bool ActionCalled { get; private set; }
        public TMessageType MessagePayload { get; private set; }
    }
}
