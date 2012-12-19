#include "Program.h"
namespace Covariance {
	void Program::Main(String* args[]){
		A_T<C>* a = new A_T<C>();
		CovIEnumerator_T<C>* b = a->Get();
		A_T<int>* a2 = new A_T<int>();
		CovIEnumerator_T<int>* b2 = a2->Get();
	}

}
