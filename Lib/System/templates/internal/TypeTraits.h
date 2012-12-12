
/*********************************************************************************************************/
/*********************************** ISFUNDAMENTALTYPE TEMPLATES  ****************************************/
/*********************************************************************************************************/
namespace __Internal__{
    
	/*
	Returns true if it is a basic type, false if not
	*/
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

	/*
	Converts the type with the following rules:
    -If it is basic type:
        -Returns the dereferenced type if is declared as a type argument
        -Returns the type if it is not declared as a type argument (i.e. type declaration)
    -If it is not basic type
        -Returns the type (or System::Object if it is a System::Object*) useful for templates
        -Returns the type as it is    
	*/
	template <typename T, bool isTypeArgument>
	struct TypeTrait {
		typedef typename DeRefBasicType<T>::Type Dereferenced; 
		typedef typename _TypeTrait<typename TypeTrait<T, isTypeArgument>::Dereferenced, isTypeArgument, IsFundamentalType<typename TypeTrait<T, isTypeArgument>::Dereferenced>::result>::Type Type;
	};

/*	//DEPRECATED: PARAMETERS
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
    */
}