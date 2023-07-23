using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour 
{
    private TMP_Text _text;

    public bool IsBlinking { get; private set; }

    private void Awake()
    {
        _text = gameObject.GetComponent<TMP_Text>();
    }

    private void Start()
    {
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        IsBlinking = true;
        _text.overrideColorTags = true;
        var isTransparent = false;

        while (IsBlinking)
        {
            if (!isTransparent)
            {
                _text.color = new Color32(255, 255, 255, 1);
                isTransparent = true;
            }
            else
            {
                _text.color = new Color32(255, 255, 255, 255);
                isTransparent = false;
            }
            yield return new WaitForSeconds(0.5f);
        }

        yield return null;
    }

    private void OnDisable()
    {
        StopCoroutine(Blink());
    }
}
