using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("HitObjectLayer")]
    [SerializeField]
    LayerMask _hitLayer;
    RaycastHit2D _hitWall;
    static RaycastHit2D _hitBW;//�F�ς����Ɏg�p
    public static RaycastHit2D HitBW { get { return _hitBW; } }
    Vector3 _dir;

    SpriteRenderer _spriteRenderer;

    float _moveX, _moveY;
    static bool _isMoving = false;
    public static bool IsMoving { get { return _isMoving; } }
    bool _isBlack = false;
    bool _isColorChanging = false;//�F�ς��̎���������g��

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //�����]��
        if (!_isMoving && !GameDirector.IsPausing)
        {
            //�ړ�����
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

        //�i�s����
        _dir = new Vector3(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad));

        //LineCasts
        Debug.DrawLine(transform.position,
            transform.position + _dir);//�ڒn����Ɋւ������
        _hitWall = Physics2D.Linecast(transform.position,
            transform.position + _dir,
            _hitLayer);
        Debug.DrawLine(transform.position,
            transform.position + _dir);//�ڒn����Ɋւ������
        _hitBW = Physics2D.Linecast(transform.position,
            transform.position + _dir,
            _hitLayer & ~(1 << LayerMask.NameToLayer("Wall")));

        //�ړ�
        if (!_isMoving && Mathf.Abs(_moveX) == 1 && !_hitWall.collider)
        {// && !_isColorChanging
            Debug.Log("���E�ړ�");
            _moveY = 0;
            _isMoving = true;
            Vector3 basePos = transform.position;
            StartCoroutine(MoveCoroutine(_moveX, _moveY, basePos));
        }
        else if (!_isMoving && Mathf.Abs(_moveY) == 1 && !_hitWall.collider)
        {// && !_isColorChanging
            Debug.Log("�㉺�ړ�");
            _moveX = 0;
            _isMoving = true;
            Vector3 basePos = transform.position;
            StartCoroutine(MoveCoroutine(_moveX, _moveY, basePos));
        }
        //�F�ύX
        /*else if (Input.GetMouseButtonDown(0) && _hitBW && !_isMoving && !_isColorChanging && CloneofPlayer.HitBW && !CloneofPlayer.IsMoving)
        {
            ColorChange(transform.eulerAngles.z * Mathf.Deg2Rad);
        }*/
    }

    /// <summary>
    /// �ړ������炩�ɂ���R���[�`��
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
                transform.position = basePos + _dir;//�ړ��ɂ��덷�̒���
                _isMoving = false;
                yield break;
            }
        }
    }

    /// <summary>
    /// �C�ӂ̎��ԑ҂R���[�`��
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
    /// ����̑���Ŕ�������𔽓]�����郁�\�b�h
    /// </summary>
    void ColorChange(float theta)
    {
        if (!_isBlack)
        {
            Debug.Log("���ɕύX");
            _hitLayer &= ~(1 << LayerMask.NameToLayer("White"));//�ύX�O�̐F�̃t���O������
            _hitLayer |= (1 << LayerMask.NameToLayer("Black"));//�ύX��̐F�̃t���O�𗧂Ă�
            _spriteRenderer.color = Color.black;
            _isBlack = true;
        }
        else
        {
            Debug.Log("���ɕύX");
            _hitLayer &= ~(1 << LayerMask.NameToLayer("Black"));
            _hitLayer |= (1 << LayerMask.NameToLayer("White"));
            _spriteRenderer.color = Color.white;
            _isBlack = false;
        }

        transform.position += new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);//�㉺���]�ɂƂ��Ȃ������W����
        theta += Mathf.PI;
        transform.eulerAngles = new Vector3(0, 0, theta * Mathf.Rad2Deg);

        StartCoroutine(ColorChangeCoroutine(0.5f));
    }
}
