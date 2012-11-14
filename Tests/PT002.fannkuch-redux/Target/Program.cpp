#include "Program.h"
Array<int>* Program::fannkuch(int n){
	Array<int>* p = new Array<int>(n);
	Array<int>* q = new Array<int>(n);
	Array<int>* s = new Array<int>(n);
	int sign = 1;
	int maxflips = 0;
	int sum = 0;
	int i = n - 1;
	for (int j = 0; j < n; j += 1){
		(*p)[j] = j;
		(*q)[j] = j;
		(*s)[j] = j;
	}
	while (true) {
		int q2 = (*p)[0];
		if (q2 != 0) {
			for (int j = 1; j < n; j += 1) {
				(*q)[j] = (*p)[j];
			}
			int flips = 1;
			while (true) {
				int qq = (*q)[q2];
				if (qq == 0) {
					break;
				}
				(*q)[q2] = q2;
				if (q2 >= 3) {
					int j = 1;
					int k = q2 - 1;
					do {
						int t = (*q)[j];
						(*q)[j] = (*q)[k];
						(*q)[k] = t;
						j += 1;
						k -= 1;
					}
					while (j < k);
				}
				q2 = qq;
				flips += 1;
			}
			sum += sign * flips;
			if (flips > maxflips) {
				maxflips = flips;
			}
		}
		if (sign == 1) {
			int t = (*p)[1];
			(*p)[1] = (*p)[0];
			(*p)[0] = t;
			sign = -1;
		}
		else {
			int t = (*p)[1];
			(*p)[1] = (*p)[2];
			(*p)[2] = t;
			sign = 1;
			for (int j = 2; j < n; j += 1) {
				int sx = (*s)[j];
				if (sx != 0) {
					(*s)[j] = sx - 1;
					break;
				}
				if (j == i) {
					goto Block_8;
				}
				(*s)[j] = j;
				t = (*p)[0];
				for (int k = 0; k <= j; k += 1) {
					(*p)[k] = (*p)[k + 1];
				}
				(*p)[j + 1] = t;
			}
		}
	}
	Block_8:
	Array<int>* _T = new Array<int>(2);
	(*_T)[0] = sum;
	(*_T)[1] = maxflips;
	return _T;
}
void Program::Main(String* args[])
{
	int i = 12;
	Array<int>* pf = Program::fannkuch(i);
	Console::WriteLine((*pf)[0]);
	Console::Write(new String("Pfannkuchen("));
	Console::Write(i);
	Console::Write(new String(") = "));
	Console::WriteLine((*pf)[1]);
}

