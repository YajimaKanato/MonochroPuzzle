using UnityEngine;

public class StageSelectButton : MonoBehaviour
{
    GameObject _director;

    [Header("StageNumber")]
    [Tooltip("���̃X�N���v�g���A�^�b�`����Ă���X�e�[�W�̔ԍ�")]
    [SerializeField]
    int _num;

    /// <summary>
    /// �X�e�[�W�̔ԍ����f�B���N�^�[�ɒm�点��֐�
    /// </summary>
    public void StageNumber()
    {
        _director = GameObject.FindWithTag("GameDirector");
        if (_director)
        {
            _director.GetComponent<GameDirector>().StageNumber(_num - 1);
        }
        else
        {
            Debug.LogWarning("GameDirector�I�u�W�F�N�g��������܂���ł���");
        }
    }

    public void StageReset()
    {
        _director = GameObject.FindWithTag("GameDirector");
        if (_director)
        {
            _director.GetComponent<GameDirector>().StageNumber(_num - 1);
            _director.GetComponent<GameDirector>().StageReset();
        }
        else
        {
            Debug.LogWarning("GameDirector�I�u�W�F�N�g��������܂���ł���");
        }
    }
}
