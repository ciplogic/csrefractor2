#include "native_sharp.hpp"
// headers imported by native methods
#include <iostream>

struct System_String;
struct TargetApp_InheritanceTests;
struct TargetApp_Foo;
struct TargetApp_BaseFoo;
struct TargetApp_Bar;
struct Texts;
struct System_String {
  System_Int32 Coder;
  RefArr<System_Byte> Data;
};
struct TargetApp_InheritanceTests {};
struct TargetApp_Foo {};
struct TargetApp_BaseFoo {};
struct TargetApp_Bar {};
struct Texts {};
namespace {
    Ref<System_String> _str(int index);
}
System_Void TargetApp_InheritanceTests_Main();
System_Void System_Console_WriteLine(System_Int32 value);
System_Void TargetApp_Foo_ctor(TargetApp_Foo _this);
System_Void TargetApp_BaseFoo_ctor(TargetApp_BaseFoo _this);
System_Void TargetApp_Bar_ctor(TargetApp_Bar _this);
Ref<System_String> Texts_FromIndex(System_Int32 index, RefArr<System_Int32> codes, RefArr<System_Int32> startPos, RefArr<System_Int32> lengths, RefArr<System_Byte> data);
Ref<System_String> Texts_BuildSystemString(System_Int32 code, RefArr<System_Byte> data, System_Int32 startPos, System_Int32 len);
System_Void System_Array_Copy(RefArr<System_Byte> sourceArray, System_Int32 sourceIndex, RefArr<System_Byte> destinationArray, System_Int32 destinationIndex, System_Int32 len);
System_Void System_String_ctor(System_String _this);
#include "native_sharp.cpp"
int main(int argc, char**argv) {
auto ARGS = argsToStrings(argc, argv);
timeItMilliseconds([&]{ TargetApp_InheritanceTests_Main (ARGS); });
return 0;
}
System_Void TargetApp_InheritanceTests_Main()
{
  TargetApp_Foo local_0,vreg_0,vreg_1,vreg_3;
  TargetApp_Bar local_1,vreg_5,vreg_6;
  System_Int32 vreg_2,vreg_4,vreg_7;

  vreg_0 = new_ref<TargetApp_Foo>();
  TargetApp_Foo_ctor(vreg_0);
  local_0 = vreg_0;
  vreg_1 = local_0;
  vreg_2 = TargetApp_BaseFoo_GetBasicFoo(vreg_1);
  System_Console_WriteLine(vreg_2);
  vreg_3 = local_0;
  vreg_4 = TargetApp_Foo_GetFoo(vreg_3);
  System_Console_WriteLine(vreg_4);
  vreg_5 = new_ref<TargetApp_Bar>();
  TargetApp_Bar_ctor(vreg_5);
  local_1 = vreg_5;
  vreg_6 = local_1;
  vreg_7 = TargetApp_Bar_GetFoo(vreg_6);
  System_Console_WriteLine(vreg_7);
}

System_Void System_Console_WriteLine(System_Int32 value) {
std::cout<<value<<'\n';
}

System_Void TargetApp_Foo_ctor(TargetApp_Foo _this)
{
  TargetApp_Foo vreg_0;

  vreg_0 = _this;
  TargetApp_BaseFoo_ctor();
}

System_Void TargetApp_BaseFoo_ctor(TargetApp_BaseFoo _this)
{
  TargetApp_BaseFoo vreg_0;

  vreg_0 = _this;
}

System_Void TargetApp_Bar_ctor(TargetApp_Bar _this)
{
  TargetApp_Bar vreg_0;

  vreg_0 = _this;
}

Ref<System_String> Texts_FromIndex(System_Int32 index, RefArr<System_Int32> codes, RefArr<System_Int32> startPos, RefArr<System_Int32> lengths, RefArr<System_Byte> data)
{
  System_Int32 local_0,local_1,local_2,vreg_1,vreg_2,vreg_4,vreg_5,vreg_7,vreg_8,vreg_9,vreg_11,vreg_12;
  System_String local_3,vreg_13,vreg_14;
  RefArr<System_Int32> vreg_0,vreg_3,vreg_6;
  RefArr<System_Byte> vreg_10;

  vreg_0 = startPos;
  vreg_1 = index;
  vreg_2 = ((*vreg_0)[vreg_1]);
  local_0 = vreg_2;
  vreg_3 = lengths;
  vreg_4 = index;
  vreg_5 = ((*vreg_3)[vreg_4]);
  local_1 = vreg_5;
  vreg_6 = codes;
  vreg_7 = index;
  vreg_8 = ((*vreg_6)[vreg_7]);
  local_2 = vreg_8;
  vreg_9 = local_2;
  vreg_10 = data;
  vreg_11 = local_0;
  vreg_12 = local_1;
  vreg_13 = Texts_BuildSystemString(vreg_9, vreg_10, vreg_11, vreg_12);
  local_3 = vreg_13;
  goto label_26;
  label_26:
  vreg_14 = local_3;
  return vreg_14;
}

Ref<System_String> Texts_BuildSystemString(System_Int32 code, RefArr<System_Byte> data, System_Int32 startPos, System_Int32 len)
{
  RefArr<System_Byte> local_0,vreg_1,vreg_2,vreg_4,vreg_13;
  System_String local_1,local_2,vreg_7,vreg_8,vreg_9,vreg_11,vreg_12,vreg_14,vreg_15;
  System_Int32 vreg_0,vreg_3,vreg_5,vreg_6,vreg_10;

  vreg_0 = len;
  vreg_1 = new_arr<System_Byte>(vreg_0);
  local_0 = vreg_1;
  vreg_2 = data;
  vreg_3 = startPos;
  vreg_4 = local_0;
  vreg_5 = 0;
  vreg_6 = len;
  System_Array_Copy(vreg_2, vreg_3, vreg_4, vreg_5, vreg_6);
  vreg_7 = new_ref<System_String>();
  System_String_ctor(vreg_7);
  vreg_8 = vreg_7;
  vreg_9 = vreg_7;
  vreg_10 = code;
  vreg_9->Coder = vreg_10;
  vreg_11 = vreg_8;
  vreg_12 = vreg_8;
  vreg_13 = local_0;
  vreg_12->Data = vreg_13;
  local_1 = vreg_11;
  vreg_14 = local_1;
  local_2 = vreg_14;
  goto label_43;
  label_43:
  vreg_15 = local_2;
  return vreg_15;
}

System_Void System_Array_Copy(RefArr<System_Byte> sourceArray, System_Int32 sourceIndex, RefArr<System_Byte> destinationArray, System_Int32 destinationIndex, System_Int32 len)
{
  System_Int32 local_0,local_1,local_2,vreg_0,vreg_1,vreg_2,vreg_3,vreg_4,vreg_5,vreg_6,vreg_8,vreg_11,vreg_13,vreg_14,vreg_15,vreg_16,vreg_17,vreg_18;
  System_Byte local_3,vreg_9,vreg_12;
  System_Boolean local_4,vreg_19;
  RefArr<System_Byte> vreg_7,vreg_10;

  vreg_0 = 0;
  local_0 = vreg_0;
  goto label_27;
  label_5:
  vreg_1 = sourceIndex;
  vreg_2 = local_0;
  vreg_3 = add (vreg_1, vreg_2);
  local_1 = vreg_3;
  vreg_4 = destinationIndex;
  vreg_5 = local_0;
  vreg_6 = add (vreg_4, vreg_5);
  local_2 = vreg_6;
  vreg_7 = sourceArray;
  vreg_8 = local_1;
  vreg_9 = ((*vreg_7)[vreg_8]);
  local_3 = vreg_9;
  vreg_10 = destinationArray;
  vreg_11 = local_2;
  vreg_12 = local_3;
  (*vreg_10)[vreg_11] = vreg_12;
  vreg_13 = local_0;
  vreg_14 = 1;
  vreg_15 = add (vreg_13, vreg_14);
  local_0 = vreg_15;
  label_27:
  vreg_16 = local_0;
  vreg_17 = len;
  vreg_18 = clt (vreg_16, vreg_17);
  local_4 = vreg_18;
  vreg_19 = local_4;
  if (brtrue_s(vreg_19)) goto label_5;
}

System_Void System_String_ctor(System_String _this)
{
  System_String vreg_0,vreg_1,vreg_2;

  vreg_0 = _this;
  vreg_1 = nullptr;
  vreg_0->Data = vreg_1;
  vreg_2 = _this;
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
