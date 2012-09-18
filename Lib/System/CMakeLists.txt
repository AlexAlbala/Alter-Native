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
#include and library path variables for boost library
##################################################
if(WIN32 OR WIN64)
SET(Boost_include_path "C:/Users/Alex/Desktop/AlterNative/Tools/External libraries/boost_1_51/win/include/boost-1_51")
SET(Boost_library_path "C:/Users/Alex/Desktop/AlterNative/Tools/External libraries/boost_1_51/win/lib")
ELSE()
SET(Boost_include_path "C:/Users/Alex/Desktop/AlterNative/Tools/External libraries/boost_1_51/unix/include/boost-1_51")
SET(Boost_library_path "C:/Users/Alex/Desktop/AlterNative/Tools/External libraries/boost_1_51/unix/lib")
ENDIF()

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

INCLUDE_DIRECTORIES(${Boost_include_path})
FIND_LIBRARY(BOOST_SYS_LIB libboost_system-vc100-mt-gd-1_51 ${Boost_library_path})
FIND_LIBRARY(BOOST_LOC_LIB libboost_locale-vc100-mt-gd-1_51 ${Boost_library_path})

TARGET_LINK_LIBRARIES(System ${BOOST_SYS_LIB})
TARGET_LINK_LIBRARIES(System ${BOOST_LOC_LIB})

#INCLUDE_DIRECTORIES(${ICU_include_path})
#LINK_DIRECTORIES(${ICU_library_path})