using UnityEngine;
using UnityEngine.Audio;

public class SEManager : MonoBehaviour
{
    //SEごとに以下のテンプレートを、SEを鳴らしたいスクリプトに記述
    /*
    [Header("SEManager")]
    [Tooltip("SEManagerスクリプトを設定")]
    [SerializeField]
    SEManager _SEManager;

    [Header("AudioClip")]
    [Tooltip("鳴らしたいSEを設定")]
    [SerializeField]
    AudioClip _a;
    */

    //必要ならば上記と合わせて使用（SEを鳴らしたいスクリプトに記述）
    /*
    [Header("AudioMixer")]
    [Tooltip("AudioMixerGroupを設定")]
    [SerializeField]
    AudioMixerGroup _mixer;
    */

    static AudioSource[] _audioSource;//多めに枠を作ることで複数同時にSEを鳴らす時の余裕ができる
    private static SEManager _instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            _audioSource = new AudioSource[20];
            for (int i = 0; i < _audioSource.Length; i++)
            {//AudioSourceをアタッチ
                _audioSource[i] = gameObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 未使用のAudioSourceを取得
    /// </summary>
    /// <returns></returns>
    AudioSource GetUnusedAudioSource()
    {
        foreach (var audioSource in _audioSource)
        {
            if (audioSource.isPlaying == false)
            {
                Debug.Log("getUnused");
                return audioSource;
            }
        }
        return null;
    }

    /// <summary>
    /// ループしないSEを鳴らすメソッド
    /// </summary>
    /// <param name="clip"> 鳴らしたいSEを設定</param>
    /// <param name="mixer"> 任意でAudioMixerGroupを指定（デフォルトはnull）</param>
    public void PlaySE(AudioClip clip, AudioMixerGroup mixer = null)
    {
        var audioSource = GetUnusedAudioSource();
        if (audioSource != null)
        {
            if (mixer != null)
            {
                audioSource.outputAudioMixerGroup = mixer;
            }
            audioSource.clip = clip;//任意のAudioClipを指定
            audioSource.PlayOneShot(audioSource.clip);
        }
        else
        {
            Debug.LogWarning("空のAudioSourceが取得できませんでした");
            return;
        }
    }


    //PlaySELoopとStopSELoopで使うAudioSource
    AudioSource _audio;

    /// <summary>
    /// ループするSEを鳴らすメソッド
    /// </summary>
    /// <param name="clip"> 鳴らしたいSEを設定</param>
    /// <param name="mixer"> 任意でAudioMixerGroupを指定（デフォルトはnull）</param>
    public void PlaySELoop(AudioClip clip, AudioMixerGroup mixer = null)
    {
        _audio = GetUnusedAudioSource();
        if (_audio != null)
        {
            if (mixer != null)
            {
                _audio.outputAudioMixerGroup = mixer;
            }
            _audio.loop = true;
            _audio.clip = clip;//任意のAudioClipを指定
            _audio.Play();
        }
        else
        {
            Debug.LogWarning("空のAudioSourceが取得できませんでした");
            return;
        }
    }

    /// <summary>
    /// _audioを止めるメソッド
    /// </summary>
    public void StopSELoop()
    {
        if (_audio != null)
        {
            _audio.loop = false;
            _audio.clip = null;
            _audio.Stop();
        }
    }
}
