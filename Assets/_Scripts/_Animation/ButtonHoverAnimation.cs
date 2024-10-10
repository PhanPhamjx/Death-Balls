using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale; // Lưu lại kích thước ban đầu
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Phát animation phình ra
        transform.DOScale(originalScale * 1.1f, 0.2f).SetEase(Ease.OutBack); // Phình ra trong 0.2 giây
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Quay lại kích thước ban đầu
        transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutBack); // Trở về trong 0.2 giây
    }

}
