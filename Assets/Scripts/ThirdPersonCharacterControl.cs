using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonCharacterControl : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _jumpForce = 5;
    [SerializeField] private bool _allowJump = true;
    [SerializeField] private float _jumpTime = 2f;

    [SerializeField] private MovementTrigger _movementTrigger;
    [SerializeField] private IdleTrigger _idleTrigger;
    [SerializeField] private JumpTrigger _jumpTrigger;

    private Animator _animator;
    private Animation _anim;

    private int _touchingColliders;
    
    private Rigidbody _rigidBody;
    private bool _elevating;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate ()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.L))
        {
            _anim.clip = _anim.GetClip("startWalk");
            _anim.Play();
        }

        var moving = hor != 0 || ver != 0;
        
        if (!_elevating && _allowJump && Input.GetKeyDown(KeyCode.Space))
        {
            if (_touchingColliders > 0)
            {
                _elevating = true;
                _jumpTrigger.Trigger();
                StartCoroutine(ExtentArms());
                _movementTrigger.Stop();
                return;
            }
        }
        else if (moving && !_elevating)
        {
            _movementTrigger.Trigger();
        }
        else
        {
            _movementTrigger.Stop();
        }
        
        // Calculate how fast we should be moving
        var targetVelocity = new Vector3(hor, 0, ver);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= _speed;
 
        // Apply a force that attempts to reach our target velocity
        var velocity = _rigidBody.velocity;
        var velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -10, 10);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -10, 10);
        velocityChange.y = 0;
        _rigidBody.AddForce(velocityChange, ForceMode.VelocityChange);


        _animator.SetFloat("speed", Mathf.Abs(velocity.magnitude));
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
        _rigidBody.AddForce(transform.forward * 15, ForceMode.Impulse);
        _jumpTrigger.Stop();
        _elevating = false;
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