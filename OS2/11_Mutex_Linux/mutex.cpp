#include <pthread.h>
#include <unistd.h>
#include <stdio.h>
#include "iostream"
using namespace std;

pthread_mutex_t lock;
time_t  timev;
time_t lasttime;
bool exit = false;
int printCounter = 0;
void* printTime(void* lpParam) {
    while(true){
        pthread_mutex_lock(&lock);
        if(timev != lasttime){
            struct tm * timeinfo;
            timeinfo = localtime (&timev);
            cout << timeinfo->tm_hour << ":" << timeinfo->tm_min << ":" << timeinfo->tm_sec << endl;
            lasttime = timev;
            printCounter = 1;
        } else if(printCounter < 5){
            
            struct tm * timeinfo;
            timeinfo = localtime (&timev);
            cout << timeinfo->tm_hour << ":" << timeinfo->tm_min << ":" << timeinfo->tm_sec << endl;
            lasttime = timev;
            printCounter++;
        } else{
            printCounter = 0;
            exit = true;
            pthread_mutex_unlock(&lock);
            break;
        }
        pthread_mutex_unlock(&lock);
    }
    
}

void* generateTime(void* lpParam) {
    while(true){
        pthread_mutex_lock(&lock);
        if(exit){
            exit = false;
            pthread_mutex_unlock(&lock);
            break;
        }
        time(&timev);
        pthread_mutex_unlock(&lock);
        usleep(50000);
    }
    
}


int main()
{
	
	while (true)
	{
       
		if (pthread_mutex_init(&lock, NULL) != 0)
        {
            printf("\n mutex init failed\n");
            return 1;
        }   
		//Param p1 = { number, 0 };
		//Param p2 = { number, 0 };

        pthread_t thread1, thread2;
        pthread_create(&thread1, NULL, generateTime, NULL);
        pthread_create(&thread2, NULL, printTime, NULL);

		pthread_join(thread1, NULL);
        pthread_join(thread2, NULL);
        cout << "Threads finished" << endl;
        pthread_mutex_destroy(&lock);

	}
	return 0;
}