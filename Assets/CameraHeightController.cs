using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHeightController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] float higherHeight;
    [SerializeField] float originalHeight;
    [SerializeField] float transitionDuration;

    private float originalFieldOfView;
    private bool isTransitioning;

    private void Start()
    {
        originalFieldOfView = virtualCamera.m_Lens.FieldOfView;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            // Start the transition to the higher camera height
            StartCoroutine(TransitionCameraHeight(higherHeight));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            // Start the transition back to the original camera height
            StartCoroutine(TransitionCameraHeight(originalHeight));
        }
    }

    private System.Collections.IEnumerator TransitionCameraHeight(float targetHeight)
    {
        isTransitioning = true;

        float elapsedTime = 0f;
        float startHeight = virtualCamera.m_Lens.FieldOfView;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            float currentHeight = Mathf.Lerp(startHeight, targetHeight, t);

            virtualCamera.m_Lens.FieldOfView = currentHeight;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        virtualCamera.m_Lens.FieldOfView = targetHeight;
        isTransitioning = false;
    }
}
