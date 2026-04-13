using System;
using System.Collections.Generic;
using _01.Scripts._00.Manager;
using UnityEngine;

namespace _01.Scripts._03.Data
{
    [CreateAssetMenu(fileName = "BackgroundImageData", menuName = "ScriptableObject/BackgroundImageData")]
    public class BackgroundImageData : ScriptableObject
    {
        [Serializable]
        public struct BackgroundImageInfo
        {
            public BackgroundImage backgroundImage;
            public Sprite image;
        }

        public List<BackgroundImageInfo> backgroundImages;
    }
}