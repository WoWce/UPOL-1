// 1 c-opakovani.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "iostream";
using namespace std;

typedef struct seznam {
	char* name;
	int age;
	seznam *next = nullptr;
};

void seznam_add(seznam *prvek, seznam *list) {
	seznam *current = list;
	while (current->next != NULL) {
		current = current->next;
	}
	current->next = prvek;
	prvek->next = nullptr;
}

void seznam_print(seznam *root) {
	seznam *current = root;
	while (current->next != nullptr) {
		cout << "Jmeno: " << current->name << endl;
		cout << "Vek: " << current->age << endl;
		current = current->next;
		
	}
	cout << "Jmeno: " << current->name << endl;
	cout << "Vek: " << current->age << endl;
}

void int2bits(char *ch, int a) {
	int b = a;
	for (int i = 0; i < 32; i++) {
		if (b & 0x80000000)
			ch[i] = '1';
		else
			ch[i] = '0';
		b <<= 1;
	}
	ch[32] = '\0';
}

int bits2int(char *a) {
	int b = 0;
	int len = strlen(a);
	for (int i = len; i > 0; i--) {
		if (a[i] == '1') {
			b = b | (1 << (len-1-i));
		}
	}
	return b;
}

void my_memset(void *dest, unsigned char c, size_t count) {
	
	unsigned char *end = (unsigned char*)dest + count;
	unsigned char *destPtr = (unsigned char *)dest;
	while (destPtr != end) {
		*destPtr = c;
		*destPtr++;
	}
}

void vypis(int p1,int p2,int p3, int p4) {
	cout << p1 << " " << p2 << " " << p3 << " " << p4 << endl;
}

int main()
{
	char buf[33];
	int2bits(buf, 42);
	cout << buf << endl;

	int x = bits2int(buf);
	cout << x << endl;
	
	char str[] = "almost every programmer should know memset!";
	my_memset(str, '-', 6);
	cout << str << endl;

	seznam *one = (seznam*)malloc(sizeof(seznam));
	seznam *two = (seznam*)malloc(sizeof(seznam));
	seznam *thr = (seznam*)malloc(sizeof(seznam));
	seznam *four = (seznam*)malloc(sizeof(seznam));
	//root
	one->name = "Ann";
	one->age = 20;
	one->next = nullptr;

	two->name = "Ben";
	two->age = 22;
	thr->name = "John";
	thr->age = 25;
	four->name = "Stew";
	four->age = 21;
	
	
	seznam_add(two, one);
	seznam_add(thr, one);
	seznam_add(four, one);
	seznam_print(one);

	int g = 0;
	vypis(g++, 2, g++, 5);

    return 0;
}

