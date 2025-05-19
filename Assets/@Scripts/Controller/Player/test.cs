using System;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Rigidbody _rigidbody;
    [SerializeField] float _speed = 5f;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
    }


    void Move()
    {
        if (transform.position.z >= 108.2)
        {
            Vector3 pos = transform.position;
            pos.z = 5;
            transform.position = pos;
        }
        // 공격 중이 아니고 wasd 입력 시 이동
        if ((Input.GetButton("Horizontal") || Input.GetButton("Vertical")))
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(h, 0, v);
            //transform.Translate(movement.normalized * _speed * Time.deltaTime, Space.World);
            _rigidbody.MovePosition(_rigidbody.position + movement.normalized * _speed * Time.deltaTime);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), _speed * Time.deltaTime);
        }
        else
        {
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
        }
        Debug.Log(_rigidbody.linearVelocity.y);
    }

}
