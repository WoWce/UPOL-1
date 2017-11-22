#include <iostream>
#include <fcntl.h>
#include <unistd.h>
#include <cstring>
#include <errno.h>

using namespace std;

#define PIPE_IN "/tmp/AccumulatorW"
#define PIPE_OUT "/tmp/AccumulatorR"
#define BUFFSIZE 512


int main()
{
	int in = open(PIPE_IN, O_RDWR);
	int out = open(PIPE_OUT, O_RDWR);
	if((in == -1) || (out == -1))
	{
		cout << "Unable to connect" << endl;
		cout << strerror(errno) << endl;
	}
	else
	{
		while(true)
		{
			string input;
            size_t count;
            
			cout << ">> ";
            cin >> input;
            
			//Send
            count = write(out, input.c_str(), input.length());
            //if not -,+,*,/
			if(input[0] != '+' && input[0] != '-' && input[0] != '*' && input[0] != '/'){
                break;
            }
			// Get response
			char buff[BUFFSIZE];
			count = read(in, &buff, BUFFSIZE);
			buff[count] = '\0';
			input = buff;
			if(input.length())
			{
				cout << input << endl;
			}
		}
	}
	return 0;
}