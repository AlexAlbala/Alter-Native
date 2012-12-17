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
GET_PROPERTY(IS_RELEASE GLOBAL PROPERTY GL_IS_RELEASE)

SET(Boost_USE_STATIC_LIBS        ON)
SET(Boost_USE_MULTITHREADED      ON)
SET(Boost_USE_STATIC_RUNTIME    OFF)
SET(Boost_ADDITIONAL_VERSIONS "1.33" "1.33.0" "1.33.1" "1.34" "1.34.0" "1.34.1" "1.35" 
								"1.35.0" "1.35.1" "1.36" "1.36.0" "1.36.1" "1.37" "1.37.0" 
								"1.38" "1.38.0" "1.39" "1.39.0" "1.40" "1.40.0" "1.41" "1.41.0" 
								"1.42" "1.42.0" "1.43" "1.43.0" "1.44" "1.44.0" "1.45" "1.45.0" 
								"1.46" "1.46.0" "1.46.1" "1.47" "1.47.0" "1.48" "1.48.0" "1.49" 
								"1.49.0" "1.50" "1.50.0" "1.51" "1.51.0" "1.52" "1.52.0" "1.53" 
								"1.53.0" "1.54" "1.54.0" "1.55" "1.55.0" "1.56" "1.56.0")
FIND_PACKAGE( Boost 1.51 COMPONENTS system locale)

IF(Boost_FOUND)
	INCLUDE_DIRECTORIES(${Boost_INCLUDE_DIRS})
	TARGET_LINK_LIBRARIES(System ${Boost_LIBRARIES})
ELSE()
	IF(DEFINED ENV{USR_BOOST_INCLUDE})
		INCLUDE_DIRECTORIES($ENV{USR_BOOST_INCLUDE)
	ELSE()
		MESSAGE("Boost include directory not found, please set the environmental variable <USR_BOOST_INCLUDE> to the include boost directory")
	ENDIF()
	IF(DEFINED ENV{USR_BOOST_LIBRARY})
		IF(ANDROID OR UNIX)
			#HOW TO DETECT THE NAME (THE VERSION NUMBER) AUTOMATICALLY ?
			TARGET_LINK_LIBRARIES(System $ENV{USR_BOOST_LIBRARY}/libboost_system-gcc-mt-1_49.a)
		ELSE()					
			IF(IS_RELEASE EQUAL 1)
				#TODO: HOW TO DETECT THE NAME (THE VERSION NUMBER) AUTOMATICALLY ?
				TARGET_LINK_LIBRARIES(System $ENV{USR_BOOST_LIBRARY}/libboost_system-vc100-mt-1_51.so)
				TARGET_LINK_LIBRARIES(System $ENV{USR_BOOST_LIBRARY}/libboost_locale-vc100-mt-1_51.so)
			ELSE()
				#TODO: HOW TO DETECT THE NAME (THE VERSION NUMBER) AUTOMATICALLY ?
				TARGET_LINK_LIBRARIES(System $ENV{USR_BOOST_LIBRARY}/libboost_system-vc100-mt-gd-1_51.so)
				TARGET_LINK_LIBRARIES(System $ENV{USR_BOOST_LIBRARY}/libboost_locale-vc100-mt-gd-1_51.so)
			ENDIF()
		ENDIF()		
	ELSE()    
		MESSAGE("Boost library directory not found, please set the environmental variable <USR_BOOST_LIBRARY> to the library boost directory")
	ENDIF()
ENDIF()