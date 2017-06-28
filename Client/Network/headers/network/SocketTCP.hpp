/**
 * @file networking.cpp
 * @brief Socket (TCP) utils header file
 * @author StealthTech
 * @project Recast-client
 * @date 27.06.17
 * @email st3althtech@mail.ru
 *
 **/

#ifndef RECAST_SERVER_SOCKET_TCP_HPP
#define RECAST_SERVER_SOCKET_TCP_HPP

#include "network/NetworkUtils.hpp"
#include "network/Socket.hpp"

class SocketTCP : public Socket {
public:
    using Socket::Socket;
public:
    void connect(const std::string &host, int port);
    void send(const std::string &str);
    void sendBytes(const char *data, size_t num);
    std::string recv();
    std::string recv(size_t bytes);
    std::string recvTimed(int timeout);
    char* recvBytes(size_t num);
    bool hasData();
};

#endif //RECAST_SERVER_SOCKET_TCP_HPP
