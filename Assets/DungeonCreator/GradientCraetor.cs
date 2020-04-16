using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientCraetor
{
    int w, h;
	public byte[,] gtdm;

	public GradientCraetor(int w, int h)
	{
		this.w = w;
		this.h = h;
		gtdm = new byte[w, h];
		for (int i = 0; i < w; i++)
		{
			for (int j = 0; j < h; j++)
			{
				byte current;
				do
					current = (byte)UnityEngine.Random.Range(0, 16);
				while (!isByteOk(i, j, current));

				gtdm[i, j] = current;
			}
		}
	}

	public bool isByteOk(int i, int j, byte b)
	{
		if (i > 0 && !(((b & 4) == 0) == ((gtdm[i - 1, j] & 2) == 0) && ((b & 8) == 0) == ((gtdm[i - 1, j] & 1) == 0))) return false;

		if (j > 0 && !(((b & 2) == 0) == ((gtdm[i, j - 1] & 1) == 0) && ((b & 4) == 0) == ((gtdm[i, j - 1] & 8) == 0))) return false;

		return true;
	}

}
