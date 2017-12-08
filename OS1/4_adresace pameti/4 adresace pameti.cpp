// 4 adresace pameti.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "iostream"
using namespace std;

static int int_array[10];
static short short_array[10];
int *a = (int*)malloc(5);

void nasobky(short n) {
	_asm {
		mov ecx, 0
	loop1:
		mov eax, ecx
		inc eax
		mul n
		mov [int_array + 4 * ecx], eax
		inc ecx
		cmp ecx, 10
		jl loop1
	}
}

void countdown() {
	_asm {
			mov ecx, 11
			mov ebx, 0
		loop1:
			mov eax, ecx
			dec eax
			mov[int_array + 4 * ebx], eax
			inc ebx
			dec ecx
			cmp ecx, 0
			jg loop1
	}
}

void mocniny() {
	_asm {
		mov cx, 0
	loop1:
		mov ax, cx
		inc ax
		mul ax
		mov [short_array + 2 * ecx], ax
		inc cx
		cmp cx, 10
		jl loop1
	}
}

void mocniny2() {
	_asm {
			mov cx, 0
			mov ax, 2
			mov[short_array + 2 * ecx], 1
			inc cx
			mov[short_array + 2 * ecx], ax
			inc cx
		loop1:
			shl ax, 1
			mov[short_array + 2 * ecx], ax
			inc cx
			cmp cx, 10
			jl loop1
	}
}

int avg(unsigned int n){
	_asm {
		mov ecx, 0
		mov eax, 0
		mov esi, a
	loop1:
		add eax, [esi + 4 * ecx]
		inc ecx
		cmp ecx, n
		jl loop1
			
		mov edx, 0
		mov ebx, n
		cdq
		idiv ebx
	}
}

int minimum() {
	int arr[10] = { 1, 4, 0, 5, 2, -7, 1, 12, -3, 8 };
	_asm {
			mov ecx, 0
			
		loop1:
			mov eax, [arr + 4 * ecx]
			
		loop2 :
			cmp eax, [arr + 4 * ecx]
			jg loop1
			inc ecx
			cmp ecx, 10
			jl loop2
			
	}
}

int main()
{
	nasobky(5);
	for (int i = 0; i < 10; i++) {
		cout << int_array[i] << " ";
	}
	cout << endl;
	countdown();
	for (int i = 0; i < 10; i++) {
		cout << int_array[i] << " ";
	}

	cout << endl;
	mocniny();
	for (int i = 0; i < 10; i++) {
		cout << short_array[i] << " ";
	}
	cout << endl;
	mocniny2();
	for (int i = 0; i < 10; i++) {
		cout << short_array[i] << " ";
	}
	cout << endl;
	a[0] = 1;
	a[1] = -1;
	a[2] = -9;
	a[3] = 4;
	a[4] = -5;
	cout << "avg(5) = "<< avg(5) << endl;
	cout << "minimum() = " << minimum() << endl;
    return 0;
}

