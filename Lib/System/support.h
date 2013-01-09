#pragma once
#define null (0)
#define byte char

/*********************************************************************************************************/
/******************************************** SUPPORT MACROS  ********************************************/
/*********************************************************************************************************/

/*
FOREACH MACRO
*/
#define FOREACH(var, container)  container->GetEnumerator()->Reset(); for(auto var = (container)->begin()++; var != (container)->end(); ++var)
/*
Dereferences the types for tepmlate arguments (i.e: List<Object*> is -> List<Object>)
*/
#define TypeArg(T) typename ::__Internal__::TypeTrait<T, true>::Type
/*
Changes the type of a variable declaration:
	-If the type is a basic type, the type is dereferenced
	-If the type is not a basic type, the type is referenced with an *
*/
#define TypeDecl(T) typename ::__Internal__::TypeTrait<T,false>::Type
/*
DEPRECATED: Declares a type as a boxed reference (Box<T>&)
*/
//#define BoxRef(T) typename ::__Internal__::Boxing<T, ::__Internal__::IsFundamentalType<T>::result, true>::Type
/*
Declares a type boxed using Box<T> format
*/
#define BoxDecl(T) typename ::__Internal__::Boxing<T, ::__Internal__::IsFundamentalType<T>::result, false>::Type
/*
BOOLEAN: returns if it is a basic type or not
*/
#define IsBasic(T) ::__Internal__::IsFundamentalType<T>::result


/*********************************************************************************************************/
/***************************************** INCLUDE TEMPLATES  ********************************************/
/*********************************************************************************************************/
#include "templates/asis.h"
#include "templates/internal/TypeRefs.h"
#include "templates/Boxing.h"
#include "templates/internal/TypeTraits.h"

/*********************************************************************************************************/
/************************************* INCLUDE EXCEPTIONS SUPPORT  ***************************************/
/*********************************************************************************************************/
#include "exceptions/exceptionSupport.h"