using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody playerRigidBody;

    Vector3 playerPosition;

    private float horizontalInput;
    private float verticalInput;

    [SerializeField]
    float xSpeed = 15f;
    [SerializeField]
    float zSpeed = 15f;


    private void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        horizontalInput = -Input.GetAxisRaw("Horizontal");
        verticalInput = -Input.GetAxisRaw("Vertical");
        playerPosition = transform.position;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            MazeGenerator.mazeGenerator.DisplayGoalText();
            SetGoalInactive(other.gameObject);
        }
    }

    void SetGoalInactive(GameObject goal)
    {
        goal.SetActive(false);
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        PlayerMove(horizontalInput, verticalInput);
    }

    void PlayerMove(float xInput, float zInput)
    {
        playerRigidBody.AddForce(transform.forward * zInput * zSpeed);
        playerRigidBody.AddForce(transform.right * xInput * xSpeed);

    }
}
