#include <stdio.h>
#include <stdlib.h>
#include "network.h"

/*****************************************************************
파일명 : main.c
실행명령 : ./IMServer 1105
프로그램 흐름 :
	network_inet() -> 각 쓰레드별로 network_communicate() 
	-> search_DB() -> network_communicate() -> network_close()
*****************************************************************/

int main(int argc, char** argv)
{
	// 서버프로그램 실행시 "실행파일명 포트번호" 형식으로 실행하도록
	// argv[0] : 실행파일명, argv[1] : linuxPort, argv[2] : winIP, argv[3] : winPort
	if(argc!=2) { 
		printf("Usage : %s <port>\n", argv[0]);
		exit(1);
	}
	network_init(argv[1]);
	network_close();	
	
	return 0;
}
