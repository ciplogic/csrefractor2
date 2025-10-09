#include "native_sharp.hpp"
// headers imported by native methods
#include <cstdio>
#include <cwchar>

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
void System_Console_WriteLine(System_String value);
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
  int32_t vreg_3;
  bool vreg_4,vreg_5,vreg_6;

  vreg_3 = 2;
  vreg_4 = cgt (2, 0);
  if (brfalse(vreg_4)) goto label_27;
  vreg_5 = cgt (vreg_3, 1);
  if (brfalse(vreg_5)) goto label_40;
  vreg_6 = cgt (vreg_3, 2);
  if (brfalse(vreg_6)) goto label_53;
  goto label_66;
  label_27:
  System_Console_WriteLine(_str(0));
  goto label_66;
  label_40:
  System_Console_WriteLine(_str(1));
  goto label_66;
  label_53:
  System_Console_WriteLine(_str(2));
  goto label_66;
  label_66:
}

void System_Console_WriteLine(System_String value) {
auto arrText= marshallStringCharStar(value.get());
if (value->Coder){
    wchar_t *text = (wchar_t*)arrText.data();
    wprintf(L"%ls\n", text);
} else {
  char *text = (char*)arrText.data();
  printf("%s\n", text);
}
}

Ref<System_String> Texts_FromIndex(int32_t index, Arr<int32_t>* codes, Arr<int32_t>* startPos, Arr<int32_t>* lengths, Arr<uint8_t>* data)
{
  Ref<System_String> local_3;
  int32_t vreg_2,vreg_5,vreg_8;

  vreg_2 = ((*startPos)[index]);
  vreg_5 = ((*lengths)[index]);
  vreg_8 = ((*codes)[index]);
  local_3 = Texts_BuildSystemString(vreg_8, data, vreg_2, vreg_5);
  goto label_26;
  label_26:
  return local_3;
}

Ref<System_String> Texts_BuildSystemString(int32_t code, Arr<uint8_t>* data, int32_t startPos, int32_t len)
{
  Ref<System_String> local_2;
  RefArr<uint8_t> vreg_1;
  System_String* vreg_7;

  vreg_1 = new_arr<uint8_t>(len);
  System_Array_Copy(data, startPos, vreg_1.get(), 0, len);
   System_String vreg_7_instance;
   vreg_7 = &vreg_7_instance;  
  vreg_7->Data = nullptr;
  vreg_7->Coder = code;
  vreg_7->Data = vreg_1;
  local_2 = vreg_7;
  goto label_43;
  label_43:
  return local_2;
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
    RefArr<int> _coders = makeArr<int> ({0,0,0});
    RefArr<int> _startPos = makeArr<int> ({0,1,4});
    RefArr<int> _lengths = makeArr<int> ({1,3,16});
    RefArr<uint8_t> _joinedTexts = makeArr<uint8_t> ({120,97,98,99,109,97,121,98,101,32,105,116,32,105,115,32,116,114,117,101});
    Ref<System_String> _str(int index) {
       return Texts_FromIndex(index, _coders.get(), _startPos.get(), _lengths.get(), _joinedTexts.get());
    }
}
