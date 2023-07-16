using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    [SerializeField] private float _loopSpeed;
    [SerializeField] private Renderer _backgroundRenderer;

    private void Update()
    {
        _backgroundRenderer.material.mainTextureOffset += new Vector2(_loopSpeed * Time.deltaTime, 0f);
    }
}
