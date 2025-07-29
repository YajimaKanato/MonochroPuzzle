using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    /*
    <このスクリプトの使い方>
    ・フェードインまたはフェードアウトさせたい"Image(UnityEngine.UI.Image)"オブジェクトにアタッチ
    ・初期状態をアクティブにするとフェードインする
    ・"SceneChange"スクリプトをアタッチしたオブジェクトのインスペクターに参照場所があるためそこにドラッグアンドドロップ
    <応用>
    ・その他のトリガーでフェードアウトを実行させる場合は、
    　非アクティブ状態のこのオブジェクトをフェードアウトさせるタイミングでアクティブにする（以下を使用）

    //フィールドに記述
    [Header("FadeOutObject")]
    [Tooltip("フェードアウトさせたいオブジェクトを設定")]
    [SerializeField]
    Image _fade;

    //関数内に記述
    _fade.gameObject.SetActive(true);
    */

    [Tooltip("フェードインしたいときにチェック")]
    [SerializeField]
    bool _fadein;
    [Tooltip("フェードアウトしたいときにチェック")]
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
    /// シーンに開始時等に実行される関数
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
            GetComponent<Image>().color = new Color(0, 0, 0, alpha);//Imageの透明度をあげる
            yield return null;
        }
        _fadein = false;
        this.gameObject.SetActive(false);
        yield break;
    }

    /// <summary>
    /// シーン遷移前等に実行される関数
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
            GetComponent<Image>().color = new Color(0, 0, 0, alpha);//Imageの透明度を下げる
            yield return null;
        }
        yield break;
    }
}
