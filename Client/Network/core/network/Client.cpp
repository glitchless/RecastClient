//#include <iostream>
//#include <stdexcept>
//#include <memory>               // shared_ptr
//#include <errno.h>
//#include <string.h>
//#include "network/Networking.hpp"
//
//#define RECV_TIMEOUT 3 // Seconds
//
//Got host & port pair somehow
//const std::string host = "localhost"; // Target host
//const int port = 8888; // Target port
//
//void echo(int argc, char *argv[]);
//void sync(std::string actions);
//
//int main_(int argc, char *argv[]) {
//    // echo(argc, argv); // Debug
//    std::string action = "CAST 12"; // i.e. 1 action at one moment
//    sync(action);
//
//    return 0;
//}

//void echo(int argc, char *argv[]) {
//    if (argc != 4) {
//        std::cerr << "usage: " << argv[0] << " host port" << std::endl;
//        return;
//    }
//
//    try {
//        std::string host_(argv[1]);
//        int port_ = std::stoi(argv[2]);
//
//        SocketTCP s;
//        s.connect(host_, port_);
//        s.setNonBlocked(true);
//        std::string line = argv[3];
//        s.send(line);
//        s.recvTimed(RECV_TIMEOUT);
//    } catch(const std::exception &e) {
//        std::cerr << e.what() << std::endl;
//    }
//}
//
//void sync(std::string actions) {
//    try {
//        SocketTCP s;
//        s.connect(host, port);
//        s.setNonBlocked(true);
//        s.send(actions);
//        std::cout << "Sent actions pack successfully" << std::endl;
//        std::string recieved = s.recvTimed(10);
//
//        std::cout << "Answer recieved: <" + recieved + ">" << std::endl;
//    } catch(const std::exception &e) {
//        std::cerr << e.what() << std::endl;
//    }
//}

