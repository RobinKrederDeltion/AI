using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Master : MonoBehaviour{

    private NavMeshAgent masterRobot;
    public GameObject player;
    public int offset;
    public int distance;
    public RaycastHit hit;
    public float i;
    public bool b;
    public bool randomAI;
    public float wanderRadius = 30;
    public float wanderTimer = 10;
    private Transform target;
    private NavMeshAgent agent;
    private float timer;
    public enum states
    {
        chase,
        idle,
        patrol
    }
    public Rigidbody rb;
    public states acties;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        offset = Random.Range(1, 5); //Dit is de afstand waarop de AI ven je aflbijft als hij je chased
        masterRobot = player.GetComponent<NavMeshAgent>();
    }

    public virtual void Nibba()
    {
        Debug.DrawRay(transform.position, transform.forward * distance, Color.blue);

        if (acties == states.chase) //Ik heb hierin offset gebruikt zodat de AI niet in je glitched
        {
            transform.LookAt(player.transform.position);
            GetComponent<NavMeshAgent>().destination = new Vector3(player.transform.position.x + offset, player.transform.position.y + offset, player.transform.position.z + offset);
        }
        if (acties == states.idle)   //Hierin kijkt hij om zich heen, doordat hij 90 graden ronddraait
        {
            StartCoroutine(Rotate(Vector3.up, -90, 0.3f));
        }
        if (acties == states.patrol)  //Hierin wordt de chase getriggerd als hij iets zit en het de tag player heeft
        {
            randomAI = true;
            if (Physics.Raycast(transform.position, transform.forward, out hit, distance) && hit.collider.gameObject.CompareTag("Player"))
            {
                acties = states.chase; 
            }
        }
    }


        public IEnumerator Rotate(Vector3 axis, float angle, float duration = 1.0f)
    {
            Quaternion from = rb.transform.rotation;
            Quaternion to = rb.transform.rotation;
            to *= Quaternion.Euler(axis * angle);

            float elapsed = 0.0f;
            while (elapsed < duration)
            {
                rb.transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return new WaitForSeconds(1);
            }
            transform.rotation = to;
        }

    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    
    void Update()
    {
        if (randomAI == true)
        {
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }
        }
        
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }


}









