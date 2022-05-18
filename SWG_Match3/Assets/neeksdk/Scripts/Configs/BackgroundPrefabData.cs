using System;
using neeksdk.Scripts.Game.Board.BoardBackgrounds;

namespace neeksdk.Scripts.Configs
{
    [Serializable]
    public class BackgroundPrefabData
    {
        public string Name;
        public BackgroundType Type;
        public BackgroundMonoContainer TilePrefab;
    }
}