using UnityEngine;

public class CameraAspect : MonoBehaviour
{
	private void Start()
	{
		Camera.main.aspect = 0.625f;
	}

	private void Update()
	{
	}
}
