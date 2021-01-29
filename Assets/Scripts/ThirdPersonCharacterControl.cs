using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonCharacterControl : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _jumpForce = 5;
    [SerializeField] private bool _allowJump = true;
    [SerializeField] private float _jumpTime = 2f;

    private int _touchingColliders;
    // private Rigidbody _rigidBody;

    // private void Awake()
    // {
    //     _rigidBody = GetComponent<Rigidbody>();
    // }

    private void Update ()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");
        var run = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;

        var moving = hor > 0 || ver > 0;

        var movementTrigger = GetComponent<MovementTrigger>();

        if (moving)
        {
            movementTrigger.Trigger();
        }
        else
        {
            movementTrigger.Stop();
        }

        if (_allowJump && Input.GetKeyDown(KeyCode.Space))
        {
            if (_touchingColliders > 0)
            {
                StartCoroutine(ExtentArms());
                // _rigidBody.AddForce(0, _jumpForce, 0, ForceMode.Impulse);
            }
        }
        var playerMovement = new Vector3(hor, 0f, ver).normalized * (_speed * run * Time.deltaTime);
        transform.Translate(playerMovement, Space.Self);
    }

    private IEnumerator ExtentArms()
    {
        var start = transform.position;
        var target = start + Vector3.up * 3;
        var t = 0f;
        while (t < _jumpTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(start, target, t/_jumpTime);
            yield return null;
        }
        transform.position += Vector3.up * 3;
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