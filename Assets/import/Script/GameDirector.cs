using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    //class
    GameObject _pause;//"Pause"タグで取得
    GameObject _result;//"Result"タグで取得
    GameObject _playerGoal, _cloneGoal;//"Goalfor..."タグで取得
    GameObject _stageButton;//"StageGroup"タグで取得

    //struct

    //変数
    int _index = -1;
    [Header("Number of Stages")]
    [Tooltip("ステージの個数")]
    [SerializeField]
    int _num;

    //PlayerPrefsで保存
    int _stageOpen;//ステージ解放するかどうか
    int _stageClear;//ステージをクリアしたかどうか
    string _writeStageOpen;

    static bool _isPausing = false;//ポーズ画面かどうか
    public static bool IsPausing { get { return _isPausing; } }

    private static GameDirector _instance;
    private void Start()
    {
        if (_instance == null)//シングルトン処理
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            //ステージのクリア判定初期化
            //データ取得
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

            //ステージボタンの配色を初期化
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
                Debug.LogWarning("\"StageGroup\"タグを持つオブジェクトを取得できませんでした");
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
        //プレイ中のUI処理
        if (_playerGoal && _cloneGoal && _pause)//ポーズ画面、2種類のゴールがあるかを判定
        {
            if (_playerGoal.GetComponent<GoalforPlayer>().GetIsGoal() && _cloneGoal.GetComponent<GoalforClone>().GetIsGoal())
            {//プレイヤーもクローンもゴールした場合
                //リザルト表示
                if (_result)
                {
                    if (!_result.activeSelf)
                    {
                        ObjectActive(_result);
                    }
                }
                else
                {//nullチェック
                    Debug.LogWarning("リザルト表示のオブジェクトがありません");
                }

                //クリア判定
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
                //ポーズ画面表示
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
            Debug.LogWarning("ポーズ画面、プレイヤー用のゴール、クローン用のゴールのいずれかのオブジェクトがありません");
        }


    }

    /// <summary>
    /// シーンがロード完了されたときに呼び出す関数
    /// </summary>
    /// <param name="scene"> ロードが完了したシーン</param>
    /// <param name="mode"> SingleかAdditiveが割り当てられる</param>
    void FindGameObject(Scene scene, LoadSceneMode mode)
    {
        //インゲームに入ったときにUpdateで必要な条件の準備
        if (scene.name.Contains("Stage"))
        {
            _isPausing = false;
            _pause = GameObject.FindWithTag("Pause");
            _result = GameObject.FindWithTag("Result");
            _cloneGoal = GameObject.FindWithTag("GoalforClone");
            _playerGoal = GameObject.FindWithTag("GoalforPlayer");
        }

        //ステージボタンの色を更新
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
                Debug.LogWarning("\"StageGroup\"タグを持つオブジェクトを取得できませんでした");
            }
        }
    }

    /// <summary>
    /// 引数のオブジェクトをアクティブにする関数
    /// </summary>
    /// <param name="obj"> アクティブにするオブジェクト</param>
    void ObjectActive(GameObject obj)
    {
        obj.SetActive(true);
    }

    /// <summary>
    /// 引数のオブジェクトを非アクティブにする関数
    /// </summary>
    /// <param name="obj"> 非アクティブにするオブジェクト</param>
    void ObjectInactive(GameObject obj)
    {
        obj.SetActive(false);
    }

    /// <summary>
    /// 選択されたステージの番号を取得する関数
    /// </summary>
    /// <param name="num"> ステージ番号-1</param>
    public void StageNumber(int num)
    {
        _index = num;
    }
}
