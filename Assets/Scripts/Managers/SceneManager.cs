using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManager : SingletonPersistent<SceneManager>
{
    [SerializeField] private GameObject _loadingScreen;
 //   [SerializeField] private Slider _loadingSlider;
    [SerializeField] private float _speed;
    [Range(0f, 1f)] 
    [SerializeField] private float _duration;

    private Image[] _loadingScreenChildren;
    private TMP_Text[] _labelChildren;

    public bool IsLoading { get; set; }

    protected override void Awake()
    {
        _loadingScreenChildren = GetComponentsInChildren<Image>();
        _labelChildren = GetComponentsInChildren<TMP_Text>();
        base.Awake();
    }

    private void Start()
    {
        _loadingScreen.SetActive(false);
    }
    public void LoadScene(int sceneId) => StartCoroutine(LoadSceneAsync(sceneId));

    private IEnumerator LoadSceneAsync(int sceneId)
    {
        _loadingScreen.SetActive(true);
    //    _loadingSlider.value = 0;

        Show(_duration);
        yield return new WaitForSeconds(_duration + _duration);

        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {
            var progressVal = Mathf.Clamp01(operation.progress / _speed);

      //      _loadingSlider.value += progressVal;

            yield return null;
        }

        Hide(_duration);
        yield return new WaitForSeconds(_duration);

        _loadingScreen.SetActive(false);
    }

    private void Show(float duration)
    {
        LeanTween.value(_loadingScreen, 0, 1, duration).setOnUpdate((float val) =>
        {
            Color newColor;
            foreach (var child in _loadingScreenChildren)
            {
                newColor = child.color;
                newColor.a = val;
                child.color = newColor;
            }

            foreach (var child in _labelChildren)
            {
                newColor = child.color;
                newColor.a = val;
                child.color = newColor;
            }
        });
    }

    private void Hide(float duration)
    {
        LeanTween.value(_loadingScreen, 1, 0, duration).setOnUpdate((float val) =>
        {
            Color newColor;
            foreach (var child in _loadingScreenChildren)
            {
                newColor = child.color;
                newColor.a = val;
                child.color = newColor;
            }

            foreach (var child in _labelChildren)
            {
                newColor = child.color;
                newColor.a = val;
                child.color = newColor;
            }
        });
    }
}
