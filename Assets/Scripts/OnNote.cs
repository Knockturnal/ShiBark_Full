using UnityEngine;

namespace BarkyBoys.Game
{
	public class OnNote : MonoBehaviour //This class moves the player objects once they are spawned by the PlayerController
	{
		#region Serializable Fields

		[SerializeField]
		private GameObject deathParticles, hitEnemyParticles;	//The particles to spawn in when we hit an invisible wall and an enemy, respectively

		[Header("Sounds")]
		[SerializeField]
		private AudioClip[] deathSounds;	//An array of the possible sounds we make when we die

		[SerializeField]
		private AudioClip hitEnemySound;	//The sound we make when we hit an enemy

		[SerializeField] [Range(0, 0.5f)]
		private float pitchVariance;    //The maximum allowed variance in pitch	

		#endregion

		#region Private Fields

		private Rigidbody rb;		//This object's rigidbody. Assinged in OnEnable
		private Vector3 currentVelocity;	//The objects current velocity. We use this to move it on the x axis

		#endregion

		#region Functions
		private void OnEnable()	//When this object enters into the world we want to initialize some of its properties
		{
			Color nextColor = Random.ColorHSV(0, 1, 1, 1, 1, 1);	//Choose a color randomly based on some parameters
			GetComponentInChildren<MeshRenderer>().material.SetColor("_BaseColor", nextColor);	//Set the color of the mesh to the color we chose

			TrailRenderer trail = GetComponentInChildren<TrailRenderer>();	//Retrieve the TrailRenderer from this object's children
			nextColor.a = 0.5f;		//We set the alpha of the color we chose previously to 50% at the start of the trail
			trail.startColor = nextColor;
			nextColor.a = 0f;		//And then set the alpha to zero at the end of the line to make it fade out
			trail.endColor = nextColor;

			rb = GetComponent<Rigidbody>();		//Finally we get a reference to our RigidBody
		}

		public void MoveNote(float gravity, float speed) //This method is being called in a FixedUpdate function on the PlayerController so it can control this object's speed
		{
			if (!GameController.control.gameEnded)	//We check that the game is still running
			{
				if (Input.GetMouseButton(0))
				{
					rb.AddForce(-Physics.gravity * (Mathf.Pow(rb.mass, 2f)) * 0.1f * gravity);	//If the mouse is down, we move up. The calculation takes into account the real gravity formula and the world gravity setting
				}
				else
				{
					rb.AddForce(Physics.gravity * (Mathf.Pow(rb.mass, 2f)) * 0.1f * gravity);   //Same as above, but we move down when the mouse is not pressed
				}

				//We set the horizontal velocity directly. We have to go through a temporary Vector3 variable because the members (x/y/z) aren't directly accessible in the RigidBody
				currentVelocity = rb.velocity;	
				currentVelocity.x = speed;
				rb.velocity = currentVelocity;

			}
			else
			{
				rb.velocity = Vector3.zero;		//If the game has ended, freeze us in place
			}
		}
		private void OnTriggerEnter(Collider other)		//Triggered when we hit something
		{
			if (other.CompareTag("WorldBorder"))	//This tag is on the colliders off-screen
			{
				OutOfMap();
			}
			else if (other.CompareTag("Enemy"))		//This tag is only on the enemy objects
			{
				Destroy(other.gameObject);	//We destroy the enemy object
				HitEnemy();
			}
		}

		void OutOfMap()
		{
			GameObject mDeathParticles = Instantiate(deathParticles, transform.position, Quaternion.identity);  //Instantiate the death particle explotion at our location
			Destroy(mDeathParticles, 3f);   //We tell Unity to destroy the object with the particles in 3 seconds

			AudioSource mDeathSound = mDeathParticles.GetComponent<AudioSource>();  //The explotion object has an AudioSource on it - we get a reference to it
			mDeathSound.clip = deathSounds[Random.Range(0, deathSounds.Length)];	//We select a random sound from our list of clips and assign it to the AudioSource
			mDeathSound.pitch = 1f + Random.Range(-pitchVariance / 2f, pitchVariance / 2f); //We set the pitch based on the maximum allowed variance
			mDeathSound.Play(); //The sound is played

			Kill();
		}

		void HitEnemy()
		{
			GameController.control.AddScore();	//We add some score when we kill an enemy. The GameController decides the amount

			GameController.control.scoreParticles.transform.position = transform.position;		//We move the particle system that displays the dog bones going up to the counter to our current position. The GameController references it
			ParticleSystem scoreParticles = GameController.control.scoreParticles.GetComponent<ParticleSystem>();	//Then we retrieve the particleSystem from that reference
			ParticleSystem.EmissionModule em = scoreParticles.emission; //We have to get a reference to the emissions module in order to change its data

			//Setting the amount of particles to the score we just got as calculated by the GameController. We multiply it by 1 over the duration of the system because the rate is per second
			em.rateOverTime = GameController.control.BaseScoreNow() * (1f / scoreParticles.main.duration);	
			scoreParticles.Play();	//Then we activate the particles

			GameObject mDeathParticles = Instantiate(hitEnemyParticles, transform.position, Quaternion.identity);	//Instantiate the death particle explotion at our location
			Destroy(mDeathParticles, 3f);	//We tell Unity to destroy the object with the particles in 3 seconds

			AudioSource mDeathSound = mDeathParticles.GetComponent<AudioSource>();	//The explotion object has an AudioSource on it - we get a reference to it
			mDeathSound.clip = hitEnemySound;	//Then we assign the clip of hitting the enemy to it. We could in theory have multiple sounds to choose from here
			mDeathSound.pitch = 1f + Random.Range(-pitchVariance / 2f, pitchVariance / 2f);	//We set the pitch based on the maximum allowed variance
			mDeathSound.Play();	//The sound is played

			Kill();
		}

		void Kill()
		{
			Destroy(gameObject);
		}
		#endregion
	}
}
