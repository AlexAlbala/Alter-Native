@ECHO off

call alternative-init.bat

cd %ALTERNATIVE_CPP_LIB_PATH%
call alternative-lib-compile.bat

::The script alternative-lib-compile.bat sets a variable msbuildPath useful for this script

cd %msbuildPath%
msbuild.exe %ALTERNATIVE_HOME%/Alternative.sln /p:Configuration=net_4_0_Debug

cd %ALTERNATIVE_HOME%