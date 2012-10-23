
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