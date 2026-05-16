using System.Collections;
using _01.Scripts._03.Data;
using _01.Scripts._04.UI;
using _01.Scripts._05.Utility;
using UnityEngine;

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
        [SerializeField] private ChatSpeakerData speakerDB;
        [SerializeField] private BackgroundImageData bgDB;
        [SerializeField] private ChatImageData illustrationDB;
        [SerializeField] private MultiChatMessageData multiChatMessageDB;

        private bool isTyping = false;
        private bool skipRequested = false;
        public bool IsPlaying { get; private set; }

        public void Start()
        {
            StartChat(multiChatMessageDB);
        }
        
        public void StartChat(MultiChatMessageData data)
        {
            if (IsPlaying) return;
            StartCoroutine(ChatRoutine(data));
        }

        private IEnumerator ChatRoutine(MultiChatMessageData data)
        {
            IsPlaying = true;
            Time.timeScale = 0f;
            chatUI.SetActive(true);

            foreach (var msgGroup in data.chatMessages)
            {
                chatUI.SetBackground(GetBgSprite(msgGroup.backgroundImage));
                chatUI.PlayIllustrationEffect(GetIllustrationSprite(msgGroup.chatImage));

                if (msgGroup.chatImage != ChatImage.Nothing)
                {
                    yield return new WaitForSecondsRealtime(1.5f);
                }
                
                var speaker = speakerDB.chatSpeakers.Find(s => s.speakerType == msgGroup.speakerName);
                Sprite face = GetFaceSprite(speaker, msgGroup.faceType);
                chatUI.SetCharacters(face, msgGroup.isLeft);
                chatUI.UpdateName(speaker.speakerName ?? "???");
                
                string accumulated = "";
                foreach (var line in msgGroup.messages)
                {
                    yield return StartCoroutine(TypeMessage(line, accumulated));
                    accumulated += line + "\n";
                    
                    float timer = 0;
                    while (timer < 3f && !Input.GetKeyDown(KeyCode.Space))
                    {
                        timer += Time.unscaledDeltaTime;
                        yield return null;
                    }
                }
            }

            EndChat();
        }

        private IEnumerator TypeMessage(string line, string prefix)
        {
            isTyping = true;
            chatUI.UpdateMessage(prefix);

            for (int i = 0; i < line.Length; i++)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    chatUI.UpdateMessage(prefix + line);
                    break;
                }
                chatUI.UpdateMessage(prefix + line[..(i + 1)]);
                yield return new WaitForSecondsRealtime(0.05f);
            }
            
            yield return null;
            isTyping = false;
        }

        private void EndChat()
        {
            chatUI.SetActive(false);
            Time.timeScale = 1f;
            IsPlaying = false;
        }
        
        private Sprite GetFaceSprite(ChatSpeakerData.ChatSpeakerInfo info, ChatSpeakerFaceType face) 
        {
            var faceInfo = info.faces.Find(f => f.faceType == face);
            return faceInfo.faceImage ?? info.image;
        }
        
        private Sprite GetBgSprite(BackgroundImage type) => bgDB.backgroundImages.Find(x => x.backgroundImage == type).image;
        private Sprite GetIllustrationSprite(ChatImage type) => illustrationDB.chatImages.Find(x => x.chatImage == type).image;
    }
}
