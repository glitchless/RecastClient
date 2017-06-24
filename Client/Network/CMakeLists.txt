cmake_minimum_required(VERSION 3.2)
project(Recast-client)

set(CMAKE_CXX_STANDARD 14)

file(GLOB_RECURSE SOURCE_FILES
        "headers/*.h"
        "headers/*.hpp"
        "core/*.c"
        "core/*.cpp")

add_executable(Recast-client ${SOURCE_FILES})