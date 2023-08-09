#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <time.h>
#include "database.h"
#include "mysql.h"
#include "error.h"

RESULT_DB search_DB(const char* RFIDTag)
{
	MYSQL conn_ptr;
	MYSQL_RES* res;
	MYSQL_ROW row;

	char query[512];
	char now_time[6] = "10000"; // default 월요일 0시 0분

	RESULT_DB res_DB = {"DB값없음", "DB값없음", "DB값없음", "0000000000", "DB값없음", "DB값없음"};	// default 값

	mysql_init(&conn_ptr);	
	
	// db에서 한글 가져오는 것 처리
	mysql_options(&conn_ptr, MYSQL_SET_CHARSET_NAME, "utf8");
	mysql_options(&conn_ptr, MYSQL_INIT_COMMAND, "SET NAMES utf8");

	if (!mysql_real_connect(&conn_ptr, "localhost", "root", "IMSQL", "IMSQL", 3306, NULL, 0))
		error_print("db connect error");

	get_now_time(now_time);

	sprintf(query,
		"SELECT c.NAME, l.START, l.ARRIVE, lc.LCode, s.EName, d.EName "		
		"FROM CLIENT c JOIN LOCATION l ON c.TID=l.TID "
		"JOIN LOCATION_CODE lc ON l.ARRIVE=lc.LName "
		"JOIN LOCATION_ENAME s ON l.START=s.KName "
		"JOIN LOCATION_ENAME d ON l.ARRIVE=d.KName "
		"WHERE c.TID=%s AND %s BETWEEN l.FROMTIME AND l.UNTILTIME",
		RFIDTag, now_time);

	if (mysql_query(&conn_ptr, query)) {
		mysql_free_result(res);
		mysql_close(&conn_ptr);
		return res_DB;
	}

	res = mysql_store_result(&conn_ptr);

	row = mysql_fetch_row(res);

	if(row==NULL) return res_DB;	// DB에 값이 없을 때

	strcpy(res_DB.Name, row[0]);
	strcpy(res_DB.Start, row[1]);
	strcpy(res_DB.Arrive, row[2]);
	strcpy(res_DB.LCode, row[3]);
	strcpy(res_DB.EStart, row[4]);
	strcpy(res_DB.EArrive, row[5]);

	mysql_free_result(res);
	mysql_close(&conn_ptr);

	return res_DB;
}

void get_now_time(char* nt)
{
	// tm_wday = 1:월요일, 2: 화요일, ... , 7:일요일
	// tm_hour = 0시~23시
	// tm_min = 0분~59분
	
	char timestamp[6] = {0};

	time_t timer;
	struct tm *t;

	timer = time(NULL);	// 현재 시각을 초 단위로 얻기
	t = localtime(&timer);	// 초 단위의 시간을 분리하여 구조체에 넣기

	if(t->tm_wday==0) t->tm_wday=7;	// 데이터베이스 저장을 편리하게 하기 위해 일요일을 7로

	// 만약 현재 시각, 현재 분이 한자리수인 경우 두자리로 맞춰주기 위해
	if(t->tm_hour<10 && t->tm_min<10)
		sprintf(timestamp, "%d0%d0%d", t->tm_wday, t->tm_hour, t->tm_min);
	else if(t->tm_hour<10)
		sprintf(timestamp, "%d0%d%d", t->tm_wday, t->tm_hour, t->tm_min);
	else if(t->tm_min<10)
		sprintf(timestamp, "%d%d0%d", t->tm_wday, t->tm_hour, t->tm_min);
	else
		sprintf(timestamp, "%d%d%d", t->tm_wday, t->tm_hour, t->tm_min);

	strcpy(nt, timestamp);
}
