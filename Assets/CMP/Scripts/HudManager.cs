using TMPro;
using UnityEngine;

namespace CMP.Scripts
{
    public class HudManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;

        public void SetScore(int score) => scoreText.text = score.ToString();
    }
}