using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CMP.Scripts
{
    public class HudManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Image[] pacmanLifeIcons;

        public void SetScore(int score) => scoreText.text = score.ToString();

        public void SetLives(int lives)
        {
            for (int i = 0; i < pacmanLifeIcons.Length; i++)
            {
                pacmanLifeIcons[i].enabled = i < lives;
            }
        }
    }
}