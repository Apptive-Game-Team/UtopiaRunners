using System;
using System.Collections.Generic;
using _01.Scripts._00.Manager;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01.Scripts._03.Data
{
    public enum ChatSpeakerFaceType
    {
        None,
        Normal,
        Angry,
        Smile,
        Sad,
        Doubt,
        Embarrassed,
        Happy,
        Nodding,
        Thinking,
    }
    
    [CreateAssetMenu(fileName = "ChatSpeakerData", menuName = "ScriptableObject/ChatSpeakerData", order = 1)]
    public class ChatSpeakerData : ScriptableObject
    {
        [Serializable]
        public struct ChatSpeakerFaceInfo
        {
            public ChatSpeakerFaceType faceType;
            public Sprite faceImage;
        }


        [Serializable]
        public struct ChatSpeakerInfo
        {
            public ChatSpeakerType speakerType;
            public string speakerName;
            public Sprite image;
            public List<ChatSpeakerFaceInfo> faces;
        }

        public List<ChatSpeakerInfo> chatSpeakers;
    }
}