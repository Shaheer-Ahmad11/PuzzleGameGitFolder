using UnityEngine;

public class Point : MonoBehaviour
{
	public Sprite spPointCheck;

	public bool enable = true;
	// public static bool heart=false;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnMouseUp()
	{
		if (enable)
		{
			enable = false;
			base.gameObject.GetComponent<SpriteRenderer>().sprite = spPointCheck;
			base.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
			string text = base.gameObject.name;
			if (text.Contains("A"))
			{
				text = text.Replace('A', 'B');
			}
			else if (text.Contains("B"))
			{
				text = text.Replace('B', 'A');
			}
			GameObject gameObject = GameObject.Find(text);
			gameObject.GetComponent<SpriteRenderer>().sprite = spPointCheck;
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
			gameObject.GetComponent<Point>().enable = false;
			GameManager.REF.PointCheck();
			Sound.REF.Play("sndPointCheck");
		}
	}
}
