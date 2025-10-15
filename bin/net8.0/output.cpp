#include "native_sharp.hpp"
// headers imported by native methods
#include <iostream>

struct System_String;
struct TargetApp_MinimalSwitchSample;
struct Texts;
struct System_String {
  int32_t Coder;
  RefArr<uint8_t> Data;
};
struct TargetApp_MinimalSwitchSample {};
struct Texts {};
namespace {
    Ref<System_String> _str(int index);
}
void TargetApp_MinimalSwitchSample_Main(RefArr<Ref<System_String>> args);
void System_Console_WriteLine(int32_t value);
Ref<System_String> Texts_FromIndex(int32_t index, Arr<int32_t>* codes, Arr<int32_t>* startPos, Arr<int32_t>* lengths, Arr<uint8_t>* data);
Ref<System_String> Texts_BuildSystemString(int32_t code, Arr<uint8_t>* data, int32_t startPos, int32_t len);
void System_Array_Copy(Arr<uint8_t>* sourceArray, int32_t sourceIndex, Arr<uint8_t>* destinationArray, int32_t destinationIndex, int32_t len);
#include "native_sharp.cpp"
int main(int argc, char**argv) {
auto ARGS = argsToStrings(argc, argv);
timeItMilliseconds([&]{ TargetApp_MinimalSwitchSample_Main (ARGS); });
return 0;
}
void TargetApp_MinimalSwitchSample_Main(RefArr<Ref<System_String>> args)
{
  int32_t local_0,local_1,vreg_4,vreg_9;

  local_0 = 2;
  local_1 = 3;
  vreg_4 = cgt (2, 3);
  if (brfalse_s(vreg_4)) goto label_22;
  System_Console_WriteLine(local_0);
  label_22:
  vreg_9 = clt (local_0, 3);
  if (brfalse_s(vreg_9)) goto label_41;
  System_Console_WriteLine(local_0);
  goto label_50;
  label_41:
  System_Console_WriteLine(local_1);
  label_50:
}

void System_Console_WriteLine(int32_t value) {
std::cout<<value<<'\n';
}

Ref<System_String> Texts_FromIndex(int32_t index, Arr<int32_t>* codes, Arr<int32_t>* startPos, Arr<int32_t>* lengths, Arr<uint8_t>* data)
{
  Ref<System_String> local_3;
  int32_t vreg_2,vreg_5,vreg_8;

  vreg_2 = ((*startPos)[index]);
  vreg_5 = ((*lengths)[index]);
  vreg_8 = ((*codes)[index]);
  local_3 = Texts_BuildSystemString(vreg_8, data, vreg_2, vreg_5);
  return local_3;
}

Ref<System_String> Texts_BuildSystemString(int32_t code, Arr<uint8_t>* data, int32_t startPos, int32_t len)
{
  RefArr<uint8_t> vreg_1;
  Ref<System_String> vreg_7;

  vreg_1 = new_arr<uint8_t>(len);
  System_Array_Copy(data, startPos, vreg_1.get(), 0, len);
  vreg_7 = new_ref<System_String>(0);
  vreg_7->Data = nullptr;
  vreg_7->Coder = code;
  vreg_7->Data = vreg_1;
  return vreg_7;
}

void System_Array_Copy(Arr<uint8_t>* sourceArray, int32_t sourceIndex, Arr<uint8_t>* destinationArray, int32_t destinationIndex, int32_t len)
{
  int32_t local_0,vreg_3,vreg_6,vreg_18;
  uint8_t vreg_9;

  local_0 = 0;
  goto label_27;
  label_5:
  vreg_3 = add (sourceIndex, local_0);
  vreg_6 = add (destinationIndex, local_0);
  vreg_9 = ((*sourceArray)[vreg_3]);
  (*destinationArray)[vreg_6] = vreg_9;
  local_0 = add (local_0, 1);
  label_27:
  vreg_18 = clt (local_0, len);
  if (brtrue_s(vreg_18)) goto label_5;
}

namespace {
    RefArr<int> _coders = makeArr<int> ({});
    RefArr<int> _startPos = makeArr<int> ({});
    RefArr<int> _lengths = makeArr<int> ({});
    RefArr<uint8_t> _joinedTexts = makeArr<uint8_t> ({});
    Ref<System_String> _str(int index) {
       return Texts_FromIndex(index, _coders.get(), _startPos.get(), _lengths.get(), _joinedTexts.get());
    }
}
