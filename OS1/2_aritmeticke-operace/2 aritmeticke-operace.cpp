// 2 aritmeticke-operace.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "iostream"
using namespace std;

int obvod_obdelnika(int a, int b) {
	_asm{
		mov eax, a
		add eax, b
		imul eax, 2	
	}
	
}

int obsah_obdelnika(int a, int b) {
	_asm {
		mov eax, a
		imul eax, b
			
	}

}

int obvod_ctverce(int a) {
	_asm {
		mov eax, a
		imul eax, 4

	}

}

int obsah_ctverce(int a) {
	_asm {
		mov eax, a
		imul eax, a

	}

}

int obvod_trojuhelnika(int a, int b, int c) {
	_asm {
		mov eax, a
		add eax, b
		add eax, c

	}

}

int obvod_trojuhelnika(int a) {
	_asm {
		mov eax, a
		imul eax, 3

	}

}

int obsah_trojuhelnika(int a, int b) {
	_asm {
			mov eax, a
			mul b
			mov edx, 0
			mov ebx, 2
			div ebx


	}

}

int obsah_trojuhelnika3(int a, int va) {
	_asm {
		mov eax, a
			imul eax, va
			mov edx, 0
			mov ebx, 2
			div ebx


	}
}

int objem_krychle(int a) {
	_asm {
			mov eax, a
			imul eax, a
			imul eax, a
	}
}

double heron(int a, int b, int c) {
	int s;
	int result;
	_asm {
		mov eax, a
		add eax, b
		add eax, c
		mov edx, 0
		mov ebx, 2
		div ebx

		mov s, eax
	
		mov ebx, s
		sub ebx, a
		imul eax, ebx

		mov ebx, s
		sub ebx, b
		imul eax, ebx

		mov ebx, s
		sub ebx, c
		imul eax, ebx

		mov result, eax
	}
	return sqrt((float)result);
}

int main()
{
	cout << "obvod_obdelnika:" << obvod_obdelnika(10, 2) << endl;
	cout << "obsah_obdelnika:" << obsah_obdelnika(10, 10) << endl 
		<< "obvod_ctverce:" << obvod_ctverce(4) << endl 
		<< "obsah_ctverce:" << obsah_ctverce(5) << endl;
	cout <<"obvod_trojuhelnika:" << obvod_trojuhelnika(6, 4, 3) << endl 
		<< "obvod_trojuhelnika:" << obvod_trojuhelnika(6) << endl 
		<< "obsah trojuh:" << obsah_trojuhelnika(4, 5) << endl 
		<< "obsah_trojuhelnika:" << obsah_trojuhelnika(4, 3) << endl 
		<< "objem krychle:" << objem_krychle(4) << endl;
	cout << heron(5, 5, 4) << endl;
    return 0;
}

