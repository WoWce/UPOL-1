// ex5.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <map>
#include "iostream"
using namespace std;

struct Prechod {
	int soucasnyStav;
	char znak;
	int nasledujiciStav;
};

const Prechod ABBA[] = { { 1,'A',2 },{ 1,'B',1 },{ 2,'A',2 },{ 2,'B',3 },{ 3,'A',2 },{ 3,'B',4 },
{ 4,'A',-5 },{ 4,'B',1 },{ -5,'A',-5 },{ -5,'B',-5 } };

const unsigned POCET = sizeof ABBA / sizeof *ABBA;

const char *vstup1 = "ABBBAABBABAB";

const char *vstup2 = "ABABAAABBBA";

bool runAutomat(map<pair<int, char>, int> automat, const char *input, int start) {
	int actual = start;
	for (int i = 0; i < strlen(input); i++)
	{
		actual = automat[make_pair(actual, input[i])];
	}
	return actual < 0;
}

int main()
{
	map<pair<int, char>, int> automat;
	for (auto x : ABBA) {
		automat[make_pair(x.soucasnyStav, x.znak)] = x.nasledujiciStav;
	}

	cout << vstup1 << ": " << (runAutomat(automat, vstup1, 1) ? "prijat" : "neprijat") << endl;
	cout << vstup2 << ": " << (runAutomat(automat, vstup2, 1) ? "prijat" : "neprijat") << endl;

    return 0;
}

