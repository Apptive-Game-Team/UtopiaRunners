using System;
using System.Collections;
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
        
        public IEnumerator ShowCinematic(CinematicName cinematicName)
        {
            FindCinematic(cinematicName);

            if (!_currentCinematic)
            {
                Debug.LogError("No Cinematic Found");
                yield break;
            }
            
            ChatManager.Instance.StartChat(_currentCinematic);
            
            UnloadCurrentCinematic();

            yield return null;
        }

        private void FindCinematic(CinematicName cinematicName)
        {
            MultiChatMessageData[] allCinematic = Resources.LoadAll<MultiChatMessageData>("Cinematic");

            if (allCinematic == null || allCinematic.Length == 0)
            {
                Debug.LogError("MultiChatMessageData가 존재하지 않음");
                return;
            }
            
            foreach (var cinematic in allCinematic)
            {
                if (cinematic.cinematicName == cinematicName && _currentCinematic == null)
                {
                    _currentCinematic = cinematic;
                }
                else
                {
                    Resources.UnloadAsset(cinematic);
                }
            }
            
            if (_currentCinematic == null)
            {
                Debug.LogError($"[{cinematicName}] 이거 없음.");
            }
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