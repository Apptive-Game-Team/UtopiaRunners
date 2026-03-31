using System;
using System.Collections.Generic;
using _01.Scripts._05.Utility;
using UnityEngine;

namespace _01.Scripts._00.Manager
{
    public enum ActionCode
    {
        Tag,
        Skill,
        Slide,
        Jump,
        Option,
        
        Menu,
        MenuLeft,
        MenuRight,
    }
    
    public enum InputType
    {
        Down,
        Up,
        Press
    }
    
    public class InputManager : SingletonObject<InputManager>
    {
        public static event Action<ActionCode, InputType> OnKeyEvent;

        private Dictionary<ActionCode, KeyCode> _keyMappings = new();
        private Dictionary<ActionCode, KeyCode> _altKeyMappings = new();
        private Dictionary<ActionCode, bool> _keyActiveFlags = new();
        
        private List<ActionCode> _actionCodes = new();

        protected override void Awake()
        {
            base.Awake();
            
            InitInputs();
        }

        private void InitInputs()
        {
            SetDefaultKey();
            
            _actionCodes.Clear();
            foreach (ActionCode action in Enum.GetValues(typeof(ActionCode)))
            {
                _actionCodes.Add(action);
                _keyActiveFlags.TryAdd(action, true);
            }
        }

        private void SetDefaultKey()
        {
            _keyMappings = new Dictionary<ActionCode, KeyCode>()
            {
                { ActionCode.Jump, KeyCode.Space },
                { ActionCode.Tag, KeyCode.Q },
                { ActionCode.Skill, KeyCode.E },
                { ActionCode.Slide, KeyCode.LeftControl },
                { ActionCode.Option, KeyCode.Escape },
            };

            _altKeyMappings = new Dictionary<ActionCode, KeyCode>()
            {
                { ActionCode.Jump, KeyCode.UpArrow },
                { ActionCode.Slide, KeyCode.DownArrow },
            };
        }

        private void Update()
        {
            foreach (ActionCode action in _actionCodes)
            {
                if (!_keyActiveFlags[action] || !_keyMappings.TryGetValue(action, out KeyCode mainKey))
                {
                    continue;
                }

                KeyCode altKey = _altKeyMappings.GetValueOrDefault(action, KeyCode.None);
                
                if (Input.GetKeyDown(mainKey) || (altKey != KeyCode.None && Input.GetKeyDown(altKey)))
                {
                    OnKeyEvent?.Invoke(action, InputType.Down);
                }
                else if (Input.GetKeyUp(mainKey) || (altKey != KeyCode.None && Input.GetKeyUp(altKey)))
                {
                    OnKeyEvent?.Invoke(action, InputType.Up);
                }
                else if (Input.GetKey(mainKey) || (altKey != KeyCode.None && Input.GetKey(altKey)))
                {
                    OnKeyEvent?.Invoke(action, InputType.Press);
                }
            }
        }

        public void SetKeyActive(ActionCode action, bool active) => _keyActiveFlags[action] = active;
        
        public void SetKeyMapping(ActionCode action, KeyCode key) => _keyMappings[action] = key;
    }
}