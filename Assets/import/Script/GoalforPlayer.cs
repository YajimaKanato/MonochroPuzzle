using UnityEngine;

public class GoalforPlayer : MonoBehaviour
{
    [Header("Target")]
    [SerializeField]
    Transform _player;

    bool _isGoal = false;

    private void Start()
    {
        _isGoal = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            if(Vector3.Distance(transform.position, _player.position) < 0.05f)
            {
                _isGoal = true;
            }
        }
    }

    public bool GetIsGoal()
    {
        return _isGoal;
    }
}
