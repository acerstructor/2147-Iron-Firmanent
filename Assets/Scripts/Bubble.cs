using System.Collections;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float _bounceSize;
    [SerializeField] private LeanTweenType _leanTweenType;
    [SerializeField] private float _elevationSpeed;
    [SerializeField] private float _duration;

    private Vector2 _transform;

    private void Awake()
    {
        _transform = transform.position;
    }

    private void OnEnable()
    {
        StartCoroutine(SparkBubble());
        StartCoroutine(BubbleDuration(_duration));
    }

    private void Update()
    {
        MoveUp();
    }

    private IEnumerator BubbleDuration(float timeDuration)
    {
        yield return new WaitForSeconds(timeDuration);
        gameObject.SetActive(false);
    }

    private IEnumerator SparkBubble()
    {
        var prevScale = _transform;
        LeanTween.scale(gameObject, _transform, 0.1f);

        LeanTween.scale(gameObject, new Vector2(_transform.x + _bounceSize, _transform.y + _bounceSize), 0.2f).setEase(_leanTweenType);

        yield return new WaitForSeconds(0.1f);

        LeanTween.scale(gameObject, prevScale, 0.2f).setEase(_leanTweenType);
    }

    private void MoveUp()
    {
        transform.Translate(Vector2.up * _elevationSpeed * Time.deltaTime);
    }
}
