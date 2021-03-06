// ex3.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "Jmena13"
#include <string>
#include <list>
#include "iostream"
using namespace std;


void printlist(list<string> jmena) {
	for (auto it = jmena.begin(); it != jmena.end(); ++it) {
		cout << *it << endl;
	}
}

int main()
{
	list<string> jmena;
	for (auto j : Jmena) {
		jmena.push_back(j);
	}
	jmena.sort();
	
	//printlist(jmena);
	cout << endl;
	
	for (int i = 0; i < PocetN; i++) {
		bool found = false;
		for (auto j : jmena) {
			const char* name = j.c_str();
			if (strcmp(name, JmenaN[i]) == 0) {
				cout << "Found: " << j << endl;
				found = true;
			}
		}
		if (!found) cout << "Not found: " << JmenaN[i] << endl;
	}
	cout << endl;
	for (int i = 0; i < PocetZ; i++) {
		for (auto j : jmena) {
			const char* name = j.c_str();
			//for using strcmpi function don't forget to turn off SDL checks (Project>Properties>C/C++>General>SDL checks > choose NO)
			if (strcmpi(name, JmenaZ[i]) == 0) {
				jmena.remove(j);
				break;
			}
		}
	}
	//printlist(jmena);
	cout << endl;
	for (int i = 0; i < PocetZN; i++) {
		bool found = false;
		for (auto j : jmena) {
			const char* name = j.c_str();
			if (strcmpi(name, JmenaZN[i]) == 0) {
				cout << "Found: " << j << endl;
				found = true;
			}
		}
		if (!found) cout << "Not found: " << JmenaN[i] << endl;
	}
	//printlist(jmena);
    return 0;
}

