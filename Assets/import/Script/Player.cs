using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("HitObjectLayer")]
    [SerializeField]
    LayerMask _hitLayer;
    RaycastHit2D _hitWall;
    static RaycastHit2D _hitBW;//色変え時に使用
    public static RaycastHit2D HitBW { get { return _hitBW; } }
    Vector3 _dir;

    SpriteRenderer _spriteRenderer;

    float _moveX, _moveY;
    static bool _isMoving = false;
    public static bool IsMoving { get { return _isMoving; } }
    bool _isBlack = false;
    bool _isColorChanging = false;//色変えの時が来たら使う

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //方向転換
        if (!_isMoving && !GameDirector.IsPausing)
        {
            //移動入力
            _moveX = Input.GetAxisRaw("Horizontal");
            _moveY = Input.GetAxisRaw("Vertical");

            if (_moveX == 1)
            {
                transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else if (_moveX == -1)
            {
                transform.localEulerAngles = new Vector3(0, 0, 180);
            }
            else if (_moveY == 1)
            {
                transform.localEulerAngles = new Vector3(0, 0, 90);
            }
            else if (_moveY == -1)
            {
                transform.localEulerAngles = new Vector3(0, 0, 270);
            }
        }

        //進行方向
        _dir = new Vector3(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad));

        //LineCasts
        Debug.DrawLine(transform.position,
            transform.position + _dir);//接地判定に関するもの
        _hitWall = Physics2D.Linecast(transform.position,
            transform.position + _dir,
            _hitLayer);
        Debug.DrawLine(transform.position,
            transform.position + _dir);//接地判定に関するもの
        _hitBW = Physics2D.Linecast(transform.position,
            transform.position + _dir,
            _hitLayer & ~(1 << LayerMask.NameToLayer("Wall")));

        //移動
        if (!_isMoving && Mathf.Abs(_moveX) == 1 && !_hitWall.collider)
        {// && !_isColorChanging
            Debug.Log("左右移動");
            _moveY = 0;
            _isMoving = true;
            Vector3 basePos = transform.position;
            StartCoroutine(MoveCoroutine(_moveX, _moveY, basePos));
        }
        else if (!_isMoving && Mathf.Abs(_moveY) == 1 && !_hitWall.collider)
        {// && !_isColorChanging
            Debug.Log("上下移動");
            _moveX = 0;
            _isMoving = true;
            Vector3 basePos = transform.position;
            StartCoroutine(MoveCoroutine(_moveX, _moveY, basePos));
        }
        //色変更
        /*else if (Input.GetMouseButtonDown(0) && _hitBW && !_isMoving && !_isColorChanging && CloneofPlayer.HitBW && !CloneofPlayer.IsMoving)
        {
            ColorChange(transform.eulerAngles.z * Mathf.Deg2Rad);
        }*/
    }

    /// <summary>
    /// 移動を滑らかにするコルーチン
    /// </summary>
    /// <param name="moveX"></param>
    /// <param name="moveY"></param>
    /// <param name="basePos"></param>
    /// <returns></returns>
    IEnumerator MoveCoroutine(float moveX, float moveY, Vector3 basePos)
    {
        _dir = new Vector3(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad));
        while (true)
        {
            if (Vector3.Distance(transform.position, basePos) < 1.0f)
            {
                transform.position += _dir / 90;
                yield return null;
            }
            else
            {
                transform.position = basePos + _dir;//移動による誤差の調整
                _isMoving = false;
                yield break;
            }
        }
    }

    /// <summary>
    /// 任意の時間待つコルーチン
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator ColorChangeCoroutine(float time)
    {
        _isColorChanging = true;
        yield return new WaitForSeconds(time);
        _isColorChanging = false;
        yield break;
    }

    /// <summary>
    /// 特定の操作で白黒操作を反転させるメソッド
    /// </summary>
    void ColorChange(float theta)
    {
        if (!_isBlack)
        {
            Debug.Log("黒に変更");
            _hitLayer &= ~(1 << LayerMask.NameToLayer("White"));//変更前の色のフラグを消す
            _hitLayer |= (1 << LayerMask.NameToLayer("Black"));//変更後の色のフラグを立てる
            _spriteRenderer.color = Color.black;
            _isBlack = true;
        }
        else
        {
            Debug.Log("白に変更");
            _hitLayer &= ~(1 << LayerMask.NameToLayer("Black"));
            _hitLayer |= (1 << LayerMask.NameToLayer("White"));
            _spriteRenderer.color = Color.white;
            _isBlack = false;
        }

        transform.position += new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);//上下反転にともなった座標調整
        theta += Mathf.PI;
        transform.eulerAngles = new Vector3(0, 0, theta * Mathf.Rad2Deg);

        StartCoroutine(ColorChangeCoroutine(0.5f));
    }
}
