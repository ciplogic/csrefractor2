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
T clr_add(T left, T right) { return left + right; }

template <typename T>
T clr_sub(T left, T right) { return left - right; }

template <typename T>
T clr_mul(T left, T right) { return left * right; }

template <typename T>
T clr_div(T left, T right) { return left / right; }

template <typename T>
T clr_rem(T left, T right) { return left % right; }

template <typename T>
T clr_neg(T left) { return -left; }

inline bool cgt(int left, int right) { return left > right; }

inline bool clt(int left, int right) { return left < right; }

inline bool ceq(int left, int right) { return left == right; }

inline bool brfalse_s(int left) { return !left; }
inline bool brtrue_s(int left) { return left; }
inline bool brfalse(int left) { return !left; }
inline bool brtrue(int left) { return left; }

inline bool blt_s(int left) { return left >= 0; }
inline bool blt(int left) { return left >= 0; }

inline int32_t conv_i4(uint32_t left) { return left; }
