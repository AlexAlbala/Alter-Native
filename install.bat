alternative-init.bat

cd %ALTERNATIVE_LIB_CPP_PATH%
alternative-lib-compile.bat

::The script alternative-lib-compile.bat sets a variable msbuildPath useful for this script

cd %msbuildPath%
msbuild.exe %ALTERNATIVE_HOME%/Alternative.sln /p:Configuration=net_4_0_Debug
