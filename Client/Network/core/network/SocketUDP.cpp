/**
 * @file SocketUDP.cpp
 * @brief Socket (UDP) utils header file
 * @author StealthTech
 * @project Recast-client
 * @date 27.06.17
 * @email st3althtech@mail.ru
 *
 **/

#include "network/SocketUDP.hpp"

using namespace std;

void SocketUDP::setReceiver(const string &host, int port) {
    connAddrCurrent = resolve(host.data(), port);

    if (socketDescr > 0) { ::close(socketDescr); }

    int sd = socket(PF_INET, SOCK_DGRAM, 0);
    if (sd <= 0) {
        throw runtime_error("[ERR] Creating UDP socket: " + string(strerror(errno)));
    }

    setReuseAddress(sd);
    socketDescr = sd;
}

void SocketUDP::sendTo(const string &str) noexcept (false) {
    if (socketDescr == -1) { throw runtime_error("[ERR] Sending failed (UDP):  Socket is not initialized"); }
    sendto(socketDescr, str.data(), str.size(), 0, (struct sockaddr *) &connAddrCurrent, sizeof(connAddrCurrent));
}

void SocketUDP::sendBytesTo(const char *data, size_t num) noexcept (false) {
    if (socketDescr == -1) { throw runtime_error("[ERR] Sending failed (UDP):  Socket is not initialized"); }
    sendto(socketDescr, data, num, 0, (struct sockaddr *) &connAddrCurrent, sizeof(connAddrCurrent));
}

string SocketUDP::recvFrom() noexcept (false) {
    if (socketDescr == -1) { throw runtime_error("[ERR] Sending failed (UDP):  Socket is not initialized"); }
    const size_t BUFFER_SIZE = 1024;
    ssize_t numBytes;
    socklen_t socketSize = sizeof(struct sockaddr_in);
    char *buffer = new char[BUFFER_SIZE];
    if ((numBytes = recvfrom(socketDescr, buffer, BUFFER_SIZE - 1, 0, (struct sockaddr*) &connAddrCurrent, &socketSize) == - 1)) {
        cerr << "[ERR] Recieve failed (recvfrom): " + string(strerror(errno)) << endl;
    }
    cout << "[INFO] Recieved message. Data: " << buffer << endl;

    string result = string(buffer);
    delete[] buffer;
    return result;
}

char* SocketUDP::recvBytesFrom() noexcept (false) {
    if (socketDescr == -1) { throw runtime_error("[ERR] Sending failed (UDP):  Socket is not initialized"); }
    const size_t BUFFER_SIZE = 1024;
    ssize_t numBytes;
    socklen_t socketSize = sizeof(struct sockaddr_in);
    char *buffer = new char[BUFFER_SIZE];
    if ((numBytes = recvfrom(socketDescr, buffer, BUFFER_SIZE - 1, 0, (struct sockaddr*) &connAddrCurrent, &socketSize) == - 1)) {
        cerr << "[ERR] Recieve failed (recvfrom): " + string(strerror(errno)) << endl;
    }
    cout << "[INFO] Recieved message. Data: " << buffer << endl;

    return buffer;
}
