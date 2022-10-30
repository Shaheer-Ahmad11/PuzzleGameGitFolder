using UnityEngine;

public class Piece : MonoBehaviour
{
	private Vector3 startPosition;

	public Vector3 moveTo;

	private bool draging;

	private bool done;

	public bool selected;

	private void Start()
	{
		ref Vector3 reference = ref startPosition;
		Vector3 localPosition = base.transform.localPosition;
		reference.x = localPosition.x;
		ref Vector3 reference2 = ref startPosition;
		Vector3 localPosition2 = base.transform.localPosition;
		reference2.y = localPosition2.y;
		ref Vector3 reference3 = ref startPosition;
		Vector3 localPosition3 = base.transform.localPosition;
		reference3.z = localPosition3.z;
		moveTo = startPosition;
	}

	private void Update()
	{
		if (!done && !draging)
		{
			MoveToUpdate();
		}
	}

	private void MoveToUpdate()
	{
		if ((double)Vector3.Distance(base.transform.localPosition, moveTo) > 0.01)
		{
			base.transform.localPosition += (moveTo - base.transform.localPosition) / 4f;
			if ((double)Vector3.Distance(base.transform.localPosition, moveTo) <= 0.01)
			{
				base.transform.localPosition = moveTo;
			}
			base.gameObject.GetComponent<Zoom>().UpdateZoomImage();
		}
	}

	public void SetMoveTo(Vector3 newMoveTo)
	{
		moveTo = newMoveTo;
	}

	private void SetScale(Vector3 newScale)
	{
		base.transform.localScale = newScale;
	}

	private void OnMouseDrag()
	{
		if (!done)
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			vector.y += 3.5f;
			Transform transform = base.transform;
			float x = vector.x;
			float y = vector.y;
			Vector3 position = base.transform.position;
			transform.SetPositionAndRotation(new Vector3(x, y, position.z), Quaternion.identity);
			draging = true;
			SetMoveTo(base.transform.position);
		}
	}

	private void OnMouseUp()
	{
		draging = false;
	}
}
