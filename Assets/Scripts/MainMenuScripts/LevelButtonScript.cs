using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelButtonScript : MonoBehaviour
{
    private Button button;
    private RectTransform rectTransform;

    public Vector3 expandedScale = new Vector3(1.2f, 1.2f, 1);
    public Vector3 normalScale = new Vector3(1f, 1f, 1);
    public Color highlightColor = Color.gray;
    private Color originalColor;

    void Start()
    {
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();

        originalColor = button.colors.normalColor;

        if (button == null || rectTransform == null)
        {
            Debug.LogError("Button or RectTransform is missing!");
            return;
        }

        if (rectTransform != null && rectTransform.gameObject.activeInHierarchy)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    public void OnButtonSelected()
    {
        if (rectTransform != null && rectTransform.gameObject.activeInHierarchy)
        {
            rectTransform.DOScale(expandedScale, 0.2f).SetEase(Ease.OutBack);
        }
    }

    public void OnButtonDeselected()
    {
        if (rectTransform != null && rectTransform.gameObject.activeInHierarchy)
        {
            rectTransform.DOScale(normalScale, 0.2f).SetEase(Ease.OutBack);
            ChangeButtonColor(originalColor);
        }
    }

    public void OnButtonClick()
    {
        if (rectTransform != null && rectTransform.gameObject.activeInHierarchy)
        {
            ChangeButtonColor(highlightColor);
        }
    }

    private void ChangeButtonColor(Color color)
    {
        if (button != null)
        {
            ColorBlock colorBlock = button.colors;
            colorBlock.normalColor = color;
            colorBlock.highlightedColor = color;
            colorBlock.pressedColor = color;
            button.colors = colorBlock;
        }
    }

    // Optional: Kill the tween when the object is destroyed or when scene changes
    private void OnDestroy()
    {
        // Optionally kill any running tweens
        if (rectTransform != null)
        {
            DOTween.Kill(rectTransform);
        }
    }
}
