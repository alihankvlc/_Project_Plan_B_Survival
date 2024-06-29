using System.Collections;
using System.Collections.Generic;
using _Other_.Runtime.Code;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public sealed class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Slider _loadingSlider;
    [SerializeField] private TextMeshProUGUI _loadingCounterTextMesh;
    [SerializeField] private string _sceneName;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad = true;
    }

    public void LoadScene()
    {
        StartCoroutine(LoadAsync(_sceneName));
    }

    private IEnumerator LoadAsync(string sceneName)
    {
        _loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            string info = "Loading " + (progress * 100f).ToString("F0") + "%";
            
            _loadingCounterTextMesh.SetText(info);
            _loadingSlider.value = progress;

            if (operation.progress >= 0.9f && _loadingSlider.value >= 1.0f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        _loadingScreen.SetActive(false);
    }
}