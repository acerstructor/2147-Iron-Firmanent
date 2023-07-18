using UnityEngine;

public class LoadGameplay : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Play Sound Effect
            LevelManager.Instance.LoadScene(1);
        }
    }
}
