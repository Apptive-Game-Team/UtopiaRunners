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
            public ChatSpeakerType speakerName;
            public ChatSpeakerFaceType faceType = ChatSpeakerFaceType.None;
            public bool isLeft;
            public List<string> messages;
            public BackgroundImage backgroundImage;
            public ChatImage chatImage;
        }

        public string bgmName;
        public List<MultiChatMessage> chatMessages;
    }
}