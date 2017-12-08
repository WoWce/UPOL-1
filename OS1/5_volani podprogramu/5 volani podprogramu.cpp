// 5 volani podprogramu.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "iostream"
using namespace std;

void factorial_iter(unsigned char a, unsigned long *b)
{
	if (a <= 1) return;
	*b *= a;
	factorial_iter(a - 1, b);
}

unsigned long factorial(unsigned char a)
{
	unsigned long ret = 1, *pret = &ret;

	_asm {
		push dword ptr pret
			push dword ptr a
			call factorial_iter
			add esp, 8
	}
	return ret;
}

char *my_strdup(char *s) {
	
	_asm {
			
			push s
			call strlen
			add esp, 4
			
			add eax, 1
			mov edx, eax
			push eax
			call malloc
			add esp, 4
			mov eax, s
			
	}
	
}

void print_fact(unsigned char n) {
	char *a = "fact(%d) = %d, ";
	_asm {
		push n
		call factorial
		add esp, 4

		push eax
		push n
		push a
		call printf

		add esp, 12
		

	}
	//cout << a;
}

void print_facts(unsigned char n) {
	
	_asm {
			movsx edx, n
			mov bl, 0
			mov ebx, 0
		loop1:
			push ebx
			call print_fact
			add esp, 4
			inc bl
			cmp bl, n
			jl loop1
		end1:
	}
}

char *abcs(unsigned char n) {
	char* s = "oops";
	
	_asm {
			mov edx, 1
			imul edx, n 
			cmp edx, 26
			jle next1
		oops:
			mov edx, 5
			push edx
			call malloc
			add esp, 4
			push s
			push eax
			call strcpy
			add esp, 8
			jmp exit1

		next1:
			mov ebx, 4
			imul ebx, n
			push edx
			call malloc
			add esp, 4
			mov edx, eax

			mov ecx, 0
			mov ebx, 65
			mov edx, 1
			imul edx, n
		loop1:
			cmp ecx, edx
			jge end1
			mov [eax + 1 * ecx], bl
			inc ecx
			inc ebx
			jmp loop1
		end1:
			mov byte ptr [eax + 1 *ecx], '\0'
		exit1:

	}
}

/*void read_and_print_fib() {
	unsigned char n;
	_asm {

	}
}*/

int main()
{
	char* s = "as long as you want this!";
	printf("%s", abcs(26));
	cout << endl;
	print_fact(4);
	cout << endl;
	print_facts(5);
	cout << endl;
	printf("%s", my_strdup(s));
	cout << endl;
	//int n;
	//scanf("%d", &n);
	//cout << n;
	return 0;
}

