#pragma once
#include "System/System.h"
#include "System/Collections/Generic/IEnumeratorCXX.h"
#include "System/IDisposable.h"
#include "System/Collections/IEnumeratorCXX.h"

using namespace System::Collections::Generic;
using namespace System;
using namespace System::Collections;
namespace CustomCollections {
	namespace _Internal {

		//The classes defined in namespace _Internal are internal types.
		//DO NOT modify this code

		template<typename T>
		class MyEnumerator_T_Base : public virtual IEnumerator_T<TypeArg(T)>, public virtual IDisposable, public virtual Object, public virtual gc_cleanup{

			//START Explicit interface: _interface_IEnumerator ****************
			public:
			class _interface_IEnumerator : public virtual IEnumerator{
				private:
				Object* getCurrent(){
					return this->getCurrent();
				}
												//IF THE BASE CLASS IS AN INTERFACE (ABSTRACT CLASS)
				bool MoveNext(){return null;}	//IMPLEMENT INTERFACEMEMBERS
				void Reset(){}
			};
			private:
				_interface_IEnumerator __interface_ienumerator;
			public:
			operator IEnumerator*() {
				return &__interface_ienumerator;
			}
			//END Explicit interface *********************

			private:
			System::Array<T>* values;
			private:
			int currentIndex;
			public:
			BoxDecl(T) getCurrent(){ //Si Return Type de la clase abstracta es Object* --> BoxDecl(T)	
				return (*this->values)[this->currentIndex];
			}

			/*private://DUPLICATED METHOD !
			Object* getCurrent()
			{
				return this->getCurrent();
			}*/

			public:
			MyEnumerator_T_Base(System::Array<T>* values)
			{
				this->values = values;
				this->Reset();
			}
			public:
			void Dispose()
			{
			}
			public:
			bool MoveNext()
			{
				this->currentIndex += 1;
				return this->currentIndex < this->values->Length;
			}
			public:
			void Reset()
			{
				this->currentIndex = -1;
			}
			public:
			MyEnumerator_T_Base()
			{
			}
		};

		template<typename T, bool>
		class MyEnumerator_T  {
		};

		//Basic types template type
		template<typename T>
		class MyEnumerator_T<T, true> : public MyEnumerator_T_Base<T>{
			public:
			MyEnumerator_T(System::Array<T>* values) : MyEnumerator_T_Base<T>(values){ //REMOVE POINTER FROM Array<T*>
			}
		};

		//Generic template type
		template<typename T>
		class MyEnumerator_T<T, false> : public virtual MyEnumerator_T_Base<Object*>{
			public:
			inline MyEnumerator_T(System::Array<T>* values) : MyEnumerator_T_Base<Object*>((System::Array<Object*>*)(values))
			{
			}
			public:
			inline void Dispose() {
				MyEnumerator_T_Base<Object*>::Dispose();
			}
			public:
			inline bool MoveNext() {
				return MyEnumerator_T_Base<Object*>::MoveNext();
			}
			public:
			inline void Reset() {
				MyEnumerator_T_Base<Object*>::Reset();
			}
			inline operator IEnumerator*() {
				return (IEnumerator*)(MyEnumerator_T_Base<Object*>::operator IEnumerator*());
			}

			inline T* getCurrent(){ //PROPERTIES INLINE!!!!
				return (T*)(MyEnumerator_T_Base<Object*>::getCurrent());
			}
		};
	}

	//Type definition
	template<typename T>
	class MyEnumerator_T : public _Internal::MyEnumerator_T<T, IsBasic(T)>{
		public:
		MyEnumerator_T(Array<T>* values) : _Internal::MyEnumerator_T<T, IsBasic(T)>(values){
		}
	};
}
