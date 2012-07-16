#pragma once
#include "IEnumerable.h"
#include <stdlib.h>

namespace System_Collections_Generic{

template<typename T>
class ListEnumerator_T : public IEnumerator_T<T>, public gc_cleanup
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
		this->initialPos = elements;
		this->count = count;
		position = 0;
	}

	virtual bool MoveNext()
	{
		position++;
		elements++;
		
		return (position == count);
	}

	virtual void Reset()
	{
		position = 0;
		elements = initialPos;
		return;
	}

	virtual T* Current()
	{		
		return elements;
	}
};

template<typename T>
class List_T : public IEnumerable_T<T>, public gc_cleanup /*, public IList*/ //TODO Implement IList(<T>) and inherit from it
{

private:
	int count;
	T *elements;

public:
	List_T()
	{
		count = 0;		
	}

	~List_T()
	{
		count = 0;
		delete(elements);
	}

	virtual IEnumerator_T<T>* GetEnumerator()
	{
		ListEnumerator_T<T>* enumerator = new ListEnumerator_T<T>(elements,count);
		return (IEnumerator_T<T>*)enumerator;
	}

	void Add(T element)
	{
		if (count == 0)
			elements = (T*)malloc(sizeof(T));
		else
			elements = (T*)realloc(elements, (count+1)*sizeof(T));

		elements[count++] = element;
	}

	T* ElementAt(int index)
	{
		return (T*)(elements+index);
	}

	int IndexOf(T* element)
	{
		for(int i = 0; i <count; i++)
		{
			if((T*)(elements + i) == element)
				return i;
		}
		return -1;
	}

	void Remove(T* element)
	{		
		int i = IndexOf(element);

		for(int j=i;j<count;j++)
		{
			elements[j]=elements[j+1];
		}

		elements = (T*)realloc(elements, (count-1)*sizeof(T));
		count--;
	}

	void RemoveAt(int index)
	{
		//TODO message
		if(index >= count) throw;

		for(int j=index;j<count;j++)
		{
			elements[j]=elements[j+1];
		}

		elements = (T*)realloc(elements, (count-1)*sizeof(T));
		count--;
	}
};
}

