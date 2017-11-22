#include "iostream"
#include <pthread.h>
#include <fstream>
using namespace std;

char F1;
char F2;
typedef struct Param
{
	string file;
	char Filtr1;
	char Filtr2;
	int result;
} param;

void *search(void *arg)
{
	Param *param = (Param*) arg;

	ifstream file (param->file.c_str(), ios::in | ios::binary | ios::ate);
	if(file.is_open())
	{
		int end=file.tellg();
		file.seekg(0);

		int counter = 0;
		char n;
		char* name;
		while(file.tellg()!=end)
		{
			bool firstLetter;
			bool lastLetter;
			file.read((char *)&n, sizeof n);
			char buffer[(int)n];
			file.read(buffer, (int)n);
			if(param->Filtr1 == buffer[0] && param->Filtr2 == buffer[(int)n - 1]){
				counter++;
			}
			
		}
		param->result = counter;
	}
	
	//pthread_exit();
}

int main(){

	int n;
	while(n != 0)
	{
		cout << "First: ";
		cin >> F1;
		cout << "Next: ";
		cin >> F2;
		Param param1 = {"/home/michael/Desktop/Jmena1b", F1, F2, 0};
		Param param2 = {"/home/michael/Desktop/Jmena2b", F1, F2, 0};

		pthread_t thread1, thread2;

		pthread_create(&thread1, NULL, search, (void*) &param1);
		pthread_create(&thread2, NULL, search, (void*) &param2);

		pthread_join(thread1, NULL);
		pthread_join(thread2, NULL);

		cout << "Jmena1b count: " << param1.result << endl;
		cout << "Jmena2b count: " << param2.result << endl;
	}

	return 0;
}