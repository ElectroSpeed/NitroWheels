using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    public TextMeshProUGUI _carSpeedText;

    public WheelCollider _wheelBackLeft;
    public WheelCollider _wheelBackRight;
    public WheelCollider _wheelFrontLeft;
    public WheelCollider _wheelFrontRight;

    public float _carTorque;
    public float _carSpeed;
    public float _carSpeedMax = 212f;
    public float _carAcceleration = 10f;
    public float _carWheelAngleMax = 30f;

    public float _steeringSmoothness = 5f;
    private float _targetSteeringAngle = 0f;

    public int _carBrake = 10000;


    public void Update()
    {
        UpdateSpeed();
    }

    void FixedUpdate()
    {
        _wheelFrontLeft.steerAngle = Mathf.Lerp(_wheelFrontLeft.steerAngle, _targetSteeringAngle, Time.deltaTime * _steeringSmoothness);
        _wheelFrontRight.steerAngle = Mathf.Lerp(_wheelFrontRight.steerAngle, _targetSteeringAngle, Time.deltaTime * _steeringSmoothness);
    }

    public void UpdateSpeed()
    {
        _carSpeed = GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
        _carSpeedText.text = "Speed : " + (int)_carSpeed;
    }

    public void CarFrontMovement(InputAction.CallbackContext context)
    {
        if (context.performed && _carSpeed < _carSpeedMax)
        {
            _wheelBackLeft.brakeTorque = 0;
            _wheelBackRight.brakeTorque = 0;

            _wheelBackLeft.motorTorque = Input.GetAxis("Vertical") * _carTorque * _carAcceleration * Time.deltaTime;
            _wheelBackRight.motorTorque = Input.GetAxis("Vertical") * _carTorque * _carAcceleration * Time.deltaTime;
        }
        if (context.canceled || _carSpeed > _carSpeedMax)
        {
            _wheelBackLeft.motorTorque = 0;
            _wheelBackRight.motorTorque = 0;

            _wheelBackLeft.brakeTorque = _carBrake * _carAcceleration * Time.deltaTime;
            _wheelBackRight.brakeTorque = _carBrake * _carAcceleration * Time.deltaTime;
        }
    }


    public void CarRightMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _targetSteeringAngle = _carWheelAngleMax;
        }
        else if (context.canceled)
        {
            _targetSteeringAngle = 0f;
        }
    }

    public void CarLeftMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _targetSteeringAngle = -_carWheelAngleMax;
        }
        else if (context.canceled)
        {
            _targetSteeringAngle = 0f;
        }
    }
}
