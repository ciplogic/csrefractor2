#pragma once

template
<typename T>
struct Box {
    int _typeId{};
    T _data{};
};


template<typename T>
struct RcData {
    int _count = 1;
    Box<T> _box{};

    T &getData() {
        return _box._data;
    }
    void setId(int id) {
        _box._typeId = id;
    }
};

template<typename T>
struct Ref {
    RcData<T> *_data;

    Ref(RcData<T> *p = nullptr) : _data(p) {}

    // Copy constructor
    Ref(const Ref &other) : _data(other._data) { ++_data->_count; }

    // Assignment operator
    Ref &operator=(const Ref &other) {
        if (this != &other) {
            release();
            _data = other._data;
            if (_data) {
                ++_data->_count;
            }
        }
        return *this;
    }

    // Destructor
    ~Ref() { release(); }

    // Dereference operators
    T &operator*() const { return _data->getData(); }
    T *operator->() const { return &_data->getData(); }
    T *get() { return &_data->getData(); }
    T *get() const { return &_data->getData(); }
    // Access reference count
    int use_count() const { return _data->_count; }

private:
    void release() {
        if (!_data) {
            return;
        }
        --_data->_count;
        if (!_data->_count) {
            delete _data;
        }
    }
};

template<typename T>
Ref<T> new_ref(int typeId = 0) {
    RcData<T> *data = new RcData<T>();
    data->setId(typeId);
    return Ref(data);
}

template<typename T>
Ref<T> new_ref_data(T &dataItem, int typeId = 0) {
    auto *data = new RcData<T>();
    data->_box._data = dataItem;
    data->setId(typeId);
    return Ref<T>(data);
}
