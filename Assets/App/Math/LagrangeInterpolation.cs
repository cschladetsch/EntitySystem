using System; 
using System.Collections; 
using System.Collections.Generic; 
using System.Linq; 

using UnityEngine; 
using UnityEngine.Assertions; 

namespace App.Math
{
	//https://people.sc.fsu.edu/~jburkardt/cpp_src/lagrange_interp_2d/lagrange_interp_2d.cpp

	public class LagrangeInterpolation 
	{
		public LagrangeInterpolation()
		{
		}

		//  Purpose:
		//    LAGRANGE_BASIS_FUNCTION_1D evaluates a 1D Lagrange basis function.
		//  Parameters:
		//
		//    Input, int MX, the degree of the basis function.
		//    Input, float XD[MX+1], the interpolation nodes.
		//    Input, int I, the index of the basis function.
		//    0 <= I <= MX.
		//    Input, float XI, the evaluation point.
		//    Output, float LAGRANGE_BASIS_FUNCTION_1D, the value of the I-th Lagrange 1D 
		//    basis function for the nodes XD, evaluated at XI.
		//
		public float lagrange_basis_function_1d (int mx, float[] xd, int i, float xi ) 
		{
			float yi = 1.0f; 

			if (xi != xd[i]) 
			{
				for (int j = 0; j < mx + 1; j++) 
				{
					if (j != i) 
					{
						yi = yi * (xi - xd[j])/(xd[i] - xd[j]); 
					}
				}
			}

			return yi; 
		}

		//****************************************************************************80
		//
		//  Purpose:
		//    LAGRANGE_INTERP_2D evaluates the Lagrange interpolant for a product grid.
		//  Parameters:
		//    Input, int MX, MY, the polynomial degree in X and Y.
		//    Input, float XD_1D[MX+1], YD_1D[MY+1], the 1D data locations.
		//    Input, float ZD[(MX+1)*(MY+1)], the 2D array of data values.
		//    Input, int NI, the number of 2D interpolation points.
		//    Input, float XI[NI], YI[NI], the 2D interpolation points.
		//    Output, float LAGRANGE_INTERP_2D[NI], the interpolated values.
		public List<float> lagrange_interp_2d (int mx, int my, 
				float[] xd_1d, float[] yd_1d, 
				float[] zd, int ni, 
				float[] xi, float[] yi) 
		{
			int i; 
			int j; 
			int k; 
			int l; 
			float lx; 
			float ly; 
			float[] zi = new float[ni];

			for (k = 0; k < ni; k++) 
			{
				l = 0; 
				zi[k] = 0.0f; 
				for (j = 0; j < my + 1; j++) 
				{
					for (i = 0; i < mx + 1; i++) 
					{
						lx = lagrange_basis_function_1d (mx, xd_1d, i, xi[k]); 
						ly = lagrange_basis_function_1d (my, yd_1d, j, yi[k]); 
						zi[k] = zi[k] + zd[l] * lx * ly; 
						l = l + 1; 
					}
				}
			}
			return zi.ToList(); 
		}

		public List<float> lagrange_interp_3d (int mx, int my, int mz, 
			float[] xd_1d, float[] yd_1d, float[] zd_1d,
			float[] zd, 
			int ni, 
			float[] xi, float[] yi, float[] zi) 
		{
			List<float> wi = new List < float > ();

			for (int k = 0; k < ni; k++) 
			{
				int l = 0; 
				wi[k] = 0.0f; 
				for (int j = 0; j < my + 1; j++) 
				{
					for (int i = 0; i < mx + 1; i++) 
					{
						for (int h = 0; h < mz; h++)
						{
							float lx = lagrange_basis_function_1d (mx, xd_1d, i, xi[k]); 
							float ly = lagrange_basis_function_1d (my, yd_1d, j, yi[k]); 
							float lz = lagrange_basis_function_1d (mz, zd_1d, k, zi[k]); 

							wi[k] = wi[k] + zd[l]*lx*ly*lz; 

							l++;
						}
					}
				}
			}

			return wi; 
		}
	}
}



