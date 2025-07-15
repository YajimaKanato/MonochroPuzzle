using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
class SceneData
{
    [Header("Scene Name"), Tooltip("シーンの名前")]
    public string _sceneName;

    [Header("Scene BGM"), Tooltip("シーンで鳴らしたいBGM")]
    public AudioClip _clip;
}

[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour
{
    [Header("Scene Data"), Tooltip("シーンのデータを設定してください")]
    [SerializeField]
    List<SceneData> _sceneData;

    static AudioSource _audioSource;
    private static BGMManager _instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_instance == null)//シングルトン処理
        {
            _instance = this;
            _audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
            //シーンが切り替わったとき（ロード完了時）にBGMが変更される
            SceneManager.sceneLoaded += BGMChange;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// シーンが切り替わったとき（ロード完了時）にBGMを変更する
    /// </summary>
    /// <param name="scene"> 変更後のシーンの名前</param>
    /// <param name="loadScene"> シーンの読み込みモード（SingleかAdditive）</param>
    void BGMChange(Scene scene, LoadSceneMode loadScene)
    {
        foreach (SceneData data in _sceneData)
        {
            if(data._sceneName == scene.name)
            {
                ChangeBGM(data._clip, data._sceneName);
            }
        }
    }

    /// <summary>
    /// シーンが切り替わったときにBGMが切り替わる
    /// </summary>
    /// <param name="clip"> 再生するクリップ</param>
    /// <param name="name"> 再生するシーンの名前</param>
    void ChangeBGM(AudioClip clip, string name)
    {
        if (_audioSource != null)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
            Debug.Log(name + "BGM");
        }
        else
        {
            Debug.LogWarning("AudioSourceが設定されていません");
            return;
        }
    }
}
