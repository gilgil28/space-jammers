using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonCharacterControl : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _jumpForce = 5;
    [SerializeField] private bool _allowJump = true;
    [SerializeField] private float _jumpTime = 2f;

    private Animator _animator;

    private int _touchingColliders;
    
    private Rigidbody _rigidBody;
    private bool _hasLight;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate ()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            ActivateLight();
        }
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");
        var run = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;

        var moving = hor != 0 || ver != 0;

        var movementTrigger = GetComponent<MovementTrigger>();

        if (_allowJump && Input.GetKeyDown(KeyCode.Space))
        {
            if (_touchingColliders > 0)
            {
                StartCoroutine(ExtentArms());
                movementTrigger.Stop(); //play something else?
                return;
                // _rigidBody.AddForce(0, 1, 0, ForceMode.Impulse);
            }
        }
        else if (moving)
        {
            movementTrigger.Trigger();
        }
        else
        {
            movementTrigger.Stop();
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

    private void ActivateLight()
    {
        if (!_hasLight)
        {
            return;
        }
        //TODO turn on/off light GameObject
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