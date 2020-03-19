using UnityEngine;

namespace BarkyBoys.Game
{
	public class EnemyMover : MonoBehaviour	//This class handles the movement of the enemy objects
	{
		[SerializeField]
		private float baseSpeed, baseAmplitude, baseFrequency;	//The horizontal speed, size of vertical curves, and how often those curves come
		private Rigidbody rb;	//The RigidBody attached to this object

		private void OnEnable()
		{
			rb = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()	//Remember to only move RigidBodies in FixedUpdate
		{
			if (!GameController.control.gameEnded)	//We check that the game is running
			{
				rb.velocity = (Vector3.left * baseSpeed) + (Vector3.up * Mathf.Sin(Time.time * baseFrequency) * baseAmplitude);	//We set the RigidBody's velocity to the speed we've chosen and add the sin of the current time. This gives us wavy movement
			}
			else
			{
				rb.velocity = Vector3.zero;	//Freeze the movement if the play is over
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))	//We hit the player collider
			{
				GameController.control.GameOver();	//Tell the GameController that the game is over
				Destroy(gameObject);
			}
		}
	}
}
