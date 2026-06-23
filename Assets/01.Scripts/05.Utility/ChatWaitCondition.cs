using System;
using _01.Scripts._00.Manager;
using UnityEngine;

namespace _01.Scripts._05.Utility
{
    [Serializable]
    public abstract class ChatWaitCondition
    {
        public abstract bool IsConditionMet();
    }

    [Serializable]
    public class WaitNoneCondition : ChatWaitCondition
    {
        public override bool IsConditionMet()
        {
            return true;
        }
    }

    [Serializable]
    public class WaitKeyCondition : ChatWaitCondition
    {
        public ActionCode targetAction = ActionCode.Jump;

        private bool _isRegistered;
        private bool _isKeyPressed;

        public override bool IsConditionMet()
        {
            if (!_isRegistered)
            {
                _isKeyPressed = false;
                InputManager.AddListener(targetAction, InputType.Down, OnTargetKeyPressed);
                _isRegistered = true;
            }
            
            if (_isKeyPressed)
            {
                CleanUp();
                return true;
            }

            return false;
        }

        private void OnTargetKeyPressed()
        {
            _isKeyPressed = true;
        }
        
        private void CleanUp()
        {
            if (_isRegistered)
            {
                InputManager.RemoveListener(targetAction, InputType.Down, OnTargetKeyPressed);
                _isRegistered = false;
            }
        }
    }
}
