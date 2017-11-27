// 6Semaphore.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <windows.h>
#include <assert.h>
#include <fstream>
#include <string>
#include "iostream"
using namespace std;


HANDLE h[2];

string foundName;
bool eof;
struct Param
{
	string soubor;
	HANDLE thisSemafor;
	HANDLE otherSemafor;
	string filtr;
};

DWORD WINAPI printName(CONST LPVOID lpParam) {
	Param arg = *((Param*)lpParam);
	while (true) {
		WaitForSingleObject(arg.thisSemafor, INFINITE);
		if (eof) break;
		cout << foundName << " ";
		ReleaseSemaphore(arg.otherSemafor, 1, NULL);
	}
	ExitThread(0);
}

DWORD WINAPI findName(CONST LPVOID lpParam) {
	int resultCount = 0;
	Param arg = *((Param*)lpParam);
	ifstream file(arg.soubor);
	string str;
	while (getline(file, str))
	{
		string lowStr;
		string lowFiltr;
		string filtr(arg.filtr);
		for (int i = 0; i < str.length(); ++i)
			lowStr += tolower(str[i]);
		for (int i = 0; i < filtr.length(); ++i)
			lowFiltr += tolower(filtr[i]);
		if (lowStr.find(lowFiltr) == 0) {
			resultCount++;
			WaitForSingleObject(arg.thisSemafor, INFINITE);
			foundName = str;
			ReleaseSemaphore(arg.otherSemafor, 1, NULL);
		}
	}
	file.close();
	eof = true;
	ReleaseSemaphore(arg.otherSemafor, 1, NULL);
	ExitThread(resultCount);
}


int main()
{
	
	while (true)
	{
		foundName = "";
		eof = false;
		string filtr;
		cout << "Enter filtr: ";
		cin >> filtr;
		HANDLE SJ = CreateSemaphore(NULL, 0, 1, NULL);
		HANDLE SV = CreateSemaphore(NULL, 1, 1, NULL);

		Param p1 = { "Jmena", SV, SJ, filtr };
		Param p2 = { "", SJ, SV, "" };

		HANDLE h[2];
		h[0] = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)findName, &p1, CREATE_SUSPENDED, NULL);
		h[1] = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)printName, &p2, CREATE_SUSPENDED, NULL);
		ResumeThread(h[0]);
		ResumeThread(h[1]);

		WaitForSingleObject(h[0], INFINITE);
		WaitForSingleObject(SV, INFINITE);

		assert(h[0] && h[1]);

		DWORD res1;
		GetExitCodeThread(h[0], &res1);
		cout << endl << "Names count: " << res1 << endl;

		CloseHandle(h[0]);
		CloseHandle(h[1]);
		CloseHandle(SJ);
		CloseHandle(SV);
	}
	return 0;
}