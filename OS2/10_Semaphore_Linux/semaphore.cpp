#include <string.h>
#include <pthread.h>
#include <fstream>
#include <semaphore.h>
#include "iostream"
using namespace std;

sem_t semaphore;
sem_t semaphore2;
int primeNum = -1;
bool prime;
struct Param
{
    int number;
    int count;
};

void* printNumber(void* lpParam) {
    //cout << "print it!" << endl;
    Param *arg = (Param*)lpParam;
    int printNum;
    int printCount = 0;
	while (true) {
        sem_wait(&semaphore);
        printNum = primeNum;
        if(primeNum == 0) break;
        sem_post(&semaphore2);
        cout << printNum << ", ";
        printCount++;
        if(printCount % 10 == 0){
            cout << endl;
        }
    }
    return (void*)printCount;
}

void* findNumber(void* lpParam) {
	int resultCount = 0;
    Param *arg = (Param*)lpParam;
    for (int i=2; i<arg->number; i++) 
    {
        bool prime=true;
        for (int j=2; j*j<=i; j++)
        {
            if (i % j == 0) 
            {
                prime=false;
                break;    
            }
        }   
        if(prime) {
            resultCount++;
            sem_wait(&semaphore2);
            primeNum = i;
            sem_post(&semaphore);
        }
    }
    sem_wait(&semaphore2);
    primeNum = 0;
	sem_post(&semaphore);
}


int main()
{
	
	while (true)
	{
        int number;
        void* count;
		prime = false;
		cout << "Enter max: ";
        cin >> number;
        if(number <= 1) break;
        if (sem_init(&semaphore, 0, 0) == -1)
        { printf("sem_init: failed1"); }
        if (sem_init(&semaphore2, 0, 1) == -1)
        { printf("sem_init: failed2"); }

		Param p1 = { number, 0 };
		Param p2 = { number, 0 };

        pthread_t thread1, thread2;
        pthread_create(&thread1, NULL, findNumber, (void*) &p1);
        pthread_create(&thread2, NULL, printNumber, (void*)&p2);

		pthread_join(thread1, NULL);
        pthread_join(thread2, &count);
        
		cout << endl << "Print count: " << *((int*)&count) << endl;

	}
	return 0;
}