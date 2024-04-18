using UnityEngine;
using UnityEngine.EventSystems;
public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform textRectTransform;
    private Vector3 originalPosition;
    SoundManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundManager>();
    }

    void Start()
    {
        // Store the original position of the RectTransform
        originalPosition = textRectTransform.localPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Move the RectTransform 30 units to the left when the mouse hovers over it
        textRectTransform.localPosition = new Vector3(originalPosition.x + 15, originalPosition.y, originalPosition.z);
        audioManager.PlaySFX(audioManager.ButtonHover);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Reset the RectTransform to its original position when the mouse leaves
        textRectTransform.localPosition = originalPosition;
    }
}
