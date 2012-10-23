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