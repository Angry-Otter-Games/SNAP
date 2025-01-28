using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{

    Controls controls;
    public float speed = 1.0f;
    public float reduceRotation = 2f;

    private void Awake() {
        controls = new Controls();
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 mouseLocation = controls.Mouse.Position.ReadValue<Vector2>();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mouseLocation);
        worldPosition = worldPosition / reduceRotation;

        Vector3 targetDirection = transform.position - worldPosition;
        float singleStep = speed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
