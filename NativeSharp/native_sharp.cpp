#include <chrono>
#include <functional>
#include <iostream>
#include <vector>

namespace {
    std::vector<uint8_t> marshallStringCharStar(System_String *text) {
        std::vector<uint8_t> result;
        uint8_t *dataPtr = text->Data->data();
        int textLen = text->Data->size();
        for (int i = 0; i < textLen; i++) {
            result.push_back(dataPtr[i]);
        }
        result.push_back(0);
        if (text->Coder) {
            result.push_back(0);
        }

        return result;
    }

    void timeItMicroseconds(std::function<void()> action) {
        auto start = std::chrono::high_resolution_clock::now();
        action();
        auto end = std::chrono::high_resolution_clock::now();
        auto duration =
            std::chrono::duration_cast<std::chrono::microseconds>(end - start);
        std::cout << "Time to run: " << duration.count() << " microseconds\n";
    }

    void timeItMilliseconds(std::function<void()> action) {
        auto start = std::chrono::high_resolution_clock::now();
        action();
        auto end = std::chrono::high_resolution_clock::now();
        auto duration =
            std::chrono::duration_cast<std::chrono::milliseconds>(end - start);
        std::cout << "Time to run: " << duration.count() << " milliseconds\n";
    }

    void timeItNanoseconds(std::function<void()> action) {
        auto start = std::chrono::high_resolution_clock::now();
        action();
        auto end = std::chrono::high_resolution_clock::now();
        auto duration =
            std::chrono::duration_cast<std::chrono::nanoseconds>(end - start);
        std::cout << "Time to run: " << duration.count() << " nanoseconds\n";
    }

    RefArr<Ref<System_String>> argsToStrings(int argc, char **argv) {
        RefArr<Ref<System_String>> result = new_arr<Ref<System_String>>(argc);
        for (int i = 1; i < argc; i++) {
            int len = strlen(argv[i]);
            auto vreg_1 = new_arr<System_Byte>(len);
            memcpy(vreg_1->data(), argv[i], len);
            Ref<System_String> item = new_ref<System_String>();
            item->Data = vreg_1;
            (*result)[i] = item;
        }
        
        return result;
    }

} // namespace
