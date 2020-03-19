using UnityEngine;

namespace BarkyBoys.Game
{
	public class PlayerController : MonoBehaviour	//This class handles the spawning and updates of the player controlled objects
	{
		[SerializeField]
		private GameObject[] playerModels;	//The possible prefabs that can spawn as player objects

		[SerializeField]
		private float baseSpeed, baseGravity;	//The base speed and gravity of the player controller objects

		[SerializeField]
		BarkController dogAnimation;	//The script that controls the movement and sounds of the dog

		private OnNote currentPlayer;	//The current moveable object we're playing as

		private void Update()
		{
			if (currentPlayer == null && !GameController.control.gameEnded)	//If we haven't spawned a playable object and the GameController tells us we don't have a game over, proceed
			{
				if (Input.GetMouseButtonDown(0))
				{
					SpawnNote();
					dogAnimation.Bark();
				}
			}
		}

		private void FixedUpdate()
		{
			if (currentPlayer != null && !GameController.control.gameEnded)	//Same as above
			{
				//We're moving the player objects from this central script so that we can tweak the speed and gravity during gameplay to iterate faster
				//It would be acceptable to move the objects in the scripts attached to the respectable objects too
				currentPlayer.MoveNote(baseGravity, baseSpeed);	
			}
		}

		private void SpawnNote()
		{
			int nextModelIndex = Random.Range(0, playerModels.Length);	//We select the index of the next spawned object randomly based on the number of playable objects in the array
			GameObject newPlayer = Instantiate(playerModels[nextModelIndex], transform.position, Quaternion.identity);		//We instantiate the new player object and sets its position to where this object is
			currentPlayer = newPlayer.GetComponent<OnNote>();	//We set the newly spawned playerobject's script as out current player
		}

	}
}