using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    /*
    <���̃X�N���v�g�̎g����>
    �P�D"Button(UnityEngine.UI.Button)"�ɃA�^�b�`���A�A�^�b�`�����I�u�W�F�N�g��I��
    �Q�D�C���X�y�N�^�[�ɂ���Button�R���|�[�l���g��"On Click()"��"+"���N���b�N
    �R�D"None"�Ə����Ă���Ƃ���ɃC���X�y�N�^�[���炱�̃X�N���v�g���h���b�O�A���h�h���b�v
    �S�D"No Function"����SceneChange->ChangeScene(String)���N���b�N
    �T�D�󗓂ɑJ�ڌ�̃V�[���̖��O��ݒ�
    */

    [Header("Fade")]
    [Tooltip("�t�F�[�h�C���A�E�g���������Ƃ���\"Fade\"��ݒ�")]
    [SerializeField]
    Image _fade;

    public void ChangeScene(string name)
    {
        if (_fade != null)
        {
            _fade.gameObject.SetActive(true);
            StartCoroutine(FadeStayCoroutine(name));
        }
        else
        {
            SceneManager.LoadScene(name);
        }
    }

    IEnumerator FadeStayCoroutine(string name)
    {
        yield return new WaitForSeconds(1.0f);//�t�F�[�h�C���A�E�g�ɍ��킹��
        SceneManager.LoadScene(name);
    }
}
