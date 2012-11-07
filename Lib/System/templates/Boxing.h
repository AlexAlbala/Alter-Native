#include "internal/Boxing.h"

/*********************************************************************************************************/
/********************************************** BOXING UNBOXING ******************************************/
/*********************************************************************************************************/
template<typename T>
System::Box_T<T>* BOX(T t){
	return new System::Box_T<T>(t);	
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