using UnityEngine;
using DG.Tweening;	//Notice we are using DoTween

namespace BarkyBoys.Game
{
	public class Bouncer : MonoBehaviour	//This class makes the object it's attached to bounce to the beat
	{
		[SerializeField] [Range(0f, 1f)]
		private float bounceAmount;	//How much the mesh should be deformed

		private Tweener t;	//The current tween that we're running. We need this reference so we can kill it when we need to

		private void OnEnable()
		{
			MusicController.Bounce += DoBounce;	//We subscribe to the event from the MusicController
		}

		void DoBounce()
		{
			t = transform.GetChild(0).DOShakeScale(0.3f, bounceAmount);	//When the MusicController triggers a beat we shake the scale of the object to make it bounce
		}
		private void OnDisable()
		{
			if (t != null) { t.Kill(); }	//If the tween is not finished running by itself we must cull it
			MusicController.Bounce -= DoBounce;	//We unsubscribe from the event from the MusicController
		}
	}
}