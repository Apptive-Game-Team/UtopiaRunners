using System.Collections.Generic;

namespace _01.Scripts._05.Utility
{
    public enum SceneType
    {
        OutGame,
        InGame,
    }
    
    public enum SceneName
    {
        Title,
        Lobby,
        WorldSelect,
        StageSelect,
        CharacterSelect,
        InGame,
    }
    
    public static class SceneInfo
    {
        public static Dictionary<SceneName, string> SceneNames = new()
        {
            { SceneName.Title, "00.TitleScene" },
            { SceneName.Lobby, "01.LobbyScene" },
            { SceneName.WorldSelect, "02.WorldSelectScene" },
            { SceneName.StageSelect, "03.StageSelectScene" },
            { SceneName.CharacterSelect, "04.CharacterSelectScene" },
            { SceneName.InGame, "05.InGameScene" },
        };

        public static Dictionary<SceneName, SceneType> SceneTypes = new()
        {
            { SceneName.Title, SceneType.OutGame },
            { SceneName.Lobby, SceneType.OutGame },
            { SceneName.WorldSelect, SceneType.OutGame },
            { SceneName.StageSelect, SceneType.OutGame },
            { SceneName.CharacterSelect, SceneType.OutGame },
            { SceneName.InGame, SceneType.InGame },
        };
    }
}
