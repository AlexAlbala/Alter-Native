Description of Alter-Native coming soon...

Included open-source libraries:

>Mono.Cecil: MIT License  
AvalonEdit: LGPL  
SharpTreeView: LGPL  
ILSpy: MIT License  
ICSharpCode.Decompiler: MIT License (developed as part of ILSpy)  

Alter-Native Contributors:  

>Alex Albalà (alejandro.albala at upc.edu)  
Juan López  (Juan.Lopez-Rubio at upc.edu)  
Gerard Solé (gerard.sole.castellvi at gmail.com)  

Getting the code
----------------

	git clone https://github.com/AlexAlbala/Alter-Native.git
	git submodule init
	git submodule update

Compiling
---------

On Windows
==========

* Install Cmake from http://www.cmake.org/
	+ Be sure to add cmake to the PATH
* Install Boost libraries from http://www.boost.org
	+ Unzip to C:\Boost_x_y_z
	+ Compile it

			cd C:\Boost_x_y_z
			bootstrap.bat
			b2
	+ Have a coffee or two :)
	+ Set Enviroment variables
			
			USR_BOOST_INCLUDE=C:\Boost_x_y_z
			USR_BOOST_LIBRARY=C:\Boost_x_y_z\stage\lib
		
* Use the supplied solution on Visual Studio 2012

On MacOS X
==========

* Install Mono >= 3.0 and Xamarin Studio >= 4.1, (http://www.mono-project.com/)
* Install MacPorts (http://www.macports.org/)
* Install cmake and boost 

		sudo port install cmake boost
	
* Open the AlterNative.Core.sln with Xamarin Studio and compile
* Or use command line:

		xbuild AlterNative.Core.sln /t:Clean
		xbuild AlterNative.Core.sln
		

On Linux
========
### Debian Wheezy (Stable)
* The following software will be installed from the official debian repositories.

	+ Install Mono Runtime and the MonoDevelop.

			sudo apt-get install mono-complete monodevelop

	+ Install the necessary tools to run AlterNative. 
	
			sudo apt-get install libboost-all-dev cmake build-essential

* If you check your software version, you should have:
	
		Mono = 2.10.8.1
		MonoDevelop = 3.0.3.2
		libboost = 1.49
	
* The next step is make some arrangements to AlterNative.Core project.

	+ Edit and modify Alter-Native/AlterNative/AlterNative.Core.sln as it's shown below

			nano Alter-Native/AlterNative/AlterNative.Core.sln
		
			Microsoft Visual Studio Solution File, Format Version 11.00
			# Visual Studio 2010

	+ Open Alter-Native/AlterNative/AlterNative.Core.sln using MonoDevelop and Build it.  
	Once it tries to compile, it will crash with an error saying "Framework '.NETFramework 4.5' not installed."  
	Correct the errors applying "Right Click -> Options -> Build/General -> and change to "Mono / .NET 4.0"" to the following projects:
			
			ICSharpCode.NRefactory
			ICSharpCode.NRefactory.CSharp
			ICSharpCode.Decompile
			ICSharpCode.NRefactory.Cpp
			ICSharpCode.NRefactory.VB
			ILSpy
			AlterNative

	+ Now remove the ICSharpCode.NRefactory.VB project (Inside "ILSpy project") and build again

* Finally you need to export two environment variables that will be used by cmake.
	
		export USR_BOOST_INCLUDE=/usr/include/boost/
		export USR_BOOST_LIBRARY=/usr/lib

Testing
-------

	cd AlterNative
	source ./alternative-init.sh

	mcs -debug test.cs
	alternative test.exe ./output/ CXX ./AlterNative.Core/Lib/
	cd output
	mkdir build
	cd build
	cmake ..
	make
	./test

Support for easy compiling is being added.

	alternative new example
	alternative make example
	cd example
	cd build
	make
	./Blank
