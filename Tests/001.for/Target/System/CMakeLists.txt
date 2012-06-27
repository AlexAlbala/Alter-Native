CMAKE_MINIMUM_REQUIRED(VERSION 2.8)
PROJECT(sys)
SET(SysLib Random.h Random.cpp Console.cpp Console.h gcptr.cpp gcptr.h System.h support.h String.cpp String.h Object.cpp Object.h GC.h GC.cpp)
ADD_LIBRARY(LIB ${SysLib})