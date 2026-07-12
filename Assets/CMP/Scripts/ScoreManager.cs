using TMPro;
using UnityEngine;

namespace CMP.Scripts
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;

        public void SetScore(int score) => scoreText.text = score.ToString();
    }
}