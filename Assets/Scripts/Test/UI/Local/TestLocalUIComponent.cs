// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.UI.Local;

public class TestLocalUIComponent 
	: LocalUIComponent
{
	public void TestAwake() 
	{
		Awake();
	}
	
	public void TestDestroy() 
	{
		OnDestroy();
	}
}

#endif // UNITY_EDITOR
