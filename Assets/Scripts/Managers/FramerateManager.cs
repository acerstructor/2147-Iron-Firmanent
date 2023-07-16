using UnityEngine;
using UnityEngine.Rendering;

public class FramerateManager : SingletonPersistent<FramerateManager>
{
    private const int BUFFER_FRAMES = 3;
    private const int LOW_POWER_FRAME_INTERVAL = 60;
    private int lastRequestedFrame = 0;

    protected override void Awake()
    {
        Application.targetFrameRate = 30;
        base.Awake();
    }

    private void Update()
    {
        OnDemandRendering.renderFrameInterval = (Time.frameCount - lastRequestedFrame) < BUFFER_FRAMES ? 1 : LOW_POWER_FRAME_INTERVAL;
    }

    public void RequestFullFrameRate()
    {
        lastRequestedFrame = Time.frameCount;
    }
}
