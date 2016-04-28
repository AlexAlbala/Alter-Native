##################################################
#Find boost libraries
##################################################
SET(Boost_USE_STATIC_LIBS       OFF)
SET(Boost_USE_MULTITHREADED      ON)
SET(Boost_USE_STATIC_RUNTIME    OFF)

if(DEFINED ENV{NDK})
    FIND_PACKAGE(Boost)
else()
    FIND_PACKAGE(Boost REQUIRED COMPONENTS system locale thread date_time chrono filesystem regex)
endif()

IF(Boost_FOUND)
	INCLUDE_DIRECTORIES(${Boost_INCLUDE_DIRS})
	TARGET_LINK_LIBRARIES(${PROJ_NAME} ${Boost_LIBRARIES})
ELSE()
	MESSAGE("Please install boost libraries before using AlterNative. 1-55 or higher version recommended")
ENDIF()
