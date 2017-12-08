// 7 aritmetika.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "iostream"
#include "bitset"
using namespace std;


/*void mask2(int x, int n, int* u1, int* u2) {
	_asm {
		mov eax, x
		mov edi, [u2]
		mov ecx, 0
		mov ebx, n
		mov esi, [u1]
		loop1:
			shr eax, 1
			jnc next1
			add ecx, 2
		next1:
			shl ecx, 1
			dec ebx
			cmp ebx, 0
			jg loop1
		mov dword ptr [esi], eax
		mov dword ptr [edi], ecx

	}
}*/

int add() {
	int i;
	int j;
	_asm {
		mov ebx, 0
		mov eax, 2147483648
		mov ecx, 2147483648
		mov edx, 0
		add eax, ecx
		adc ebx, edx
		mov i, eax
		mov j, ebx
	}
	cout << bitset<32>(j) << " " << bitset<32>(i) << endl;
}

void sub() {
	int i;
	int j;
	_asm {
			mov ebx, 1
			mov eax, 0
			mov ecx, 2147483648
			mov edx, 0
			sub eax, ecx
			sbb ebx, edx
			mov i, eax
			mov j, ebx
	}
	cout << bitset<32>(j) << " " << bitset<32>(i) << endl;
}

unsigned int roll(unsigned int r, unsigned n) {
	
		if((n &= 31) == 0) {
			return n;
		}
		return (r << n) | (r >> (32 - n));
	
}

int multiply(int n) {
	_asm {
		mov eax, n
		mov ebx, eax
		shl eax, 4
		shl ebx, 3
		add eax, ebx
		add eax, n
	}
}

unsigned int ror(unsigned int r, unsigned n) {

	if ((n &= 31) == 0) {
		return n;
	}
	return (r >> n) | (r << (32 - n));

}

/*unsigned char rotl(unsigned char c)
{
	return (c << 1) | (c >> 7);
}*/

unsigned int ror_a(unsigned int r, unsigned n) {
	_asm {
		mov ebx, n
		mov eax, r
			loop1:
		cmp ebx, 0
		je end1
		ror eax, 1
		dec ebx
		jmp loop1
			end1:

	}
}

unsigned int rol_a(unsigned int r, unsigned n) {
	_asm {
		mov ebx, n
			mov eax, r
			loop1 :
		cmp ebx, 0
			je end1
			rol eax, 1
			dec ebx
			jmp loop1
			end1 :

	}
}

struct arm_num { unsigned char ror4; unsigned char imm8; };

arm_num* decoded = (arm_num*)malloc(sizeof(arm_num));
void arm_decode_parts_asm(unsigned short n, struct arm_num *decoded) {
	int i;
	int j;
	_asm {
		movzx eax, word ptr[n]
		and eax, 0xFF
		mov ecx, dword ptr[decoded]
		mov byte ptr[ecx + 1], al
		movzx eax, word ptr[n]
		and eax, 0xF00
		sar eax, 8
		mov ecx, dword ptr[decoded]
		mov byte ptr[ecx], al

	}
}

void arm_decode_parts(unsigned short n, struct arm_num *decoded) {
	//int i = 0xFF & n;
	//char j = (0xF00 & n) >> 8;
	decoded->imm8 = 0xFF & n;
	decoded->ror4 = (0xF00 & n) >> 8;
}

int zero_extend(unsigned char imm8) {
	int i = 0;
	i = (int)imm8;
	return i;
}

unsigned int arm_decode_asm(unsigned short n) {
	int i = 0;
	_asm {
		mov eax, [decoded]
		push eax
		movzx ecx, word ptr[n]
		push ecx
		call arm_decode_parts_asm
		add esp, 8

		mov ecx, dword ptr [decoded]
		movzx eax, byte ptr [ecx + 1]
		mov ebx, 0
		mov bl, byte ptr [ecx]
		
		shl bl, 1
	loop1:
		cmp bl, 0
		je end1
		dec bl
		ror eax, 1
		jmp loop1
	end1:
		mov i, eax
	}
	return i;
	//cout << i << endl;
}

unsigned int arm_decode(unsigned short n) {
	arm_decode_parts_asm(n, decoded);
	int imm32 = ror_a((int)decoded->imm8, decoded->ror4 * 2);
	return imm32;
}

int arm_encodable(unsigned int n) {
	int m = n;
	for (int i = 0; i < 33; i++) {
		
		m = rol_a(n, i);
		if (((m >> 8) & 0xFFFFFF) == 0) {
			return i;
		}
		
		
	}
	return -1;
}

int arm_encodable_asm(unsigned int n) {
	int i = 0;
	_asm {
		mov ecx, 0
	loop1:
		mov eax, n
		inc ecx
		cmp ecx, 33
		
		jge end2

		mov ebx, ecx
		loop2:
			rol eax, 1
			dec ebx
			cmp ebx, 0
			jg loop2
		sar eax, 8
		and eax, 0xFFFFFF
		cmp eax, 0
		je end1
		jmp loop1


		end1:
		inc ecx
		mov i, ecx
		end2:
	}
	return i;
}

unsigned short arm_encode(unsigned int n) {
	if (arm_encodable(n) != 0) {
		int x = arm_encodable(n) - 1;
		unsigned short i = 0;
		i = rol_a(n, x);
		unsigned short j = (x / 2) << 8;
		i = (i | j) & 0xFFF;
		return i;
	}
	return 0;
}

unsigned short arm_encode_asm(unsigned int n) {
	_asm {
		push n
		call arm_encodable_asm
		add esp, 4

		mov ecx, eax
		cmp ecx, 0
		je end1
		dec ecx
		mov eax, n
		mov ebx, 0
	rec1:
		rol eax, 1
		inc ebx
		cmp ebx, ecx
		jl rec1

		shl ecx, 7
		or eax, ecx
		and eax, 0xFFF
		end1:

	}
}

int main()
{
	/*int* u1 = (int*)malloc(sizeof (int));
	int* u2 = (int*)malloc(sizeof(int));
	mask2(15, 3, u1, u2);
	cout << *u1 << " " << *u2 << endl;*/
	add();  cout << endl;
	sub(); cout << endl;
	cout<< roll(1, 31)<< endl;
	cout << multiply(5) << endl;
	cout << "ex6" << endl;

	cout << "encode: " << arm_encode_asm(772) << endl;
	cout << "decode: " << arm_decode_asm(4033) << endl;
	
	//704 3339, 204 3891
    return 0;
}

