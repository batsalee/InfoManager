IMServer: main.o database.o error.o network.o
	gcc -o IMServer main.o database.o error.o network.o -I/usr/include/mysql -L/usr/lib -lmysqlclient

main.o: main.c
	gcc -c main.c

database.o: database.c
	gcc -c database.c -I/usr/include/mysql -L/usr/lib -lmysqlclient 

error.o: error.c
	gcc -c error.c


network.o: network.c
	gcc -c network.c

clean:
	rm -f IMServer *.o
