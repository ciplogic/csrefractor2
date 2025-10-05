#include "native_sharp.hpp"
// headers imported by native methods
#include <iostream>

struct System_String;
struct nbody;
struct NBodySystem;
struct Body;
struct Texts;
struct System_String {
  System_Int32 Coder;
  RefArr<System_Byte> Data;
};
struct nbody {};
struct NBodySystem {
  RefArr<Ref<Body>> bodies;
};
struct Body {
  System_Double _X_k__BackingField,_Y_k__BackingField,_Z_k__BackingField,Vx,Vy,Vz,Mass;
};
struct Texts {};
namespace {
    Ref<System_String> _str(int index);
}
System_Void nbody_Main();
System_Void System_Console_WriteLine(System_Double value);
System_Void NBodySystem_ctor(NBodySystem* _this);
Ref<Body> Body_sun();
System_Void Body_ctor(Body _this);
Ref<Body> Body_jupiter();
Ref<Body> Body_saturn();
Ref<Body> Body_uranus();
Ref<Body> Body_neptune();
Ref<System_String> Texts_FromIndex(System_Int32 index, Arr<System_Int32>* codes, Arr<System_Int32>* startPos, Arr<System_Int32>* lengths, Arr<System_Byte>* data);
Ref<System_String> Texts_BuildSystemString(System_Int32 code, Arr<System_Byte>* data, System_Int32 startPos, System_Int32 len);
System_Void System_Array_Copy(Arr<System_Byte>* sourceArray, System_Int32 sourceIndex, Arr<System_Byte>* destinationArray, System_Int32 destinationIndex, System_Int32 len);
System_Void System_String_ctor(System_String* _this);
#include "native_sharp.cpp"
int main(int argc, char**argv) {
timeItMilliseconds(nbody_Main);
return 0;
}
System_Void nbody_Main()
{
  System_Int32 local_0,local_2,vreg_12;
  NBodySystem* vreg_1;
  System_Double vreg_3,vreg_15;

  local_0 = 10000000;
   NBodySystem vreg_1_instance;
   vreg_1 = &vreg_1_instance;  
  NBodySystem_ctor(vreg_1);
  vreg_3 = NBodySystem_energy(vreg_2);
  System_Console_WriteLine(vreg_3);
  local_2 = 0;
  goto label_49;
  label_29:
  NBodySystem_advance(vreg_5, vreg_6);
  local_2 = add (local_2, 1);
  label_49:
  vreg_12 = clt (local_2, local_0);
  if (brtrue_s(vreg_12)) goto label_29;
  vreg_15 = NBodySystem_energy(vreg_14);
  System_Console_WriteLine(vreg_15);
}

System_Void System_Console_WriteLine(System_Double value) {
std::cout<<value<<'\n';
}

System_Void NBodySystem_ctor(NBodySystem* _this)
{
  System_Double local_0,local_1,local_2,vreg_33,vreg_38,vreg_39,vreg_46,vreg_51,vreg_52,vreg_59,vreg_64,vreg_65;
  System_Int32 local_3,vreg_74,vreg_75;
  RefArr<Ref<Body>> vreg_3;
  Arr<Ref<Body>>* vreg_30,*vreg_35,*vreg_43,*vreg_48,*vreg_56,*vreg_61,*vreg_72,*vreg_78;
  Body* vreg_32,*vreg_37,*vreg_45,*vreg_50,*vreg_58,*vreg_63;
  System_UInt32 vreg_73;
  Body vreg_80,vreg_84;
  Ref<Body> vreg_86,vreg_87,vreg_88,vreg_89,vreg_90;

  vreg_3 = new_arr<Ref<Body>>(5);
  vreg_86 = new_ref<Body>(2);
  vreg_86->Mass = 39.47841760435743;
  (*vreg_3)[0] = vreg_86;
  vreg_87 = new_ref<Body>(2);
  Body_set_X(vreg_1, vreg_2);
  Body_set_Y(vreg_3, vreg_4);
  Body_set_Z(vreg_5, vreg_6);
  vreg_87->Vx = 0.606326392995832;
  vreg_87->Vy = 2.81198684491626;
  vreg_87->Vz = -0.02521836165988763;
  vreg_87->Mass = 0.03769367487038949;
  (*vreg_3)[1] = vreg_87;
  vreg_88 = new_ref<Body>(2);
  Body_set_X(vreg_1, vreg_2);
  Body_set_Y(vreg_3, vreg_4);
  Body_set_Z(vreg_5, vreg_6);
  vreg_88->Vx = -1.0107743461787924;
  vreg_88->Vy = 1.8256623712304119;
  vreg_88->Vz = 0.008415761376584154;
  vreg_88->Mass = 0.011286326131968767;
  (*vreg_3)[2] = vreg_88;
  vreg_89 = new_ref<Body>(2);
  Body_set_X(vreg_1, vreg_2);
  Body_set_Y(vreg_3, vreg_4);
  Body_set_Z(vreg_5, vreg_6);
  vreg_89->Vx = 1.0827910064415354;
  vreg_89->Vy = 0.8687130181696082;
  vreg_89->Vz = -0.010832637401363636;
  vreg_89->Mass = 0.0017237240570597112;
  (*vreg_3)[3] = vreg_89;
  vreg_90 = new_ref<Body>(2);
  Body_set_X(vreg_1, vreg_2);
  Body_set_Y(vreg_3, vreg_4);
  Body_set_Z(vreg_5, vreg_6);
  vreg_90->Vx = 0.979090732243898;
  vreg_90->Vy = 0.5946989986476762;
  vreg_90->Vz = -0.034755955504078104;
  vreg_90->Mass = 0.0020336868699246304;
  (*vreg_3)[4] = vreg_90;
  _this->bodies = vreg_3;
  local_0 = 0;
  local_1 = 0;
  local_2 = 0;
  local_3 = 0;
  goto label_190;
  label_94:
  vreg_30 = _this->bodies.get();
  vreg_32 = ((*vreg_30)[local_3]).get();
  vreg_33 = vreg_32->Vx;
  vreg_35 = _this->bodies.get();
  vreg_37 = ((*vreg_35)[local_3]).get();
  vreg_38 = vreg_37->Mass;
  vreg_39 = mul (vreg_33, vreg_38);
  local_0 = add (local_0, vreg_39);
  vreg_43 = _this->bodies.get();
  vreg_45 = ((*vreg_43)[local_3]).get();
  vreg_46 = vreg_45->Vy;
  vreg_48 = _this->bodies.get();
  vreg_50 = ((*vreg_48)[local_3]).get();
  vreg_51 = vreg_50->Mass;
  vreg_52 = mul (vreg_46, vreg_51);
  local_1 = add (local_1, vreg_52);
  vreg_56 = _this->bodies.get();
  vreg_58 = ((*vreg_56)[local_3]).get();
  vreg_59 = vreg_58->Vz;
  vreg_61 = _this->bodies.get();
  vreg_63 = ((*vreg_61)[local_3]).get();
  vreg_64 = vreg_63->Mass;
  vreg_65 = mul (vreg_59, vreg_64);
  local_2 = add (local_2, vreg_65);
  local_3 = add (local_3, 1);
  label_190:
  vreg_72 = _this->bodies.get();
  vreg_73 = vreg_72->size();
  vreg_74 = conv_i4 (vreg_73);
  vreg_75 = clt (local_3, vreg_74);
  if (brtrue_s(vreg_75)) goto label_94;
  vreg_78 = _this->bodies.get();
  vreg_80 = ((*vreg_78)[0]);
  vreg_84 = Body_offsetMomentum(vreg_80, vreg_81, vreg_82, vreg_83);
}

Ref<Body> Body_sun()
{
  Ref<Body> vreg_0;

  vreg_0 = new_ref<Body>(2);
  vreg_0->Mass = 39.47841760435743;
  return vreg_0;
}

System_Void Body_ctor(Body _this)
{
}

Ref<Body> Body_jupiter()
{
  Ref<Body> vreg_0;

  vreg_0 = new_ref<Body>(2);
  Body_set_X(vreg_1, vreg_2);
  Body_set_Y(vreg_3, vreg_4);
  Body_set_Z(vreg_5, vreg_6);
  vreg_0->Vx = 0.606326392995832;
  vreg_0->Vy = 2.81198684491626;
  vreg_0->Vz = -0.02521836165988763;
  vreg_0->Mass = 0.03769367487038949;
  return vreg_0;
}

Ref<Body> Body_saturn()
{
  Ref<Body> vreg_0;

  vreg_0 = new_ref<Body>(2);
  Body_set_X(vreg_1, vreg_2);
  Body_set_Y(vreg_3, vreg_4);
  Body_set_Z(vreg_5, vreg_6);
  vreg_0->Vx = -1.0107743461787924;
  vreg_0->Vy = 1.8256623712304119;
  vreg_0->Vz = 0.008415761376584154;
  vreg_0->Mass = 0.011286326131968767;
  return vreg_0;
}

Ref<Body> Body_uranus()
{
  Ref<Body> vreg_0;

  vreg_0 = new_ref<Body>(2);
  Body_set_X(vreg_1, vreg_2);
  Body_set_Y(vreg_3, vreg_4);
  Body_set_Z(vreg_5, vreg_6);
  vreg_0->Vx = 1.0827910064415354;
  vreg_0->Vy = 0.8687130181696082;
  vreg_0->Vz = -0.010832637401363636;
  vreg_0->Mass = 0.0017237240570597112;
  return vreg_0;
}

Ref<Body> Body_neptune()
{
  Ref<Body> vreg_0;

  vreg_0 = new_ref<Body>(2);
  Body_set_X(vreg_1, vreg_2);
  Body_set_Y(vreg_3, vreg_4);
  Body_set_Z(vreg_5, vreg_6);
  vreg_0->Vx = 0.979090732243898;
  vreg_0->Vy = 0.5946989986476762;
  vreg_0->Vz = -0.034755955504078104;
  vreg_0->Mass = 0.0020336868699246304;
  return vreg_0;
}

Ref<System_String> Texts_FromIndex(System_Int32 index, Arr<System_Int32>* codes, Arr<System_Int32>* startPos, Arr<System_Int32>* lengths, Arr<System_Byte>* data)
{
  Ref<System_String> local_3;
  System_Int32 vreg_2,vreg_5,vreg_8;

  vreg_2 = ((*startPos)[index]);
  vreg_5 = ((*lengths)[index]);
  vreg_8 = ((*codes)[index]);
  local_3 = Texts_BuildSystemString(vreg_8, data, vreg_2, vreg_5);
  return local_3;
}

Ref<System_String> Texts_BuildSystemString(System_Int32 code, Arr<System_Byte>* data, System_Int32 startPos, System_Int32 len)
{
  RefArr<System_Byte> vreg_1;
  Ref<System_String> vreg_7;

  vreg_1 = new_arr<System_Byte>(len);
  System_Array_Copy(data, startPos, vreg_1.get(), 0, len);
  vreg_7 = new_ref<System_String>(3);
  vreg_7->Data = nullptr;
  vreg_7->Coder = code;
  vreg_7->Data = vreg_1;
  return vreg_7;
}

System_Void System_Array_Copy(Arr<System_Byte>* sourceArray, System_Int32 sourceIndex, Arr<System_Byte>* destinationArray, System_Int32 destinationIndex, System_Int32 len)
{
  System_Int32 local_0,vreg_3,vreg_6,vreg_18;
  System_Byte vreg_9;

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

System_Void System_String_ctor(System_String* _this)
{
  _this->Data = nullptr;
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
