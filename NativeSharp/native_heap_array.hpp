#pragma once

#include "native_rc.hpp"

template <typename T>
struct HeapArray
{
    int _size;
    Ref<T*> _data;

    // Factory method
    static HeapArray create(int n)
    {
        HeapArray result(n);
        return result;
    }

    static HeapArray create(std::initializer_list<T> init)
    {
        HeapArray result(init);
        return result;
    }

    HeapArray() : _size(0), _data(nullptr)
    {
    }

    HeapArray(int size)
    {
        _size = size;
        T* items = new T[_size];
        _data = new_ref_data<T*>(items);
    }

    HeapArray(std::initializer_list<T> init)
    {
        _size = init.size();
        T* items = new T[_size];
        int pos = 0;
        for (auto it : init)
        {
            items[pos++] = it;
        }
        _data = new_ref_data<T*>(items);
    }

    // Copy constructor
    HeapArray(const HeapArray& other)
    {
        _size = other._size;
        _data = other._data;
    }

    // Copy assignment
    HeapArray& operator=(const HeapArray& other)
    {
        if (this != &other)
        {
            _size = other._size;
            _data = other._data;
        }
        return *this;
    }

    // Move constructor
    HeapArray(HeapArray&& other) noexcept
        : _size(other._size), _data(other._data)
    {
    }

    // Move assignment
    HeapArray& operator=(HeapArray&& other) noexcept
    {
        if (this != &other)
        {
            _size = other._size;
            _data = other._data;
        }
        return *this;
    }

    // Size
    int size() const { return _size; }

    std::vector<T> toVector() const
    {
        std::vector < T > result;
        result.reserve(size());
        for (int i = 0; i < size(); i++)
        {
            result.push_back(data()[i]);
        }
        return result;
    }

    // Access
    T& operator[](int i) { return data()[i]; }
    const T& operator[](int i) const { return data()[i]; }

    T* data() { return *_data; }
    const T* data() const { return *_data; }
};
