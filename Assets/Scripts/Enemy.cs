using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	private GameObject player;
	private NavMeshAgent agent;

	void Start()
	{
		player = GameObject.FindObjectOfType<Player>().gameObject;
		agent = GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		if (player == null)
		{
			player = GameObject.FindObjectOfType<Player>().gameObject;
		}
		else
		{
			agent.SetDestination(player.transform.position);
		}
	}
}
