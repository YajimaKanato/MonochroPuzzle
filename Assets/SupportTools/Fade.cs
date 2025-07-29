using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    /*
    <���̃X�N���v�g�̎g����>
    �E�t�F�[�h�C���܂��̓t�F�[�h�A�E�g��������"Image(UnityEngine.UI.Image)"�I�u�W�F�N�g�ɃA�^�b�`
    �E������Ԃ��A�N�e�B�u�ɂ���ƃt�F�[�h�C������
    �E"SceneChange"�X�N���v�g���A�^�b�`�����I�u�W�F�N�g�̃C���X�y�N�^�[�ɎQ�Əꏊ�����邽�߂����Ƀh���b�O�A���h�h���b�v
    <���p>
    �E���̑��̃g���K�[�Ńt�F�[�h�A�E�g�����s������ꍇ�́A
    �@��A�N�e�B�u��Ԃ̂��̃I�u�W�F�N�g���t�F�[�h�A�E�g������^�C�~���O�ŃA�N�e�B�u�ɂ���i�ȉ����g�p�j

    //�t�B�[���h�ɋL�q
    [Header("FadeOutObject")]
    [Tooltip("�t�F�[�h�A�E�g���������I�u�W�F�N�g��ݒ�")]
    [SerializeField]
    Image _fade;

    //�֐����ɋL�q
    _fade.gameObject.SetActive(true);
    */

    [Tooltip("�t�F�[�h�C���������Ƃ��Ƀ`�F�b�N")]
    [SerializeField]
    bool _fadein;
    [Tooltip("�t�F�[�h�A�E�g�������Ƃ��Ƀ`�F�b�N")]
    [SerializeField]
    bool _fadeout;

    private void OnEnable()
    {
        if (_fadein)
        {
            StartCoroutine(FadeInCoroutine());
        }
        else if (_fadeout)
        {
            StartCoroutine(FadeOutCoroutine());
        }
    }

    private void Start()
    {
        if (!_fadein)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// �V�[���ɊJ�n�����Ɏ��s�����֐�
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeInCoroutine()
    {
        Debug.Log("FadeIn");
        float alpha = 1.0f;
        while (alpha != 0)
        {
            alpha -= Time.deltaTime;
            if (alpha <= 0)
            {
                alpha = 0;
            }
            GetComponent<Image>().color = new Color(0, 0, 0, alpha);//Image�̓����x��������
            yield return null;
        }
        _fadein = false;
        this.gameObject.SetActive(false);
        yield break;
    }

    /// <summary>
    /// �V�[���J�ڑO���Ɏ��s�����֐�
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOutCoroutine()
    {
        Debug.Log("FadeOut");
        float alpha = 0.0f;
        while (alpha != 1)
        {
            alpha += Time.deltaTime;
            if (alpha >= 1)
            {
                alpha = 1;
            }
            GetComponent<Image>().color = new Color(0, 0, 0, alpha);//Image�̓����x��������
            yield return null;
        }
        yield break;
    }
}
