using UnityEngine;
using TMPro;
    
public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private TMP_Text _scoreText;

    protected override void Awake()
    {
        base.Awake();
    }

    

}
