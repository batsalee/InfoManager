#ifndef NETWORK_H
#define NETWORK_H

void network_init(const char* portNo);
void* network_communicate(void* arg);
void network_close();

#endif
