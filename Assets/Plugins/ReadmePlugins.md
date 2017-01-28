# Writing C++ Plugins for Unity3d

christian@twobulls.com

## C++ Side

Create the plugin using Clion. It may work as well with VCcode.

Wrap functions and variables in:

	extern "C" {
	
	  int HelloFromCpp()
	  {
	    return 42;
	  }
	
	  int Entry2(V3 p[], int n)
	  {
	    return 456;
	  }
	
	  int Entry3(V3 *p)
	  {
	    return 123;
	  }
	
	}


## Unity3d Side

Rename the plugin from libFoo.dylib to Foo.bundle.

Move Foo.bundle to any **Plugins/** folder in Unity3d.

Ensure the CPU is under **Platform Settings** is set to x86_64. OS can be set to **Any OS**.

### To access the C++ code from Unity3d:

		[DllImport ("LagrangeInterpolation")]
		private static extern int HelloFromCpp();
		
Then just call it like any other C# method.

I am still figuring out how to marshall things like structures and arrays and arrays of structures.	