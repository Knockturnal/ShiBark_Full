using UnityEngine;

namespace BarkyBoys.Game
{
	public class RotateBehavior : MonoBehaviour	//This class makes the object it's attached to rotate
	{
		private enum Axis { X, Y, Z };	//This enum describes the axies so we can choose from a dropdown

		[SerializeField]
		private Axis axis;	//The variable of the type declared above

		[SerializeField]
		private float rotateSpeed;	//How fast this object should rotate

		private Vector3 rotateNext;	//The next calculated rotation

		private void Update()
		{
			switch (axis)
			{
				case Axis.X:
					rotateNext.x = rotateSpeed;
					break;
				case Axis.Y:
					rotateNext.y = rotateSpeed;
					break;
				case Axis.Z:
					rotateNext.z = rotateSpeed;
					break;
			}

			transform.localEulerAngles += rotateNext * Time.deltaTime;	//Remember to multiply by deltaTime when not in FixedUpdate
		}
	}
}