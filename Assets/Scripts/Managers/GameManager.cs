using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private void Update()
    {
        FramerateManager.Instance.RequestFullFrameRate();
    }
}
