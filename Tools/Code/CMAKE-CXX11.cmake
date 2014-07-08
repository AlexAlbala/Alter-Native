##################################################
#Enable C++11 mode
##################################################
if ("${CMAKE_CXX_COMPILER_ID}" STREQUAL "Clang" OR CMAKE_COMPILER_IS_GNUCXX)
    include(CheckCXXCompilerFlag)
    check_cxx_compiler_flag(--std=c++11 SUPPORTS_STD_CXX11)
    check_cxx_compiler_flag(--std=c++0x SUPPORTS_STD_CXX01)
    if(SUPPORTS_STD_CXX11)
        set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} --std=c++11")
        #set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} --std=c++11")
	message("FLAGS: " ${CMAKE_CXX_FLAGS})
    elseif(SUPPORTS_STD_CXX01)
        set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} --std=c++0x")
        #set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} --std=c++0x")
	message("FLAGS: " ${CMAKE_CXX_FLAGS})
    else()
        message(ERROR "Compiler does not support --std=c++11 or --std=c++0x.")
    endif()
endif()
