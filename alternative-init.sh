#/bin/bash

# ALTERNATIVE_PATH
declare -x ALTERNATIVE_HOME=`pwd`

# ALTERNATIVE_BIN_PATH
declare -x ALTERNATIVE_BIN_PATH=$ALTERNATIVE_HOME/AlterNative.Core/AlterNative.Core/bin/Debug

# CPP_LIB_PATH
declare -x ALTERNATIVE_CPP_LIB_PATH=$ALTERNATIVE_HOME/Lib/src

# Shell Scripts
PATH=$PATH:$ALTERNATIVE_HOME/Tools/ShellScripts
