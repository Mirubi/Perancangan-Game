using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitStopManager : MonoBehaviour
{
    public static HitStopManager Instance;
    public bool IsHitStop { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void DoHitStop(float duration)
    {
        StartCoroutine(HitStopCoroutine(duration));
    }

    private IEnumerator HitStopCoroutine(float duration)
    {
        IsHitStop = true;
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = originalTimeScale;
        IsHitStop = false;
    }
}
