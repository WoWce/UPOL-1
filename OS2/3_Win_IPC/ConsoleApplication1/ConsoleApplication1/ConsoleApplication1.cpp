// ConsoleApplication1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <Windows.h>
#include "iostream"
using namespace std;

int main(int argc, char *argv[])
{
	cout << "Start" << endl;
	int start = GetTickCount();
	int waiting = atoi(argv[0]);
	int end = start + waiting;
	int Time = 0;
	int actual = 0;
	while (GetTickCount() < end) {
		actual = GetTickCount();
		
		Sleep(100);

		Time += GetTickCount() - actual;
		cout << "ms:" << Time << endl;
	}
	system("pause");
    return 0;
}

