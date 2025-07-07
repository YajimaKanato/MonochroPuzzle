using UnityEngine;

public class StageSelectButton : MonoBehaviour
{
    GameObject _director;

    [Header("StageNumber")]
    [Tooltip("このスクリプトがアタッチされているステージの番号")]
    [SerializeField]
    int _num;

    /// <summary>
    /// ステージの番号をディレクターに知らせる関数
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
            Debug.LogWarning("GameDirectorオブジェクトが見つかりませんでした");
        }
    }
}
