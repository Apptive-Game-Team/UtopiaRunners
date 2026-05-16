using System;
using System.Collections.Generic;
using _01.Scripts._00.Manager;
using UnityEngine;

namespace _01.Scripts._03.Data
{
    [CreateAssetMenu(fileName = "ChatImageData", menuName = "ScriptableObject/ChatImageData")]
    public class ChatImageData : ScriptableObject
    {
        [Serializable]
        public struct ChatImageInfo
        {
            public ChatImage chatImage;
            public Sprite image;
        }

        public List<ChatImageInfo> chatImages;
    }
}