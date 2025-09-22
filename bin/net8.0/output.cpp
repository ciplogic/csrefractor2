#include "native_sharp.hpp"
// headers imported by native methods
#include <cstdio>

struct System_String;
struct TargetApp_Program;
struct System_String {
  System_Int32 Coder;
  RefArr<System_Byte> Data;
};
struct TargetApp_Program {};
namespace {
    Ref<System_String> _clr_str(int index);
}
System_Void TargetApp_Program_Main();
System_Void System_Console_WriteLine(System_Int32 value);
System_Int32 TargetApp_Program_GetPrimeCount(System_Int32 rangeValue);
System_Boolean TargetApp_Program_IsPrime(System_Int32 number);
Ref<System_String> Texts_FromIndex(System_Int32 index, RefArr<System_Int32> codes, RefArr<System_Int32> startPos, RefArr<System_Int32> lengths, RefArr<System_Byte> data);
Ref<System_String> Texts_BuildSystemString(System_Int32 code, RefArr<System_Byte> data, System_Int32 startPos, System_Int32 len);
System_Void System_Array_Copy(RefArr<System_Byte> sourceArray, System_Int32 sourceIndex, RefArr<System_Byte> destinationArray, System_Int32 destinationIndex, System_Int32 len);
int main() {
TargetApp_Program_Main();
return 0;
}
namespace {
  std::vector<uint8_t> marshallStringCharStar(System_String* text) {
    std::vector<uint8_t> result;
    uint8_t* dataPtr = text->Data->data();
    int textLen = text->Data->size();
    for (int i = 0; i < textLen; i++) {
      result.push_back(dataPtr[i]);
    }
    result.push_back(0);
    if (text->Coder) {
      result.push_back(0);
    }

    return result;
  };
}
System_Void TargetApp_Program_Main()
{
  System_Console_WriteLine(664582);
}

System_Void System_Console_WriteLine(System_Int32 value) {
printf("%i\n", value);
}

System_Int32 TargetApp_Program_GetPrimeCount(System_Int32 rangeValue)
{
  System_Int32 local_0,local_1,vreg_13;
  System_Boolean vreg_3;

  local_0 = 0;
  local_1 = 0;
  goto label_29;
  label_7:
  vreg_3 = TargetApp_Program_IsPrime(local_1);
  if (brfalse_s(vreg_3)) goto label_24;
  local_0 = clr_add (local_0, 1);
  label_24:
  local_1 = clr_add (local_1, 1);
  label_29:
  vreg_13 = clt (local_1, rangeValue);
  if (brtrue_s(vreg_13)) goto label_7;
  return local_0;
}

System_Boolean TargetApp_Program_IsPrime(System_Int32 number)
{
  System_Boolean local_1;
  System_Int32 local_3,vreg_2,vreg_4,vreg_9,vreg_11,vreg_17,vreg_19,vreg_27,vreg_29,vreg_31;

  vreg_2 = cgt (number, 4);
  vreg_4 = ceq (vreg_2, 0);
  if (brfalse_s(vreg_4)) goto label_17;
  local_1 = 1;
  goto label_78;
  label_17:
  vreg_9 = clr_rem (number, 2);
  vreg_11 = ceq (vreg_9, 0);
  if (brfalse_s(vreg_11)) goto label_32;
  local_1 = 0;
  goto label_78;
  label_32:
  local_3 = 3;
  goto label_59;
  label_36:
  vreg_17 = clr_rem (number, local_3);
  vreg_19 = ceq (vreg_17, 0);
  if (brfalse_s(vreg_19)) goto label_54;
  local_1 = 0;
  goto label_78;
  label_54:
  local_3 = clr_add (local_3, 2);
  label_59:
  vreg_27 = clr_mul (local_3, local_3);
  vreg_29 = cgt (vreg_27, number);
  vreg_31 = ceq (vreg_29, 0);
  if (brtrue_s(vreg_31)) goto label_36;
  local_1 = 1;
  label_78:
  return local_1;
}

Ref<System_String> Texts_FromIndex(System_Int32 index, RefArr<System_Int32> codes, RefArr<System_Int32> startPos, RefArr<System_Int32> lengths, RefArr<System_Byte> data)
{
  Ref<System_String> local_3;
  System_Int32 vreg_2,vreg_5,vreg_8;

  vreg_2 = (*startPos)[index];
  vreg_5 = (*lengths)[index];
  vreg_8 = (*codes)[index];
  local_3 = Texts_BuildSystemString(vreg_8, data, vreg_2, vreg_5);
  return local_3;
}

Ref<System_String> Texts_BuildSystemString(System_Int32 code, RefArr<System_Byte> data, System_Int32 startPos, System_Int32 len)
{
  RefArr<System_Byte> vreg_1;
  Ref<System_String> vreg_7;

  vreg_1 = new_arr<System_Byte>(len);
  System_Array_Copy(data, startPos, vreg_1, 0, len);
  vreg_7 = new_ref<System_String>();
  vreg_7->Coder = code;
  vreg_7->Data = vreg_1;
  return vreg_7;
}

System_Void System_Array_Copy(RefArr<System_Byte> sourceArray, System_Int32 sourceIndex, RefArr<System_Byte> destinationArray, System_Int32 destinationIndex, System_Int32 len)
{
  System_Int32 local_0,vreg_3,vreg_6,vreg_18;
  System_Byte vreg_9;

  local_0 = 0;
  goto label_27;
  label_5:
  vreg_3 = clr_add (sourceIndex, local_0);
  vreg_6 = clr_add (destinationIndex, local_0);
  vreg_9 = (*sourceArray)[vreg_3];
  (*destinationArray)[vreg_6] = vreg_9;
  local_0 = clr_add (local_0, 1);
  label_27:
  vreg_18 = clt (local_0, len);
  if (brtrue_s(vreg_18)) goto label_5;
}

namespace {
    RefArr<int> _coders = makeArr<int> ({});
    RefArr<int> _startPos = makeArr<int> ({});
    RefArr<int> _lengths = makeArr<int> ({});
    RefArr<uint8_t> _joinedTexts = makeArr<uint8_t> ({});
    Ref<System_String> _clr_str(int index) {
       return Texts_FromIndex(index, _coders, _startPos, _lengths, _joinedTexts);
    }
}
