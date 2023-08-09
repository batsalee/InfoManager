#ifndef DATABASE_H
#define DATABASE_H

typedef struct result_DB
{
	char Name[30];
	char Start[30];
	char Arrive[30];
	char LCode[30];
	char EStart[30];
	char EArrive[30];
}RESULT_DB;

RESULT_DB search_DB(const char* RFIDTag);
void get_now_time(char* nt);

#endif
