// 3 skoky.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "iostream"
#include <bitset>
using namespace std;

int avg_int(int a, int b, int c) {
	_asm {
		mov eax, a
			add eax, b
			add eax, c
			mov edx, 0
			mov ebx, 3
			div ebx
	}
}

short avg_short(short a, short b, short c) {
	_asm {
		mov ax, a
			add ax, b
			add ax, c
			mov edx, 0
			cwd
			mov bx, 3
			idiv bx
	}
}

int sgn(int i) {
	_asm {
		mov eax, i
		cmp eax, 0
		jg lgr
		jl ls
	zero:
		mov eax, 0
		jmp end1
	lgr:
		mov eax, 1
		jmp end1
	ls:
		mov eax, -1
			end1:
	}
}

int min3(unsigned char a, short b, int c) {
	
	_asm {
			mov eax, 0
			mov al, a
			mov bx, b
			cmp ax, bx
			jl next1

		greater:
			movsx eax, bx
				

		next1:
			mov ebx, c
			movsx eax, ax
			cmp eax, ebx
			jge greater2
			jl next2

		greater2:
			mov eax, ebx
			
		next2:

	}
	
}

/*int kladne(int a, int b, int c) {

}*/

int mocnina(int n, unsigned int m) {
	_asm {
		mov ecx, m
			sub ecx, 1
		mov eax, n
		mov ebx, n
	loop1:
		imul eax, ebx
		loop loop1
	}
}

int main()
{
	cout << "avg int: " << avg_int(-1, 5, 6) << endl;
	cout << "avg short: " << avg_short((short)1,(short)2,(short)3 ) << endl;
	cout << "sgn: " << sgn(-5) << endl;
	cout << "min3: " << min3(255, -1, -6) << endl;
	char c = -2;
	cout << ":" << bitset<8> (c) << endl;
	cout << "int mocnina:" << mocnina(5, 3) << endl;
    return 0;
}

