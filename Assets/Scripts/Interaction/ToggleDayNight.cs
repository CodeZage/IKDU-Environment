using System.Collections;
using System.Collections.Generic;
using Interaction;
using UnityEngine;

public class ToggleDayNight : Interactable
{
    private Light sun;
    private Quaternion initialRotation;

    protected override void Start()
    {
        if (interactInfo == "") interactInfo = "Change Time of Day";

        sun = RenderSettings.sun;
        initialRotation = sun.transform.rotation;
    }

    public override void Interact()
    {
        StartCoroutine(LerpRotation(sun.transform, Quaternion.Inverse(sun.transform.rotation), 4f));
    }

    protected IEnumerator LerpRotation(Transform target, Quaternion targetRotation, float duration)
    {
        float time = 0;
        Quaternion startRotation = target.rotation;

        while (time < duration)
        {
            target.rotation = Quaternion.Lerp(startRotation, targetRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
    }

}
