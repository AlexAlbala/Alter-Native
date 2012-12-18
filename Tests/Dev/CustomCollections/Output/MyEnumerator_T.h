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
		class MyEnumerator_T_Base : public virtual IEnumerator_T<T>, public virtual IDisposable, public virtual Object{

			//START Explicit interface: _interface_IEnumerator ****************
			public:
			class _interface_IEnumerator : public virtual IEnumerator{
				private:
				Object* getCurrent(){
					return this->getCurrent();
				}

				bool MoveNext()
				{return false;}

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
			Array<T>* values;
			private:
			int currentIndex;
			public:
			Object* getCurrent(){
				return (*this->values)[this->currentIndex];
			}
			/*private:
			Object* getCurrent()
			{
				return BOX<TypeDecl(T)>(this->Current);
			}*/
			public:
			MyEnumerator_T_Base(Array<T>* values)
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
		class MyEnumerator_T<T, true> : public virtual MyEnumerator_T_Base<T>{
			public:
			MyEnumerator_T(Array<T>* values) : MyEnumerator_T_Base<T>(values){
			}
		};

		//Generic template type
		template<typename T>
		class MyEnumerator_T<T, false> : public virtual MyEnumerator_T_Base<Object>{
			public:
			inline MyEnumerator_T(Array<T>* values) : MyEnumerator_T_Base<Object>((Array<Object>*)(values))
			{
			}
			public:
			inline void Dispose() {
				MyEnumerator_T_Base<Object>::Dispose();
			}
			public:
			inline bool MoveNext() {
				return MyEnumerator_T_Base<Object>::MoveNext();
			}
			public:
			inline void Reset() {
				MyEnumerator_T_Base<Object>::Reset();
			}
			inline operator IEnumerator*() {
				return (IEnumerator*)(MyEnumerator_T_Base<Object>::operator IEnumerator*());
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
