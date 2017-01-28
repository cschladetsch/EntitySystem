using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using App.Math;
using Mystery.Graphing;

namespace App
{
	public class TestLagrangeInterpolation : MonoBehaviour
	{
		public Vector3[] Points;
		public int NumInterpolationPoints = 50;
		public int PolynomialDegree = 3;

		private void Awake()
		{
		}

		private void Start()
		{
		}

		private void Update()
		{
			int mx = PolynomialDegree;
			int my = PolynomialDegree;
			float[] xd_1d = new float[mx + 1];
			float[] yd_1d = new float[my + 1];

			xd_1d = _interp.r8vec_cheby_extreme_new(mx + 1, 0.0f, 1.0f);
			yd_1d = _interp.r8vec_cheby_extreme_new(my + 1, 0.0f, 1.0f);

			// number of data points
			int nd = (mx + 1)*(my + 1);
			float[] xd = new float[nd];
			float[] yd = new float[nd];
			float[] zd = new float[nd];

			int ij = 0;
			for (int j = 0; j < my + 1; j++)
			{
				for (int i = 0; i < mx + 1; i++ )
				{
					xd[ij] = xd_1d[i];
					yd[ij] = yd_1d[j];
					ij = ij + 1;
				}
			}

			int ni = nd;//NumInterpolationPoints;
			float[] xi = new float[nd];
			float[] yi = new float[nd];
			for (int i = 0; i < nd; i++)
			{
				xi[i] = xd[i];
				yi[i] = yd[i];
			}

			List<float> points = _interp.lagrange_interp_2d(mx,my, 
				xd_1d, yd_1d, zd, ni, 
				xi, yi);

			foreach (var p in points)
				DebugGraph.Log(p);
		}

		private void FixedUpdate()
		{
		}

		private LagrangeInterpolation _interp = new LagrangeInterpolation();
	}
}

