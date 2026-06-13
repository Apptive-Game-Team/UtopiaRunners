using System;
using System.Collections.Generic;
using _01.Scripts._00.Manager;
using UnityEngine;

namespace _01.Scripts._03.Data
{
    [CreateAssetMenu(fileName = "MultiChatMessageData", menuName = "ScriptableObject/MultiChatMessageData", order = 1)]
    public class MultiChatMessageData : ScriptableObject
    {
        [Serializable]
        public class MultiChatMessage
        {
            [Header("Speaker Setting")]
            public ChatSpeakerType speakerName;
            public ChatSpeakerFaceType faceType = ChatSpeakerFaceType.None;
            public bool isLeft;
            
            [Header("Content")]
            [TextArea(3, 10)]
            public List<string> messages;
            
            [Header("Image Setting")]
            public BackgroundImage backgroundImage;
            public ChatImage chatImage;
            
            [Header("Sound Setting")]
            public bool changeBgm;
            public BGM targetBgm;
        }
        
        public CinematicName cinematicName;
        public List<MultiChatMessage> chatMessages;
    }
}