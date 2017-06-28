/**
 * @file networking.cpp
 * @brief Basic quasi-server source file
 * @author StealthTech
 * @project Recast-client
 * @date 25.06.17
 * @email st3althtech@mail.ru
 *
 **/

#include "network/Networking.hpp"

using namespace std;

int main() {
//    Usage example

    SocketTCP sock;
    sock.connect("localhost", DEFAULT_PORT_TCP);
    sock.send("hey");
    string buffer = sock.recvTimed(3);
    cout << "TCP: " << buffer << endl;

    SocketUDP sockd;
    sockd.setReceiver("localhost", DEFAULT_PORT_UDP);
    sockd.sendTo("hello");
    buffer = sockd.recvFrom();
    cout << "UDP: " << buffer << endl;

    return 0;
}