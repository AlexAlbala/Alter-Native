CMAKE_MINIMUM_REQUIRED(VERSION 2.8)
PROJECT(sys)
##################################################
#Macro for searching subdirectories
##################################################
MACRO(HEADER_DIRECTORIES return_list)
    FILE(GLOB_RECURSE new_list *.h *.hpp *.cpp)
    SET(dir_list "")
    FOREACH(file_path ${new_list})
        GET_FILENAME_COMPONENT(dir_path ${file_path} PATH)
        # remove ":" from path
        STRING(REGEX REPLACE ":" "_" dir_path ${dir_path})

        # convert "/" to "_"
        STRING(REGEX REPLACE "/" "_" dir_path ${dir_path})

        # remove build subdir from path
        STRING(REGEX REPLACE "${root}_" "" dir_path ${dir_path})

	#insert again the "/" characters
	STRING(REGEX REPLACE "_" "\\\\" dir_path ${dir_path})
        SET(dir_list ${dir_list} ${dir_path})
    ENDFOREACH()
    LIST(REMOVE_DUPLICATES dir_list)
    SET(${return_list} ${dir_list})
ENDMACRO()

##################################################
#useful variables for make source groups
##################################################
SET(root ${CMAKE_CURRENT_SOURCE_DIR})
STRING(REGEX REPLACE ":" "_" root ${root})
STRING(REGEX REPLACE "/" "_" root ${root})

##################################################
#Get subdirectories
##################################################
HEADER_DIRECTORIES(SUBDIRS)
FILE(GLOB_RECURSE SysLib *.h *.cpp *.hpp)

##################################################
#Add groups
##################################################
FOREACH(subdir ${SUBDIRS})
  SET(subdir_cpp "Source Files\\${subdir}")
  SET(subdir_h "Header Files\\${subdir}")
  FILE(GLOB tmpcpp ${subdir}/*.cpp)
  SOURCE_GROUP(${subdir_cpp} FILES ${tmpcpp})   
  FILE(GLOB tmph ${subdir}/*.h)
  SOURCE_GROUP(${subdir_h} FILES ${tmph}) 
ENDFOREACH()

##################################################
#LINK LIBRARIES
##################################################

ADD_LIBRARY(System ${SysLib})
SET_TARGET_PROPERTIES(System PROPERTIES LINKER_LANGUAGE CXX)

SET(Boost_USE_STATIC_LIBS        ON)
SET(Boost_USE_MULTITHREADED      ON)
SET(Boost_USE_STATIC_RUNTIME    OFF)
SET(Boost_ADDITIONAL_VERSIONS "1.33" "1.33.0" "1.33.1" "1.34" "1.34.0" "1.34.1" "1.35" "1.35.0" "1.35.1" "1.36" "1.36.0" "1.36.1" "1.37" "1.37.0" "1.38" "1.38.0" "1.39" "1.39.0" "1.40" "1.40.0" "1.41" "1.41.0" "1.42" "1.42.0" "1.43" "1.43.0" "1.44" "1.44.0" "1.45" "1.45.0" "1.46" "1.46.0" "1.46.1" "1.47" "1.47.0" "1.48" "1.48.0" "1.49" "1.49.0" "1.50" "1.50.0" "1.51" "1.51.0" "1.52" "1.52.0" "1.53" "1.53.0" "1.54" "1.54.0" "1.55" "1.55.0" "1.56" "1.56.0")
FIND_PACKAGE( Boost 1.51 COMPONENTS date_time filesystem system locale)

IF(Boost_FOUND)
  INCLUDE_DIRECTORIES(${Boost_INCLUDE_DIRS})
  TARGET_LINK_LIBRARIES(System ${Boost_LIBRARIES})
ELSE()
  IF(IS_DIRECTORY $ENV{BOOST_PATH})
    IF(IS_RELEASE EQUAL 1)
      FIND_LIBRARY(BOOST_SYS_LIBR libboost_system-vc100-mt-1_51 $ENV{BOOST_PATH}/lib)
      FIND_LIBRARY(BOOST_LOC_LIBR libboost_locale-vc100-mt-1_51 $ENV{BOOST_PATH}/lib)
      TARGET_LINK_LIBRARIES(System ${BOOST_SYS_LIBR})
      TARGET_LINK_LIBRARIES(System ${BOOST_LOC_LIBR})
    ELSE()
      FIND_LIBRARY(BOOST_SYS_LIB libboost_system-vc100-mt-gd-1_51 $ENV{BOOST_PATH}/lib)
      FIND_LIBRARY(BOOST_LOC_LIB libboost_locale-vc100-mt-gd-1_51 $ENV{BOOST_PATH}/lib)
      TARGET_LINK_LIBRARIES(System ${BOOST_SYS_LIB})
      TARGET_LINK_LIBRARIES(System ${BOOST_LOC_LIB})
    ENDIF()
  ELSE()
    MESSAGE("Boost library not found. Please install the library or set the environment variable BOOST_PATH with a valid boost library path")
  ENDIF() 
ENDIF()