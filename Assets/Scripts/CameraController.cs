using System;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    
    [SerializeField] private float _rotationSpeed = 2f;
    private Coroutine _rotationCoroutine;
    private Quaternion _previousTargetRotation;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _previousTargetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        InputManager.Instance.CameraRotationInputEvent.AddListener((rotation) =>
        {
            if (_rotationCoroutine != null)
                StopCoroutine(_rotationCoroutine);

            _rotationCoroutine = StartCoroutine(Rotate(-rotation));
        });
    }
    private void Update()
    {
        transform.position = PlayerController.Instance.transform.position;
    }

    private IEnumerator Rotate(int rotation)
    {
        float targetYRotation = _previousTargetRotation.eulerAngles.y + 45 * rotation;
        Vector3 direction = new Vector3(transform.rotation.eulerAngles.x, targetYRotation, transform.rotation.eulerAngles.z);
        Quaternion targetRotation = Quaternion.Euler(direction);
        _previousTargetRotation = targetRotation;

        while (Quaternion.Angle(Quaternion.Euler(transform.rotation.eulerAngles), targetRotation) > 0f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
            yield return null;
        }
    }
}
