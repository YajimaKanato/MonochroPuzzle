using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    /*
    <このスクリプトの使い方>
    １．"Button(UnityEngine.UI.Button)"にアタッチし、アタッチしたオブジェクトを選択
    ２．インスペクターにあるButtonコンポーネントの"On Click()"の"+"をクリック
    ３．"None"と書いてあるところにインスペクターからこのスクリプトをドラッグアンドドロップ
    ４．"No Function"からSceneChange->ChangeScene(String)をクリック
    ５．空欄に遷移後のシーンの名前を設定
    */

    [Header("Fade")]
    [Tooltip("フェードインアウトをしたいときに\"Fade\"を設定")]
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
        yield return new WaitForSeconds(1.0f);//フェードインアウトに合わせる
        SceneManager.LoadScene(name);
    }
}
