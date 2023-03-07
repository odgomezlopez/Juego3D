using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Utils.Utils;

public class EnemyAnimationcontroller : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigid;

    private Vector3 previousPosition;

    void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isGrounded", IsGrounded(gameObject));
        animator.SetFloat("velocityY", transform.position.y- previousPosition.y);

        previousPosition = transform.position;
    }
}
