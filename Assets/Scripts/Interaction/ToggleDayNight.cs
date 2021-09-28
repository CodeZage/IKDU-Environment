using System.Collections;
using Interaction;
using UnityEngine;

public class ToggleDayNight : Interactable
{
    private Light _sun;

    protected override void Start()
    {
        if (interactInfo == "") interactInfo = "Change Time of Day";

        _sun = RenderSettings.sun;
    }

    public override void Interact()
    {
        StartCoroutine(LerpRotation(_sun.transform, Quaternion.Inverse(_sun.transform.rotation), 4f));
    }

    protected IEnumerator LerpRotation(Transform target, Quaternion targetRotation, float duration)
    {
        isInteractable = false;

        float time = 0;
        var startRotation = target.rotation;

        while (time < duration)
        {
            target.rotation = Quaternion.Lerp(startRotation, targetRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        isInteractable = true;
    }
}
