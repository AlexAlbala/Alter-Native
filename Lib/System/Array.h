#pragma once
#include <vector>
#include "Collections/Generic/IteratorCXX.h"
#include <iostream>

using namespace std;
using namespace System::Collections::Generic;
namespace System{
	namespace _Internal {
		template<typename T>
		class Array_T_Base : public Object{
		private:
			vector<TypeArg(T)>* data;		
		public:
			int Length;
			Array_T_Base()
			{
				data = new vector<TypeArg(T)>();
				this->Length = 0;
			}

			Array_T_Base(int Length)
			{
				data = new vector<TypeArg(T)>(Length);
				this->Length = Length;
			}

			Array_T_Base(T* elements, int Length)
			{
				data = new vector<TypeArg(T)>(elements, elements + Length);
				this->Length = Length;
			}

			T* GetData()
			{
				return data->data();
			}

			operator T*()
			{
				return data->data();
			}

			T& operator[](int index)
			{
				return data->at(index);
			}

			System::Collections::Generic::iteratorcxx<TypeArg(T)>* begin()
			{			
				return data->begin();
			}

			System::Collections::Generic::iteratorcxx<TypeArg(T)>* begin() const
			{
				return data->begin();
			}

			System::Collections::Generic::iteratorcxx<TypeArg(T)>* end()
			{			
				return data->end();
			}

			System::Collections::Generic::iteratorcxx<TypeArg(T)>* end() const
			{
				return data->end();
			}

			System::Collections::Generic::iteratorcxx<TypeArg(T)>* rbegin()
			{			
				return data->rbegin();
			}

			System::Collections::Generic::iteratorcxx<TypeArg(T)>* rbegin() const
			{
				return data->rbegin();
			}

			System::Collections::Generic::iteratorcxx<TypeArg(T)>* rend()
			{			
				return data->rend();
			}

			System::Collections::Generic::iteratorcxx<TypeArg(T)>* rend() const
			{
				return data->rend();
			}
		};


		template< typename T, bool>
		class Array{
		};

		template<typename T>
		class Array<T, true> : public Array_T_Base<T>{
			public:
			Array() : Array_T_Base<T>(){}
			Array(int Length) : Array_T_Base<T>(Length){}
			Array(T* elements, int Length) : Array_T_Base<T>(elements, Length){}
		};

		template<typename T>
		class Array<T, false> : public Array_T_Base<Object*>{
			public:
			inline  Array() : Array_T_Base<Object*>(){}
			inline Array(int Length) : Array_T_Base<Object*>(Length){}
 			inline Array(T* elements, int Length) : Array_T_Base<Object*>((Object*)(elements), Length){}
			inline T* GetData(){
				return dynamic_cast<T*>(Array_T_Base<Object*>::GetData());//CAST !
			}

			inline T& operator[](int index){
				return reinterpret_cast<T&>(Array_T_Base<Object*>::operator[](index));//CUIDADO EL CAST
			}
		};
	}

	template<typename T>
	class Array : public _Internal::Array<T, IsBasic(T)>{
	public:
		Array() : _Internal::Array<T, IsBasic(T)>(){}
		Array(int Length) : _Internal::Array<T, IsBasic(T)>(Length){}
		Array(T* elements, int Length) : _Internal::Array<T, IsBasic(T)>(elements, Length){}	
	};
}