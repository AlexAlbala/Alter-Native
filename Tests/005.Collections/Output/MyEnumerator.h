#pragma once
#include "System/System.h"
#include "IEnumerator.h"
#include "IDisposable.h"
#include "System/Collections/Generic/List.h"

using namespace System.Collections.Generic;
using namespace System;
using namespace System.Collections;
namespace CollectionsExample{

	template<typename T>
	class MyEnumerator : public IEnumerator<T>, public IDisposable, public IEnumerator, public Object, public gc_cleanup{
		List<T>* MyEnumerator::values;
		int MyEnumerator::currentIndex;
		MyEnumerator::MyEnumerator(List<T>* values){
			this->values = new List<T>(values);
			this->Reset();
		}
		void MyEnumerator::Dispose()
		{
		}
		bool MyEnumerator::MoveNext()
		{
			this->currentIndex += 1;
			return this->currentIndex < this->values->Count;
		}
		void MyEnumerator::Reset()
		{
			this->currentIndex = -1;
		}
	};
}