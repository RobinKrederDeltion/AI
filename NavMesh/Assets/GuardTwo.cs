using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardTwo : Master{

    public void FixedUpdate()
    {
        Nibba(); //Hier roep ik de virtual void aan
    }

    public override void Nibba() //Hierin heb ik de rotation en de speed van de rotation veranderd
    {
        
            Debug.DrawRay(transform.position, transform.forward * distance, Color.blue);

            if (acties == states.chase)
            {
                transform.LookAt(player.transform.position);
                GetComponent<NavMeshAgent>().destination = new Vector3(player.transform.position.x + offset, player.transform.position.y + offset, player.transform.position.z + offset);
            }
            if (acties == states.idle)
            {
                StartCoroutine(Rotate(Vector3.up, -180, 0.6f));
            }
            if (acties == states.patrol)
            {
                randomAI = true;
                if (Physics.Raycast(transform.position, transform.forward, out hit, distance) && hit.collider.gameObject.CompareTag("Player"))
                {
                    acties = states.chase;
                }
            }

        }





    }
