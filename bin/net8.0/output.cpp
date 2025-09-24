#include "native_sharp.hpp"
// headers imported by native methods
#include <cstdio>
#include <cmath>

struct System_String;
struct nbody;
struct NBodySystem;
struct Body;
struct System_String {
  System_Int32 Coder;
  RefArr<System_Byte> Data;
};
struct nbody {};
struct NBodySystem {
  RefArr<Ref<Body>> bodies;
};
struct Body {
  System_Double x,y,z,vx,vy,vz,mass;
};
namespace {
    Ref<System_String> _clr_str(int index);
}
System_Void nbody_Main();
System_Void System_Console_WriteLine(System_Double value);
System_Void NBodySystem_ctor(Ref<NBodySystem> _this);
Ref<Body> Body_sun();
System_Void Body_ctor(Ref<Body> _this);
Ref<Body> Body_jupiter();
Ref<Body> Body_saturn();
Ref<Body> Body_uranus();
Ref<Body> Body_neptune();
Ref<Body> Body_offsetMomentum(Ref<Body> _this, System_Double px, System_Double py, System_Double pz);
System_Void NBodySystem_advance(Ref<NBodySystem> _this, System_Double dt);
System_Double System_Math_Sqrt(System_Double val);
System_Double NBodySystem_energy(Ref<NBodySystem> _this);
Ref<System_String> Texts_FromIndex(System_Int32 index, RefArr<System_Int32> codes, RefArr<System_Int32> startPos, RefArr<System_Int32> lengths, RefArr<System_Byte> data);
Ref<System_String> Texts_BuildSystemString(System_Int32 code, RefArr<System_Byte> data, System_Int32 startPos, System_Int32 len);
System_Void System_Array_Copy(RefArr<System_Byte> sourceArray, System_Int32 sourceIndex, RefArr<System_Byte> destinationArray, System_Int32 destinationIndex, System_Int32 len);
int main() {
nbody_Main();
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
System_Void nbody_Main()
{
  System_Int32 local_0,local_2,vreg_12;
  Ref<NBodySystem> local_1,vreg_1;
  System_Double vreg_3,vreg_15;

  local_0 = 10000;
  vreg_1 = new_ref<NBodySystem>();
  NBodySystem_ctor();
  local_1 = vreg_1;
  vreg_3 = NBodySystem_energy(vreg_1);
  System_Console_WriteLine(vreg_3);
  local_2 = 0;
  goto label_49;
  label_29:
  NBodySystem_advance(local_1, 0.01);
  local_2 = clr_add (local_2, 1);
  label_49:
  vreg_12 = clt (local_2, local_0);
  if (brtrue_s(vreg_12)) goto label_29;
  vreg_15 = NBodySystem_energy(local_1);
  System_Console_WriteLine(vreg_15);
}

System_Void System_Console_WriteLine(System_Double value) {
printf("%lf\n", value);
}

System_Void NBodySystem_ctor(Ref<NBodySystem> _this)
{
  System_Double local_0,local_1,local_2,vreg_33,vreg_38,vreg_39,vreg_46,vreg_51,vreg_52,vreg_59,vreg_64,vreg_65;
  System_Int32 local_3,vreg_74,vreg_75;
  RefArr<Ref<Body>> vreg_3,vreg_30,vreg_35,vreg_43,vreg_48,vreg_56,vreg_61,vreg_72,vreg_78;
  Ref<Body> vreg_7,vreg_11,vreg_15,vreg_19,vreg_23,vreg_32,vreg_37,vreg_45,vreg_50,vreg_58,vreg_63,vreg_80;
  System_UInt32 vreg_73;

  System_Object_ctor();
  vreg_3 = new_arr<Ref<Body>>(5);
  vreg_7 = Body_sun();
  (*vreg_3)[0] = vreg_7;
  vreg_11 = Body_jupiter();
  (*vreg_3)[1] = vreg_11;
  vreg_15 = Body_saturn();
  (*vreg_3)[2] = vreg_15;
  vreg_19 = Body_uranus();
  (*vreg_3)[3] = vreg_19;
  vreg_23 = Body_neptune();
  (*vreg_3)[4] = vreg_23;
  _this->bodies = vreg_3;
  local_0 = 0;
  local_1 = 0;
  local_2 = 0;
  local_3 = 0;
  goto label_190;
  label_94:
  vreg_30 = _this->bodies;
  vreg_32 = (*vreg_30)[local_3];
  vreg_33 = vreg_32->vx;
  vreg_35 = _this->bodies;
  vreg_37 = (*vreg_35)[local_3];
  vreg_38 = vreg_37->mass;
  vreg_39 = clr_mul (vreg_33, vreg_38);
  local_0 = clr_add (local_0, vreg_39);
  vreg_43 = _this->bodies;
  vreg_45 = (*vreg_43)[local_3];
  vreg_46 = vreg_45->vy;
  vreg_48 = _this->bodies;
  vreg_50 = (*vreg_48)[local_3];
  vreg_51 = vreg_50->mass;
  vreg_52 = clr_mul (vreg_46, vreg_51);
  local_1 = clr_add (local_1, vreg_52);
  vreg_56 = _this->bodies;
  vreg_58 = (*vreg_56)[local_3];
  vreg_59 = vreg_58->vz;
  vreg_61 = _this->bodies;
  vreg_63 = (*vreg_61)[local_3];
  vreg_64 = vreg_63->mass;
  vreg_65 = clr_mul (vreg_59, vreg_64);
  local_2 = clr_add (local_2, vreg_65);
  local_3 = clr_add (local_3, 1);
  label_190:
  vreg_72 = _this->bodies;
  vreg_73 = vreg_72->size();
  vreg_74 = conv_i4 (vreg_73);
  vreg_75 = clt (local_3, vreg_74);
  if (brtrue_s(vreg_75)) goto label_94;
  vreg_78 = _this->bodies;
  vreg_80 = (*vreg_78)[0];
  vreg_84 = Body_offsetMomentum(vreg_80, local_0, local_1, local_2);
}

Ref<Body> Body_sun()
{
  Ref<Body> vreg_0;

  vreg_0 = new_ref<Body>();
  Body_ctor();
  vreg_0->mass = 39.47841760435743;
  return vreg_0;
}

System_Void Body_ctor(Ref<Body> _this)
{
  System_Object_ctor();
}

Ref<Body> Body_jupiter()
{
  Ref<Body> vreg_0;

  vreg_0 = new_ref<Body>();
  Body_ctor();
  vreg_0->x = 4.841431442464721;
  vreg_0->y = -1.1603200440274284;
  vreg_0->z = -0.10362204447112311;
  vreg_0->vx = 0.606326392995832;
  vreg_0->vy = 2.81198684491626;
  vreg_0->vz = -0.02521836165988763;
  vreg_0->mass = 0.03769367487038949;
  return vreg_0;
}

Ref<Body> Body_saturn()
{
  Ref<Body> vreg_0;

  vreg_0 = new_ref<Body>();
  Body_ctor();
  vreg_0->x = 8.34336671824458;
  vreg_0->y = 4.124798564124305;
  vreg_0->z = -0.4035234171143214;
  vreg_0->vx = -1.0107743461787924;
  vreg_0->vy = 1.8256623712304119;
  vreg_0->vz = 0.008415761376584154;
  vreg_0->mass = 0.011286326131968767;
  return vreg_0;
}

Ref<Body> Body_uranus()
{
  Ref<Body> vreg_0;

  vreg_0 = new_ref<Body>();
  Body_ctor();
  vreg_0->x = 12.894369562139131;
  vreg_0->y = -15.111151401698631;
  vreg_0->z = -0.22330757889265573;
  vreg_0->vx = 1.0827910064415354;
  vreg_0->vy = 0.8687130181696082;
  vreg_0->vz = -0.010832637401363636;
  vreg_0->mass = 0.0017237240570597112;
  return vreg_0;
}

Ref<Body> Body_neptune()
{
  Ref<Body> vreg_0;

  vreg_0 = new_ref<Body>();
  Body_ctor();
  vreg_0->x = 15.379697114850917;
  vreg_0->y = -25.919314609987964;
  vreg_0->z = 0.17925877295037118;
  vreg_0->vx = 0.979090732243898;
  vreg_0->vy = 0.5946989986476762;
  vreg_0->vz = -0.034755955504078104;
  vreg_0->mass = 0.0020336868699246304;
  return vreg_0;
}

Ref<Body> Body_offsetMomentum(Ref<Body> _this, System_Double px, System_Double py, System_Double pz)
{
  System_Double vreg_2,vreg_4,vreg_7,vreg_9,vreg_12,vreg_14;

  vreg_2 = clr_neg (vreg_1);
  vreg_4 = clr_div (vreg_2, 39.47841760435743);
  _this->vx = vreg_4;
  vreg_7 = clr_neg (vreg_6);
  vreg_9 = clr_div (vreg_7, 39.47841760435743);
  _this->vy = vreg_9;
  vreg_12 = clr_neg (vreg_11);
  vreg_14 = clr_div (vreg_12, 39.47841760435743);
  _this->vz = vreg_14;
  return _this;
}

System_Void NBodySystem_advance(Ref<NBodySystem> _this, System_Double dt)
{
  System_Int32 local_0,local_2,local_12,vreg_141,vreg_142,vreg_151,vreg_152,vreg_193;
  Ref<Body> local_1,vreg_13,vreg_21,vreg_29,vreg_58,vreg_72,vreg_86,vreg_95,vreg_109,vreg_123,vreg_159;
  RefArr<Ref<Body>> local_11,vreg_2,vreg_11,vreg_19,vreg_27,vreg_56,vreg_70,vreg_84,vreg_93,vreg_107,vreg_121,vreg_139,vreg_149,vreg_191;
  System_Double vreg_9,vreg_14,vreg_15,vreg_17,vreg_22,vreg_23,vreg_25,vreg_30,vreg_31,vreg_34,vreg_37,vreg_38,vreg_41,vreg_42,vreg_44,vreg_48,vreg_49,vreg_53,vreg_59,vreg_60,vreg_62,vreg_63,vreg_67,vreg_73,vreg_74,vreg_76,vreg_77,vreg_81,vreg_87,vreg_88,vreg_90,vreg_91,vreg_98,vreg_101,vreg_102,vreg_104,vreg_105,vreg_112,vreg_115,vreg_116,vreg_118,vreg_119,vreg_126,vreg_129,vreg_130,vreg_132,vreg_133,vreg_163,vreg_166,vreg_167,vreg_168,vreg_172,vreg_175,vreg_176,vreg_177,vreg_181,vreg_184,vreg_185,vreg_186;
  System_UInt32 vreg_140,vreg_150,vreg_192;
  System_Boolean vreg_194;

  local_0 = 0;
  goto label_348;
  label_8:
  vreg_2 = _this->bodies;
  local_1 = (*vreg_2)[local_0];
  local_2 = clr_add (local_0, 1);
  goto label_323;
  label_27:
  vreg_9 = local_1->x;
  vreg_11 = _this->bodies;
  vreg_13 = (*vreg_11)[local_2];
  vreg_14 = vreg_13->x;
  vreg_15 = clr_sub (vreg_9, vreg_14);
  vreg_17 = local_1->y;
  vreg_19 = _this->bodies;
  vreg_21 = (*vreg_19)[local_2];
  vreg_22 = vreg_21->y;
  vreg_23 = clr_sub (vreg_17, vreg_22);
  vreg_25 = local_1->z;
  vreg_27 = _this->bodies;
  vreg_29 = (*vreg_27)[local_2];
  vreg_30 = vreg_29->z;
  vreg_31 = clr_sub (vreg_25, vreg_30);
  vreg_34 = clr_mul (vreg_15, vreg_15);
  vreg_37 = clr_mul (vreg_23, vreg_23);
  vreg_38 = clr_add (vreg_34, vreg_37);
  vreg_41 = clr_mul (vreg_31, vreg_31);
  vreg_42 = clr_add (vreg_38, vreg_41);
  vreg_44 = System_Math_Sqrt(vreg_42);
  vreg_48 = clr_mul (vreg_42, vreg_44);
  vreg_49 = clr_div (dt, vreg_48);
  vreg_53 = local_1->vx;
  vreg_56 = _this->bodies;
  vreg_58 = (*vreg_56)[local_2];
  vreg_59 = vreg_58->mass;
  vreg_60 = clr_mul (vreg_15, vreg_59);
  vreg_62 = clr_mul (vreg_60, vreg_49);
  vreg_63 = clr_sub (vreg_53, vreg_62);
  local_1->vx = vreg_63;
  vreg_67 = local_1->vy;
  vreg_70 = _this->bodies;
  vreg_72 = (*vreg_70)[local_2];
  vreg_73 = vreg_72->mass;
  vreg_74 = clr_mul (vreg_23, vreg_73);
  vreg_76 = clr_mul (vreg_74, vreg_49);
  vreg_77 = clr_sub (vreg_67, vreg_76);
  local_1->vy = vreg_77;
  vreg_81 = local_1->vz;
  vreg_84 = _this->bodies;
  vreg_86 = (*vreg_84)[local_2];
  vreg_87 = vreg_86->mass;
  vreg_88 = clr_mul (vreg_31, vreg_87);
  vreg_90 = clr_mul (vreg_88, vreg_49);
  vreg_91 = clr_sub (vreg_81, vreg_90);
  local_1->vz = vreg_91;
  vreg_93 = _this->bodies;
  vreg_95 = (*vreg_93)[local_2];
  vreg_98 = vreg_95->vx;
  vreg_101 = local_1->mass;
  vreg_102 = clr_mul (vreg_15, vreg_101);
  vreg_104 = clr_mul (vreg_102, vreg_49);
  vreg_105 = clr_add (vreg_98, vreg_104);
  vreg_95->vx = vreg_105;
  vreg_107 = _this->bodies;
  vreg_109 = (*vreg_107)[local_2];
  vreg_112 = vreg_109->vy;
  vreg_115 = local_1->mass;
  vreg_116 = clr_mul (vreg_23, vreg_115);
  vreg_118 = clr_mul (vreg_116, vreg_49);
  vreg_119 = clr_add (vreg_112, vreg_118);
  vreg_109->vy = vreg_119;
  vreg_121 = _this->bodies;
  vreg_123 = (*vreg_121)[local_2];
  vreg_126 = vreg_123->vz;
  vreg_129 = local_1->mass;
  vreg_130 = clr_mul (vreg_31, vreg_129);
  vreg_132 = clr_mul (vreg_130, vreg_49);
  vreg_133 = clr_add (vreg_126, vreg_132);
  vreg_123->vz = vreg_133;
  local_2 = clr_add (local_2, 1);
  label_323:
  vreg_139 = _this->bodies;
  vreg_140 = vreg_139->size();
  vreg_141 = conv_i4 (vreg_140);
  vreg_142 = clt (local_2, vreg_141);
  if (brtrue(vreg_142)) goto label_27;
  local_0 = clr_add (local_0, 1);
  label_348:
  vreg_149 = _this->bodies;
  vreg_150 = vreg_149->size();
  vreg_151 = conv_i4 (vreg_150);
  vreg_152 = clt (local_0, vreg_151);
  if (brtrue(vreg_152)) goto label_8;
  local_11 = _this->bodies;
  local_12 = 0;
  goto label_466;
  label_382:
  vreg_159 = (*local_11)[local_12];
  vreg_163 = vreg_159->x;
  vreg_166 = vreg_159->vx;
  vreg_167 = clr_mul (dt, vreg_166);
  vreg_168 = clr_add (vreg_163, vreg_167);
  vreg_159->x = vreg_168;
  vreg_172 = vreg_159->y;
  vreg_175 = vreg_159->vy;
  vreg_176 = clr_mul (dt, vreg_175);
  vreg_177 = clr_add (vreg_172, vreg_176);
  vreg_159->y = vreg_177;
  vreg_181 = vreg_159->z;
  vreg_184 = vreg_159->vz;
  vreg_185 = clr_mul (dt, vreg_184);
  vreg_186 = clr_add (vreg_181, vreg_185);
  vreg_159->z = vreg_186;
  local_12 = clr_add (local_12, 1);
  label_466:
  vreg_191 = local_11;
  vreg_192 = vreg_191->size();
  vreg_193 = conv_i4 (vreg_192);
  vreg_194 = clr_blt_s (local_12, vreg_193);
  if (brtrue(vreg_194)) goto label_382;
}

System_Double System_Math_Sqrt(System_Double val) {
return std::sqrt(val);
}

System_Double NBodySystem_energy(Ref<NBodySystem> _this)
{
  System_Double local_4,vreg_9,vreg_10,vreg_12,vreg_14,vreg_15,vreg_17,vreg_19,vreg_20,vreg_21,vreg_23,vreg_25,vreg_26,vreg_27,vreg_28,vreg_38,vreg_40,vreg_41,vreg_43,vreg_45,vreg_46,vreg_48,vreg_50,vreg_51,vreg_54,vreg_57,vreg_58,vreg_61,vreg_62,vreg_63,vreg_66,vreg_68,vreg_69,vreg_71;
  System_Int32 local_5,local_7,vreg_80,vreg_81,vreg_90,vreg_91;
  Ref<Body> local_6,vreg_5,vreg_36;
  RefArr<Ref<Body>> vreg_3,vreg_34,vreg_78,vreg_88;
  System_UInt32 vreg_79,vreg_89;

  local_4 = 0;
  local_5 = 0;
  goto label_241;
  label_20:
  vreg_3 = _this->bodies;
  vreg_5 = (*vreg_3)[local_5];
  local_6 = vreg_5;
  vreg_9 = vreg_5->mass;
  vreg_10 = clr_mul (0.5, vreg_9);
  vreg_12 = vreg_5->vx;
  vreg_14 = vreg_5->vx;
  vreg_15 = clr_mul (vreg_12, vreg_14);
  vreg_17 = vreg_5->vy;
  vreg_19 = vreg_5->vy;
  vreg_20 = clr_mul (vreg_17, vreg_19);
  vreg_21 = clr_add (vreg_15, vreg_20);
  vreg_23 = vreg_5->vz;
  vreg_25 = vreg_5->vz;
  vreg_26 = clr_mul (vreg_23, vreg_25);
  vreg_27 = clr_add (vreg_21, vreg_26);
  vreg_28 = clr_mul (vreg_10, vreg_27);
  local_4 = clr_add (local_4, vreg_28);
  local_7 = clr_add (local_5, 1);
  goto label_216;
  label_110:
  vreg_34 = _this->bodies;
  vreg_36 = (*vreg_34)[local_7];
  vreg_38 = local_6->x;
  vreg_40 = vreg_36->x;
  vreg_41 = clr_sub (vreg_38, vreg_40);
  vreg_43 = local_6->y;
  vreg_45 = vreg_36->y;
  vreg_46 = clr_sub (vreg_43, vreg_45);
  vreg_48 = local_6->z;
  vreg_50 = vreg_36->z;
  vreg_51 = clr_sub (vreg_48, vreg_50);
  vreg_54 = clr_mul (vreg_41, vreg_41);
  vreg_57 = clr_mul (vreg_46, vreg_46);
  vreg_58 = clr_add (vreg_54, vreg_57);
  vreg_61 = clr_mul (vreg_51, vreg_51);
  vreg_62 = clr_add (vreg_58, vreg_61);
  vreg_63 = System_Math_Sqrt(vreg_62);
  vreg_66 = local_6->mass;
  vreg_68 = vreg_36->mass;
  vreg_69 = clr_mul (vreg_66, vreg_68);
  vreg_71 = clr_div (vreg_69, vreg_63);
  local_4 = clr_sub (local_4, vreg_71);
  local_7 = clr_add (local_7, 1);
  label_216:
  vreg_78 = _this->bodies;
  vreg_79 = vreg_78->size();
  vreg_80 = conv_i4 (vreg_79);
  vreg_81 = clt (local_7, vreg_80);
  if (brtrue_s(vreg_81)) goto label_110;
  local_5 = clr_add (local_5, 1);
  label_241:
  vreg_88 = _this->bodies;
  vreg_89 = vreg_88->size();
  vreg_90 = conv_i4 (vreg_89);
  vreg_91 = clt (local_5, vreg_90);
  if (brtrue(vreg_91)) goto label_20;
  return local_4;
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
  System_String_ctor();
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
