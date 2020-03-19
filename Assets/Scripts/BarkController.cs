using UnityEngine;
using DG.Tweening;	//Notice that we're using DoTween

namespace BarkyBoys.Game
{
	//This script controls the visuals and audio of the dog itself
	public class BarkController : MonoBehaviour
	{
		[SerializeField]
		private Animator dogAnims;	//The animator to control the dogs movement

		[SerializeField]
		private AudioSource barkSource;	//The AudioSource to play the barks

		[SerializeField]
		private AudioClip[] barks;	//The list of possible barks

		[SerializeField] [Range(0, 0.5f)]
		private float pitchVariance;	//The maximum variance in pitch we allow

		[SerializeField]
		private Transform dogModel;	//The transform of the dog mesh

		[SerializeField]
		[Range(0, 1f)]
		private float barkBounce;	//How much the model deforms when barking

		private static readonly string barkTrigger = "bark";	//The name of the animation trigger set in the animator. We cache it staticly so we can't misspell the string

		public void Bark()	//This method is called from the PlayerController
		{
			dogAnims.SetTrigger(barkTrigger);	//Activates the animation clip

			barkSource.clip = barks[Random.Range(0, barks.Length)];		//Selects a random sound from our array of sound clips
			barkSource.pitch = 1f + Random.Range(-pitchVariance / 2f, pitchVariance / 2f);	//Sets the pitch of the sound randomly based on the pitchVariance variable
			barkSource.Play();	//Plays the sound clip

			dogModel.DOShakeScale(barkBounce, barkBounce * 10f);	//Shake the scale of the dog mesh. The result is that the dog "jiggles"
		}
	}
}