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
			for (int k = 1; k < n; k += 1) {
				(*q)[k] = (*p)[k];
			}
			int flips = 1;
			while (true) {
				int qq = (*q)[q2];
				if (qq == 0) {
					break;
				}
				(*q)[q2] = q2;
				if (q2 >= 3) {
					int l = 1;
					int m = q2 - 1;
					do {
						int t = (*q)[l];
						(*q)[l] = (*q)[m];
						(*q)[m] = t;
						l += 1;
						m -= 1;
					}
					while (l < m);
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
			int t2 = (*p)[1];
			(*p)[1] = (*p)[0];
			(*p)[0] = t2;
			sign = -1;
		}
		else {
			int t3 = (*p)[1];
			(*p)[1] = (*p)[2];
			(*p)[2] = t3;
			sign = 1;
			for (int i2 = 2; i2 < n; i2 += 1) {
				int sx = (*s)[i2];
				if (sx != 0) {
					(*s)[i2] = sx - 1;
					break;
				}
				if (i2 == i) {
					goto Block_8;
				}
				(*s)[i2] = i2;
				t3 = (*p)[0];
				for (int j2 = 0; j2 <= i2; j2 += 1) {
					(*p)[j2] = (*p)[j2 + 1];
				}
				(*p)[i2 + 1] = t3;
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

