using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace neeksdk.Scripts.Game.GameUIView
{
    [RequireComponent(typeof(RectTransform))]
    public class GameUiView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Button _shuffleButton;
        [SerializeField] private Button _restartButton;
        
        private int _scores;

        public Action OnShuffleClick;
        public Action OnRestartClick;

        public Vector3 ScorePosition => _scoreText.rectTransform.position;

        public void AnimateScorePoints(int score)
        {
            _scores += score;
            _scoreText.text = _scores.ToString("0000");
        }

        public void ResetScorePoints()
        {
            _scores = 0;
            _scoreText.text = _scores.ToString("0000");
        }

        private void Awake()
        {
            _shuffleButton.onClick.AddListener(ShuffleClick);
            _restartButton.onClick.AddListener(RestartClick);
        }

        private void OnDestroy()
        {
            _shuffleButton.onClick.RemoveListener(ShuffleClick);
            _restartButton.onClick.RemoveListener(RestartClick);
        }

        private void ShuffleClick() =>
            OnShuffleClick?.Invoke();

        private void RestartClick() =>
            OnRestartClick?.Invoke();
    }
}