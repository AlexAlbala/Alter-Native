Description of Alter-Native coming soon...

Included open-source libraries:
 Mono.Cecil: MIT License
 AvalonEdit: LGPL
 SharpTreeView: LGPL
 ILSpy: MIT License
 ICSharpCode.Decompiler: MIT License (developed as part of ILSpy)

Alter-Native Contributors:
	Alex Albalà (alejandro.albala at upc.edu)
	Juan López  (Juan.Lopez-Rubio at upc.edu)
        Gerard Solè (gerard.sole.castellvi at gmail.com)

Getting the code
----------------

	git clone https://github.com/AlexAlbala/Alter-Native.git
	git submodule init
	git submodule update

Compiling
---------

On Windows
==========
Use the supplied solution on Visual Studio 2012
* TODO What libraries (i.e boost, cmake) are needed?

On MacOS X
==========

* Install Mono 3.0 and Xamarin Studio 4.1, (http://www.mono-project.com/)
* Install MacPorts (http://www.macports.org/)
* Install cmake and boost 

	sudo port install cmake boost

* Open the AlterNative.Core.sln with Xamarin Studio and compile

On Linux
========

Testing
-------

	source ./alternative-init.sh
	cd AlterNative
	source alternative-init.sh
	mcs -debug test.cs
	alternative test.exe ./output/ CXX ./AlterNative.Core/Lib/
	cd output
	mkdir build
	cd build
	cmake ..
	make
	./test
