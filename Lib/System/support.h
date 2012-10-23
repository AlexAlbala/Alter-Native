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
/***************************************** INCLUDE TEMPLATES  ********************************************/
/*********************************************************************************************************/
#include "templates/asis.h"
#include "templates/internal/TypeRefs.h"
#include "templates/Boxing.h"
#include "templates/internal/TypeTraits.h"