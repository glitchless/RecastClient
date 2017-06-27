/**
 * @file SocketUDP.hpp
 * @brief Socket (UDP) utils header file
 * @author StealthTech
 * @project Recast-client
 * @date 27.06.17
 * @email st3althtech@mail.ru
 *
 **/

#ifndef RECAST_SERVER_SOCKET_UDP_HPP
#define RECAST_SERVER_SOCKET_UDP_HPP

#include "network/NetworkUtils.hpp"
#include "network/Socket.hpp"

class SocketUDP : public Socket {
public:
    using Socket::Socket;
public:
    void setReceiver(const std::string &host, int port);

    void sendTo(const std::string &str);
    std::string recvFrom();

    void sendBytesTo(const char *data, size_t num);
    char* recvBytesFrom();
public:
    struct sockaddr_in connAddrCurrent;
};

#endif //RECAST_SERVER_SOCKET_UDP_HPP
