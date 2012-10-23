/*********************************************************************************************************/
/********************************************** BOXING UNBOXING ******************************************/
/*********************************************************************************************************/
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