using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
class SceneData
{
    [Header("Scene Name"), Tooltip("�V�[���̖��O")]
    public string _sceneName;

    [Header("Scene BGM"), Tooltip("�V�[���Ŗ炵����BGM")]
    public AudioClip _clip;
}

[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour
{
    [Header("Scene Data"), Tooltip("�V�[���̃f�[�^��ݒ肵�Ă�������")]
    [SerializeField]
    List<SceneData> _sceneData;

    static AudioSource _audioSource;
    private static BGMManager _instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_instance == null)//�V���O���g������
        {
            _instance = this;
            _audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
            //�V�[�����؂�ւ�����Ƃ��i���[�h�������j��BGM���ύX�����
            SceneManager.sceneLoaded += BGMChange;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �V�[�����؂�ւ�����Ƃ��i���[�h�������j��BGM��ύX����
    /// </summary>
    /// <param name="scene"> �ύX��̃V�[���̖��O</param>
    /// <param name="loadScene"> �V�[���̓ǂݍ��݃��[�h�iSingle��Additive�j</param>
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
    /// �V�[�����؂�ւ�����Ƃ���BGM���؂�ւ��
    /// </summary>
    /// <param name="clip"> �Đ�����N���b�v</param>
    /// <param name="name"> �Đ�����V�[���̖��O</param>
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
            Debug.LogWarning("AudioSource���ݒ肳��Ă��܂���");
            return;
        }
    }
}
