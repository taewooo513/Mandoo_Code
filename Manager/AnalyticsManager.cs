using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Linq;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
public class AnalyticsManager : Singleton<AnalyticsManager>
{
    private int _step = 0;
    public int Step => _step;
    private void Start()
    {

    }
    public void Update()
    {
        //Unity.Services.Analytics.Event ev = new Unity.Services.Analytics.Event();
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    AnalyticsService.Instance.RecordEvent("Test");
        //    AnalyticsService.Instance.RecordEvent(,)
        //}
    }

    async public UniTask Init()
    {
        try
        {
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    IEnumerator CoInit()
    {
        yield return UnityServices.InitializeAsync();

        AnalyticsService.Instance.StartDataCollection();
        Debug.Log("AnalyticsManager Completed");
    }


    public void SendEventStep(int step)
    {
        // Debug.Log($"currentStep: {_step} inputStep: {step}");
        if (step - 1 == _step)
        {
            // Debug.Log($"{step} Send Event");

            var myEvent = new Unity.Services.Analytics.CustomEvent("funnelStepv2")
            {
                { "stepNumberv3", step }
            };

            AnalyticsService.Instance.RecordEvent(myEvent);

            _step = step;
        }
    }

    IEnumerator SendEvent(int step)
    {
        while (UnityServices.State != ServicesInitializationState.Initialized)
            yield return new WaitForSeconds(1f);
        var myEvent = new Unity.Services.Analytics.CustomEvent("FunnelStep")
            {
                { "stepNumber", step }
            };

        AnalyticsService.Instance.RecordEvent(myEvent);

        _step = step;
        yield break;
    }
}
