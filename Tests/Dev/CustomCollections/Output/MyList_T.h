#pragma once
#include "System/System.h"
#include "System/Collections/Generic/IEnumerable.h"
#include "System/Collections/IEnumerable.h"
#include "System/Collections/Generic/IEnumeratorCXX.h"
#include "MyEnumerator_T.h"
#include "System/Collections/IEnumeratorCXX.h"

using namespace System::Collections::Generic;
using namespace System::Collections;
using namespace System;
namespace CustomCollections {
	namespace _Internal {

		//The classes defined in namespace _Internal are internal types.
		//DO NOT modify this code

		template<typename T>
		class MyList_T_Base : public virtual IEnumerable_T<TypeArg(T)>, public virtual Object, public virtual gc_cleanup{

			//START Explicit interface: _interface_IEnumerable ****************
			public:
			class _interface_IEnumerable : public virtual IEnumerable{
				private:
				IEnumerator* GetEnumerator(){
					return this->GetEnumerator();
				}
			};
			private:
				_interface_IEnumerable __interface_ienumerable;
			public:
			operator IEnumerable*() {
				return &__interface_ienumerable;
			}
			//END Explicit interface *********************

			private:
			System::Array<TypeArg(T)>* mylist;
			public:
			MyList_T_Base(System::Array<TypeArg(T)>* values){
				this->mylist = values;
			}
			public:
			IEnumerator_T<TypeArg(T)>* GetEnumerator()
			{
				return new CustomCollections::MyEnumerator_T<TypeArg(T)>(this->mylist);
			}
			public:
			MyList_T_Base()
			{
			}
		};

		template<typename T, bool>
		class MyList_T  {
		};

		//Basic types template type
		template<typename T>
		class MyList_T<T, true> : public MyList_T_Base<T>{
			public:
			MyList_T(System::Array<T>* values) : MyList_T_Base<T>(values){
			}

			public:
			inline IEnumerator_T<T>* GetEnumerator() {
				Object* var_tmp = MyList_T_Base<T>::GetEnumerator();
				return dynamic_cast<IEnumerator_T<T>*>(var_tmp);//CAST!!
			}

		};

		//Generic template type
		template<typename T>
		class MyList_T<T, false> : public virtual MyList_T_Base<Object*>{
			public:
			inline MyList_T(System::Array<T>* values) : MyList_T_Base<Object*>((System::Array<Object>*)(values)){
			}
			public:
			inline IEnumerator_T<T>* GetEnumerator() {
				Object* var_tmp = MyList_T_Base<Object*>::GetEnumerator();
				return dynamic_cast<IEnumerator_T<T>*>(var_tmp);//CAST!!
			}
			inline operator IEnumerable*() {
				return (IEnumerable*)(MyList_T_Base<Object*>::operator IEnumerable*());
			}
		};
	}

	//Type definition
	template<typename T>
	class MyList_T : public _Internal::MyList_T<T, IsBasic(T)>{
		public:
		MyList_T(Array<T>* values) : _Internal::MyList_T<T, IsBasic(T)>(values){//Remove pointer from Array<T*>
		}
	};
}
