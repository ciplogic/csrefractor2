#pragma once

#include "native_sharp_primitives.hpp"

#include <vector>

#include "native_rc.hpp"
#include "native_heap_array.hpp"

template <typename T>
using Arr = HeapArray<T>;
template <typename T>
using RefArr = Ref<Arr<T>>;

template <class T>
RefArr<T> new_arr(int size)
{
    Arr<T> arr = Arr<T>::create(size);
    return new_ref_data(arr);
}

template <class T>
RefArr<T> makeArr(std::initializer_list<T> init)
{
    Arr<T> arr = Arr<T>::create(init);
    return new_ref_data(arr);
}

template <typename T>
T add(T left, T right) { return left + right; }

template <typename T>
T sub(T left, T right) { return left - right; }

template <typename T>
T mul(T left, T right) { return left * right; }

template <typename T>
T div(T left, T right) { return left / right; }

template <typename T>
T rem(T left, T right) { return left % right; }

template <typename T>
T neg(T left) { return -left; }

inline bool cgt(int left, int right) { return left > right; }

inline bool clt(int left, int right) { return left < right; }

inline bool ceq(int left, int right) { return left == right; }

inline bool brfalse_s(int left) { return !left; }
inline bool brtrue_s(int left) { return left; }
inline bool brfalse(int left) { return !left; }
inline bool brtrue(int left) { return left; }

inline bool blt_s(int left, int right) { return left < right; }
inline bool blt(int left, int right) { return left < right; }

inline int32_t conv_i4(uint32_t left) { return left; }
