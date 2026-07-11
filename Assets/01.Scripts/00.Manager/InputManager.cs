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
        public event Action OnKeyMappingChanged;

        private static Dictionary<(ActionCode, InputType), Action> _eventDict = new();

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
                { ActionCode.Tag, KeyCode.Z },
                { ActionCode.Skill, KeyCode.C },
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
                if (!_keyActiveFlags[action] ||
                    !_keyMappings.TryGetValue(action, out KeyCode mainKey))
                {
                    continue;
                }

                KeyCode altKey =
                    _altKeyMappings.GetValueOrDefault(action, KeyCode.None);

                if (Input.GetKeyDown(mainKey) ||
                    (altKey != KeyCode.None && Input.GetKeyDown(altKey)))
                {
                    ExecuteEvent(action, InputType.Down);
                }
                else if (Input.GetKeyUp(mainKey) ||
                         (altKey != KeyCode.None && Input.GetKeyUp(altKey)))
                {
                    ExecuteEvent(action, InputType.Up);
                }
                else if (Input.GetKey(mainKey) ||
                         (altKey != KeyCode.None && Input.GetKey(altKey)))
                {
                    ExecuteEvent(action, InputType.Press);
                }
            }
        }

        public void ExecuteEvent(ActionCode action, InputType type)
        {
            if (_eventDict.TryGetValue((action, type), out var targetEvent))
            {
                targetEvent?.Invoke();
            }
        }

        public static void AddListener(
            ActionCode action,
            InputType type,
            Action callback
        )
        {
            var key = (action, type);
            _eventDict.TryAdd(key, null);
            _eventDict[key] += callback;
        }

        public static void RemoveListener(
            ActionCode action,
            InputType type,
            Action callback
        )
        {
            var key = (action, type);

            if (_eventDict.ContainsKey(key))
            {
                _eventDict[key] -= callback;
            }
        }

        public void SetKeyActive(ActionCode action, bool active)
        {
            _keyActiveFlags[action] = active;
        }

        public void SetKeyMapping(ActionCode action, KeyCode key)
        {
            _keyMappings[action] = key;
            OnKeyMappingChanged?.Invoke();
        }

        public KeyCode GetKeyMapping(ActionCode action)
        {
            if (_keyMappings.TryGetValue(action, out KeyCode key))
            {
                return key;
            }

            return KeyCode.None;
        }

        public KeyCode GetAltKeyMapping(ActionCode action)
        {
            if (_altKeyMappings.TryGetValue(action, out KeyCode key))
            {
                return key;
            }

            return KeyCode.None;
        }

        public void RebindKeyWithSwap(ActionCode targetAction, KeyCode newKey)
        {
            if (!_keyMappings.ContainsKey(targetAction))
                return;

            if (IsFixedAltKey(newKey))
                return;

            KeyCode oldKey = _keyMappings[targetAction];

            ActionCode? duplicatedAction = null;

            foreach (var pair in _keyMappings)
            {
                if (pair.Key == targetAction)
                    continue;

                if (pair.Value == newKey)
                {
                    duplicatedAction = pair.Key;
                    break;
                }
            }

            _keyMappings[targetAction] = newKey;

            if (duplicatedAction != null)
            {
                _keyMappings[duplicatedAction.Value] = oldKey;
            }

            OnKeyMappingChanged?.Invoke();
        }

        private bool IsFixedAltKey(KeyCode key)
        {
            foreach (var pair in _altKeyMappings)
            {
                if (pair.Value == key)
                {
                    return true;
                }
            }

            return false;
        }
    }
}