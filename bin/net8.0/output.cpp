#include "native_sharp.hpp"
// headers imported by native methods
#include <iostream>
#include <cmath>

struct System_String;
struct nbody;
struct NBodySystem;
struct Body;
struct Texts;
struct NativeSharp_Lib_ResolvedMethods;
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
struct NativeSharp_Lib_ResolvedMethods {};
namespace {
    Ref<System_String> _str(int index);
}
System_Void nbody_Main();
System_Void System_Console_WriteLine(System_Double value);
System_Void NBodySystem_ctor(NBodySystem _this);
Ref<Body> Body_sun();
System_Void Body_ctor(Body _this);
Ref<Body> Body_jupiter();
Ref<Body> Body_saturn();
Ref<Body> Body_uranus();
Ref<Body> Body_neptune();
System_Double NBodySystem_energy(NBodySystem _this);
System_Double System_Math_Sqrt(System_Double val);
System_Void NBodySystem_advance(NBodySystem _this, System_Double dt);
System_Void Body_set_X(Body _this, System_Double value);
System_Void Body_set_Y(Body _this, System_Double value);
System_Void Body_set_Z(Body _this, System_Double value);
Ref<Body> Body_offsetMomentum(Body _this, System_Double px, System_Double py, System_Double pz);
System_Double Body_get_X(Body _this);
System_Double Body_get_Y(Body _this);
System_Double Body_get_Z(Body _this);
System_Void NBodySystem_AdvanceTwoLoops(NBodySystem _this, System_Double dt);
System_Void NBodySystem_advanceInnerLoop(NBodySystem _this, System_Double dt, Body iBody, System_Int32 j);
System_Void NBodySystem_AdvanceBodiesEnergy(NBodySystem _this, System_Double dt);
Ref<System_String> Texts_FromIndex(System_Int32 index, RefArr<System_Int32> codes, RefArr<System_Int32> startPos, RefArr<System_Int32> lengths, RefArr<System_Byte> data);
Ref<System_String> Texts_BuildSystemString(System_Int32 code, RefArr<System_Byte> data, System_Int32 startPos, System_Int32 len);
System_Void System_Array_Copy(Arr<System_Byte>* sourceArray, System_Int32 sourceIndex, Arr<System_Byte>* destinationArray, System_Int32 destinationIndex, System_Int32 len);
System_Void System_String_ctor(System_String _this);
System_Void NativeSharp_Lib_ResolvedMethods_System_Console_WriteLine(System_Double value);
System_Double NativeSharp_Lib_ResolvedMethods_System_Math_Sqrt(System_Double val);
System_Void NativeSharp_Lib_ResolvedMethods_System_Array_Copy(RefArr<System_Byte> sourceArray, System_Int32 sourceIndex, RefArr<System_Byte> destinationArray, System_Int32 destinationIndex, System_Int32 len);
#include "native_sharp.cpp"
int main(int argc, char**argv) {
timeItMilliseconds(nbody_Main);
return 0;
}
System_Void nbody_Main()
{
  System_Int32 local_0,local_2,vreg_0,vreg_4,vreg_7,vreg_8,vreg_9,vreg_10,vreg_11,vreg_12;
  NBodySystem local_1,vreg_1,vreg_2,vreg_5,vreg_14;
  System_Boolean local_3,vreg_13;
  System_Double vreg_3,vreg_6,vreg_15;

  vreg_0 = 10000000;
  local_0 = vreg_0;
  vreg_1 = new_ref<NBodySystem>(1);
  NBodySystem_ctor(vreg_1);
  local_1 = vreg_1;
  vreg_2 = local_1;
  vreg_3 = NBodySystem_energy(vreg_2);
  System_Console_WriteLine(vreg_3);
  vreg_4 = 0;
  local_2 = vreg_4;
  goto label_49;
  label_29:
  vreg_5 = local_1;
  vreg_6 = 0.01;
  NBodySystem_advance(vreg_5, vreg_6);
  vreg_7 = local_2;
  vreg_8 = 1;
  vreg_9 = add (vreg_7, vreg_8);
  local_2 = vreg_9;
  label_49:
  vreg_10 = local_2;
  vreg_11 = local_0;
  vreg_12 = clt (vreg_10, vreg_11);
  local_3 = vreg_12;
  vreg_13 = local_3;
  if (brtrue_s(vreg_13)) goto label_29;
  vreg_14 = local_1;
  vreg_15 = NBodySystem_energy(vreg_14);
  System_Console_WriteLine(vreg_15);
}

System_Void System_Console_WriteLine(System_Double value) {
std::cout<<value<<'\n';
}

System_Void NBodySystem_ctor(NBodySystem _this)
{
  System_Double local_0,local_1,local_2,vreg_24,vreg_25,vreg_26,vreg_28,vreg_33,vreg_38,vreg_39,vreg_40,vreg_41,vreg_46,vreg_51,vreg_52,vreg_53,vreg_54,vreg_59,vreg_64,vreg_65,vreg_66,vreg_81,vreg_82,vreg_83;
  System_Int32 local_3,vreg_2,vreg_6,vreg_10,vreg_14,vreg_18,vreg_22,vreg_27,vreg_31,vreg_36,vreg_44,vreg_49,vreg_57,vreg_62,vreg_67,vreg_68,vreg_69,vreg_70,vreg_74,vreg_75,vreg_79;
  System_Boolean local_4,vreg_76;
  NBodySystem vreg_0,vreg_1,vreg_29,vreg_34,vreg_42,vreg_47,vreg_55,vreg_60,vreg_71,vreg_77;
  RefArr<Ref<Body>> vreg_3,vreg_4,vreg_5,vreg_8,vreg_9,vreg_12,vreg_13,vreg_16,vreg_17,vreg_20,vreg_21,vreg_30,vreg_35,vreg_43,vreg_48,vreg_56,vreg_61,vreg_72,vreg_78;
  Body vreg_7,vreg_11,vreg_15,vreg_19,vreg_23,vreg_32,vreg_37,vreg_45,vreg_50,vreg_58,vreg_63,vreg_80,vreg_84;
  System_UInt32 vreg_73;

  vreg_0 = _this;
  vreg_1 = _this;
  vreg_2 = 5;
  vreg_3 = new_arr<Ref<Body>>(vreg_2);
  vreg_4 = vreg_3;
  vreg_5 = vreg_3;
  vreg_6 = 0;
  vreg_7 = Body_sun();
  (*vreg_5)[vreg_6] = vreg_7;
  vreg_8 = vreg_4;
  vreg_9 = vreg_4;
  vreg_10 = 1;
  vreg_11 = Body_jupiter();
  (*vreg_9)[vreg_10] = vreg_11;
  vreg_12 = vreg_8;
  vreg_13 = vreg_8;
  vreg_14 = 2;
  vreg_15 = Body_saturn();
  (*vreg_13)[vreg_14] = vreg_15;
  vreg_16 = vreg_12;
  vreg_17 = vreg_12;
  vreg_18 = 3;
  vreg_19 = Body_uranus();
  (*vreg_17)[vreg_18] = vreg_19;
  vreg_20 = vreg_16;
  vreg_21 = vreg_16;
  vreg_22 = 4;
  vreg_23 = Body_neptune();
  (*vreg_21)[vreg_22] = vreg_23;
  vreg_1->bodies = vreg_20;
  vreg_24 = 0;
  local_0 = vreg_24;
  vreg_25 = 0;
  local_1 = vreg_25;
  vreg_26 = 0;
  local_2 = vreg_26;
  vreg_27 = 0;
  local_3 = vreg_27;
  goto label_190;
  label_94:
  vreg_28 = local_0;
  vreg_29 = _this;
  vreg_30 = vreg_29->bodies;
  vreg_31 = local_3;
  vreg_32 = ((*vreg_30)[vreg_31]);
  vreg_33 = vreg_32->Vx;
  vreg_34 = _this;
  vreg_35 = vreg_34->bodies;
  vreg_36 = local_3;
  vreg_37 = ((*vreg_35)[vreg_36]);
  vreg_38 = vreg_37->Mass;
  vreg_39 = mul (vreg_33, vreg_38);
  vreg_40 = add (vreg_28, vreg_39);
  local_0 = vreg_40;
  vreg_41 = local_1;
  vreg_42 = _this;
  vreg_43 = vreg_42->bodies;
  vreg_44 = local_3;
  vreg_45 = ((*vreg_43)[vreg_44]);
  vreg_46 = vreg_45->Vy;
  vreg_47 = _this;
  vreg_48 = vreg_47->bodies;
  vreg_49 = local_3;
  vreg_50 = ((*vreg_48)[vreg_49]);
  vreg_51 = vreg_50->Mass;
  vreg_52 = mul (vreg_46, vreg_51);
  vreg_53 = add (vreg_41, vreg_52);
  local_1 = vreg_53;
  vreg_54 = local_2;
  vreg_55 = _this;
  vreg_56 = vreg_55->bodies;
  vreg_57 = local_3;
  vreg_58 = ((*vreg_56)[vreg_57]);
  vreg_59 = vreg_58->Vz;
  vreg_60 = _this;
  vreg_61 = vreg_60->bodies;
  vreg_62 = local_3;
  vreg_63 = ((*vreg_61)[vreg_62]);
  vreg_64 = vreg_63->Mass;
  vreg_65 = mul (vreg_59, vreg_64);
  vreg_66 = add (vreg_54, vreg_65);
  local_2 = vreg_66;
  vreg_67 = local_3;
  vreg_68 = 1;
  vreg_69 = add (vreg_67, vreg_68);
  local_3 = vreg_69;
  label_190:
  vreg_70 = local_3;
  vreg_71 = _this;
  vreg_72 = vreg_71->bodies;
  vreg_73 = vreg_72->size();
  vreg_74 = conv_i4 (vreg_73);
  vreg_75 = clt (vreg_70, vreg_74);
  local_4 = vreg_75;
  vreg_76 = local_4;
  if (brtrue_s(vreg_76)) goto label_94;
  vreg_77 = _this;
  vreg_78 = vreg_77->bodies;
  vreg_79 = 0;
  vreg_80 = ((*vreg_78)[vreg_79]);
  vreg_81 = local_0;
  vreg_82 = local_1;
  vreg_83 = local_2;
  vreg_84 = Body_offsetMomentum(vreg_80, vreg_81, vreg_82, vreg_83);
}

Ref<Body> Body_sun()
{
  Body local_0,local_1,vreg_0,vreg_1,vreg_3,vreg_4;
  System_Double vreg_2;

  vreg_0 = new_ref<Body>(2);
  Body_ctor(vreg_0);
  local_0 = vreg_0;
  vreg_1 = local_0;
  vreg_2 = 39.47841760435743;
  vreg_1->Mass = vreg_2;
  vreg_3 = local_0;
  local_1 = vreg_3;
  goto label_26;
  label_26:
  vreg_4 = local_1;
  return vreg_4;
}

System_Void Body_ctor(Body _this)
{
  Body vreg_0;

  vreg_0 = _this;
}

Ref<Body> Body_jupiter()
{
  Body local_0,local_1,vreg_0,vreg_1,vreg_3,vreg_5,vreg_7,vreg_9,vreg_11,vreg_13,vreg_15,vreg_16;
  System_Double vreg_2,vreg_4,vreg_6,vreg_8,vreg_10,vreg_12,vreg_14;

  vreg_0 = new_ref<Body>(2);
  Body_ctor(vreg_0);
  local_0 = vreg_0;
  vreg_1 = local_0;
  vreg_2 = 4.841431442464721;
  Body_set_X(vreg_1, vreg_2);
  vreg_3 = local_0;
  vreg_4 = -1.1603200440274284;
  Body_set_Y(vreg_3, vreg_4);
  vreg_5 = local_0;
  vreg_6 = -0.10362204447112311;
  Body_set_Z(vreg_5, vreg_6);
  vreg_7 = local_0;
  vreg_8 = 0.606326392995832;
  vreg_7->Vx = vreg_8;
  vreg_9 = local_0;
  vreg_10 = 2.81198684491626;
  vreg_9->Vy = vreg_10;
  vreg_11 = local_0;
  vreg_12 = -0.02521836165988763;
  vreg_11->Vz = vreg_12;
  vreg_13 = local_0;
  vreg_14 = 0.03769367487038949;
  vreg_13->Mass = vreg_14;
  vreg_15 = local_0;
  local_1 = vreg_15;
  goto label_119;
  label_119:
  vreg_16 = local_1;
  return vreg_16;
}

Ref<Body> Body_saturn()
{
  Body local_0,local_1,vreg_0,vreg_1,vreg_3,vreg_5,vreg_7,vreg_9,vreg_11,vreg_13,vreg_15,vreg_16;
  System_Double vreg_2,vreg_4,vreg_6,vreg_8,vreg_10,vreg_12,vreg_14;

  vreg_0 = new_ref<Body>(2);
  Body_ctor(vreg_0);
  local_0 = vreg_0;
  vreg_1 = local_0;
  vreg_2 = 8.34336671824458;
  Body_set_X(vreg_1, vreg_2);
  vreg_3 = local_0;
  vreg_4 = 4.124798564124305;
  Body_set_Y(vreg_3, vreg_4);
  vreg_5 = local_0;
  vreg_6 = -0.4035234171143214;
  Body_set_Z(vreg_5, vreg_6);
  vreg_7 = local_0;
  vreg_8 = -1.0107743461787924;
  vreg_7->Vx = vreg_8;
  vreg_9 = local_0;
  vreg_10 = 1.8256623712304119;
  vreg_9->Vy = vreg_10;
  vreg_11 = local_0;
  vreg_12 = 0.008415761376584154;
  vreg_11->Vz = vreg_12;
  vreg_13 = local_0;
  vreg_14 = 0.011286326131968767;
  vreg_13->Mass = vreg_14;
  vreg_15 = local_0;
  local_1 = vreg_15;
  goto label_119;
  label_119:
  vreg_16 = local_1;
  return vreg_16;
}

Ref<Body> Body_uranus()
{
  Body local_0,local_1,vreg_0,vreg_1,vreg_3,vreg_5,vreg_7,vreg_9,vreg_11,vreg_13,vreg_15,vreg_16;
  System_Double vreg_2,vreg_4,vreg_6,vreg_8,vreg_10,vreg_12,vreg_14;

  vreg_0 = new_ref<Body>(2);
  Body_ctor(vreg_0);
  local_0 = vreg_0;
  vreg_1 = local_0;
  vreg_2 = 12.894369562139131;
  Body_set_X(vreg_1, vreg_2);
  vreg_3 = local_0;
  vreg_4 = -15.111151401698631;
  Body_set_Y(vreg_3, vreg_4);
  vreg_5 = local_0;
  vreg_6 = -0.22330757889265573;
  Body_set_Z(vreg_5, vreg_6);
  vreg_7 = local_0;
  vreg_8 = 1.0827910064415354;
  vreg_7->Vx = vreg_8;
  vreg_9 = local_0;
  vreg_10 = 0.8687130181696082;
  vreg_9->Vy = vreg_10;
  vreg_11 = local_0;
  vreg_12 = -0.010832637401363636;
  vreg_11->Vz = vreg_12;
  vreg_13 = local_0;
  vreg_14 = 0.0017237240570597112;
  vreg_13->Mass = vreg_14;
  vreg_15 = local_0;
  local_1 = vreg_15;
  goto label_119;
  label_119:
  vreg_16 = local_1;
  return vreg_16;
}

Ref<Body> Body_neptune()
{
  Body local_0,local_1,vreg_0,vreg_1,vreg_3,vreg_5,vreg_7,vreg_9,vreg_11,vreg_13,vreg_15,vreg_16;
  System_Double vreg_2,vreg_4,vreg_6,vreg_8,vreg_10,vreg_12,vreg_14;

  vreg_0 = new_ref<Body>(2);
  Body_ctor(vreg_0);
  local_0 = vreg_0;
  vreg_1 = local_0;
  vreg_2 = 15.379697114850917;
  Body_set_X(vreg_1, vreg_2);
  vreg_3 = local_0;
  vreg_4 = -25.919314609987964;
  Body_set_Y(vreg_3, vreg_4);
  vreg_5 = local_0;
  vreg_6 = 0.17925877295037118;
  Body_set_Z(vreg_5, vreg_6);
  vreg_7 = local_0;
  vreg_8 = 0.979090732243898;
  vreg_7->Vx = vreg_8;
  vreg_9 = local_0;
  vreg_10 = 0.5946989986476762;
  vreg_9->Vy = vreg_10;
  vreg_11 = local_0;
  vreg_12 = -0.034755955504078104;
  vreg_11->Vz = vreg_12;
  vreg_13 = local_0;
  vreg_14 = 0.0020336868699246304;
  vreg_13->Mass = vreg_14;
  vreg_15 = local_0;
  local_1 = vreg_15;
  goto label_119;
  label_119:
  vreg_16 = local_1;
  return vreg_16;
}

System_Double NBodySystem_energy(NBodySystem _this)
{
  System_Double local_0,local_1,local_2,local_3,local_4,local_11,vreg_0,vreg_6,vreg_7,vreg_9,vreg_10,vreg_12,vreg_14,vreg_15,vreg_17,vreg_19,vreg_20,vreg_21,vreg_23,vreg_25,vreg_26,vreg_27,vreg_28,vreg_29,vreg_38,vreg_40,vreg_41,vreg_43,vreg_45,vreg_46,vreg_48,vreg_50,vreg_51,vreg_52,vreg_53,vreg_54,vreg_55,vreg_56,vreg_57,vreg_58,vreg_59,vreg_60,vreg_61,vreg_62,vreg_63,vreg_64,vreg_66,vreg_68,vreg_69,vreg_70,vreg_71,vreg_72,vreg_93,vreg_94;
  System_Int32 local_5,local_7,vreg_1,vreg_4,vreg_30,vreg_31,vreg_32,vreg_35,vreg_73,vreg_74,vreg_75,vreg_76,vreg_80,vreg_81,vreg_83,vreg_84,vreg_85,vreg_86,vreg_90,vreg_91;
  Body local_6,local_8,vreg_5,vreg_8,vreg_11,vreg_13,vreg_16,vreg_18,vreg_22,vreg_24,vreg_36,vreg_37,vreg_39,vreg_42,vreg_44,vreg_47,vreg_49,vreg_65,vreg_67;
  System_Boolean local_9,local_10,vreg_82,vreg_92;
  NBodySystem vreg_2,vreg_33,vreg_77,vreg_87;
  RefArr<Ref<Body>> vreg_3,vreg_34,vreg_78,vreg_88;
  System_UInt32 vreg_79,vreg_89;

  vreg_0 = 0;
  local_4 = vreg_0;
  vreg_1 = 0;
  local_5 = vreg_1;
  goto label_241;
  label_20:
  vreg_2 = _this;
  vreg_3 = vreg_2->bodies;
  vreg_4 = local_5;
  vreg_5 = ((*vreg_3)[vreg_4]);
  local_6 = vreg_5;
  vreg_6 = local_4;
  vreg_7 = 0.5;
  vreg_8 = local_6;
  vreg_9 = vreg_8->Mass;
  vreg_10 = mul (vreg_7, vreg_9);
  vreg_11 = local_6;
  vreg_12 = vreg_11->Vx;
  vreg_13 = local_6;
  vreg_14 = vreg_13->Vx;
  vreg_15 = mul (vreg_12, vreg_14);
  vreg_16 = local_6;
  vreg_17 = vreg_16->Vy;
  vreg_18 = local_6;
  vreg_19 = vreg_18->Vy;
  vreg_20 = mul (vreg_17, vreg_19);
  vreg_21 = add (vreg_15, vreg_20);
  vreg_22 = local_6;
  vreg_23 = vreg_22->Vz;
  vreg_24 = local_6;
  vreg_25 = vreg_24->Vz;
  vreg_26 = mul (vreg_23, vreg_25);
  vreg_27 = add (vreg_21, vreg_26);
  vreg_28 = mul (vreg_10, vreg_27);
  vreg_29 = add (vreg_6, vreg_28);
  local_4 = vreg_29;
  vreg_30 = local_5;
  vreg_31 = 1;
  vreg_32 = add (vreg_30, vreg_31);
  local_7 = vreg_32;
  goto label_216;
  label_110:
  vreg_33 = _this;
  vreg_34 = vreg_33->bodies;
  vreg_35 = local_7;
  vreg_36 = ((*vreg_34)[vreg_35]);
  local_8 = vreg_36;
  vreg_37 = local_6;
  vreg_38 = Body_get_X(vreg_37);
  vreg_39 = local_8;
  vreg_40 = Body_get_X(vreg_39);
  vreg_41 = sub (vreg_38, vreg_40);
  local_0 = vreg_41;
  vreg_42 = local_6;
  vreg_43 = Body_get_Y(vreg_42);
  vreg_44 = local_8;
  vreg_45 = Body_get_Y(vreg_44);
  vreg_46 = sub (vreg_43, vreg_45);
  local_1 = vreg_46;
  vreg_47 = local_6;
  vreg_48 = Body_get_Z(vreg_47);
  vreg_49 = local_8;
  vreg_50 = Body_get_Z(vreg_49);
  vreg_51 = sub (vreg_48, vreg_50);
  local_2 = vreg_51;
  vreg_52 = local_0;
  vreg_53 = local_0;
  vreg_54 = mul (vreg_52, vreg_53);
  vreg_55 = local_1;
  vreg_56 = local_1;
  vreg_57 = mul (vreg_55, vreg_56);
  vreg_58 = add (vreg_54, vreg_57);
  vreg_59 = local_2;
  vreg_60 = local_2;
  vreg_61 = mul (vreg_59, vreg_60);
  vreg_62 = add (vreg_58, vreg_61);
  vreg_63 = System_Math_Sqrt(vreg_62);
  local_3 = vreg_63;
  vreg_64 = local_4;
  vreg_65 = local_6;
  vreg_66 = vreg_65->Mass;
  vreg_67 = local_8;
  vreg_68 = vreg_67->Mass;
  vreg_69 = mul (vreg_66, vreg_68);
  vreg_70 = local_3;
  vreg_71 = div (vreg_69, vreg_70);
  vreg_72 = sub (vreg_64, vreg_71);
  local_4 = vreg_72;
  vreg_73 = local_7;
  vreg_74 = 1;
  vreg_75 = add (vreg_73, vreg_74);
  local_7 = vreg_75;
  label_216:
  vreg_76 = local_7;
  vreg_77 = _this;
  vreg_78 = vreg_77->bodies;
  vreg_79 = vreg_78->size();
  vreg_80 = conv_i4 (vreg_79);
  vreg_81 = clt (vreg_76, vreg_80);
  local_9 = vreg_81;
  vreg_82 = local_9;
  if (brtrue_s(vreg_82)) goto label_110;
  vreg_83 = local_5;
  vreg_84 = 1;
  vreg_85 = add (vreg_83, vreg_84);
  local_5 = vreg_85;
  label_241:
  vreg_86 = local_5;
  vreg_87 = _this;
  vreg_88 = vreg_87->bodies;
  vreg_89 = vreg_88->size();
  vreg_90 = conv_i4 (vreg_89);
  vreg_91 = clt (vreg_86, vreg_90);
  local_10 = vreg_91;
  vreg_92 = local_10;
  if (brtrue(vreg_92)) goto label_20;
  vreg_93 = local_4;
  local_11 = vreg_93;
  goto label_268;
  label_268:
  vreg_94 = local_11;
  return vreg_94;
}

System_Double System_Math_Sqrt(System_Double val) {
return std::sqrt(val);
}

System_Void NBodySystem_advance(NBodySystem _this, System_Double dt)
{
  NBodySystem vreg_0,vreg_2;
  System_Double vreg_1,vreg_3;

  vreg_0 = _this;
  vreg_1 = dt;
  NBodySystem_AdvanceTwoLoops(vreg_0, vreg_1);
  vreg_2 = _this;
  vreg_3 = dt;
  NBodySystem_AdvanceBodiesEnergy(vreg_2, vreg_3);
}

System_Void Body_set_X(Body _this, System_Double value)
{
  Body vreg_0;
  System_Double vreg_1;

  vreg_0 = _this;
  vreg_1 = value;
  vreg_0->_X_k__BackingField = vreg_1;
}

System_Void Body_set_Y(Body _this, System_Double value)
{
  Body vreg_0;
  System_Double vreg_1;

  vreg_0 = _this;
  vreg_1 = value;
  vreg_0->_Y_k__BackingField = vreg_1;
}

System_Void Body_set_Z(Body _this, System_Double value)
{
  Body vreg_0;
  System_Double vreg_1;

  vreg_0 = _this;
  vreg_1 = value;
  vreg_0->_Z_k__BackingField = vreg_1;
}

Ref<Body> Body_offsetMomentum(Body _this, System_Double px, System_Double py, System_Double pz)
{
  Body local_0,vreg_0,vreg_5,vreg_10,vreg_15,vreg_16;
  System_Double vreg_1,vreg_2,vreg_3,vreg_4,vreg_6,vreg_7,vreg_8,vreg_9,vreg_11,vreg_12,vreg_13,vreg_14;

  vreg_0 = _this;
  vreg_1 = px;
  vreg_2 = neg (vreg_1);
  vreg_3 = 39.47841760435743;
  vreg_4 = div (vreg_2, vreg_3);
  vreg_0->Vx = vreg_4;
  vreg_5 = _this;
  vreg_6 = py;
  vreg_7 = neg (vreg_6);
  vreg_8 = 39.47841760435743;
  vreg_9 = div (vreg_7, vreg_8);
  vreg_5->Vy = vreg_9;
  vreg_10 = _this;
  vreg_11 = pz;
  vreg_12 = neg (vreg_11);
  vreg_13 = 39.47841760435743;
  vreg_14 = div (vreg_12, vreg_13);
  vreg_10->Vz = vreg_14;
  vreg_15 = _this;
  local_0 = vreg_15;
  goto label_59;
  label_59:
  vreg_16 = local_0;
  return vreg_16;
}

System_Double Body_get_X(Body _this)
{
  Body vreg_0;
  System_Double vreg_1;

  vreg_0 = _this;
  vreg_1 = vreg_0->_X_k__BackingField;
  return vreg_1;
}

System_Double Body_get_Y(Body _this)
{
  Body vreg_0;
  System_Double vreg_1;

  vreg_0 = _this;
  vreg_1 = vreg_0->_Y_k__BackingField;
  return vreg_1;
}

System_Double Body_get_Z(Body _this)
{
  Body vreg_0;
  System_Double vreg_1;

  vreg_0 = _this;
  vreg_1 = vreg_0->_Z_k__BackingField;
  return vreg_1;
}

System_Void NBodySystem_AdvanceTwoLoops(NBodySystem _this, System_Double dt)
{
  System_Int32 local_0,local_2,vreg_0,vreg_3,vreg_5,vreg_6,vreg_7,vreg_11,vreg_12,vreg_13,vreg_14,vreg_15,vreg_19,vreg_20,vreg_22,vreg_23,vreg_24,vreg_25,vreg_29,vreg_30;
  Body local_1,vreg_4,vreg_10;
  System_Boolean local_3,local_4,vreg_21,vreg_31;
  NBodySystem vreg_1,vreg_8,vreg_16,vreg_26;
  RefArr<Ref<Body>> vreg_2,vreg_17,vreg_27;
  System_Double vreg_9;
  System_UInt32 vreg_18,vreg_28;

  vreg_0 = 0;
  local_0 = vreg_0;
  goto label_57;
  label_5:
  vreg_1 = _this;
  vreg_2 = vreg_1->bodies;
  vreg_3 = local_0;
  vreg_4 = ((*vreg_2)[vreg_3]);
  local_1 = vreg_4;
  vreg_5 = local_0;
  vreg_6 = 1;
  vreg_7 = add (vreg_5, vreg_6);
  local_2 = vreg_7;
  goto label_37;
  label_21:
  vreg_8 = _this;
  vreg_9 = dt;
  vreg_10 = local_1;
  vreg_11 = local_2;
  NBodySystem_advanceInnerLoop(vreg_8, vreg_9, vreg_10, vreg_11);
  vreg_12 = local_2;
  vreg_13 = 1;
  vreg_14 = add (vreg_12, vreg_13);
  local_2 = vreg_14;
  label_37:
  vreg_15 = local_2;
  vreg_16 = _this;
  vreg_17 = vreg_16->bodies;
  vreg_18 = vreg_17->size();
  vreg_19 = conv_i4 (vreg_18);
  vreg_20 = clt (vreg_15, vreg_19);
  local_3 = vreg_20;
  vreg_21 = local_3;
  if (brtrue_s(vreg_21)) goto label_21;
  vreg_22 = local_0;
  vreg_23 = 1;
  vreg_24 = add (vreg_22, vreg_23);
  local_0 = vreg_24;
  label_57:
  vreg_25 = local_0;
  vreg_26 = _this;
  vreg_27 = vreg_26->bodies;
  vreg_28 = vreg_27->size();
  vreg_29 = conv_i4 (vreg_28);
  vreg_30 = clt (vreg_25, vreg_29);
  local_4 = vreg_30;
  vreg_31 = local_4;
  if (brtrue_s(vreg_31)) goto label_5;
}

System_Void NBodySystem_advanceInnerLoop(NBodySystem _this, System_Double dt, Body iBody, System_Int32 j)
{
  System_Double local_0,local_1,local_2,local_3,local_4,local_5,vreg_1,vreg_6,vreg_7,vreg_9,vreg_14,vreg_15,vreg_17,vreg_22,vreg_23,vreg_24,vreg_25,vreg_26,vreg_27,vreg_28,vreg_29,vreg_30,vreg_31,vreg_32,vreg_33,vreg_34,vreg_35,vreg_36,vreg_37,vreg_38,vreg_39,vreg_40,vreg_41,vreg_45,vreg_46,vreg_51,vreg_52,vreg_53,vreg_54,vreg_55,vreg_59,vreg_60,vreg_65,vreg_66,vreg_67,vreg_68,vreg_69,vreg_73,vreg_74,vreg_79,vreg_80,vreg_81,vreg_82,vreg_83,vreg_90,vreg_91,vreg_93,vreg_94,vreg_95,vreg_96,vreg_97,vreg_104,vreg_105,vreg_107,vreg_108,vreg_109,vreg_110,vreg_111,vreg_118,vreg_119,vreg_121,vreg_122,vreg_123,vreg_124,vreg_125;
  Body vreg_0,vreg_5,vreg_8,vreg_13,vreg_16,vreg_21,vreg_42,vreg_43,vreg_44,vreg_50,vreg_56,vreg_57,vreg_58,vreg_64,vreg_70,vreg_71,vreg_72,vreg_78,vreg_87,vreg_88,vreg_89,vreg_92,vreg_101,vreg_102,vreg_103,vreg_106,vreg_115,vreg_116,vreg_117,vreg_120;
  NBodySystem vreg_2,vreg_10,vreg_18,vreg_47,vreg_61,vreg_75,vreg_84,vreg_98,vreg_112;
  RefArr<Ref<Body>> vreg_3,vreg_11,vreg_19,vreg_48,vreg_62,vreg_76,vreg_85,vreg_99,vreg_113;
  System_Int32 vreg_4,vreg_12,vreg_20,vreg_49,vreg_63,vreg_77,vreg_86,vreg_100,vreg_114;

  vreg_0 = iBody;
  vreg_1 = Body_get_X(vreg_0);
  vreg_2 = _this;
  vreg_3 = vreg_2->bodies;
  vreg_4 = j;
  vreg_5 = ((*vreg_3)[vreg_4]);
  vreg_6 = Body_get_X(vreg_5);
  vreg_7 = sub (vreg_1, vreg_6);
  local_0 = vreg_7;
  vreg_8 = iBody;
  vreg_9 = Body_get_Y(vreg_8);
  vreg_10 = _this;
  vreg_11 = vreg_10->bodies;
  vreg_12 = j;
  vreg_13 = ((*vreg_11)[vreg_12]);
  vreg_14 = Body_get_Y(vreg_13);
  vreg_15 = sub (vreg_9, vreg_14);
  local_1 = vreg_15;
  vreg_16 = iBody;
  vreg_17 = Body_get_Z(vreg_16);
  vreg_18 = _this;
  vreg_19 = vreg_18->bodies;
  vreg_20 = j;
  vreg_21 = ((*vreg_19)[vreg_20]);
  vreg_22 = Body_get_Z(vreg_21);
  vreg_23 = sub (vreg_17, vreg_22);
  local_2 = vreg_23;
  vreg_24 = local_0;
  vreg_25 = local_0;
  vreg_26 = mul (vreg_24, vreg_25);
  vreg_27 = local_1;
  vreg_28 = local_1;
  vreg_29 = mul (vreg_27, vreg_28);
  vreg_30 = add (vreg_26, vreg_29);
  vreg_31 = local_2;
  vreg_32 = local_2;
  vreg_33 = mul (vreg_31, vreg_32);
  vreg_34 = add (vreg_30, vreg_33);
  local_3 = vreg_34;
  vreg_35 = local_3;
  vreg_36 = System_Math_Sqrt(vreg_35);
  local_4 = vreg_36;
  vreg_37 = dt;
  vreg_38 = local_3;
  vreg_39 = local_4;
  vreg_40 = mul (vreg_38, vreg_39);
  vreg_41 = div (vreg_37, vreg_40);
  local_5 = vreg_41;
  vreg_42 = iBody;
  vreg_43 = vreg_42;
  vreg_44 = vreg_42;
  vreg_45 = vreg_44->Vx;
  vreg_46 = local_0;
  vreg_47 = _this;
  vreg_48 = vreg_47->bodies;
  vreg_49 = j;
  vreg_50 = ((*vreg_48)[vreg_49]);
  vreg_51 = vreg_50->Mass;
  vreg_52 = mul (vreg_46, vreg_51);
  vreg_53 = local_5;
  vreg_54 = mul (vreg_52, vreg_53);
  vreg_55 = sub (vreg_45, vreg_54);
  vreg_43->Vx = vreg_55;
  vreg_56 = iBody;
  vreg_57 = vreg_56;
  vreg_58 = vreg_56;
  vreg_59 = vreg_58->Vy;
  vreg_60 = local_1;
  vreg_61 = _this;
  vreg_62 = vreg_61->bodies;
  vreg_63 = j;
  vreg_64 = ((*vreg_62)[vreg_63]);
  vreg_65 = vreg_64->Mass;
  vreg_66 = mul (vreg_60, vreg_65);
  vreg_67 = local_5;
  vreg_68 = mul (vreg_66, vreg_67);
  vreg_69 = sub (vreg_59, vreg_68);
  vreg_57->Vy = vreg_69;
  vreg_70 = iBody;
  vreg_71 = vreg_70;
  vreg_72 = vreg_70;
  vreg_73 = vreg_72->Vz;
  vreg_74 = local_2;
  vreg_75 = _this;
  vreg_76 = vreg_75->bodies;
  vreg_77 = j;
  vreg_78 = ((*vreg_76)[vreg_77]);
  vreg_79 = vreg_78->Mass;
  vreg_80 = mul (vreg_74, vreg_79);
  vreg_81 = local_5;
  vreg_82 = mul (vreg_80, vreg_81);
  vreg_83 = sub (vreg_73, vreg_82);
  vreg_71->Vz = vreg_83;
  vreg_84 = _this;
  vreg_85 = vreg_84->bodies;
  vreg_86 = j;
  vreg_87 = ((*vreg_85)[vreg_86]);
  vreg_88 = vreg_87;
  vreg_89 = vreg_87;
  vreg_90 = vreg_89->Vx;
  vreg_91 = local_0;
  vreg_92 = iBody;
  vreg_93 = vreg_92->Mass;
  vreg_94 = mul (vreg_91, vreg_93);
  vreg_95 = local_5;
  vreg_96 = mul (vreg_94, vreg_95);
  vreg_97 = add (vreg_90, vreg_96);
  vreg_88->Vx = vreg_97;
  vreg_98 = _this;
  vreg_99 = vreg_98->bodies;
  vreg_100 = j;
  vreg_101 = ((*vreg_99)[vreg_100]);
  vreg_102 = vreg_101;
  vreg_103 = vreg_101;
  vreg_104 = vreg_103->Vy;
  vreg_105 = local_1;
  vreg_106 = iBody;
  vreg_107 = vreg_106->Mass;
  vreg_108 = mul (vreg_105, vreg_107);
  vreg_109 = local_5;
  vreg_110 = mul (vreg_108, vreg_109);
  vreg_111 = add (vreg_104, vreg_110);
  vreg_102->Vy = vreg_111;
  vreg_112 = _this;
  vreg_113 = vreg_112->bodies;
  vreg_114 = j;
  vreg_115 = ((*vreg_113)[vreg_114]);
  vreg_116 = vreg_115;
  vreg_117 = vreg_115;
  vreg_118 = vreg_117->Vz;
  vreg_119 = local_2;
  vreg_120 = iBody;
  vreg_121 = vreg_120->Mass;
  vreg_122 = mul (vreg_119, vreg_121);
  vreg_123 = local_5;
  vreg_124 = mul (vreg_122, vreg_123);
  vreg_125 = add (vreg_118, vreg_124);
  vreg_116->Vz = vreg_125;
}

System_Void NBodySystem_AdvanceBodiesEnergy(NBodySystem _this, System_Double dt)
{
  RefArr<Ref<Body>> local_0,vreg_1,vreg_3,vreg_37;
  System_Int32 local_1,vreg_2,vreg_4,vreg_33,vreg_34,vreg_35,vreg_36,vreg_39;
  Body local_2,vreg_5,vreg_6,vreg_7,vreg_8,vreg_11,vreg_15,vreg_16,vreg_17,vreg_20,vreg_24,vreg_25,vreg_26,vreg_29;
  NBodySystem vreg_0;
  System_Double vreg_9,vreg_10,vreg_12,vreg_13,vreg_14,vreg_18,vreg_19,vreg_21,vreg_22,vreg_23,vreg_27,vreg_28,vreg_30,vreg_31,vreg_32;
  System_UInt32 vreg_38;
  System_Boolean vreg_40;

  vreg_0 = _this;
  vreg_1 = vreg_0->bodies;
  local_0 = vreg_1;
  vreg_2 = 0;
  local_1 = vreg_2;
  goto label_89;
  label_13:
  vreg_3 = local_0;
  vreg_4 = local_1;
  vreg_5 = ((*vreg_3)[vreg_4]);
  local_2 = vreg_5;
  vreg_6 = local_2;
  vreg_7 = vreg_6;
  vreg_8 = vreg_6;
  vreg_9 = Body_get_X(vreg_8);
  vreg_10 = dt;
  vreg_11 = local_2;
  vreg_12 = vreg_11->Vx;
  vreg_13 = mul (vreg_10, vreg_12);
  vreg_14 = add (vreg_9, vreg_13);
  Body_set_X(vreg_7, vreg_14);
  vreg_15 = local_2;
  vreg_16 = vreg_15;
  vreg_17 = vreg_15;
  vreg_18 = Body_get_Y(vreg_17);
  vreg_19 = dt;
  vreg_20 = local_2;
  vreg_21 = vreg_20->Vy;
  vreg_22 = mul (vreg_19, vreg_21);
  vreg_23 = add (vreg_18, vreg_22);
  Body_set_Y(vreg_16, vreg_23);
  vreg_24 = local_2;
  vreg_25 = vreg_24;
  vreg_26 = vreg_24;
  vreg_27 = Body_get_Z(vreg_26);
  vreg_28 = dt;
  vreg_29 = local_2;
  vreg_30 = vreg_29->Vz;
  vreg_31 = mul (vreg_28, vreg_30);
  vreg_32 = add (vreg_27, vreg_31);
  Body_set_Z(vreg_25, vreg_32);
  vreg_33 = local_1;
  vreg_34 = 1;
  vreg_35 = add (vreg_33, vreg_34);
  local_1 = vreg_35;
  label_89:
  vreg_36 = local_1;
  vreg_37 = local_0;
  vreg_38 = vreg_37->size();
  vreg_39 = conv_i4 (vreg_38);
  vreg_40 = blt_s (vreg_39, vreg_36);
  if (brtrue(vreg_40)) goto label_13;
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
  System_Array_Copy(vreg_2.get(), vreg_3, vreg_4.get(), vreg_5, vreg_6);
  vreg_7 = new_ref<System_String>(3);
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

System_Void System_String_ctor(System_String _this)
{
  System_String vreg_0,vreg_1,vreg_2;

  vreg_0 = _this;
  vreg_1 = nullptr;
  vreg_0->Data = vreg_1;
  vreg_2 = _this;
}

System_Void NativeSharp_Lib_ResolvedMethods_System_Console_WriteLine(System_Double value)
{
}

System_Double NativeSharp_Lib_ResolvedMethods_System_Math_Sqrt(System_Double val)
{
  System_Double local_0,vreg_0,vreg_1,vreg_2;

  vreg_0 = val;
  vreg_1 = System_Math_Sqrt(vreg_0);
  local_0 = vreg_1;
  goto label_10;
  label_10:
  vreg_2 = local_0;
  return vreg_2;
}

System_Void NativeSharp_Lib_ResolvedMethods_System_Array_Copy(RefArr<System_Byte> sourceArray, System_Int32 sourceIndex, RefArr<System_Byte> destinationArray, System_Int32 destinationIndex, System_Int32 len)
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

namespace {
    RefArr<int> _coders = makeArr<int> ({});
    RefArr<int> _startPos = makeArr<int> ({});
    RefArr<int> _lengths = makeArr<int> ({});
    RefArr<uint8_t> _joinedTexts = makeArr<uint8_t> ({});
    Ref<System_String> _str(int index) {
       return Texts_FromIndex(index, _coders.get(), _startPos.get(), _lengths.get(), _joinedTexts.get());
    }
}
