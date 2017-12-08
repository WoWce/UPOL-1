// 8 rychlost operaci.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <string>
#include <iostream>

using namespace std;

unsigned long long measure_mov()
{
	_asm {
		mov ecx, 100

			rdtsc
			push edx
			push eax

		while1:
			mov eax, 0
			mov eax, 1
			mov eax, 2
			mov eax, 3
		loop while1

			rdtsc
			sub eax, [esp];
			sbb edx, [esp + 4]
			add esp, 8
	}
}

int myAtoi(char *str)
{
	int res = 0;
	for (int i = 0; str[i] != '\0'; ++i)
		res = res * 10 + str[i] - '0';
	return res;
}

unsigned long long measure_mul()
{
	_asm {
		rdtsc
			push edx
			push eax

			mov ecx, 100
			mov eax, 5
			while1:
		mul ecx
			mul ecx
			mul ecx
			mul ecx
			loop while1

			rdtsc
			sub eax, [esp];
		sub edx, [esp + 4]
			add esp, 8
	}
}

int myAtoi_asm(char *str)
{
	int res = 0;
	_asm {
		rdtsc
			push edx
			push eax

			mov ebx, 0
			mov esi, str

			mov eax, 4
			mov ecx, 0

			rec1:
		imul ebx, 10
			movsx edx, byte ptr[esi + ecx]
			add ebx, edx
			sub ebx, '0'
			inc ecx
			cmp ecx, eax
			jl rec1
			end1 :
		//mov res, ebx

			rdtsc
			sub eax, [esp];
		sub edx, [esp + 4]
			add esp, 8
		mov res, eax
	}
	cout << "resAtoi= " << res << endl;
}

int myAtoi_asm_v2(char *str)
{
	int time = 0;
	int res = 0;
	_asm {
		rdtsc
			push edx
			push eax

			push str
			call atoi
			add esp, 4
			mov res, eax


			rdtsc
			sub eax, [esp];
		sub edx, [esp + 4]
			add esp, 8
			mov time, eax
	}
	cout << "time atoi2 = " << time << endl;
	return res;
}

int main()
{
	printf("mov:     %d\n", measure_mov());
	printf("mul:     %d\n", measure_mul());
	myAtoi("666");
	cout << "atoi: " << myAtoi_asm("1234") << endl;
	cout << "atoi2: " << myAtoi_asm_v2("1234") << endl;
	return 0;
}

