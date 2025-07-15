using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    //class
    GameObject _pause;//"Pause"�^�O�Ŏ擾
    GameObject _result;//"Result"�^�O�Ŏ擾
    GameObject _playerGoal, _cloneGoal;//"Goalfor..."�^�O�Ŏ擾
    GameObject _stageButton;//"StageGroup"�^�O�Ŏ擾

    //struct

    //�ϐ�
    int _index = -1;
    [Header("Number of Stages")]
    [Tooltip("�X�e�[�W�̌�")]
    [SerializeField]
    int _num;

    //PlayerPrefs�ŕۑ�
    int _stageOpen;//�X�e�[�W������邩�ǂ���
    int _stageClear;//�X�e�[�W���N���A�������ǂ���
    string _writeStageOpen;

    static bool _isPausing = false;//�|�[�Y��ʂ��ǂ���
    public static bool IsPausing { get { return _isPausing; } }

    private static GameDirector _instance;
    private void Start()
    {
        if (_instance == null)//�V���O���g������
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            //�X�e�[�W�̃N���A���菉����
            //�f�[�^�擾
            _writeStageOpen = PlayerPrefs.GetString("StageOpenData");
            if (_writeStageOpen == "")
            {
                _stageClear = 0;
                _stageOpen = 1 << 0;
                _writeStageOpen = "Save";
            }
            else
            {
                _stageOpen = PlayerPrefs.GetInt("StageOpen");
            }

            //�X�e�[�W�{�^���̔z�F��������
            _stageButton = GameObject.FindWithTag("StageGroup");
            if (_stageButton != null)
            {
                int i = 0;
                foreach (Transform stageButton in _stageButton.transform)
                {
                    if ((_stageOpen & (1 << i)) != 0)
                    {
                        stageButton.gameObject.GetComponent<Image>().color = Color.black;
                    }
                    else
                    {
                        stageButton.gameObject.GetComponent<Image>().color = Color.gray;
                    }
                    i++;
                }
            }
            else
            {
                Debug.LogWarning("\"StageGroup\"�^�O�����I�u�W�F�N�g���擾�ł��܂���ł���");
            }
            SceneManager.sceneLoaded += FindGameObject;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //�v���C����UI����
        if (_playerGoal && _cloneGoal && _pause)//�|�[�Y��ʁA2��ނ̃S�[�������邩�𔻒�
        {
            if (_playerGoal.GetComponent<GoalforPlayer>().GetIsGoal() && _cloneGoal.GetComponent<GoalforClone>().GetIsGoal())
            {//�v���C���[���N���[�����S�[�������ꍇ
                //���U���g�\��
                if (_result)
                {
                    if (!_result.activeSelf)
                    {
                        ObjectActive(_result);
                    }
                }
                else
                {//null�`�F�b�N
                    Debug.LogWarning("���U���g�\���̃I�u�W�F�N�g������܂���");
                }

                //�N���A����
                if (_index != -1)
                {
                    if ((_stageOpen & (1 << _index)) != 0)
                    {
                        _stageClear |= 1 << _index;
                        _stageOpen |= 1 << (_index + 1);
                        PlayerPrefs.SetInt("StageClear", _stageClear);
                        PlayerPrefs.SetInt("StageOpen", _stageOpen);
                        PlayerPrefs.SetString("StageOpenData", _writeStageOpen);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                //�|�[�Y��ʕ\��
                if (_pause.activeSelf == false)
                {
                    _isPausing = true;
                    ObjectActive(_pause);
                }
                else
                {
                    _isPausing = false;
                    ObjectInactive(_pause);
                }
            }
        }
        else if (SceneManager.GetActiveScene().name.Contains("Stage"))
        {
            Debug.LogWarning("�|�[�Y��ʁA�v���C���[�p�̃S�[���A�N���[���p�̃S�[���̂����ꂩ�̃I�u�W�F�N�g������܂���");
        }


    }

    /// <summary>
    /// �V�[�������[�h�������ꂽ�Ƃ��ɌĂяo���֐�
    /// </summary>
    /// <param name="scene"> ���[�h�����������V�[��</param>
    /// <param name="mode"> Single��Additive�����蓖�Ă���</param>
    void FindGameObject(Scene scene, LoadSceneMode mode)
    {
        //�C���Q�[���ɓ������Ƃ���Update�ŕK�v�ȏ����̏���
        if (scene.name.Contains("Stage"))
        {
            _isPausing = false;
            _pause = GameObject.FindWithTag("Pause");
            _result = GameObject.FindWithTag("Result");
            _cloneGoal = GameObject.FindWithTag("GoalforClone");
            _playerGoal = GameObject.FindWithTag("GoalforPlayer");
        }

        //�X�e�[�W�{�^���̐F���X�V
        if (scene.name == "Select")
        {
            _stageButton = GameObject.FindWithTag("StageGroup");
            if (_stageButton != null)
            {
                int i = 0;
                foreach (Transform stageButton in _stageButton.transform)
                {
                    if ((_stageOpen & (1 << i)) != 0)
                    {
                        stageButton.gameObject.GetComponent<Image>().color = Color.black;
                    }
                    else
                    {
                        stageButton.gameObject.GetComponent<Image>().color = Color.gray;
                    }
                    i++;
                }
            }
            else
            {
                Debug.LogWarning("\"StageGroup\"�^�O�����I�u�W�F�N�g���擾�ł��܂���ł���");
            }
        }
    }

    /// <summary>
    /// �����̃I�u�W�F�N�g���A�N�e�B�u�ɂ���֐�
    /// </summary>
    /// <param name="obj"> �A�N�e�B�u�ɂ���I�u�W�F�N�g</param>
    void ObjectActive(GameObject obj)
    {
        obj.SetActive(true);
    }

    /// <summary>
    /// �����̃I�u�W�F�N�g���A�N�e�B�u�ɂ���֐�
    /// </summary>
    /// <param name="obj"> ��A�N�e�B�u�ɂ���I�u�W�F�N�g</param>
    void ObjectInactive(GameObject obj)
    {
        obj.SetActive(false);
    }

    /// <summary>
    /// �I�����ꂽ�X�e�[�W�̔ԍ����擾����֐�
    /// </summary>
    /// <param name="num"> �X�e�[�W�ԍ�-1</param>
    public void StageNumber(int num)
    {
        _index = num;
    }
}
