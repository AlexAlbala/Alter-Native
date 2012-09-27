#pragma once
#define null (0)

/*********************************************************************************************************/
/*********************************************** AS IS CASTS  ********************************************/
/*********************************************************************************************************/
template < class T, class U > 
bool is_inst_of(U u) {
   return dynamic_cast< T >(u) != nullptr;
};

template < class T, class U > 
T as_cast(U u) {
   return dynamic_cast< T >(u);
};

/*********************************************************************************************************/
/*************************************** TYPETRATITS CONVERSIONS  ****************************************/
/*********************************************************************************************************/
template <typename T>
struct DeRefType {
	typedef T Type;
};
template <>
struct DeRefType<System::Object*> {
	typedef System::Object Type;
};


template <typename T>
struct DeRefBasicType {
	typedef T Type;
};

template <>
struct DeRefBasicType<int*> {
	typedef int Type;
};

template <>
struct DeRefBasicType<unsigned int*> {
	typedef unsigned int Type;
};

template <>
struct DeRefBasicType<short*> {
	typedef short Type;
};

template <>
struct DeRefBasicType<unsigned short*> {
	typedef unsigned short Type;
};

template <>
struct DeRefBasicType<long*> {
	typedef long Type;
};

template <>
struct DeRefBasicType<unsigned long*> {
	typedef unsigned long Type;
};

template <>
struct DeRefBasicType<char*> {
	typedef char Type;
};

template <>
struct DeRefBasicType<float*> {
	typedef float Type;
};


/*********************************************************************************************************/
/*********************************** ISFUNDAMENTALTYPE TEMPLATES  ****************************************/
/*********************************************************************************************************/
template<typename T>
struct IsFundamentalType {
	enum { result = false };
};

template<>
struct IsFundamentalType<int> {
	enum { result = true };
};

template<>
struct IsFundamentalType<unsigned int> {
	enum { result = true };
};

template<>
struct IsFundamentalType<short> {
	enum { result = true };
};

template<>
struct IsFundamentalType<unsigned short> {
	enum { result = true };
};

template<>
struct IsFundamentalType<long> {
	enum { result = true };
};

template<>
struct IsFundamentalType<unsigned long> {
	enum { result = true };
};

template<>
struct IsFundamentalType<float> {
	enum { result = true };
};