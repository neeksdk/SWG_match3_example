using System;
using RSG;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour
{
    public interface ICollectable : IBehaviourClearable
    {
        IPromise Collect(Vector3 scorePosition, Action onComplete);
    }
}