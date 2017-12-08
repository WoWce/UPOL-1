// 6 struktury.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <stddef.h>
#include "iostream"
using namespace std;

struct foo {
	char* name;
	unsigned int price;
	unsigned int count;
};

struct foo bar[10];
int iter = 0;

void receipt_add(char* name, unsigned int price, unsigned int count) {
	struct foo a;
	a.name = name;
	a.price = price;
	a.count = count;
	bar[iter] = a;
	iter++;
}

void receipt_add_asm(char* name, unsigned int price, unsigned int count) {
	_asm {
		
		mov ecx, iter
		imul ecx, 12
		mov eax, name
		mov [bar + ecx], eax

		mov eax, price
		mov [bar + ecx + 4], eax
		mov eax, count
		mov [bar + ecx + 8], eax

		mov eax, iter
		mov edx, 12
		imul edx

		inc iter
	}
}

void receipt_print() {
	for (int i = 0; i < iter; i++)
	{
		cout << bar[i].name << " " << bar[i].price << " " << bar[i].count << endl;
	}
}

void receipt_print_asm(){
	char *format = "%s - %d - %d\n";
	_asm {
			mov ebx, 0
		for:
			cmp ebx, iter
			jae end1

			mov eax, 12
			mul ebx

			mov esi, [offset bar]
			movsx edx, [esi + eax + 8]
			push edx
			push  [esi + eax + 4] 
			push[esi + eax + 0] 

			push dword ptr format
			call dword ptr[printf]
			add esp, 16

			inc ebx
			jmp for

			end1:
	}
}

int receipt_total() {
	int sum = 0;
	for (int i = 0; i < iter; i++)
	{
		sum += bar[i].price;
	}
	return sum;
}

int receipt_total_asm() {
	
	_asm {
		mov eax, 0
		mov edx, 0
	loop1:
		mov ecx, 12
			imul ecx, edx
			add eax, dword ptr [bar + 4 + ecx]
			inc edx
			cmp edx, iter
			jl loop1
	}


}

int divide_asm(int a, int b) {
	_asm {
		mov eax, [ebp + 12]

	}
}

int main()
{
	receipt_add_asm("one", 20, 1);
	receipt_add_asm("two", 30, 1);
	receipt_print_asm();
	cout << receipt_total_asm() << endl;
	cout << divide_asm(10, 12) << endl;
	//cout << sizeof foo2;
	//printf("%i\n", offsetof(struct foo2, b));
    return 0;
}

