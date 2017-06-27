/**
 * @file Socket.hpp
 * @brief Socket abstract utils header file
 * @author StealthTech
 * @project Recast-client
 * @date 27.06.17
 * @email st3althtech@mail.ru
 *
 **/

#ifndef RECAST_SERVER_SOCKET_HPP
#define RECAST_SERVER_SOCKET_HPP

#include "network/NetworkUtils.hpp"

class Socket {
public:
    Socket()       : socketDescr(-1) {}
    Socket(int sd) : socketDescr(sd) {}
    ~Socket() { if (socketDescr > 0) ::close(socketDescr); }
public:
    int  getSocketDescr() const noexcept { return socketDescr; }
    void setNonBlocked(bool option);
    void close() { ::close(socketDescr); }
protected:
    void setReuseAddress(int sd);
    int socketDescr;
};

#endif //RECAST_SERVER_SOCKET_HPP
