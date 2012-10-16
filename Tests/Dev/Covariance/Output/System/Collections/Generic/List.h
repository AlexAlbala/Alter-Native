#pragma once
#include "IEnumerable.h"
#include <stdlib.h>

namespace System{
	namespace Collections{
		namespace Generic{

			template<typename T>
			class ListEnumerator_T : public IEnumerator_T<TypeArg(T)>, public gc_cleanup
			{
			private:
				T* initialPos;
				T* elements;
				int count;
				int position;

			public:
				ListEnumerator_T(T* elements, int count)
				{
					this->elements = elements;
					this->initialPos = elements-1;
					this->count = count;
					this->Reset();
				}
			
				virtual bool MoveNext()
				{
					position++;
					elements++;
					
					return (position < count);
				}

				virtual void Reset()
				{
					position = -1;
					elements = initialPos;
					return;
				}
			
				virtual T* getCurrent()
				{		
					return (T*)(elements);
				}
			};
			
			template<typename T>
			class List_T : public IEnumerable_T<T>, public gc_cleanup /*, public IList*/ //TODO Implement IList(<T>) and inherit from it
			{
			
			private:	
				T* elements;
			
			public:
				int Count;
				List_T()
				{
					Count = 0;		
				}
			
				List_T(List_T<T>* values)
				{
					for(int i=0; i<values->Count;i++)
					{
						T* val = values->ElementAt(i);
						this->Add(*val);
					}
				}
			
				~List_T()
				{
					Count = 0;
					delete(elements);
				}
			
				virtual IEnumerator_T<T>* GetEnumerator()
				{
					ListEnumerator_T<T>* enumerator = new ListEnumerator_T<T>(elements,Count);
					return (IEnumerator_T<T>*)enumerator;
				}

				/*void Add(T element)
				{
					if (Count == 0)
						elements = (T*)malloc(sizeof(T));
					else
						elements = (T*)realloc(elements, (Count+1)*sizeof(T));
			
					elements[Count++] = element;
				}*/
			
				void Add(TypeParam(T) element)
				{
					if (Count == 0)
						elements = (T*)malloc(sizeof(T));
					else
						elements = (T*)realloc(elements, (Count+1)*sizeof(T));
			
					elements[Count++] = element;
				}
			
				T* ElementAt(int index)
				{
					return (T*)(elements + index);					
				}

				T* operator[](int index)
				{
					return this->ElementAt(index);
				}
			
				/*T& operator[](int index)
				{
					return *this->ElementAt(index);
				}

				const T& operator[](int index) const
				{
					return *this->ElementAt(index);
				}*/

				int IndexOf(TypeParam(T) element)
				{
					for(int i = 0; i <Count; i++)
					{
						if((T*)(elements + i) == element)
							return i;
					}
					return -1;
				}

				void Remove(TypeParam(T) element)
				{		
					int i = IndexOf(element);

					for(int j=i;j<Count;j++)
					{
						elements[j]=elements[j+1];
					}

					elements = (TypeDecl(T))realloc(elements, (Count-1)*sizeof(T));
					Count--;
				}

				void RemoveAt(int index)
				{
					//TODO message
					if(index >= Count) throw;

					for(int j=index;j<Count;j++)
					{
						elements[j]=elements[j+1];
					}

					elements = (T*)realloc(elements, (Count-1)*sizeof(T));
					Count--;
				}
			};
		}
	}
}

