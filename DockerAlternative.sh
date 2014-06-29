#!/bin/bash

function init
{

   # ALTERNATIVE_PATH
   export ALTERNATIVE_HOME=`pwd`

   # ALTERNATIVE_BIN_PATH
   export ALTERNATIVE_BIN_PATH=$ALTERNATIVE_HOME/AlterNative.Core.bin/bin/Debug

   # ALTERNATIVE_BIN_PATH
   export ALTERNATIVE_BIN=$ALTERNATIVE_HOME/AlterNative.Core.bin/bin/Debug/AlterNative.Core.exe

   # CPP_LIB_PATH
   export ALTERNATIVE_CPP_LIB_PATH=$ALTERNATIVE_HOME/Lib

   # ALTERNATIVE_TOOLS
   export ALTERNATIVE_TOOLS_PATH=$ALTERNATIVE_HOME/Tools

   # Shell Scripts
   export PATH=$PATH:$ALTERNATIVE_HOME/Tools/ShellScripts

   chmod +x $ALTERNATIVE_HOME/Tools/ShellScripts/alternative
}

function decompile
{
   echo "Decompiling assembly " $1
   cd /executables
   alternative $1 $1/output/
}

function compile
{
   echo "Compiling assembly " $1
   cd /executables/$1/output/
   mkdir build
   cd build
   cmake ..
   make
}


echo "Starting AlterNative Container Command"
init
decompile $1
compile $1
