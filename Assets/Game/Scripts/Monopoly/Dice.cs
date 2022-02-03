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
            float dirX = Random.Range(0, 500);
            float dirY = Random.Range(0, 500);
            float dirZ = Random.Range(0, 500);
            transform.position = new Vector3(0, 4, 0);
            transform.rotation = Quaternion.identity;
            rigid.AddForce(transform.up * 500);
            rigid.AddTorque(dirX, dirY, dirZ);
        }
        diceThrowed = false;
    }

    public void RollDice()
        => diceThrowed = true;
}
