namespace neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour
{
    public interface ISelectable : IBehaviourClearable
    {
        void Select();
        void Deselect();
    }
}