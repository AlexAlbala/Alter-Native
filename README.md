# Alter-Native [![Build Status](https://travis-ci.org/AlexAlbala/Alter-Native.svg?branch=master)](https://travis-ci.org/AlexAlbala/Alter-Native)

AlterNative is a tool made by Developers for Developers. Its concept is to maximize the idea of happy-coding. It provides a tool to easy port applications from high-level languages such as .NET to native languages like C++.

Most of the actual systems are C++ compatible, thus if the application is ported to this language, it can be executed in several platforms (i.e. smartphones, tablets, embedded systems and computers).

AlterNative's philosophy is to provide the user with a system for taking the advantage of the .NET fast development and also take the advantage of performance of low-level languages. But also, exploit the possibility to run this native code in many systems or in other words, follow the WORA (Write Once, Run Everywhere) philosophy.

### Included open-source libraries:

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
	+ Compile it (NOTE: The current version is working with compiler msvc-11.0 included in Visual Studio 2012)

			cd C:\Boost_x_y_z
			bootstrap.bat
			b2.exe --prefix=<INSTALLATION_DIR> --toolset=msvc-11.0
	+ Have a coffee or two :)
	+ Probably you shohuld set the environment variable BOOST_INCLUDEDIR pointing to the installation directory in order to enable cmake to find the libraries. Check this variable if cmake is not working
* For use the c++11 features you will need the compiler v120 CTP of November 2012
	+ http://www.microsoft.com/en-us/download/details.aspx?id=35515
	+ The new released v120 CTP November 2013 for visual Studio 2013 does not work properly
	
* Execute the provided .bat file

			install.bat
			
* Or use command line:
	+ Initialize environment variables

			alternative-init.bat
	+ Compile the AlterNative library

			cd Lib/
			alternative-lib-compile.bat
			
	+ Compile AlterNative.sln solution (Debug or Release)

			msbuild AlterNative.sln /p:Configuration=net_4_0_Debug
			msbuild AlterNative.sln /p:Configuration=net_4_0_Release

	+ You also can use the supplied solution on Visual Studio 2012 or higher
		+ Be sure to select one of these solution configurations Otherwise it won't compile:

	 			net_4_0_Debug
				net_4_0_Release


			

On MacOS X
==========

* Install Mono >= 3.0 and Xamarin Studio >= 4.1, (http://www.mono-project.com/)
* Install MacPorts (http://www.macports.org/)
* Install cmake and boost 

		sudo port install cmake boost

* Use the provided command:

		source ./install.sh

* Or use command line:
	+ Initialize environment variables

			source ./alternative-init.sh
	+ Compile AlterNative

			./alternative.core-compile.sh
	* Also you can open the AlterNative.Core.sln with Xamarin Studio and compile
	+ Compile the AlterNative library

			cd Lib/
			source ./alternative-lib-compile.sh

On Linux
========
### Debian Wheezy (Stable)
* The following software will be installed from the official debian repositories.

	+ Install Mono Runtime.

			sudo apt-get install mono-complete

	+ Install the necessary tools to run AlterNative. 

			sudo apt-get install libboost-all-dev cmake build-essential

* If you check your software version, you should have:

		Mono = 2.10.8.1 or higher
		MonoDevelop = 3.0.3.2 or higher
		libboost = 1.49 or higher
* Use the provided command:

		source ./install.sh
* Or use command line:
	+ Initialize environment variables

			source ./alternative-init.sh
	+ Compile AlterNative

			./alternative.core-compile.sh
	+ Compile the AlterNative library

			cd Lib/
			source ./alternative-lib-compile.sh

Testing
-------

* Make sure you have a c++11 compatible compiler

* Make sure you have compiled the C++ AlterNative library before running some test: https://github.com/AlexAlbala/AlterNative-CXX-Lib

		cd AlterNative
		source ./alternative-init.sh
		
		alternative MyExe.exe ./output/
		cd output
		mkdir build
		cd build
		cmake ..
		make
		./MyExe

* Support for easy testing is being added.

		cd AlterNative
		source ./alternative-init.sh
		alternative new example
		alternative make example
		cd build
		./Blank
