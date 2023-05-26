using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;
using Unity.Jobs;

public class FPSCounter : MonoBehaviour
{
    public Text fpsText;
    public float updateInterval = 0.5f;

    private float fpsAccumulator = 0f;
    private int frameCount = 0;
    private float timeLeft;

    private NativeArray<float> fpsValues;
    private JobHandle fpsJobHandle;

    private void Start()
    {
        timeLeft = updateInterval;
        fpsValues = new NativeArray<float>(1, Allocator.Persistent);
    }

    private void Update()
    {
        fpsAccumulator += Time.timeScale / Time.deltaTime;
        frameCount++;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f) {
            CalculateFPS();
            timeLeft = updateInterval;
        }
    }

    private void LateUpdate()
    {
        fpsJobHandle.Complete();

        if (fpsText != null) {
            fpsText.text = $"FPS: {fpsValues[0]:F1}";
        }
    }

    private void CalculateFPS()
    {
        if (frameCount > 0) {
            var fpsJob = new CalculateFPSJob
            {
                fpsValues = fpsValues,
                frameCount = frameCount,
                fpsAccumulator = fpsAccumulator
            };

            fpsJobHandle = fpsJob.Schedule();
        }

        frameCount = 0;
        fpsAccumulator = 0f;
    }

    private void OnDestroy()
    {
        fpsValues.Dispose();
    }

    [Unity.Burst.BurstCompile]
    private struct CalculateFPSJob : IJob
    {
        public NativeArray<float> fpsValues;
        public int frameCount;
        public float fpsAccumulator;

        public void Execute()
        {
            fpsValues[0] = fpsAccumulator / frameCount;
        }
    }
}