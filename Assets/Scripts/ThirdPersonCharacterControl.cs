using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonCharacterControl : MonoBehaviour
{
    [SerializeField] private float _speed = 5;

    [SerializeField] private float _jumpForce = 5;
    [SerializeField] private bool _allowJump = true;

    private int _touchingColliders;
    private Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update ()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");
        var run = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
<<<<<<< HEAD

        var playerMovement = new Vector3(hor, 0f, ver) * (_speed * run * Time.deltaTime);
=======
        
        if (_allowJump && Input.GetKeyDown(KeyCode.Space))
        {
            if (_touchingColliders > 0)
            {
                _rigidBody.AddForce(0, _jumpForce, 0, ForceMode.Impulse);
            }
        }
        var playerMovement = new Vector3(hor, 0f, ver).normalized * (_speed * run * Time.deltaTime);
>>>>>>> c4cd7b6c0a3467f0bdadd59f4ef3c500d15ace06
        transform.Translate(playerMovement, Space.Self);
    }

    private void OnCollisionEnter ()
    {
        _touchingColliders++;
    }

    private void OnCollisionExit ()
    {
        _touchingColliders--;
    }
}