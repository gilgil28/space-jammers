using UnityEngine;

public class ThirdPersonCameraControl : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _player;

    [SerializeField] private float _rotationSpeed = 1;
    [SerializeField] private float _zoomSpeed = 2;

    private Transform _obstruction;
    private float _mouseX, _mouseY;


    private void Start()
    {
        _obstruction = _target;
         //Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        CamControl();
        ViewObstructed();
    }


    private void CamControl()
    {
        _mouseX += Input.GetAxis("Mouse X") * _rotationSpeed;
        _mouseY -= Input.GetAxis("Mouse Y") * _rotationSpeed;
        _mouseY = Mathf.Clamp(_mouseY, -35, 60);

        transform.LookAt(_target);
        
        _target.rotation = Quaternion.Euler(_mouseY, _mouseX, 0);
        _player.rotation = Quaternion.Euler(0, _mouseX, 0);
    }


    private void ViewObstructed()
    {
        if (Physics.Raycast(transform.position, _target.position - transform.position, out var hit, 4.5f))
        {
            if (!hit.collider.gameObject.CompareTag("Player"))
            {
                _obstruction = hit.transform;
                
                if(Vector3.Distance(_obstruction.position, transform.position) >= 3f && Vector3.Distance(transform.position, _target.position) >= 1.5f)
                    transform.Translate(Vector3.forward * _zoomSpeed * Time.deltaTime);
            }
            else
            {
                if (Vector3.Distance(transform.position, _target.position) < 4.5f)
                    transform.Translate(Vector3.back * _zoomSpeed * Time.deltaTime);
            }
        }
    }
}