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
        CharacterInfo,
        WeaponInfo,
        WorldSelect,
        StageSelect,
        CharacterSelect,
        WeaponSelect,
        InGame,
    }
    
    public static class SceneInfo
    {
        public static readonly Dictionary<SceneName, string> SceneNames = new()
        {
            { SceneName.Title, "00.TitleScene" },
            { SceneName.Lobby, "01-0.LobbyScene" },
            { SceneName.CharacterInfo, "01-1.CharacterInfoAndUpgradeScene" },
            { SceneName.WeaponInfo, "01-2.WeaponInfoAndUpgradeScene" },
            { SceneName.WorldSelect, "02.WorldSelectScene" },
            { SceneName.StageSelect, "03.StageSelectScene" },
            { SceneName.CharacterSelect, "04.CharacterSelectScene" },
            { SceneName.WeaponSelect, "05.WeaponSelectScene" },
            { SceneName.InGame, "05.InGame(Temp)" },
        };

        public static readonly Dictionary<SceneName, SceneType> SceneTypes = new()
        {
            { SceneName.Title, SceneType.OutGame },
            { SceneName.Lobby, SceneType.OutGame },
            { SceneName.CharacterInfo, SceneType.OutGame },
            { SceneName.WeaponInfo, SceneType.OutGame },
            { SceneName.WorldSelect, SceneType.OutGame },
            { SceneName.StageSelect, SceneType.OutGame },
            { SceneName.CharacterSelect, SceneType.OutGame },
            { SceneName.WeaponSelect, SceneType.OutGame },
            { SceneName.InGame, SceneType.InGame },
        };
    }
}
