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
        Test2,
    }

    public class CinematicManager : SingletonObject<CinematicManager>
    {
        private MultiChatMessageData _currentCinematic;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                StartCoroutine(ShowCinematic(CinematicName.Test));
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(ShowCinematic(CinematicName.Test2));
            }
        }
        
        public IEnumerator ShowCinematic(CinematicName cinematicName)
        {
            FindCinematic(cinematicName);

            if (!_currentCinematic)
            {
                Debug.LogError("No Cinematic Found");
                yield break;
            }
            
            ChatManager.Instance.ShowChat(_currentCinematic);
            
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
                if (cinematic.cinematicName == cinematicName && !_currentCinematic)
                {
                    _currentCinematic = cinematic;
                }
                else
                {
                    Resources.UnloadAsset(cinematic);
                }
            }
            
            if (!_currentCinematic)
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