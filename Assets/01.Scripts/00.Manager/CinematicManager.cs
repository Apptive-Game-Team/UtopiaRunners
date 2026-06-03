using System;
using System.Collections.Generic;
using _01.Scripts._03.Data;
using _01.Scripts._05.Utility;
using UnityEngine;

namespace _01.Scripts._00.Manager
{
    public enum CinematicName
    {
        Test,
    }

    public class CinematicManager : SingletonObject<CinematicManager>
    {
        private MultiChatMessageData _currentCinematic;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                ShowCinematic(CinematicName.Test);
            }
        }
        
        public void ShowCinematic(CinematicName cinematicName)
        {
            string path = $"Cinematic/{cinematicName}";
            _currentCinematic = Resources.Load<MultiChatMessageData>(path);

            if (!_currentCinematic)
            {
                Debug.LogError("No Cinematic Found");
                return;
            }
            
            ChatManager.Instance.StartChat(_currentCinematic, UnloadCurrentCinematic);
        }

        private void UnloadCurrentCinematic()
        {
            if (_currentCinematic)
            {
                Resources.UnloadAsset(_currentCinematic);
                _currentCinematic = null;
            }
        }
    }
}