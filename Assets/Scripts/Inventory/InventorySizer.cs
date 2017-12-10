using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySizer : MonoBehaviour {

	private RectTransform pos;
	private GridLayoutGroup layout;

	void Start ()
	{
		layout = transform.FindChild ("Layout").gameObject.GetComponent<GridLayoutGroup>();
		pos = layout.GetComponent<RectTransform> ();
		Resize ();
	}

	void Resize()
	{
		int sideremaining = (int)Mathf.Min (pos.rect.width, pos.rect.height);
		Debug.Log(sideremaining);

		int numcols = 0;
		for(int x = 1; numcols < layout.transform.childCount; x++)
			numcols = x*x;
		numcols = (int)Mathf.Sqrt(numcols);
		layout.constraintCount = numcols;


		int cellsize = 64;
		for(int x = 0; x<5 ; x++)
		{
			if(cellsize * 2 > (sideremaining/layout.constraintCount) )
				break;
			else
				cellsize *=2;
		}
		layout.cellSize = new Vector2(cellsize,cellsize);
		Debug.Log(sideremaining + "-" + numcols*cellsize + "=" + (sideremaining-numcols*cellsize));
		sideremaining-=(numcols*cellsize);

		int spacing = 16;
		for(int x = 0; x < 20 ; x++)
		{
			if(spacing * (numcols-1) > (sideremaining/ (numcols-1)) )
				break;
			else
				spacing *= 2;
		}
		layout.spacing = new Vector2(spacing,spacing);
		spacing = spacing*(numcols-1);
		Debug.Log(sideremaining + "-" + spacing + "=" + (sideremaining-spacing));
		sideremaining-=spacing;

		int pad = sideremaining/2;
		if(pad<0) pad = 0;
		layout.padding = new RectOffset(pad,pad,pad,pad);
		//layout.cellSize = new Vector2( (int)(side - 48) / 2, (int)(side - 48) / 2);
	}

}
