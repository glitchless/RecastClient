/**
 * @file SocketTCP.cpp
 * @brief Socket (TCP) utils source file
 * @author StealthTech
 * @project Recast-client
 * @date 27.06.17
 * @email st3althtech@mail.ru
 *
 **/

#include "network/SocketTCP.hpp"

using namespace std;

void SocketTCP::connect(const string &host, int port) {
    struct sockaddr_in addr = resolve(host.data(), port);

    int sd = socket(PF_INET, SOCK_STREAM, IPPROTO_TCP);
    if (sd <= 0) {
        throw runtime_error("[ERR] Failed to create socket (TCP): " + string(strerror(errno)));
    }

    int connected = ::connect(sd, (struct sockaddr*)&addr, sizeof(addr));
    if (connected == -1) {
        ::close(sd);
        throw runtime_error("[ERR] Failed to connect server (TCP): " + string(strerror(errno)));
    }

    socketDescr = sd;
}

void SocketTCP::send(const string &str) noexcept (false) {
    size_t left = str.size();
    ssize_t sent = 0;
    int flags = 0;

    while (left > 0) {
        sent = ::send(socketDescr, str.data() + sent, str.size() - sent, flags);
        if (-1 == sent) {
            throw runtime_error("[ERR] Sending failed (TCP): " + string(strerror(errno)));
        }

        left -= sent;
    }
}

void SocketTCP::sendBytes(const char *data, size_t num) noexcept (false) {
    size_t left = num;
    ssize_t sent = 0;
    int flags = 0;

    while (left > 0) {
        sent = ::send(socketDescr, data + sent, num - sent, flags);
        if (-1 == sent) {
            throw runtime_error("[ERR] Sending failed (TCP): " + string(strerror(errno)));
        }

        left -= sent;
    }
}

string SocketTCP::recv(size_t bytes) noexcept (false) {
    char *buffer = new char[bytes];
    size_t r = 0;
    while (r != bytes) {
        ssize_t rc = ::recv(socketDescr, buffer + r, bytes - r, 0);
        cerr << "recv_ex: " << rc << " bytes\n";

        if (rc == -1 || rc == 0) {
            delete [] buffer;
            throw runtime_error("[ERR] Receiving failed (TCP): " + string(strerror(errno)));
        }
        r += rc;
    }
    string ret(buffer, buffer + bytes);
    delete [] buffer;
    return ret;
}

char* SocketTCP::recvBytes(size_t bytes) noexcept (false) {
    char *buffer = new char[bytes];
    size_t r = 0;
    while (r != bytes) {
        ssize_t rc = ::recv(socketDescr, buffer + r, bytes - r, 0);
        cerr << "recv_ex: " << rc << " bytes\n";

        if (rc == -1 || rc == 0) {
            delete [] buffer;
            throw runtime_error("[ERR] Receiving failed (TCP): " + string(strerror(errno)));
        }
        r += rc;
    }
    return buffer;
}

string SocketTCP::recv() noexcept (false) {
    char buffer[256];
#ifdef __APPLE__
    // mac os x doesn't define MSG_NOSIGNAL
    int n = ::recv(socketDescr, buffer, sizeof(buffer), 0);
#else
    int n = ::recv(socketDescr, buffer, sizeof(buffer), MSG_NOSIGNAL);
#endif

    if (-1 == n && errno != EAGAIN) {
        throw runtime_error("read failed: " + string(strerror(errno)));
    }
    if (0 == n) {
        throw runtime_error("client: " + to_string(socketDescr) + " disconnected");
    }
    if (-1 == n) {
        throw runtime_error("client: " + to_string(socketDescr) + " timeouted");
    }

    string ret(buffer, buffer + n);
    while (ret.back() == '\r' || ret.back() == '\n') {
        ret.pop_back();
    }
    cerr << "client: " << socketDescr << ", recv: " << ret << " [" << n << " bytes]" << endl;
    return ret;
}

string SocketTCP::recvTimed(int timeout) noexcept (false) {
    fd_set read_fds;
    FD_ZERO(&read_fds);
    FD_SET(socketDescr, &read_fds);
    struct timeval tm;
    tm.tv_sec = timeout;
    tm.tv_usec = 0;
    int sel = select(socketDescr + 1, &read_fds, NULL, NULL, &tm); // read, write, exceptions
    if (sel != 1) { throw runtime_error("read timeout"); }

    return recv();
}

bool SocketTCP::hasData() noexcept (false) {
    char buf[1];
    int n = ::recv(socketDescr, buf, sizeof(buf), MSG_PEEK);
    if (n > 0) { return true; }
    return false;
}
