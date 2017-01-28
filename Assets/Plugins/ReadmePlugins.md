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


To avoid headache later, change your CMakeLists.txt to write directly to the Plugins folder with correct naming:

	# set target path for plugin
	set(TARGET_PATH ~/...Unity3dProjectPath.../Assets/Plugins)

	# get rid of leading 'lib' name
	set(CMAKE_SHARED_LIBRARY_PREFIX "")
	
	# rename suffix from 'dylib' to 'bundle'
	set(CMAKE_SHARED_LIBRARY_SUFFIX ".bundle")
	
	# write directly to Unity3d's project structure
	set(LIBRARY_OUTPUT_PATH ${TARGET_PATH})

	# build it!
	add_library(${PROJECT_NAME} SHARED ${SOURCE_FILES})

## Unity3d Side

If you didn't do anything to generate the plugin at the right place with right name, rename the plugin from libFoo.dylib to Foo.bundle. Then move Foo.bundle to any **Plugins/** folder in Unity3d.

* Open the Inspector for the plugin. 
* Ensure the CPU is under **Platform Settings** is set to ***x86_64***. 
* OS can be set to **Any OS**.

### To access the C++ code from Unity3d:

		[DllImport ("LagrangeInterpolation")]
		private static extern int HelloFromCpp();
		
Then just call it like any other C# method.

I am still figuring out how to marshall things like structures and arrays and arrays of structures.

Seems like the Library/ folder has to be deleted for changes to a plugin to be propagated. Sigh. There has to be a work-around for this.	