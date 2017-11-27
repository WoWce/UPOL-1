// dllproject.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
using namespace std;
#define DLL extern __declspec(dllimport)
DLL void draw(int, char, char);
DLL void draw2(int, int, int, char);

int main()
{
	draw(5, '#', ':');
	draw2(5, 4, 3, '*');
    return 0;
}

