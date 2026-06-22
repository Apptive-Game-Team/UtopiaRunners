using _01.Scripts._00.Manager;
using UnityEditor;
using UnityEngine;

namespace _01.Scripts._09.Editor
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (!Application.isPlaying)
            {
                return;
            }
            
            GameManager gm = (GameManager)target;
            
            GUILayout.Space(20); 
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("캐릭터 전체 해금", GUILayout.Width(120), GUILayout.Height(30))) 
            {
                gm.UnlockCharacters();
            }
            
            if (GUILayout.Button("캐릭터 전체 초기화", GUILayout.Width(120), GUILayout.Height(30))) 
            {
                gm.ResetUnlockedCharacters();
            }
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(8);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("무기 전체 해금", GUILayout.Width(120), GUILayout.Height(30))) 
            {
                gm.UnlockWeapons();
            }
            
            if (GUILayout.Button("무기 전체 초기화", GUILayout.Width(120), GUILayout.Height(30))) 
            {
                gm.ResetUnlockedWeapons();
            }
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }
}