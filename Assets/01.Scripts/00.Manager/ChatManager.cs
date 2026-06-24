using System;
using System.Collections;
using _01.Scripts._03.Data;
using _01.Scripts._04.UI;
using _01.Scripts._05.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01.Scripts._00.Manager
{
    public enum ChatSpeakerType //0~99 : 주역, 100~199 : 조역, 200~ : 엑스트라, 999=???
    {
        None = 0,
        Icarus = 1,
        Karok = 2,
        Eve = 3,
        Hani = 4,
        Person5 = 5,

        IcarusDad = 101,
        Kar = 102,

        ExtraRobot1 =201,
        ExtraRobot2 = 202,
        ExtraPolice = 203,
        QuestionablePerson1 = 204,
        QuestionablePerson2 = 205,
        Bartender = 206,
        SecurityRobot = 207,

        Extra = 999,
    }

    public enum BackgroundImage
    {
        None = 0,
        Black = 1,
        Eden = 2,
        Etc = 3,
    }

    public enum ChatImage //시네마틱용 이미지
    {
        Nothing,
        OpenLog1,
        OpenLog2,
        OpenLog3,
        OpenLog4,
        OpenLog5,
        OpenLog6,
        OpenLog7,
        OpenLog8,
    }
    
    public class ChatManager : SingletonObject<ChatManager>
    {
        [Header("Components")]
        [SerializeField] private ChatUI chatUI;
        
        [Header("Databases")]
        [SerializeField] private ChatSpeakerData chatSpeakerDB;
        [SerializeField] private BackgroundImageData backgroundImageDB;
        [SerializeField] private ChatImageData chatImageDB;
        
        public bool IsPlaying { get; private set; }
        
        public void ShowChat(MultiChatMessageData data)
        {
            if (IsPlaying)
            {
                return;
            }
            
            StartCoroutine(ChatRoutine(data));
        }

        private IEnumerator ChatRoutine(MultiChatMessageData data)
        {
            StartChat();

            bool isFirst = true;

            foreach (var msgGroup in data.chatMessages)
            {
                Time.timeScale = msgGroup.stopCinematic ? 0f : 1f;
                
                chatUI.SetBackground(GetBgSprite(msgGroup.backgroundImage));
                chatUI.PlayIllustrationEffect(GetIllustrationSprite(msgGroup.chatImage));

                if (msgGroup.changeBgm)
                {
                    SoundManager.Instance.PlayBgm(msgGroup.targetBgm);
                }
                
                var speaker = chatSpeakerDB.chatSpeakers.Find(s => s.speakerType == msgGroup.speakerName);
                Sprite face = GetFaceSprite(speaker, msgGroup.faceType);
                chatUI.SetCharacters(face, msgGroup.isLeft);
                chatUI.UpdateName(speaker.speakerName ?? "???");

                if (isFirst && data.useFade)
                {
                    chatUI.SetActiveChatting(false);
                    yield return chatUI.FadeInAndOut(true);
                    chatUI.SetActiveChatting(true);
                    isFirst = false;
                }
                
                string accumulated = "";
                foreach (var line in msgGroup.messages)
                {
                    yield return StartCoroutine(TypeMessage(line, accumulated));
                    accumulated += line + "\n";
                    
                    float timer = 0;
                    while (timer < 1f && !Input.GetKeyDown(KeyCode.Return))
                    {
                        timer += Time.unscaledDeltaTime;
                        yield return null;
                    }
                }
                
                if (msgGroup.chatWaitCondition != null)
                {
                    yield return new WaitUntil(() => msgGroup.chatWaitCondition.IsConditionMet());
                }
            }

            if (data.useFade)
            {
                yield return chatUI.FadeInAndOut(false);
            }
            
            EndChat();
        }

        private IEnumerator TypeMessage(string line, string prefix)
        {
            chatUI.UpdateMessage(prefix);

            for (int i = 0; i < line.Length; i++)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    chatUI.UpdateMessage(prefix + line);
                    break;
                }
                chatUI.UpdateMessage(prefix + line[..(i + 1)]);
                yield return new WaitForSecondsRealtime(0.05f);
            }
            
            yield return null;
        }

        private void StartChat()
        {
            IsPlaying = true;
            Time.timeScale = 0f;
            chatUI.SetActive(true);
        }
        
        private void EndChat()
        {
            chatUI.SetActive(false);
            Time.timeScale = 1f;
            IsPlaying = false;
        }

        private void FadeInAndOut(bool fadeIn)
        {
            
        }
        
        private Sprite GetFaceSprite(ChatSpeakerData.ChatSpeakerInfo info, ChatSpeakerFaceType face) 
        {
            var faceInfo = info.faces.Find(f => f.faceType == face);
            return faceInfo.faceImage ?? info.image;
        }
        
        private Sprite GetBgSprite(BackgroundImage type) => backgroundImageDB.backgroundImages.Find(x => x.backgroundImage == type).image;
        private Sprite GetIllustrationSprite(ChatImage type) => chatImageDB.chatImages.Find(x => x.chatImage == type).image;
    }
}
