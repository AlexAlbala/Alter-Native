/*********************************************************************************************************/
/********************************************** BOXING UNBOXING ******************************************/
/*********************************************************************************************************/
namespace __Internal__{
	
	/*
	Changes the types:
		-Box<T>*
			-If T is basic type, it is dereferenced
			-if T is not basic type, also is dereferenced
		-Box<T>& DEPRECATED
			-If T is basic type, it is dereferenced
			-If T is not basic type, also it is dereferenced
	*/

	
	template<typename T, bool isFundamental, bool isRef>
	struct Boxing{	
	};

	template<typename T>
	struct Boxing<T, true, false>{	//Basic type
		typedef System::Box_T<typename DeRefBasicType<T>::Type>* Type;
	};

	template<typename T>
	struct Boxing<T, false, false>{	//Not Basic type
		typedef typename DeRefType<T>::Type* Type;
	};

	/*template<typename T>
	struct Boxing<T, true, true>{	//Basic type Reference
		typedef System::Box_T<typename DeRefBasicType<T>::Type>& Type;
	};

	template<typename T>
	struct Boxing<T, false, true>{	//Not Basic type Reference
		typedef typename DeRefType<T>::Type& Type;
	};*/
	
}