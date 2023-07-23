using System.Collections;
using UnityEngine;
using TMPro;
    
public class HighscoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    private void Awake()
    {
        var currentHighscore = PlayerPrefs.GetInt("Highscore");
        _scoreText.text = currentHighscore.ToString().PadLeft(8, '0');
    }
}
