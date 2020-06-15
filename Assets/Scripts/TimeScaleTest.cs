using PlayControllerScript;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaleTest : MonoBehaviour
{
    public float TimescalParam = 1;
    private float _lastUpdateTime = 0;
    private float _lastLateUpdateTime = 0;
    private float _lastFixedUpdateTime = 0;

    private float _startRealtime = 0;
    private float _startTime = 0;
    private bool IsFirstCountRealtime = true;

    public InputField IFTimeScale;
    // Start is called before the first frame update
    void Start()
    {
        _startRealtime = Time.realtimeSinceStartup;
        _startTime = Time.time;
        IsFirstCountRealtime = true;
        Time.timeScale = TimescalParam;
        Debug.LogError("_startRealtime : " + _startRealtime);
        Debug.LogError("_startTime : " + _startTime);
        Debug.LogError("Time.timeScale : " + Time.timeScale);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFirstCountRealtime  && (Time.time - _startTime) > 5)
        {
            float timeInterval = Time.time - _startTime;

            Debug.LogError("RealTime interval : " + (Time.realtimeSinceStartup - _startRealtime));
            Debug.LogError("Time interval : " + timeInterval);
            IsFirstCountRealtime = false;
        }
        float realTimeInterval = Time.realtimeSinceStartup - _lastUpdateTime;
        Debug.Log("----------------------------");
        Debug.Log("[Update time interval:]" + realTimeInterval);
        Debug.Log("Update Time.deltaTime: " + Time.deltaTime);
        Debug.Log("Update Time.time: " + Time.time);
        _lastUpdateTime = Time.realtimeSinceStartup;
    }

    void LateUpdate()
    {
        float realTimeInterval = Time.realtimeSinceStartup - _lastLateUpdateTime;
        Debug.Log("----------------------------");
        Debug.Log("[LateUpdate time interval:]" + realTimeInterval);
        Debug.Log("LateUpdate Time.deltaTime: " + Time.deltaTime);
        Debug.Log("LateUpdate Time.time: " + Time.time);
        _lastLateUpdateTime = Time.realtimeSinceStartup;
    }

    void FixedUpdate()
    {
        float realTimeInterval = Time.realtimeSinceStartup - _lastFixedUpdateTime;
        Debug.Log("----------------------------");
        Debug.Log("[FixedUpdate time interval:]" + realTimeInterval);
        Debug.Log("FixedUpdate Time.fixedDeltaTime : " + Time.fixedDeltaTime);
        Debug.Log("FixedUpdate Time.time: ]" + Time.time);
        _lastFixedUpdateTime = Time.realtimeSinceStartup;
    }

    public void SetTimeScale()
    {
        if (IFTimeScale != null)
        {
            if (!string.IsNullOrEmpty(IFTimeScale.text))
            {
                TimescalParam = float.Parse(IFTimeScale.text);
                Time.timeScale = TimescalParam;
                Debug.LogError("Time.timeScale : " + Time.timeScale);
            }
        }

    }

    public void ChangeSpeed()
    {

        //设置音视频播放速度
        PlayController.Instance.SetVideoAudioSpeed(Time.timeScale);

        //设置时间轴速度
        //PlayController.Instance.ChangeTimelineSpeed(Time.timeScale);
    }
}
