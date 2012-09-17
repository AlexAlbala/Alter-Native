CMAKE_MINIMUM_REQUIRED(VERSION 2.8)
PROJECT(sys)

#Macro for searching subdirectories
SET(root ${CMAKE_CURRENT_SOURCE_DIR})
STRING(REGEX REPLACE ":" "_" root ${root})
STRING(REGEX REPLACE "/" "_" root ${root})

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

#Get subdirectories
#SUBDIRLIST(SUBDIRS ${CMAKE_CURRENT_SOURCE_DIR} "")
HEADER_DIRECTORIES(SUBDIRS)
FILE(GLOB_RECURSE SysLib *.h *.cpp *.hpp)

#Add groups
FOREACH(subdir ${SUBDIRS})
  SET(subdir_cpp "Source Files\\${subdir}")
  SET(subdir_h "Header Files\\${subdir}")
  FILE(GLOB tmpcpp ${subdir}/*.cpp)
  SOURCE_GROUP(${subdir_cpp} FILES ${tmpcpp})   
  FILE(GLOB tmph ${subdir}/*.h)
  SOURCE_GROUP(${subdir_h} FILES ${tmph}) 
ENDFOREACH()

ADD_LIBRARY(System ${SysLib})
SET_TARGET_PROPERTIES(System PROPERTIES LINKER_LANGUAGE CXX)