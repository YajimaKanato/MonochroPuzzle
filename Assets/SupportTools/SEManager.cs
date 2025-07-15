using UnityEngine;
using UnityEngine.Audio;

public class SEManager : MonoBehaviour
{
    //SE���ƂɈȉ��̃e���v���[�g���ASE��炵�����X�N���v�g�ɋL�q
    /*
    [Header("SEManager")]
    [Tooltip("SEManager�X�N���v�g��ݒ�")]
    [SerializeField]
    SEManager _SEManager;

    [Header("AudioClip")]
    [Tooltip("�炵����SE��ݒ�")]
    [SerializeField]
    AudioClip _a;
    */

    //�K�v�Ȃ�Ώ�L�ƍ��킹�Ďg�p�iSE��炵�����X�N���v�g�ɋL�q�j
    /*
    [Header("AudioMixer")]
    [Tooltip("AudioMixerGroup��ݒ�")]
    [SerializeField]
    AudioMixerGroup _mixer;
    */

    static AudioSource[] _audioSource;//���߂ɘg����邱�Ƃŕ���������SE��炷���̗]�T���ł���
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
            {//AudioSource���A�^�b�`
                _audioSource[i] = gameObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ���g�p��AudioSource���擾
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
    /// ���[�v���Ȃ�SE��炷���\�b�h
    /// </summary>
    /// <param name="clip"> �炵����SE��ݒ�</param>
    /// <param name="mixer"> �C�ӂ�AudioMixerGroup���w��i�f�t�H���g��null�j</param>
    public void PlaySE(AudioClip clip, AudioMixerGroup mixer = null)
    {
        var audioSource = GetUnusedAudioSource();
        if (audioSource != null)
        {
            if (mixer != null)
            {
                audioSource.outputAudioMixerGroup = mixer;
            }
            audioSource.clip = clip;//�C�ӂ�AudioClip���w��
            audioSource.PlayOneShot(audioSource.clip);
        }
        else
        {
            Debug.LogWarning("���AudioSource���擾�ł��܂���ł���");
            return;
        }
    }


    //PlaySELoop��StopSELoop�Ŏg��AudioSource
    AudioSource _audio;

    /// <summary>
    /// ���[�v����SE��炷���\�b�h
    /// </summary>
    /// <param name="clip"> �炵����SE��ݒ�</param>
    /// <param name="mixer"> �C�ӂ�AudioMixerGroup���w��i�f�t�H���g��null�j</param>
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
            _audio.clip = clip;//�C�ӂ�AudioClip���w��
            _audio.Play();
        }
        else
        {
            Debug.LogWarning("���AudioSource���擾�ł��܂���ł���");
            return;
        }
    }

    /// <summary>
    /// _audio���~�߂郁�\�b�h
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
