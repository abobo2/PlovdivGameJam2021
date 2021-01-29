using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricPlayerMovementController : MonoBehaviour
{

    public float MovementSpeed = 5f;

    public float ChargeSpeed;
    IsometricCharacterRenderer isoRenderer;

    Rigidbody2D rbody;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 currentPos = rbody.position;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);

        Vector2 movement = inputVector * MovementSpeed;
        Vector2 newPos = currentPos + movement * Time.deltaTime;

        // RaycastHit2D hit = Physics2D.Raycast(transform.position, inputVector, ovementSpeed * Time.deltaTime);
        //
        // if (hit.collider != null)
        // {
        //
        //     // Debug.Log("Colider hit");
        //     // Debug.Log(hit.collider.name);
        //     // var vec = inputVector * (hit.distance - 0.05f);
        //     // newPos = transform.position + new Vector3(vec.x, vec.y);
        // }

        isoRenderer.SetDirection(movement);
        rbody.MovePosition(newPos);
        // transform.position = newPos;
    }

    public void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            
        }
    }

    public void Charge()
    {
        
    }
}
