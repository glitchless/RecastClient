cmake_minimum_required(VERSION 3.6)
project(Recast_Local)

set(CMAKE_CXX_STANDARD 11)

include_directories(../utils/)
set(SOURCE_FILES client.cpp ../utils/networking.cpp)
add_executable(client.lxe ${SOURCE_FILES})