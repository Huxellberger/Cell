// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;

namespace Assets.Scripts.Services.Time
{
    public class TimeService 
        : ITimeServiceInterface
    {
        private readonly IList<IPauseListenerInterface> _pauseListeners;

        private EPauseStatus _currentPauseStatus;
        private float _priorTimeScale;

        public TimeService()
        {
            _pauseListeners = new List<IPauseListenerInterface>();
            _currentPauseStatus = EPauseStatus.Unpaused;
            _priorTimeScale = 0.0f;
        }

        // ITimeServiceInterface
        public void AddPauseListener(IPauseListenerInterface inListener)
        {
            if (!_pauseListeners.Contains(inListener))
            {
               _pauseListeners.Add(inListener);
            }
            else
            {
                UnityEngine.Debug.LogError("Added a listener which already exists!");
            }
        }

        public void RemovePauseListener(IPauseListenerInterface inListener)
        {
            if (_pauseListeners.Contains(inListener))
            {
                _pauseListeners.Remove(inListener);
            }
            else
            {
                UnityEngine.Debug.LogError("Tried to remove a listener which was not added!");
            }
        }

        public void SetPauseStatus(EPauseStatus inPauseStatus)
        {
            if (_currentPauseStatus != inPauseStatus)
            {
                _currentPauseStatus = inPauseStatus;

                switch (_currentPauseStatus)
                {
                    case EPauseStatus.Paused:
                        _priorTimeScale = UnityEngine.Time.timeScale;
                        UnityEngine.Time.timeScale = 0.0f;
                        break;
                    case EPauseStatus.Unpaused:
                        UnityEngine.Time.timeScale = _priorTimeScale;
                        break;
                    default:
                        break;
                }

                UpdatePauseListeners();
            }
        }

        public EPauseStatus GetPauseStatus()
        {
            return _currentPauseStatus;
        }
        // ~ITimeServiceInterface

        private void UpdatePauseListeners()
        {
            foreach (var pauseListener in _pauseListeners)
            {
                pauseListener.UpdatePauseStatus(_currentPauseStatus);
            }
        }
    }
}
