// winthreads.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <windows.h>
#include <assert.h>
#include <fstream>
#include <string>
#include "iostream"
using namespace std;


char Filtr[30];
struct Param { 
	const char *soubor, *filtr;
} argum1 = { "Jmena1", Filtr }, argum2 = { "Jmena2", Filtr };;

DWORD WINAPI hledat(CONST LPVOID param) {
	int counter = 0;
	Param arg = *((Param*)param);
	string filename = arg.soubor;
	ifstream File("..\\" + filename);
	string line;
		while (getline(File, line))
		{
			string lowS;
			string lowF;
			string filtr(arg.filtr);
			for (int i = 0; i < line.length(); i++) {
				lowS += tolower(line[i]);
			}
			for (int i = 0; i < filtr.length(); i++) {
				lowF += tolower(filtr[i]);
			}
			if (lowS.find(lowF) == 0) {
				counter++;
			}
			
		}
		ExitThread(counter);
}

DWORD WINAPI ThreadFunc(Param* lpParam) {
	hledat(lpParam);
	return 0;
}



int main()
{
	HANDLE h[2];
	cout << "Filtr jmen: ";
	cin >> Filtr;
	Param argum1 = { "Jmena1", Filtr }; 
	Param argum2 = { "Jmena2", Filtr };

	h[0] = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)hledat, &argum1, 0, NULL);


	h[1] = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)hledat, &argum2, 0, NULL);
	WaitForMultipleObjects(2, h, TRUE, INFINITE);

	assert(h[0] && h[1]);

	DWORD result1;
	GetExitCodeThread(h[0], &result1);

	DWORD result2;
	GetExitCodeThread(h[1], &result2);
	cout << "Vyskyty: "<< result1 << ", " << result2 << endl;
    return 0;
}

