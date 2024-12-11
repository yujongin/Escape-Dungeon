using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// 날짜 : 2021-01-30 PM 8:02:49
// 작성자 : Rito


public class BindingPairUI : MonoBehaviour
{
    public bool selected = false;

    public TMP_Text actionLabel;
    public TMP_Text codeLabel;
    public Button codeButton;
    public Image buttonImage;

    public void Select()
    {
        selected = true;
        buttonImage.color = Color.green;
    }

    public void Deselect()
    {
        selected = false;
        buttonImage.color = Color.white;
    }

    public void InitLabels(string actionText, string codeText)
    {
        actionLabel.text = actionText;
        codeLabel.text = codeText;
    }

    public void SetCodeLabel(string text)
    {
        codeLabel.text = text;
    }

    public void AddButtonListener(UnityAction method)
    {
        codeButton.onClick.AddListener(method);
    }
}
