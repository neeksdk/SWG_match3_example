using TMPro;
using UnityEngine;

namespace neeksdk.Scripts.Game.GameUIView
{
    [RequireComponent(typeof(RectTransform))]
    public class GameUiView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        private int _scores;
        
        public Vector3 ScorePosition => _scoreText.rectTransform.position;

        public void AnimateScorePoints(int score)
        {
            _scores += score;
            _scoreText.text = _scores.ToString("0000");
        }
    }
}