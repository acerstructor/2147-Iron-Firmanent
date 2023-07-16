using UnityEngine;

public class LoadGameplay : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Play Sound Effect
            SceneManager.Instance.LoadScene(1);
        }
    }
}
