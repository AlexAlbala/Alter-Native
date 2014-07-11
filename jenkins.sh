#!/bin/bash
chmod +x $ALTERNATIVE_HOME/Tools/ShellScripts/alternative

xbuild AlterNative.Core.sln /t:Clean
xbuild AlterNative.Core.sln

cd Lib

if [ -z "$ALTERNATIVE_CPP_LIB_PATH" ]; then
    declare -x ALTERNATIVE_CPP_LIB_PATH=`pwd`
fi

export ALTERNATIVE_LIB_BUILD=$ALTERNATIVE_CPP_LIB_PATH/build
export ALTERNATIVE_LIB_BIN=$ALTERNATIVE_LIB_BUILD/bin



if [ ! -d "$ALTERNATIVE_LIB_BIN" ]; then

  mkdir $ALTERNATIVE_LIB_BIN

fi



cd $ALTERNATIVE_LIB_BIN

rm CMakeCache.txt

cmake -G "Unix Makefiles" ..

make

if [ ! -d "$ALTERNATIVE_LIB_BUILD/libfiles" ]; then

  mkdir $ALTERNATIVE_LIB_BUILD/libfiles
fi

cp $ALTERNATIVE_LIB_BIN/src/libSystem.a $ALTERNATIVE_LIB_BUILD/libfiles
cp $ALTERNATIVE_LIB_BIN/src/public/gc/libgc-lib.a $ALTERNATIVE_LIB_BUILD/libfiles

cd $ALTERNATIVE_CPP_LIB_PATH
