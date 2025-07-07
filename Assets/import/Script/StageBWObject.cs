using UnityEngine;

public class StageBWObject : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    Transform _player;

    [Header("Clone")]
    [SerializeField]
    Transform _clone;

    SpriteRenderer _sprite;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _sprite.color = Color.white;
            this.gameObject.layer = LayerMask.NameToLayer("White");
        }
        else if (collision.gameObject.tag == "Clone")
        {
            _sprite.color = Color.black;
            this.gameObject.layer = LayerMask.NameToLayer("Black");
        }
    }
}
