namespace neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour
{
    public interface ISelectable
    {
        void Select();
        void Deselect();
        void Clear();
    }
}