using DG.Tweening;
using neeksdk.Scripts.Constants;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour
{
    public class SelectBehaviour : ISelectable
    {
        private Transform _targetTransform;
        private Sequence _selectionTween;
        public SelectBehaviour(Transform transform) =>
            _targetTransform = transform;

        public void Select()
        {
            if (_selectionTween != null && _selectionTween.IsPlaying())
            {
                return;
            }
            
            _selectionTween = DOTween.Sequence();
            _selectionTween.Append(_targetTransform.DOScale(TileConstants.TILE_SELECTION_SCALE, TileConstants.TILE_SELECTION_ANIMATION_DURATION)).SetEase(Ease.OutSine);
            _selectionTween.Append(_targetTransform.DOScale(Vector3.one, TileConstants.TILE_SELECTION_ANIMATION_DURATION)).SetEase(Ease.InSine);
            _selectionTween.SetLoops(-1);
            _selectionTween.Play();
        }

        public void Deselect()
        {
            if (_selectionTween == null || !_selectionTween.IsPlaying())
            {
                return;
            }
            
            _selectionTween.Kill(true);
        }

        public void Clear()
        {
            if (_selectionTween == null)
            {
                return;
            }
            
            _selectionTween.Kill();
        }
    }
}