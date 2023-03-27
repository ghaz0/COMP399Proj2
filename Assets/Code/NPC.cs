using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public PlayerCtrl player;

    public int Health = 3;
    public Animator anim;
    public NavMeshAgent agent;
    public Transform[] waypoints;
    public float idleTime = 2f;
    private float waitTime = 0;
    private bool doNotUpdate = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void Update(){
        if (doNotUpdate)
        {
            return;
        }
        anim.SetFloat("Move", agent.velocity.magnitude/agent.speed);
        //print("Move vel: " + agent.velocity.normalized.magnitude);
        if (Health <= 0){
            agent.isStopped = true;
            player.UpdateScore();
            doNotUpdate = true;
            rb.useGravity = false;
            rb.isKinematic = true;
            return;
        }
        if (agent.velocity.magnitude < .1f && waitTime < idleTime){
            waitTime += Time.deltaTime;
        }
        if (waitTime >= idleTime){
            waitTime = 0;
            agent.destination = waypoints[Random.Range(0, waypoints.Length)].position;
        } 
    }

    public void SetHealth(int val){
        if (Health <= 0){
            return;
        }
        anim.SetTrigger("TakeHit");
        Health += val;
        if (Health <= 0){
            print("I have died! OOOF!");
            anim.SetBool("KOed", true);
        }
    }
}
