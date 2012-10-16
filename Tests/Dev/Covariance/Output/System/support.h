#pragma once
#define null (0)

/*********************************************************************************************************/
/******************************************** SUPPORT MACROS  ********************************************/
/*********************************************************************************************************/
#define FOREACH(var, container)  container->GetEnumerator()->Reset(); for(auto var = (container)->begin()++; var != (container)->end(); ++var)
#define TypeArg(T) typename ::__Internal__::TypeTrait<T, true>::Type
#define TypeDecl(T) typename ::__Internal__::TypeTrait<T,false>::Type
#define BoxRef(T) typename ::__Internal__::Boxing<T, ::__Internal__::IsFundamentalType<T>::result, true>::Type
#define BoxDecl(T) typename ::__Internal__::Boxing<T, ::__Internal__::IsFundamentalType<T>::result, false>::Type
#define IsBasic(T) ::__Internal__::IsFundamentalType<T>::result

//#define TypeParam(T) typename ::__Internal__::ParamTrait<T>::Type
//#define TypeRet(T, element) (IsBasic(T) ? new System::Box_T<T>(*(element)) : (T*)(elements));

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
/******************************** TYPETRAITS INTERNAL CONVERSIONS  ***************************************/
/*********************************************************************************************************/
namespace __Internal__{	
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
}

/*********************************************************************************************************/
/********************************************** BOXING UNBOXING ******************************************/
/*********************************************************************************************************/
template<typename T>
System::Box_T<T>* BOX(T t){
	return new Box_T<T>(t);	
}

template<typename T>
T& UNBOX(System::Object* t){
	System::Box_T<T>* tmp = (System::Box_T<T>*)t;
	return *tmp;
}

/*
template<typename T>
T& UNBOX(System::Object t){
	System::Box_T<T>* tmp = (System::Box_T<T>*)(&t);
	return *tmp;
}*/

namespace __Internal__{
	template<typename T, bool isFundamental, bool isRef>
	struct Boxing{	
	};

	template<typename T>
	struct Boxing<T, true, false>{
		typedef System::Box_T<typename DeRefBasicType<T>::Type>* Type;
	};

	template<typename T>
	struct Boxing<T, false, false>{
		typedef typename DeRefType<T>::Type* Type;
	};

	template<typename T>
	struct Boxing<T, true, true>{
		typedef System::Box_T<typename DeRefBasicType<T>::Type>& Type;
	};

	template<typename T>
	struct Boxing<T, false, true>{
		typedef typename DeRefType<T>::Type& Type;
	};
}


/*********************************************************************************************************/
/*********************************** ISFUNDAMENTALTYPE TEMPLATES  ****************************************/
/*********************************************************************************************************/
namespace __Internal__{
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

	template<>
	struct IsFundamentalType<char> {
		enum { result = true };
	};
}

/*********************************************************************************************************/
/********************************* TYPETRAITS PUBLIC CONVERSIONS  ****************************************/
/*********************************************************************************************************/
namespace __Internal__{
	template <typename T, bool isTypeArgument, bool isBasicType>
	struct _TypeTrait {
		typedef T Type;
	};

	template <typename T>
	struct _TypeTrait<T, true, true> {
		typedef typename DeRefBasicType<T>::Type Type;
	};

	template <typename T>
	struct _TypeTrait<T, true, false>{
		typedef typename DeRefType<T>::Type Type;
	};

	template <typename T>
	struct _TypeTrait<T, false, true> {
		typedef T Type;
	};

	template <typename T>
	struct _TypeTrait<T, false, false>{
		typedef typename DeRefType<T>::Type* Type;
	};

	template <typename T, bool isTypeArgument>
	struct TypeTrait {
		typedef typename DeRefBasicType<T>::Type Dereferenced; 
		typedef typename _TypeTrait<typename Dereferenced, isTypeArgument, IsFundamentalType<typename Dereferenced>::result>::Type Type;
	};

	//PARAMETERS
	template <typename T, bool isBasic>
	struct _ParamTrait {
	};

	template <typename T>
	struct _ParamTrait<T,true> {
		typedef T Type;
	};

	template <typename T>
	struct _ParamTrait<T,false> {
		typedef T Type;
	};

	template <typename T>
	struct ParamTrait {
		typedef typename _ParamTrait<T, IsFundamentalType<T>::result>::Type Type;	
	};
}