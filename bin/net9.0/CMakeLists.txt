cmake_minimum_required(VERSION 4.0)
project(net8_0)

set(CMAKE_CXX_STANDARD 23)

set(CMAKE_CXX_FLAGS "-flto -fomit-frame-pointer -mavx2 -ffast-math -march=native -mtune=native")
 set(CMAKE_EXE_LINKER_FLAGS " -flto -fomit-frame-pointer -mavx2 -ffast-math -march=native -mtune=native")

add_executable(net8_0 output.cpp)
