using UnityEngine;

// For the convenience of testing, the actual execution results will not be affected
[DisallowMultipleComponent]
public class TimeScaler : MonoBehaviour
{
    [Range(0f, 2f)]
    public float timeScale = 1f;


    private void Update()
    {
        if (!Mathf.Approximately(Time.timeScale, timeScale))
        {
            Time.timeScale = timeScale;
            Debug.Log($"Set Time.timeScale to {timeScale:F2}.");
        }
    }
}
