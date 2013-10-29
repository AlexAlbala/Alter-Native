if(MSVC OR MSVC_IDE) 
  if( MSVC_VERSION LESS 1700 )       # VC10-/VS2010- 
    message(FATAL_ERROR "The project requires C++11 features. You need at least Visual Studio 11 (Microsoft Visual Studio 2012), with Microsoft Visual C++ Compiler Nov 2012 CTP (v120_CTP_Nov2012).")
  elseif( MSVC_VERSION EQUAL 1700 )  # VC11/VS2012 
    message( "VC11: use Microsoft Visual Studio 2012 with Microsoft Visual C++ Compiler Nov 2012 CTP (v120_CTP_Nov2012)" )
    set(CMAKE_GENERATOR_TOOLSET "v120_CTP_Nov2012" CACHE STRING "Platform Toolset" FORCE) 
  else() # VC12+, assuming C++11 supported. 
  endif() 
endif()