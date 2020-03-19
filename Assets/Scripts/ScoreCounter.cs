using UnityEngine;
using TMPro;

namespace BarkyBoys.Game
{
	public class ScoreCounter : MonoBehaviour	//This class controls the score counter
	{
		[HideInInspector]
		public float countGoal;	//The actual score

		[SerializeField]
		private TextMeshProUGUI scoreText;	//The text showing the score

		private float current, countSpeed;	//Where we currently are in the counting, and a reference float for how fast we are counting

		public void AddScore(float newScore)	//This method is called from the GameController
		{
			countGoal += newScore;	//We add the newly gained score to our counting target
		}

		private void Update()
		{
			if (current != countGoal)	//Do the following if we are not done counting
			{
				current = Mathf.SmoothDamp(current, countGoal, ref countSpeed, 0.2f);	//Set the current counting value to a smooth-damped value between where we are currently counting and our counting target
				scoreText.text = "x" + Mathf.RoundToInt(current).ToString();	//Update the score display text
			}
		}
	}
}