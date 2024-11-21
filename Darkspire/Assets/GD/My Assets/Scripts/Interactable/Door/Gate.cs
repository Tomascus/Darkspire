using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 initialPosition;
    private bool isMovingUp = false;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (isMovingUp)
        {
            MoveUp();
        }
    }

    private void MoveUp()
    {
        transform.position = Vector3.MoveTowards(transform.position, initialPosition + Vector3.up * moveDistance, moveSpeed * Time.deltaTime);

        if (transform.position == initialPosition + Vector3.up * moveDistance)
        {
            isMovingUp = false;
        }
    }

    public void StartMoving()
    {
        isMovingUp = true;
    }
}