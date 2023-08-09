#include <sys/socket.h>	// socket
#include <string.h>		// memset
#include <stdlib.h>
#include <arpa/inet.h>	// bind
#include <sys/uio.h>	// iovec
#include <stdio.h>
#include <fcntl.h>

#include "network.h"
#include "database.h"
#include "error.h"

int serv_sock;
int clnt_sock;

void network_init(const char* portNo)
{
	struct sockaddr_in serv_addr;
	struct sockaddr_in clnt_addr;

	socklen_t clnt_addr_size;	// accpet()함수에 사용할 clnt_addr의 크

	// 1. socket() 함수 호출
	serv_sock = socket(PF_INET, SOCK_STREAM, 0);
	if (serv_sock == -1) error_print("socket() error");

//
	fputs("서버소켓 오픈\n", stderr);

	// bind() 함수에 사용할 주소정보
	memset(&serv_addr, 0, sizeof(serv_addr));
	serv_addr.sin_family = AF_INET;
	serv_addr.sin_addr.s_addr = htonl(INADDR_ANY);
	serv_addr.sin_port = htons(atoi(portNo));

	// 2. bind() 함수 호출
	if (bind(serv_sock, (struct sockaddr*)&serv_addr, sizeof(serv_addr)) == -1)
		error_print("bind() error");

	// 3. listen() 함수 호출
	if (listen(serv_sock, 15) == -1)
		error_print("listen() error");
	
	while(1) {
		// 4. accept() 함수 호출
		clnt_addr_size = sizeof(clnt_addr);
		clnt_sock = accept(serv_sock, (struct sockaddr*)&clnt_addr, &clnt_addr_size);
		if (clnt_sock == -1)
			error_print("accept() error");


//
	fputs("윈도우서버와 연결 성공\n", stderr);

		char RFIDTag[10] = { 0 };
	
		// WinServer에게서 RFIDTag값 받기
		ssize_t nRecv = recv(clnt_sock, RFIDTag, sizeof(RFIDTag) - 1, 0);
		if (nRecv == 0)
			error_print("recv() error");
	

//
	fputs("태그값 받았음\n", stderr);

		// DB에서 RFIDTag값의 정보 얻기
		RESULT_DB res_DB = search_DB(RFIDTag);
	
		// Windows Server로 사용자 정보 전달하기
		struct iovec vec[6];
		vec[0].iov_base = res_DB.Name;
		vec[0].iov_len = strlen(res_DB.Name)+1;
		vec[1].iov_base = res_DB.Start;
		vec[1].iov_len = strlen(res_DB.Start)+1;
		vec[2].iov_base = res_DB.Arrive;
		vec[2].iov_len = strlen(res_DB.Arrive)+1;
		vec[3].iov_base = res_DB.LCode;
		vec[3].iov_len = strlen(res_DB.LCode)+1;
		vec[4].iov_base = res_DB.EStart;
		vec[4].iov_len = strlen(res_DB.EStart)+1;
		vec[5].iov_base = res_DB.EArrive;
		vec[5].iov_len = strlen(res_DB.EArrive)+1;
	
		// Windows Server에 사용자정보 전송
		writev(clnt_sock, vec, 6);
	
//
		fputs("전송 끝", stderr);

		shutdown(clnt_sock, SHUT_WR);
	}
}

void network_close()
{
	close(serv_sock);
	close(clnt_sock);
}
