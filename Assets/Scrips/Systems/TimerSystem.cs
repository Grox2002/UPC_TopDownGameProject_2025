using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerSystem : MonoBehaviour
{
    public TMP_Text timerUI;
    private float _time;

    void Update()
    {
        _time += Time.deltaTime;

        int minutes = Mathf.FloorToInt(_time / 60);
        int secons = Mathf.FloorToInt(_time % 60);

        timerUI.text = $"{minutes:00}:{secons:00}";
    }

    public void ResetTime()
    {
        _time = 0;
    }
}
