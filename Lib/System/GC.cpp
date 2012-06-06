#include "GC.h"
#include "gcptr.h"

void GC::Collect(){	
	gc_collect();
}