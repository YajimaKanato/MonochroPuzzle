using UnityEngine;
using System.Collections;

public class CloneofPlayer : MonoBehaviour
{
    //Playerスクリプトをほぼコピーしている
    
    [Header("HitObjectLayer")]
    [Tooltip("プレイヤーと同じ色を設定")]
    [SerializeField]
    LayerMask _hitLayer;
    RaycastHit2D _hitWall;
    static RaycastHit2D _hitBW;
    public static RaycastHit2D HitBW { get { return _hitBW; } }
    Vector3 _dir;

    [Header("CloneScale")]
    [Tooltip("プレイヤーに対して鏡写しにできる")]
    [SerializeField]
    Transform _cloneTransform;
    SpriteRenderer _spriteRenderer;
    Vector _vector;

    [Header("Direction(Degree)")]
    [Tooltip("右を基準に反時計回りに角度をとる")]
    [SerializeField]
    float _direction;

    float _moveX, _moveY;
    static bool _isMoving = false;
    public static bool IsMoving { get { return _isMoving; } }
    bool _isBlack = true;
    bool _isColorChanging = false;//色変えの時が来たら使う

    //Debug.Log("C:");デバッグ用

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _vector = new Vector();
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
        _dir = new Vector3(Mathf.Cos((transform.eulerAngles.z + _direction) * Mathf.Deg2Rad), Mathf.Sin((transform.eulerAngles.z + _direction) * Mathf.Deg2Rad));
        _dir = _vector.Multiple(_dir, _vector.CoordinatetoOne(_cloneTransform.localScale));
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
            Debug.Log("C:左右移動");
            _moveY = 0;
            _isMoving = true;
            Vector3 basePos = transform.position;
            StartCoroutine(MoveCoroutine(basePos));
        }
        else if (!_isMoving && Mathf.Abs(_moveY) == 1 && !_hitWall.collider)
        {// && !_isColorChanging
            Debug.Log("C:上下移動");
            _moveX = 0;
            _isMoving = true;
            Vector3 basePos = transform.position;
            StartCoroutine(MoveCoroutine(basePos));
        }
        //色変更
        /*else if (Input.GetMouseButtonDown(0) && _hitBW && !_isMoving && !_isColorChanging && Player.HitBW && !Player.IsMoving)
        {
            ColorChange(transform.eulerAngles.z * Mathf.Deg2Rad);
        }*/
    }

    /// <summary>
    /// 移動を滑らかにするコルーチン
    /// </summary>
    /// <param name="basePos"></param>
    /// <returns></returns>
    IEnumerator MoveCoroutine(Vector3 basePos)
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, basePos) < 1.0f)
            {
                transform.position += _dir / 90;
                yield return null;
            }
            else
            {
                transform.position = basePos + _dir;
                //移動による誤差の調整
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
            Debug.Log("C:黒に変更");
            _hitLayer &= ~(1 << LayerMask.NameToLayer("White"));
            _hitLayer |= (1 << LayerMask.NameToLayer("Black"));
            _spriteRenderer.color = Color.black;
            _isBlack = true;
        }
        else
        {
            Debug.Log("C:白に変更");
            _hitLayer &= ~(1 << LayerMask.NameToLayer("Black"));
            _hitLayer |= (1 << LayerMask.NameToLayer("White"));
            _spriteRenderer.color = Color.white;
            _isBlack = false;
        }

        transform.position += new Vector3(Mathf.Cos(theta + _direction * Mathf.Deg2Rad), Mathf.Sin(theta + _direction * Mathf.Deg2Rad), 0);//上下反転にともなった座標調整
        theta += Mathf.PI;
        transform.eulerAngles = new Vector3(0, 0, theta * Mathf.Rad2Deg);

        StartCoroutine(ColorChangeCoroutine(0.5f));
    }
}
