using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : NetworkBehaviour
{
    Rigidbody rigid;
    public Vector3 diceVelocity;
    bool diceThrowed;

    public int rolledNumber = 0;

    private int Force = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        diceVelocity = rigid.velocity;
    }

    private void FixedUpdate()
    {
        if (diceThrowed)
        {
            float dirX = Random.Range(700, 1000);
            float dirY = Random.Range(700, 1000);
            float dirZ = Random.Range(700, 1000);
            transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            transform.rotation = Quaternion.identity;
            rigid.AddForce(Vector3.up * Random.Range(500, 1000));
            rigid.AddForce(Vector3.right * Random.Range(100, 300));
            rigid.AddForce(Vector3.left * Random.Range(100, 300));
            rigid.AddTorque(dirX, dirY, dirZ);
        }
        diceThrowed = false;
    }

    public void RollDice(int force)
    {
        diceThrowed = true;
        Force = force;
    }
}
