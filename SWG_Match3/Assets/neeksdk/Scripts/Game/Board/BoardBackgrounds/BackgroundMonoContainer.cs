using UnityEngine;

namespace neeksdk.Scripts.Game.Board.BoardBackgrounds
{
    public class BackgroundMonoContainer : MonoBehaviour
    {
        private IBackground _background;

        public IBackground Background => _background;

        public void SetupBackground(IBackground background) =>
            _background = background;
    }
}
