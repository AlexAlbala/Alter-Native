#/usr/bin/env bash

source ./alternative-init.sh
source ./alternative.core-compile.sh

cd $ALTERNATIVE_CPP_LIB_PATH
source ./alternative-lib-compile.sh

cd $ALTERNATIVE_HOME
