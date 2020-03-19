using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace BarkyBoys.Game
{
	public class GameController : MonoBehaviour	//This class is a singleton class that controls some of the main game functionality
	{
		public static GameController control;   //The static reference to this object

		public GameObject scoreParticles;	//The particle system that spawns the little dogbones that go up to the counter
		public bool gameEnded;	//A bool we can check elsewhere to see if the game is running

		[SerializeField]
		private ScoreCounter counter;	//The script that manages the score display

		[SerializeField]
		private GameObject gameOverScreen;	//The parent object that contains the "Game Over" overlay

		[SerializeField]
		private TextMeshProUGUI endScore;	//The text object that we use to display our final score

		private void Awake()
		{
			DoSingletonCheck();
		}
		bool DoSingletonCheck()	//This logic checks if this is the only copy of this class in existance. It also returns a bool so we can do some logic only after this returns true, though we're not currently implementing that
		{
			if (control != this)	//Is this object *already* the static reference?
			{
				if (control)	//If not, then is there *another* static reference?
				{
					Destroy(gameObject);    //If yes, we don't want this one
					return false;
				}
				else
				{
					control = this;	//If else, make this the new static reference

					//We could make this object persist between scenes here if we needed it to:
					//DontDestroyOnLoad(this);

					return true;
				}
			}
			else
			{
				return true;
			}
		}
		public void AddScore()	//Called from where ever we want to award score
		{
			Invoke("AddScoreActual", 0.5f);	//We wait a bit with actually adding the score, so that the particles have time to play
		}

		void AddScoreActual()
		{
			counter.AddScore(BaseScoreNow());	//We add score based on the below calculation
		}

		public void Damage() 
		{
			if(counter.countGoal > 0)
			{
				counter.ResetScore();
			}
			else
			{
				GameOver();
			}
		}

		public float BaseScoreNow()
		{
			return Mathf.Ceil(Time.time);	//We simply take the current time in the game and round up to get the score we add per kill
		}

		public void GameOver()	//Called from an enemy when it hits the player collider
		{
			gameEnded = true;	//We toggle the bool to signify the game has ended. We could in theory also trigger an event to make other objects react to this
			gameOverScreen.SetActive(true);	//We show the Game Over overlay
			endScore.text = counter.countGoal.ToString();	//And finally set the big score text to our final score
		}

		public void RestartGame()	//Called from the UI button after a Game Over
		{
			SceneManager.LoadScene(0);	//Not a very flexible approach, but as long as we know the build index we can restart the scene like this
		}
	}
}