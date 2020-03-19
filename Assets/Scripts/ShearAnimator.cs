using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace BarkyBoys.Game
{
	public class ShearAnimator : MonoBehaviour	//This class animates the "shear" enemy
	{
		[SerializeField] [Range(0f, 90f)]
		private float rotateAmount;	//How far to rotate the shears in degrees

		[SerializeField]
		private Transform bottomBlade, topBlade;	//The two blades of the shears

		private Tweener t, t2;  //The current tweens that we're running. We need this reference so we can kill them when we need to

		private void OnEnable()
		{
			MusicController.Bounce += DoCut;    //We subscribe to the event from the MusicController
		}

		void DoCut()
		{
			StartCoroutine(Cut());
		}

		private IEnumerator Cut()	//We use a coroutine so we can snap the shears shut at the beat, wait a bit, then reopen them
		{
			t = bottomBlade.DOLocalRotate(new Vector3(0, rotateAmount, 0), 0.2f);	//Snap the bottom blade shut
			t.SetEase(Ease.OutBounce);	//We set the ease type to overshoot and bounce
			t2 = topBlade.DOLocalRotate(new Vector3(0, -rotateAmount, 0), 0.2f);	//Same as above but top blade
			t2.SetEase(Ease.OutBounce);	//Same as above

			yield return new WaitForSeconds(0.2f);	//We wait a moment

			t = bottomBlade.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);	//Reopen the bottom blade
			t.SetEase(Ease.OutExpo);	//We set the ease to snap open again
			t2 = topBlade.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);	//Same as above but top blade
			t2.SetEase(Ease.OutExpo);	//Same as above
		}
		private void OnDisable()
		{
			if (t != null) { t.Kill(); }    //If the tween is not finished running by itself we must cull it
			if (t2 != null) { t2.Kill(); }  //If the tween is not finished running by itself we must cull it
			MusicController.Bounce -= DoCut;    //We unsubscribe from the event from the MusicController
		}
	}
}