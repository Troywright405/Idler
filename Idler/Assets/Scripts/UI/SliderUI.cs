using UnityEngine;
using UnityEngine.UI;

public class SliderUIUtility
{
    private float targetValue = 1f;

    public void SetTarget(float value)
    {
        targetValue = Mathf.Clamp01(value); // Ensure valid range
    }
    public static float SmoothStepTowards(float current, float target, float speed = 0.01f)
    {
        return current + (target - current) * speed;
    }

}
