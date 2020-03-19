using UnityEngine;

namespace BarkyBoys.Game
{
	public class MusicController : MonoBehaviour	//This class both plays the music and activates an event every other beat of the song. We can listen for the event and make other objects dance to the beat
	{
		public delegate void DoBounce();		//The delegate type we activate
		public static event DoBounce Bounce;	//The event that is triggered by the music

		[SerializeField]
		private AudioSource music;	//The AudioSource holding our music

		[SerializeField] [Range(0f, 250f)]
		private double BPM;	//We need to know the BPM of the song in order to calculate the beats

		private double samplesPerBeat, lastSample;  //The amount of audio samples to wait between triggering beats anf the sample at which we last triggered a beat
		void Start()
		{
			StartMusic();
		}

		private void StartMusic() 
		{
			music.Play();
			samplesPerBeat = (double)(music.clip.frequency) / (BPM / 120d); //The samples per beat is calculated as the samples per second over (BPM/120), or "beats per half second". This gives us every other beat
		}

		private void Update()
		{
			if (music.timeSamples > samplesPerBeat + lastSample)	//If we've reached or overshot the next beat, trigger the event
			{
				lastSample += samplesPerBeat;	//We set the time of the last sample to the current sample
				Bounce?.Invoke();	//If anyone is listening for the event, we incoke it
			}

			if (!music.isPlaying)
			{
				music.Play();
			}
		}

	}
}
