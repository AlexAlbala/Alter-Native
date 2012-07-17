CMAKE_MINIMUM_REQUIRED(VERSION 2.8)
PROJECT(sys)

#Macro for searching subdirectories
SET(dirlist "")
MACRO(SUBDIRLIST result curdir)
  FILE(GLOB children RELATIVE ${curdir} ${curdir}/*)  
  FOREACH(child ${children})

    IF(IS_DIRECTORY ${curdir}/${child})
        SET(dirlist ${dirlist} ${child})
	SUBDIRLIST(tmplist ${curdir}/${child})
	FOREACH(tmpdir ${tmplist})
		SET(dirlist ${dirlist} ${child}\\${tmpdir})	
	ENDFOREACH()
    ENDIF()
  ENDFOREACH()
  SET(${result} ${dirlist})
ENDMACRO()

#ADD_SUBDIRECTORY(Collections)

#Get subdirectories
SUBDIRLIST(SUBDIRS ${CMAKE_CURRENT_SOURCE_DIR})
FILE(GLOB_RECURSE SysLib *.h *.cpp)

#Add groups
FOREACH(subdir ${SUBDIRS})
  SET(subdir_cpp "Source Files\\${subdir}")
  SET(subdir_h "Header Files\\${subdir}")
  FILE(GLOB tmpcpp ${subdir}/*.cpp)
  SOURCE_GROUP(${subdir_cpp} FILES ${tmpcpp})   
  FILE(GLOB tmph ${subdir}/*.h)
  SOURCE_GROUP(${subdir_h} FILES ${tmph}) 
ENDFOREACH()

#FILE(GLOB Col Collections/*.h Collections/*.cpp)
#FILE(GLOB Gen Collections/Generic/*.h Collections/Generic/*.cpp)
#SOURCE_GROUP(Collections FILES ${Col})
#SOURCE_GROUP(Collections\\Generic FILES ${Gen})

ADD_LIBRARY(System ${SysLib})
SET_TARGET_PROPERTIES(System PROPERTIES LINKER_LANGUAGE CXX)