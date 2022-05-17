using UnityEngine;

namespace neeksdk.Scripts.Game.Board.BoardBackgrounds
{
    public interface IBackground
    {
        GameObject GameObject { get; }
        BackgroundType BackgroundType { get; }
    }
}