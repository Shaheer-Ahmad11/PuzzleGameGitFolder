using UnityEngine;

public class Sound : MonoBehaviour
{
	public bool debugMode;

	public static Sound REF;

	private void Awake()
	{
		if (REF == null)
		{
			REF = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void Play(string name)
	{
		GameObject gameObject = base.transform.Find(name).gameObject;
		if ((bool)gameObject)
		{
			gameObject.GetComponent<AudioSource>().Play();
		}
	}

	public void PlayMusic(string name)
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			base.transform.GetChild(i).gameObject.GetComponent<AudioSource>().Stop();
		}
		GameObject gameObject = base.transform.Find(name).gameObject;
		if ((bool)gameObject)
		{
			gameObject.GetComponent<AudioSource>().Play();
		}
		if (debugMode)
		{
			MonoBehaviour.print("PlayMusic('" + name + "')");
		}
	}
}
