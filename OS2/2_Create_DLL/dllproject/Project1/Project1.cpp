// Project1.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include <iostream>
using namespace std;

#define DLL extern __declspec(dllexport)
//DLL file functions
DLL void draw(int d, char z1, char z2) {
	for (int i = 0; i < d; i++) {
		for (int j = 0; j < d; j++)
		{
			if (i == 0 || j == 0 || i == d - 1 || j == d - 1) {
				cout << z1;
			}
			else cout << z2;
		}
		cout << endl;
	}
}

DLL void draw2(int p, int h, int w, char z) {
	int height = p * w;
	int width = p * h;
	for (int i = 0; i <= height; i++) {
		for (int j = 0; j <= width; j++)
		{
			if (i == 0 || j == 0 || i % p == 0 || j % p == 0 || i == height || j == width) {
				cout << z;
			}
			else cout << " ";
		}
		cout << endl;
	}
}

