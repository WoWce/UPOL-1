#include <stdlib.h>
#include <sys/stat.h>
#include <unistd.h>
#include <cstring>
#include <string>
#include <cmath>
#include <cstdio>
#include <errno.h>
#include <fcntl.h>
#include "iostream"
using namespace std;

#define PIPE_IN "/tmp/AccumulatorR"
#define PIPE_OUT "/tmp/AccumulatorW"
#define BUFFSIZE 512



int main()
{
    int accumulator = 0;
	// Create the pipes
	int pipeIn = mkfifo(PIPE_IN, S_IRUSR | S_IWUSR);
	int pipeOut = mkfifo(PIPE_OUT, S_IRUSR | S_IWUSR);

	pipeIn = open(PIPE_IN, O_RDWR);
	pipeOut = open(PIPE_OUT, O_RDWR);

	if((pipeIn == -1) || (pipeOut == -1))
	{
		cout << "Unable to start server" << endl;
		cout << strerror(errno) << endl;
	}
	else
	{
		while(true)
		{
			size_t count;
            int number;
			// Receive message from client
			char buff[BUFFSIZE];
			count = read(pipeIn, &buff, BUFFSIZE);
			buff[count] = '\0';
			string input(buff);
            char output[BUFFSIZE];
			if(input[0] != '+' && input[0] != '-' && input[0] != '*' && input[0] != '/'){
                break;
            }

			
			switch(input[0])
			{
                case '+':
                    number = atoi(input.substr(1).c_str());
                    accumulator += number;  
                    break;
                case '-':
                    number = atoi(input.substr(1).c_str()); 
                    accumulator -= number;  
                    break;
                case '*':
                    number = atoi(input.substr(1).c_str()); 
                    accumulator *= number;
                    break;
                case '/':
                    number = atoi(input.substr(1).c_str()); 
                    accumulator /= number;
                    break;
                default:
                    break;
            }
                
            sprintf(output, "%d", accumulator);

			// Send message to client
			count = write(pipeOut, output, strlen(output));
		}
	}

	// Close the pipe
	unlink(PIPE_IN);
	unlink(PIPE_OUT);

	cout << "Quiting server" << endl;
	return 0;
}