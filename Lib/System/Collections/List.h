#include "IEnumerable.h"
#include <stdlib.h>

template<typename T>
class MyEnumerator : IEnumerator<T>
{
private:
	T* initialPos;
	T* elements;
	int count;
	int position;

public:

	MyEnumerator(T* elements, int count)
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
class List : public IEnumerable<T>
{

private:
	int count;
	T *elements;

public:
	List()
	{
		count = 0;		
	}

	~List()
	{
		count = 0;
		delete(elements);
	}

	virtual IEnumerator<T>* GetEnumerator()
	{
		//When we free this memory ????
		MyEnumerator<T>* enumerator = new MyEnumerator<T>(elements,count);
		return (IEnumerator<T>*)enumerator;
	}

	void Add(T element)
	{
		if (count == 0)
			elements = (T*)malloc(sizeof(T));
		else
			elements = (T*)realloc(elements, (count+1)*sizeof(T));

		elements[count++] = element;
	}	
};

