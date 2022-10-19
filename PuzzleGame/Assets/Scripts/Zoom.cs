using UnityEngine;

public class Zoom : MonoBehaviour
{
	public static Zoom REF;

	public GameObject zoomImage;

	private Vector2 d;

	public Vector3 o;

	private float zoomScale = 1f;

	public bool enable = true;

	public static bool show;

	private void Start()
	{
		if (REF == null)
		{
			REF = this;
		}
		if (zoomImage == null)
		{
			MonoBehaviour.print("zoomImage is null");
			return;
		}
		o = zoomImage.transform.position;
		Vector3 localScale = zoomImage.transform.localScale;
		zoomScale = localScale.x;
	}

	private void OnMouseDrag()
	{
		if (enable && Input.GetMouseButton(0))
		{
			UpdateZoomImage();
		}
	}

	public void UpdateZoomImage()
	{
		ref Vector2 reference = ref d;
		Vector3 localPosition = base.transform.localPosition;
		reference.x = (localPosition.x - o.x) * zoomScale * 0.6f;
		ref Vector2 reference2 = ref d;
		Vector3 localPosition2 = base.transform.localPosition;
		reference2.y = (localPosition2.y - o.y) * zoomScale * 0.6f;
		if (zoomImage != null)
		{
			Transform transform = zoomImage.transform;
			float x = o.x - d.x;
			float y = 3.5f + o.y - d.y;
			Vector3 localPosition3 = zoomImage.transform.localPosition;
			transform.localPosition = new Vector3(x, y, localPosition3.z);
		}
	}
}
