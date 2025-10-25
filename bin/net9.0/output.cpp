#include "native_sharp.hpp"
// headers imported by native methods
#include <iostream>
#include <cmath>

struct System_String;
struct Nbody;
struct NBodySystem;
struct Body;
struct Texts;
struct System_String {
  int32_t Coder;
  RefArr<uint8_t> Data;
};
struct Nbody {};
struct NBodySystem {
  RefArr<Ref<Body>> bodies;
};
struct Body {
  double _X_k__BackingField,_Y_k__BackingField,_Z_k__BackingField,Vx,Vy,Vz,Mass;
};
struct Texts {};
namespace {
    Ref<System_String> _str(int index);
}
void Nbody_Main();
void Nbody_Run();
void System_Console_WriteLine(double value);
void NBodySystem_ctor(Ref<NBodySystem>& _this);
Ref<Body> Body_Jupiter();
Ref<Body> Body_Saturn();
Ref<Body> Body_Uranus();
Ref<Body> Body_Neptune();
Ref<Body> Body_OffsetMomentum(Ref<Body>& _this, double px, double py, double pz);
double NBodySystem_Energy(Ref<NBodySystem>& _this);
double System_Math_Sqrt(double val);
void NBodySystem_Advance(Ref<NBodySystem>& _this, double dt);
void NBodySystem_AdvanceTwoLoops(Ref<NBodySystem>& _this, double dt);
void NBodySystem_AdvanceInnerLoop(Ref<NBodySystem>& _this, double dt, Ref<Body>& iBody, int32_t j);
void NBodySystem_AdvanceBodiesEnergy(Ref<NBodySystem>& _this, double dt);
Ref<System_String> Texts_FromIndex(int32_t index, RefArr<int32_t>& codes, RefArr<int32_t>& startPos, RefArr<int32_t>& lengths, RefArr<uint8_t>& data);
Ref<System_String> Texts_BuildSystemString(int32_t code, RefArr<uint8_t>& data, int32_t startPos, int32_t len);
void System_Array_Copy(RefArr<uint8_t>& sourceArray, int32_t sourceIndex, RefArr<uint8_t>& destinationArray, int32_t destinationIndex, int32_t len);
#include "native_sharp.cpp"
int main(int argc, char**argv) {
timeItMilliseconds(Nbody_Main);
return 0;
}
void Nbody_Main()
{
  Nbody_Run();
}

void Nbody_Run()
{
  int32_t local_0,local_2,vreg_12;
  Ref<NBodySystem> local_1,vreg_1;
  double vreg_3,vreg_15;

  local_0 = 100000000;
  vreg_1 = new_ref<NBodySystem>(0);
  NBodySystem_ctor(vreg_1);
  local_1 = vreg_1;
  vreg_3 = NBodySystem_Energy(vreg_1);
  System_Console_WriteLine(vreg_3);
  local_2 = 0;
  goto label_49;
  label_29:
  NBodySystem_Advance(local_1, 0.01);
  local_2 = add (local_2, 1);
  label_49:
  vreg_12 = clt (local_2, local_0);
  if (brtrue(vreg_12)) goto label_29;
  vreg_15 = NBodySystem_Energy(local_1);
  System_Console_WriteLine(vreg_15);
}

void System_Console_WriteLine(double value) {
std::cout<<value<<'\n';
}

void NBodySystem_ctor(Ref<NBodySystem>& _this)
{
  double local_0,local_1,local_2,vreg_33,vreg_38,vreg_39,vreg_46,vreg_51,vreg_52,vreg_59,vreg_64,vreg_65;
  int32_t local_3,vreg_74,vreg_75;
  RefArr<Ref<Body>> vreg_3,vreg_30,vreg_35,vreg_43,vreg_48,vreg_56,vreg_61,vreg_72,vreg_78;
  Ref<Body> vreg_11,vreg_15,vreg_19,vreg_23,vreg_32,vreg_37,vreg_45,vreg_50,vreg_58,vreg_63,vreg_80,vreg_84,vreg_86;
  uint32_t vreg_73;

  vreg_3 = new_arr<Ref<Body>>(5);
  vreg_86 = new_ref<Body>(0);
  vreg_86->Mass = 39.47841760435743;
  (*vreg_3)[0] = vreg_86;
  vreg_11 = Body_Jupiter();
  (*vreg_3)[1] = vreg_11;
  vreg_15 = Body_Saturn();
  (*vreg_3)[2] = vreg_15;
  vreg_19 = Body_Uranus();
  (*vreg_3)[3] = vreg_19;
  vreg_23 = Body_Neptune();
  (*vreg_3)[4] = vreg_23;
  _this->bodies = vreg_3;
  local_0 = 0;
  local_1 = 0;
  local_2 = 0;
  local_3 = 0;
  goto label_190;
  label_94:
  vreg_30 = _this->bodies;
  vreg_32 = ((*vreg_30)[local_3]);
  vreg_33 = vreg_32->Vx;
  vreg_35 = _this->bodies;
  vreg_37 = ((*vreg_35)[local_3]);
  vreg_38 = vreg_37->Mass;
  vreg_39 = mul (vreg_33, vreg_38);
  local_0 = add (local_0, vreg_39);
  vreg_43 = _this->bodies;
  vreg_45 = ((*vreg_43)[local_3]);
  vreg_46 = vreg_45->Vy;
  vreg_48 = _this->bodies;
  vreg_50 = ((*vreg_48)[local_3]);
  vreg_51 = vreg_50->Mass;
  vreg_52 = mul (vreg_46, vreg_51);
  local_1 = add (local_1, vreg_52);
  vreg_56 = _this->bodies;
  vreg_58 = ((*vreg_56)[local_3]);
  vreg_59 = vreg_58->Vz;
  vreg_61 = _this->bodies;
  vreg_63 = ((*vreg_61)[local_3]);
  vreg_64 = vreg_63->Mass;
  vreg_65 = mul (vreg_59, vreg_64);
  local_2 = add (local_2, vreg_65);
  local_3 = add (local_3, 1);
  label_190:
  vreg_72 = _this->bodies;
  vreg_73 = vreg_72->size();
  vreg_74 = conv_i4 (vreg_73);
  vreg_75 = clt (local_3, vreg_74);
  if (brtrue(vreg_75)) goto label_94;
  vreg_78 = _this->bodies;
  vreg_80 = ((*vreg_78)[0]);
  vreg_84 = Body_OffsetMomentum(vreg_80, local_0, local_1, local_2);
}

Ref<Body> Body_Jupiter()
{
  Ref<Body> vreg_0;

  vreg_0 = new_ref<Body>(0);
  vreg_0->_X_k__BackingField = 4.841431442464721;
  vreg_0->_Y_k__BackingField = -1.1603200440274284;
  vreg_0->_Z_k__BackingField = -0.10362204447112311;
  vreg_0->Vx = 0.606326392995832;
  vreg_0->Vy = 2.81198684491626;
  vreg_0->Vz = -0.02521836165988763;
  vreg_0->Mass = 0.03769367487038949;
  return vreg_0;
}

Ref<Body> Body_Saturn()
{
  Ref<Body> vreg_0;

  vreg_0 = new_ref<Body>(0);
  vreg_0->_X_k__BackingField = 8.34336671824458;
  vreg_0->_Y_k__BackingField = 4.124798564124305;
  vreg_0->_Z_k__BackingField = -0.4035234171143214;
  vreg_0->Vx = -1.0107743461787924;
  vreg_0->Vy = 1.8256623712304119;
  vreg_0->Vz = 0.008415761376584154;
  vreg_0->Mass = 0.011286326131968767;
  return vreg_0;
}

Ref<Body> Body_Uranus()
{
  Ref<Body> vreg_0;

  vreg_0 = new_ref<Body>(0);
  vreg_0->_X_k__BackingField = 12.894369562139131;
  vreg_0->_Y_k__BackingField = -15.111151401698631;
  vreg_0->_Z_k__BackingField = -0.22330757889265573;
  vreg_0->Vx = 1.0827910064415354;
  vreg_0->Vy = 0.8687130181696082;
  vreg_0->Vz = -0.010832637401363636;
  vreg_0->Mass = 0.0017237240570597112;
  return vreg_0;
}

Ref<Body> Body_Neptune()
{
  Ref<Body> vreg_0;

  vreg_0 = new_ref<Body>(0);
  vreg_0->_X_k__BackingField = 15.379697114850917;
  vreg_0->_Y_k__BackingField = -25.919314609987964;
  vreg_0->_Z_k__BackingField = 0.17925877295037118;
  vreg_0->Vx = 0.979090732243898;
  vreg_0->Vy = 0.5946989986476762;
  vreg_0->Vz = -0.034755955504078104;
  vreg_0->Mass = 0.0020336868699246304;
  return vreg_0;
}

Ref<Body> Body_OffsetMomentum(Ref<Body>& _this, double px, double py, double pz)
{
  double vreg_2,vreg_4,vreg_7,vreg_9,vreg_12,vreg_14;

  vreg_2 = neg (px);
  vreg_4 = mul (vreg_2, 0.025330295910584444);
  _this->Vx = vreg_4;
  vreg_7 = neg (py);
  vreg_9 = mul (vreg_7, 0.025330295910584444);
  _this->Vy = vreg_9;
  vreg_12 = neg (pz);
  vreg_14 = mul (vreg_12, 0.025330295910584444);
  _this->Vz = vreg_14;
  return _this;
}

double NBodySystem_Energy(Ref<NBodySystem>& _this)
{
  double local_4,vreg_9,vreg_10,vreg_12,vreg_14,vreg_15,vreg_17,vreg_19,vreg_20,vreg_21,vreg_23,vreg_25,vreg_26,vreg_27,vreg_28,vreg_41,vreg_46,vreg_51,vreg_54,vreg_57,vreg_58,vreg_61,vreg_62,vreg_63,vreg_66,vreg_68,vreg_69,vreg_71,vreg_93,vreg_95,vreg_97,vreg_99,vreg_101,vreg_103;
  int32_t local_5,local_7,vreg_80,vreg_81,vreg_90,vreg_91;
  Ref<Body> local_6,vreg_5,vreg_36;
  RefArr<Ref<Body>> vreg_3,vreg_34,vreg_78,vreg_88;
  uint32_t vreg_79,vreg_89;

  local_4 = 0;
  local_5 = 0;
  goto label_241;
  label_20:
  vreg_3 = _this->bodies;
  vreg_5 = ((*vreg_3)[local_5]);
  local_6 = vreg_5;
  vreg_9 = vreg_5->Mass;
  vreg_10 = mul (0.5, vreg_9);
  vreg_12 = vreg_5->Vx;
  vreg_14 = vreg_5->Vx;
  vreg_15 = mul (vreg_12, vreg_14);
  vreg_17 = vreg_5->Vy;
  vreg_19 = vreg_5->Vy;
  vreg_20 = mul (vreg_17, vreg_19);
  vreg_21 = add (vreg_15, vreg_20);
  vreg_23 = vreg_5->Vz;
  vreg_25 = vreg_5->Vz;
  vreg_26 = mul (vreg_23, vreg_25);
  vreg_27 = add (vreg_21, vreg_26);
  vreg_28 = mul (vreg_10, vreg_27);
  local_4 = add (local_4, vreg_28);
  local_7 = add (local_5, 1);
  goto label_216;
  label_110:
  vreg_34 = _this->bodies;
  vreg_36 = ((*vreg_34)[local_7]);
  vreg_93 = local_6->_X_k__BackingField;
  vreg_95 = vreg_36->_X_k__BackingField;
  vreg_41 = sub (vreg_93, vreg_95);
  vreg_97 = local_6->_Y_k__BackingField;
  vreg_99 = vreg_36->_Y_k__BackingField;
  vreg_46 = sub (vreg_97, vreg_99);
  vreg_101 = local_6->_Z_k__BackingField;
  vreg_103 = vreg_36->_Z_k__BackingField;
  vreg_51 = sub (vreg_101, vreg_103);
  vreg_54 = mul (vreg_41, vreg_41);
  vreg_57 = mul (vreg_46, vreg_46);
  vreg_58 = add (vreg_54, vreg_57);
  vreg_61 = mul (vreg_51, vreg_51);
  vreg_62 = add (vreg_58, vreg_61);
  vreg_63 = System_Math_Sqrt(vreg_62);
  vreg_66 = local_6->Mass;
  vreg_68 = vreg_36->Mass;
  vreg_69 = mul (vreg_66, vreg_68);
  vreg_71 = div (vreg_69, vreg_63);
  local_4 = sub (local_4, vreg_71);
  local_7 = add (local_7, 1);
  label_216:
  vreg_78 = _this->bodies;
  vreg_79 = vreg_78->size();
  vreg_80 = conv_i4 (vreg_79);
  vreg_81 = clt (local_7, vreg_80);
  if (brtrue(vreg_81)) goto label_110;
  local_5 = add (local_5, 1);
  label_241:
  vreg_88 = _this->bodies;
  vreg_89 = vreg_88->size();
  vreg_90 = conv_i4 (vreg_89);
  vreg_91 = clt (local_5, vreg_90);
  if (brtrue(vreg_91)) goto label_20;
  return local_4;
}

double System_Math_Sqrt(double val) {
return std::sqrt(val);
}

void NBodySystem_Advance(Ref<NBodySystem>& _this, double dt)
{
  NBodySystem_AdvanceTwoLoops(_this, dt);
  NBodySystem_AdvanceBodiesEnergy(_this, dt);
}

void NBodySystem_AdvanceTwoLoops(Ref<NBodySystem>& _this, double dt)
{
  int32_t local_0,local_2,vreg_19,vreg_20,vreg_29,vreg_30;
  Ref<Body> local_1;
  RefArr<Ref<Body>> vreg_2,vreg_17,vreg_27;
  uint32_t vreg_18,vreg_28;

  local_0 = 0;
  goto label_57;
  label_5:
  vreg_2 = _this->bodies;
  local_1 = ((*vreg_2)[local_0]);
  local_2 = add (local_0, 1);
  goto label_37;
  label_21:
  NBodySystem_AdvanceInnerLoop(_this, dt, local_1, local_2);
  local_2 = add (local_2, 1);
  label_37:
  vreg_17 = _this->bodies;
  vreg_18 = vreg_17->size();
  vreg_19 = conv_i4 (vreg_18);
  vreg_20 = clt (local_2, vreg_19);
  if (brtrue(vreg_20)) goto label_21;
  local_0 = add (local_0, 1);
  label_57:
  vreg_27 = _this->bodies;
  vreg_28 = vreg_27->size();
  vreg_29 = conv_i4 (vreg_28);
  vreg_30 = clt (local_0, vreg_29);
  if (brtrue(vreg_30)) goto label_5;
}

void NBodySystem_AdvanceInnerLoop(Ref<NBodySystem>& _this, double dt, Ref<Body>& iBody, int32_t j)
{
  RefArr<Ref<Body>> vreg_3,vreg_11,vreg_19,vreg_48,vreg_62,vreg_76,vreg_85,vreg_99,vreg_113;
  Ref<Body> vreg_5,vreg_13,vreg_21,vreg_50,vreg_64,vreg_78,vreg_87,vreg_101,vreg_115;
  double vreg_7,vreg_15,vreg_23,vreg_26,vreg_29,vreg_30,vreg_33,vreg_34,vreg_36,vreg_40,vreg_41,vreg_45,vreg_51,vreg_52,vreg_54,vreg_55,vreg_59,vreg_65,vreg_66,vreg_68,vreg_69,vreg_73,vreg_79,vreg_80,vreg_82,vreg_83,vreg_90,vreg_93,vreg_94,vreg_96,vreg_97,vreg_104,vreg_107,vreg_108,vreg_110,vreg_111,vreg_118,vreg_121,vreg_122,vreg_124,vreg_125,vreg_127,vreg_129,vreg_131,vreg_133,vreg_135,vreg_137;

  vreg_127 = iBody->_X_k__BackingField;
  vreg_3 = _this->bodies;
  vreg_5 = ((*vreg_3)[j]);
  vreg_129 = vreg_5->_X_k__BackingField;
  vreg_7 = sub (vreg_127, vreg_129);
  vreg_131 = iBody->_Y_k__BackingField;
  vreg_11 = _this->bodies;
  vreg_13 = ((*vreg_11)[j]);
  vreg_133 = vreg_13->_Y_k__BackingField;
  vreg_15 = sub (vreg_131, vreg_133);
  vreg_135 = iBody->_Z_k__BackingField;
  vreg_19 = _this->bodies;
  vreg_21 = ((*vreg_19)[j]);
  vreg_137 = vreg_21->_Z_k__BackingField;
  vreg_23 = sub (vreg_135, vreg_137);
  vreg_26 = mul (vreg_7, vreg_7);
  vreg_29 = mul (vreg_15, vreg_15);
  vreg_30 = add (vreg_26, vreg_29);
  vreg_33 = mul (vreg_23, vreg_23);
  vreg_34 = add (vreg_30, vreg_33);
  vreg_36 = System_Math_Sqrt(vreg_34);
  vreg_40 = mul (vreg_34, vreg_36);
  vreg_41 = div (dt, vreg_40);
  vreg_45 = iBody->Vx;
  vreg_48 = _this->bodies;
  vreg_50 = ((*vreg_48)[j]);
  vreg_51 = vreg_50->Mass;
  vreg_52 = mul (vreg_7, vreg_51);
  vreg_54 = mul (vreg_52, vreg_41);
  vreg_55 = sub (vreg_45, vreg_54);
  iBody->Vx = vreg_55;
  vreg_59 = iBody->Vy;
  vreg_62 = _this->bodies;
  vreg_64 = ((*vreg_62)[j]);
  vreg_65 = vreg_64->Mass;
  vreg_66 = mul (vreg_15, vreg_65);
  vreg_68 = mul (vreg_66, vreg_41);
  vreg_69 = sub (vreg_59, vreg_68);
  iBody->Vy = vreg_69;
  vreg_73 = iBody->Vz;
  vreg_76 = _this->bodies;
  vreg_78 = ((*vreg_76)[j]);
  vreg_79 = vreg_78->Mass;
  vreg_80 = mul (vreg_23, vreg_79);
  vreg_82 = mul (vreg_80, vreg_41);
  vreg_83 = sub (vreg_73, vreg_82);
  iBody->Vz = vreg_83;
  vreg_85 = _this->bodies;
  vreg_87 = ((*vreg_85)[j]);
  vreg_90 = vreg_87->Vx;
  vreg_93 = iBody->Mass;
  vreg_94 = mul (vreg_7, vreg_93);
  vreg_96 = mul (vreg_94, vreg_41);
  vreg_97 = add (vreg_90, vreg_96);
  vreg_87->Vx = vreg_97;
  vreg_99 = _this->bodies;
  vreg_101 = ((*vreg_99)[j]);
  vreg_104 = vreg_101->Vy;
  vreg_107 = iBody->Mass;
  vreg_108 = mul (vreg_15, vreg_107);
  vreg_110 = mul (vreg_108, vreg_41);
  vreg_111 = add (vreg_104, vreg_110);
  vreg_101->Vy = vreg_111;
  vreg_113 = _this->bodies;
  vreg_115 = ((*vreg_113)[j]);
  vreg_118 = vreg_115->Vz;
  vreg_121 = iBody->Mass;
  vreg_122 = mul (vreg_23, vreg_121);
  vreg_124 = mul (vreg_122, vreg_41);
  vreg_125 = add (vreg_118, vreg_124);
  vreg_115->Vz = vreg_125;
}

void NBodySystem_AdvanceBodiesEnergy(Ref<NBodySystem>& _this, double dt)
{
  RefArr<Ref<Body>> local_0,vreg_37;
  int32_t local_1,vreg_39;
  Ref<Body> vreg_5;
  double vreg_12,vreg_13,vreg_14,vreg_21,vreg_22,vreg_23,vreg_30,vreg_31,vreg_32,vreg_42,vreg_44,vreg_46;
  uint32_t vreg_38;
  bool vreg_40;

  local_0 = _this->bodies;
  local_1 = 0;
  goto label_89;
  label_13:
  vreg_5 = ((*local_0)[local_1]);
  vreg_42 = vreg_5->_X_k__BackingField;
  vreg_12 = vreg_5->Vx;
  vreg_13 = mul (dt, vreg_12);
  vreg_14 = add (vreg_42, vreg_13);
  vreg_5->_X_k__BackingField = vreg_14;
  vreg_44 = vreg_5->_Y_k__BackingField;
  vreg_21 = vreg_5->Vy;
  vreg_22 = mul (dt, vreg_21);
  vreg_23 = add (vreg_44, vreg_22);
  vreg_5->_Y_k__BackingField = vreg_23;
  vreg_46 = vreg_5->_Z_k__BackingField;
  vreg_30 = vreg_5->Vz;
  vreg_31 = mul (dt, vreg_30);
  vreg_32 = add (vreg_46, vreg_31);
  vreg_5->_Z_k__BackingField = vreg_32;
  local_1 = add (local_1, 1);
  label_89:
  vreg_37 = local_0;
  vreg_38 = vreg_37->size();
  vreg_39 = conv_i4 (vreg_38);
  vreg_40 = blt_s (local_1, vreg_39);
  if (brtrue(vreg_40)) goto label_13;
}

Ref<System_String> Texts_FromIndex(int32_t index, RefArr<int32_t>& codes, RefArr<int32_t>& startPos, RefArr<int32_t>& lengths, RefArr<uint8_t>& data)
{
  Ref<System_String> local_3;
  int32_t vreg_2,vreg_5,vreg_8;

  vreg_2 = ((*startPos)[index]);
  vreg_5 = ((*lengths)[index]);
  vreg_8 = ((*codes)[index]);
  local_3 = Texts_BuildSystemString(vreg_8, data, vreg_2, vreg_5);
  return local_3;
}

Ref<System_String> Texts_BuildSystemString(int32_t code, RefArr<uint8_t>& data, int32_t startPos, int32_t len)
{
  RefArr<uint8_t> vreg_1;
  Ref<System_String> vreg_7;

  vreg_1 = new_arr<uint8_t>(len);
  System_Array_Copy(data, startPos, vreg_1, 0, len);
  vreg_7 = new_ref<System_String>(0);
  vreg_7->Coder = code;
  vreg_7->Data = vreg_1;
  return vreg_7;
}

void System_Array_Copy(RefArr<uint8_t>& sourceArray, int32_t sourceIndex, RefArr<uint8_t>& destinationArray, int32_t destinationIndex, int32_t len)
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
  if (brtrue(vreg_18)) goto label_5;
}

namespace {
    RefArr<int> _coders = makeArr<int> ({});
    RefArr<int> _startPos = makeArr<int> ({});
    RefArr<int> _lengths = makeArr<int> ({});
    RefArr<uint8_t> _joinedTexts = makeArr<uint8_t> ({});
Ref<System_String> _str(int index) {return Texts_FromIndex(index, _coders, _startPos, _lengths, _joinedTexts);    }
}
