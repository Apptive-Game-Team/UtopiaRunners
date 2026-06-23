#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace _01.Scripts._09.Editor
{
    public class GameDataManagerWindow : EditorWindow
    {
        private enum DataCategory
        {
            Character,
            Weapon,
            Monster,
            Boss,
            Wave,
            Cinematic
        }

        private DataCategory _currentCategory = DataCategory.Character;

        private List<ScriptableObject> _characterDataList = new();
        private List<ScriptableObject> _weaponDataList = new();
        private List<ScriptableObject> _monsterDataList = new();
        private List<ScriptableObject> _bossDataList = new();
        private List<ScriptableObject> _cinematicDataList = new();
        private List<ScriptableObject> _waveDataList = new();

        private Vector2 _leftScrollPos;
        private Vector2 _rightScrollPos;

        private ScriptableObject _selectedAsset;
        private UnityEditor.Editor _cachedEditor;

        private Dictionary<string, bool> _foldouts = new();

        [MenuItem("Tools/Game Data Manager")]
        public static void ShowWindow()
        {
            GetWindow<GameDataManagerWindow>("Game Data Manager");
        }

        private void OnEnable()
        {
            RefreshAllData();
        }

        private void OnDisable()
        {
            if (_cachedEditor != null)
            {
                DestroyImmediate(_cachedEditor);
                _cachedEditor = null;
            }

            AssetDatabase.SaveAssets();
        }

        private void RefreshAllData()
        {
            _waveDataList = LoadAssetsOfType<ScriptableObject>("t:StageWaveData");
            _characterDataList = LoadAssetsOfType<ScriptableObject>("t:CharacterData");
            _weaponDataList = LoadAssetsOfType<ScriptableObject>("t:WeaponData");
            _monsterDataList = LoadAssetsOfType<ScriptableObject>("t:EnemyData");
            _cinematicDataList = LoadAssetsOfType<ScriptableObject>("t:MultiChatMessageData");
            _bossDataList = LoadAssetsOfType<ScriptableObject>("t:BossData");
        }

        private List<ScriptableObject> LoadAssetsOfType<T>(string filter) where T : Object
        {
            List<ScriptableObject> list = new();

            string[] guids = AssetDatabase.FindAssets(filter);

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                T asset = AssetDatabase.LoadAssetAtPath<T>(path);

                if (asset)
                {
                    list.Add(asset as ScriptableObject);
                }
            }

            return list;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical(
                GUI.skin.box,
                GUILayout.Width(250),
                GUILayout.ExpandHeight(true));

            DrawLeftPanel();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(
                GUI.skin.box,
                GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true));

            DrawRightPanel();

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        private void DrawLeftPanel()
        {
            GUILayout.Label("Categories", EditorStyles.boldLabel);

            _currentCategory = (DataCategory)GUILayout.Toolbar(
                (int)_currentCategory,
                System.Enum.GetNames(typeof(DataCategory)));

            GUILayout.Space(10);

            GUILayout.Label("Asset List", EditorStyles.boldLabel);

            if (GUILayout.Button("Refresh Data ↻"))
            {
                AssetDatabase.SaveAssets();
                RefreshAllData();
            }

            GUILayout.Space(5);

            _leftScrollPos = EditorGUILayout.BeginScrollView(_leftScrollPos);

            List<ScriptableObject> activeList = GetActiveList();

            foreach (var asset in activeList)
            {
                GUI.backgroundColor = _selectedAsset == asset ? Color.cyan : Color.white;

                if (GUILayout.Button(asset.name, EditorStyles.miniButton))
                {
                    if (_selectedAsset != asset)
                    {
                        GUIUtility.keyboardControl = 0; 
                        EditorGUI.FocusTextInControl(null);

                        if (_selectedAsset)
                        {
                            EditorUtility.SetDirty(_selectedAsset);
                        }

                        _selectedAsset = asset;
                        UnityEditor.Editor.CreateCachedEditor(_selectedAsset, null, ref _cachedEditor);

                        GUIUtility.ExitGUI(); 
                    }
                }
            }

            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndScrollView();

            GUILayout.FlexibleSpace();

            GUI.backgroundColor = new Color(0.6f, 1f, 0.6f);

            if (GUILayout.Button("Save All Changes", GUILayout.Height(25)))
            {
                AssetDatabase.SaveAssets();
                Debug.Log("[GameDataManager] 모든 데이터가 저장되었습니다.");
            }

            GUI.backgroundColor = Color.white;

            GUILayout.Space(5);

            if (GUILayout.Button("+ Create New Asset", GUILayout.Height(30)))
            {
                CreateNewAssetWindow();
            }
        }

        private void DrawRightPanel()
        {
            if (!_selectedAsset)
            {
                GUILayout.Label("수정할 데이터를 좌측 리스트에서 선택해주세요.", EditorStyles.centeredGreyMiniLabel);

                return;
            }

            GUILayout.Label($"Editing: {_selectedAsset.name}", EditorStyles.boldLabel);

            GUILayout.Space(10);

            _rightScrollPos = EditorGUILayout.BeginScrollView(_rightScrollPos);
            
            if (Event.current.type == EventType.MouseDrag || Event.current.type == EventType.ScrollWheel)
            {
                GUIUtility.keyboardControl = 0;
            }

            if (_selectedAsset is StageWaveData waveData)
            {
                DrawStageWaveDataCustomEditor(waveData);
            }
            else
            {
                if (_cachedEditor == null || _cachedEditor.target != _selectedAsset)
                {
                    UnityEditor.Editor.CreateCachedEditor(
                        _selectedAsset,
                        null,
                        ref _cachedEditor);
                }

                _cachedEditor.OnInspectorGUI();
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawStageWaveDataCustomEditor(StageWaveData waveData)
        {
            if (!waveData || waveData.waveTimelines == null)
            {
                return;
            }
            
            SerializedObject waveDataSO = new SerializedObject(waveData);
            waveDataSO.Update();
            
            SerializedProperty scriptProp = waveDataSO.FindProperty("m_Script");
    
            if (scriptProp != null)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(scriptProp);
                EditorGUI.EndDisabledGroup();
        
                GUILayout.Space(10);
            }

            for (int i = 0; i < waveData.waveTimelines.Count; i++)
            {
                WaveTimelineData timeline = waveData.waveTimelines[i];

                if (!timeline)
                {
                    EditorGUILayout.HelpBox($"[{i}] Timeline 슬롯이 비어있습니다.", MessageType.Warning);
                    continue;
                }

                string timelineKey = $"timeline_{timeline.GetInstanceID()}";
                _foldouts.TryAdd(timelineKey, false);

                EditorGUILayout.BeginVertical("box");

                _foldouts[timelineKey] =
                    EditorGUILayout.Foldout(
                        _foldouts[timelineKey],
                        $"[Timeline {i}] {timeline.name}",
                        true);

                if (_foldouts[timelineKey])
                {
                    EditorGUI.indentLevel++;

                    SerializedObject timelineSO = new SerializedObject(timeline);
                    timelineSO.Update();

                    SerializedProperty baseTimeProp = timelineSO.FindProperty("baseTime");
                    SerializedProperty spawnEventsProp = timelineSO.FindProperty("spawnEvents");

                    EditorGUILayout.PropertyField(baseTimeProp);

                    GUILayout.Space(5);

                    EditorGUILayout.LabelField("Spawn Events", EditorStyles.boldLabel);
                    
                    int indexToRemove = -1;

                    for (int j = 0; j < spawnEventsProp.arraySize; j++)
                    {
                        SerializedProperty eventProp = spawnEventsProp.GetArrayElementAtIndex(j);
                        string eventKey = $"event_{timeline.GetInstanceID()}_{j}";
                        _foldouts.TryAdd(eventKey, false);

                        EditorGUILayout.BeginVertical("helpbox");
                        
                        EditorGUILayout.BeginHorizontal();

                        _foldouts[eventKey] =
                            EditorGUILayout.Foldout(
                                _foldouts[eventKey],
                                $"Event {j}",
                                true);
                        
                        GUI.backgroundColor = new Color(0.9f, 0.3f, 0.3f);
                        if (GUILayout.Button("✕", EditorStyles.miniButton, GUILayout.Width(24), GUILayout.Height(16)))
                        {
                            indexToRemove = j;
                        }
                        GUI.backgroundColor = Color.white;

                        EditorGUILayout.EndHorizontal();

                        if (_foldouts[eventKey])
                        {
                            EditorGUI.indentLevel++;

                            EditorGUILayout.PropertyField(eventProp.FindPropertyRelative("prefab"));
                            EditorGUILayout.PropertyField(eventProp.FindPropertyRelative("spawnPointId"));
                            EditorGUILayout.PropertyField(eventProp.FindPropertyRelative("delayFromBaseTime"));

                            EditorGUI.indentLevel--;
                        }

                        EditorGUILayout.EndVertical();
                    }
                    
                    if (indexToRemove != -1)
                    {
                        int oldSize = spawnEventsProp.arraySize;
                        
                        spawnEventsProp.DeleteArrayElementAtIndex(indexToRemove);
                        if (spawnEventsProp.arraySize == oldSize)
                        {
                            spawnEventsProp.DeleteArrayElementAtIndex(indexToRemove);
                        }
                    }
                    
                    GUILayout.Space(5);
                    if (GUILayout.Button("+ Add Event", GUILayout.Height(20)))
                    {
                        spawnEventsProp.InsertArrayElementAtIndex(spawnEventsProp.arraySize);
                    }

                    if (timelineSO.ApplyModifiedProperties())
                    {
                        EditorUtility.SetDirty(timeline);
                    }

                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.EndVertical();
                GUILayout.Space(4);
            }
        }

        private List<ScriptableObject> GetActiveList()
        {
            switch (_currentCategory)
            {
                case DataCategory.Wave:
                    return _waveDataList;

                case DataCategory.Character:
                    return _characterDataList;

                case DataCategory.Weapon:
                    return _weaponDataList;

                case DataCategory.Monster:
                    return _monsterDataList;

                case DataCategory.Boss:
                    return _bossDataList;
                
                case DataCategory.Cinematic:
                    return _cinematicDataList;

                default:
                    return new List<ScriptableObject>();
            }
        }

        private void CreateNewAssetWindow()
        {
        }
    }
}
#endif