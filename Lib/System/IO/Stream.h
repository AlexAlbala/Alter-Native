#pragma once
#include "../IDisposable.h"
namespace System_IO{
class Stream : public IDisposable
{
public:
	void Dispose();
};
}