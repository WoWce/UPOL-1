// ex4.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "iostream"
#include <algorithm>
#include <set>
using namespace std;

const int M1 = 1, M2 = 2, M5 = 5, M10 = 10, M20 = 20, M50 = 50;

const int pohyb[] = { M5,M5,M2,M2,M1,M50,M2,M5,M5,M5,-M2,M10,M50,M1,M1,-M5,-M5,-M5,M2,-M1,-M10,-M50 }; //M1=2 M2=3 M5=2 M50=1

const unsigned pohybPocet = sizeof pohyb / sizeof *pohyb; //used only for standard "for" loop

void printset(multiset<int> m) {
	for (auto t : m) {
		cout << t << ", " << endl;
	}
}

int main()
{
	multiset<int> s;

	for (auto x : pohyb) {
		if (x > 0) {
			s.insert(x);
		}
		
	}

	for (auto x : pohyb) {
		if (x < 0) {
			s.erase(s.find(-x));
		}
	}

	//printset(s);

	set<int> count;
	for (auto x : s) {
		if (s.count(x) > 0) {
			count.insert(x);
		}
	}

	

	for (auto x : count) {
		cout << "M" << x << " " <<s.count(x) << "x" << endl;
	}

    return 0;
}

