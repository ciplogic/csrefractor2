#include "native_sharp.hpp"
// headers imported by native methods
#include <iostream>
#include <cmath>

struct System_String;
struct nbody;
struct NBodySystem;
struct Body;
struct System_String {
System_Int32 Coder;
RefArr<System_Byte> Data;
};
struct nbody {
};
struct NBodySystem {
RefArr<Ref<Body>> bodies;
};
struct Body {
System_Double x;
System_Double y;
System_Double z;
System_Double vx;
System_Double vy;
System_Double vz;
System_Double mass;
};
namespace {
    Ref<System_String> _clr_str(int index);
}
System_Void nbody_Main();
System_Void System_Console_WriteLine(System_Double value);
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
System_Void nbody_Main()
{
System_Int32 local_0;
Ref<NBodySystem> local_1;
System_Int32 local_2;
Ref<NBodySystem> vreg_1;
System_Double vreg_3;
System_Int32 vreg_9;
System_Double vreg_13;
local_0 = 10000;
vreg_1 = new_ref<NBodySystem>();
local_1 = vreg_1;
vreg_3 = NBodySystem_energy(vreg_1);
System_Console_WriteLine(vreg_3);
local_2 = 0;
goto label_46;
label_27:
NBodySystem_advance(local_1, 0.01);
vreg_9 = clr_add (local_2, 1);
local_2 = vreg_9;
label_46:
if (blt_s(local_0)) goto label_27;
vreg_13 = NBodySystem_energy(local_1);
System_Console_WriteLine(vreg_13);
}

System_Void System_Console_WriteLine(System_Double value) {
std::cout<<value<<'\n';
}

System_Void NBodySystem_advance(Ref<NBodySystem> _this, System_Double dt)
{
System_Int32 local_0;
Ref<Body> local_1;
System_Int32 local_2;
RefArr<Ref<Body>> local_9;
System_Int32 local_10;
Ref<NBodySystem> vreg_1;
RefArr<Ref<Body>> vreg_2;
Ref<Body> vreg_4;
System_Int32 vreg_7;
Ref<Body> vreg_8;
System_Double vreg_9;
Ref<NBodySystem> vreg_10;
RefArr<Ref<Body>> vreg_11;
Ref<Body> vreg_13;
System_Double vreg_14;
System_Double vreg_15;
Ref<Body> vreg_16;
System_Double vreg_17;
Ref<NBodySystem> vreg_18;
RefArr<Ref<Body>> vreg_19;
Ref<Body> vreg_21;
System_Double vreg_22;
System_Double vreg_23;
Ref<Body> vreg_24;
System_Double vreg_25;
Ref<NBodySystem> vreg_26;
RefArr<Ref<Body>> vreg_27;
Ref<Body> vreg_29;
System_Double vreg_30;
System_Double vreg_31;
System_Double vreg_34;
System_Double vreg_37;
System_Double vreg_38;
System_Double vreg_41;
System_Double vreg_42;
System_Double vreg_44;
System_Double vreg_48;
System_Double vreg_49;
Ref<Body> vreg_52;
System_Double vreg_53;
Ref<NBodySystem> vreg_55;
RefArr<Ref<Body>> vreg_56;
Ref<Body> vreg_58;
System_Double vreg_59;
System_Double vreg_60;
System_Double vreg_62;
System_Double vreg_63;
Ref<Body> vreg_66;
System_Double vreg_67;
Ref<NBodySystem> vreg_69;
RefArr<Ref<Body>> vreg_70;
Ref<Body> vreg_72;
System_Double vreg_73;
System_Double vreg_74;
System_Double vreg_76;
System_Double vreg_77;
Ref<Body> vreg_80;
System_Double vreg_81;
Ref<NBodySystem> vreg_83;
RefArr<Ref<Body>> vreg_84;
Ref<Body> vreg_86;
System_Double vreg_87;
System_Double vreg_88;
System_Double vreg_90;
System_Double vreg_91;
Ref<NBodySystem> vreg_92;
RefArr<Ref<Body>> vreg_93;
Ref<Body> vreg_95;
Ref<Body> vreg_97;
System_Double vreg_98;
Ref<Body> vreg_100;
System_Double vreg_101;
System_Double vreg_102;
System_Double vreg_104;
System_Double vreg_105;
Ref<NBodySystem> vreg_106;
RefArr<Ref<Body>> vreg_107;
Ref<Body> vreg_109;
Ref<Body> vreg_111;
System_Double vreg_112;
Ref<Body> vreg_114;
System_Double vreg_115;
System_Double vreg_116;
System_Double vreg_118;
System_Double vreg_119;
Ref<NBodySystem> vreg_120;
RefArr<Ref<Body>> vreg_121;
Ref<Body> vreg_123;
Ref<Body> vreg_125;
System_Double vreg_126;
Ref<Body> vreg_128;
System_Double vreg_129;
System_Double vreg_130;
System_Double vreg_132;
System_Double vreg_133;
System_Int32 vreg_136;
Ref<NBodySystem> vreg_138;
RefArr<Ref<Body>> vreg_139;
System_UInt32 vreg_140;
System_Int32 vreg_141;
System_Int32 vreg_144;
Ref<NBodySystem> vreg_146;
RefArr<Ref<Body>> vreg_147;
System_UInt32 vreg_148;
System_Int32 vreg_149;
Ref<NBodySystem> vreg_150;
RefArr<Ref<Body>> vreg_151;
Ref<Body> vreg_155;
Ref<Body> vreg_158;
System_Double vreg_159;
Ref<Body> vreg_161;
System_Double vreg_162;
System_Double vreg_163;
System_Double vreg_164;
Ref<Body> vreg_167;
System_Double vreg_168;
Ref<Body> vreg_170;
System_Double vreg_171;
System_Double vreg_172;
System_Double vreg_173;
Ref<Body> vreg_176;
System_Double vreg_177;
Ref<Body> vreg_179;
System_Double vreg_180;
System_Double vreg_181;
System_Double vreg_182;
System_Int32 vreg_185;
RefArr<Ref<Body>> vreg_187;
System_UInt32 vreg_188;
System_Int32 vreg_189;
local_0 = 0;
goto label_337;
label_7:
vreg_1 = _this;
vreg_2 = vreg_1->bodies;
vreg_4 = (*vreg_2)[local_0];
local_1 = vreg_4;
vreg_7 = clr_add (local_0, 1);
local_2 = vreg_7;
goto label_319;
label_25:
vreg_8 = local_1;
vreg_9 = vreg_8->x;
vreg_10 = _this;
vreg_11 = vreg_10->bodies;
vreg_13 = (*vreg_11)[local_2];
vreg_14 = vreg_13->x;
vreg_15 = clr_sub (vreg_9, vreg_14);
vreg_16 = local_1;
vreg_17 = vreg_16->y;
vreg_18 = _this;
vreg_19 = vreg_18->bodies;
vreg_21 = (*vreg_19)[local_2];
vreg_22 = vreg_21->y;
vreg_23 = clr_sub (vreg_17, vreg_22);
vreg_24 = local_1;
vreg_25 = vreg_24->z;
vreg_26 = _this;
vreg_27 = vreg_26->bodies;
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
vreg_52 = local_1;
vreg_53 = vreg_52->vx;
vreg_55 = _this;
vreg_56 = vreg_55->bodies;
vreg_58 = (*vreg_56)[local_2];
vreg_59 = vreg_58->mass;
vreg_60 = clr_mul (vreg_15, vreg_59);
vreg_62 = clr_mul (vreg_60, vreg_49);
vreg_63 = clr_sub (vreg_53, vreg_62);
local_1->vx = vreg_63;
vreg_66 = local_1;
vreg_67 = vreg_66->vy;
vreg_69 = _this;
vreg_70 = vreg_69->bodies;
vreg_72 = (*vreg_70)[local_2];
vreg_73 = vreg_72->mass;
vreg_74 = clr_mul (vreg_23, vreg_73);
vreg_76 = clr_mul (vreg_74, vreg_49);
vreg_77 = clr_sub (vreg_67, vreg_76);
local_1->vy = vreg_77;
vreg_80 = local_1;
vreg_81 = vreg_80->vz;
vreg_83 = _this;
vreg_84 = vreg_83->bodies;
vreg_86 = (*vreg_84)[local_2];
vreg_87 = vreg_86->mass;
vreg_88 = clr_mul (vreg_31, vreg_87);
vreg_90 = clr_mul (vreg_88, vreg_49);
vreg_91 = clr_sub (vreg_81, vreg_90);
local_1->vz = vreg_91;
vreg_92 = _this;
vreg_93 = vreg_92->bodies;
vreg_95 = (*vreg_93)[local_2];
vreg_97 = vreg_95;
vreg_98 = vreg_97->vx;
vreg_100 = local_1;
vreg_101 = vreg_100->mass;
vreg_102 = clr_mul (vreg_15, vreg_101);
vreg_104 = clr_mul (vreg_102, vreg_49);
vreg_105 = clr_add (vreg_98, vreg_104);
vreg_95->vx = vreg_105;
vreg_106 = _this;
vreg_107 = vreg_106->bodies;
vreg_109 = (*vreg_107)[local_2];
vreg_111 = vreg_109;
vreg_112 = vreg_111->vy;
vreg_114 = local_1;
vreg_115 = vreg_114->mass;
vreg_116 = clr_mul (vreg_23, vreg_115);
vreg_118 = clr_mul (vreg_116, vreg_49);
vreg_119 = clr_add (vreg_112, vreg_118);
vreg_109->vy = vreg_119;
vreg_120 = _this;
vreg_121 = vreg_120->bodies;
vreg_123 = (*vreg_121)[local_2];
vreg_125 = vreg_123;
vreg_126 = vreg_125->vz;
vreg_128 = local_1;
vreg_129 = vreg_128->mass;
vreg_130 = clr_mul (vreg_31, vreg_129);
vreg_132 = clr_mul (vreg_130, vreg_49);
vreg_133 = clr_add (vreg_126, vreg_132);
vreg_123->vz = vreg_133;
vreg_136 = clr_add (local_2, 1);
local_2 = vreg_136;
label_319:
vreg_138 = _this;
vreg_139 = vreg_138->bodies;
vreg_140 = vreg_139->size();
vreg_141 = conv_i4 (vreg_140);
if (blt(vreg_141)) goto label_25;
vreg_144 = clr_add (local_0, 1);
local_0 = vreg_144;
label_337:
vreg_146 = _this;
vreg_147 = vreg_146->bodies;
vreg_148 = vreg_147->size();
vreg_149 = conv_i4 (vreg_148);
if (blt(vreg_149)) goto label_7;
vreg_150 = _this;
vreg_151 = vreg_150->bodies;
local_9 = vreg_151;
local_10 = 0;
goto label_446;
label_364:
vreg_155 = (*local_9)[local_10];
vreg_158 = vreg_155;
vreg_159 = vreg_158->x;
vreg_161 = vreg_155;
vreg_162 = vreg_161->vx;
vreg_163 = clr_mul (dt, vreg_162);
vreg_164 = clr_add (vreg_159, vreg_163);
vreg_155->x = vreg_164;
vreg_167 = vreg_155;
vreg_168 = vreg_167->y;
vreg_170 = vreg_155;
vreg_171 = vreg_170->vy;
vreg_172 = clr_mul (dt, vreg_171);
vreg_173 = clr_add (vreg_168, vreg_172);
vreg_155->y = vreg_173;
vreg_176 = vreg_155;
vreg_177 = vreg_176->z;
vreg_179 = vreg_155;
vreg_180 = vreg_179->vz;
vreg_181 = clr_mul (dt, vreg_180);
vreg_182 = clr_add (vreg_177, vreg_181);
vreg_155->z = vreg_182;
vreg_185 = clr_add (local_10, 1);
local_10 = vreg_185;
label_446:
vreg_187 = local_9;
vreg_188 = vreg_187->size();
vreg_189 = conv_i4 (vreg_188);
if (blt_s(vreg_189)) goto label_364;
}

System_Double System_Math_Sqrt(System_Double val) {
return std::sqrt(val);
}

System_Double NBodySystem_energy(Ref<NBodySystem> _this)
{
System_Double local_3;
System_Int32 local_4;
Ref<Body> local_5;
System_Int32 local_6;
Ref<NBodySystem> vreg_2;
RefArr<Ref<Body>> vreg_3;
Ref<Body> vreg_5;
Ref<Body> vreg_8;
System_Double vreg_9;
System_Double vreg_10;
Ref<Body> vreg_11;
System_Double vreg_12;
Ref<Body> vreg_13;
System_Double vreg_14;
System_Double vreg_15;
Ref<Body> vreg_16;
System_Double vreg_17;
Ref<Body> vreg_18;
System_Double vreg_19;
System_Double vreg_20;
System_Double vreg_21;
Ref<Body> vreg_22;
System_Double vreg_23;
Ref<Body> vreg_24;
System_Double vreg_25;
System_Double vreg_26;
System_Double vreg_27;
System_Double vreg_28;
System_Double vreg_29;
System_Int32 vreg_32;
Ref<NBodySystem> vreg_33;
RefArr<Ref<Body>> vreg_34;
Ref<Body> vreg_36;
Ref<Body> vreg_37;
System_Double vreg_38;
Ref<Body> vreg_39;
System_Double vreg_40;
System_Double vreg_41;
Ref<Body> vreg_42;
System_Double vreg_43;
Ref<Body> vreg_44;
System_Double vreg_45;
System_Double vreg_46;
Ref<Body> vreg_47;
System_Double vreg_48;
Ref<Body> vreg_49;
System_Double vreg_50;
System_Double vreg_51;
System_Double vreg_54;
System_Double vreg_57;
System_Double vreg_58;
System_Double vreg_61;
System_Double vreg_62;
System_Double vreg_63;
Ref<Body> vreg_65;
System_Double vreg_66;
Ref<Body> vreg_67;
System_Double vreg_68;
System_Double vreg_69;
System_Double vreg_71;
System_Double vreg_72;
System_Int32 vreg_75;
Ref<NBodySystem> vreg_77;
RefArr<Ref<Body>> vreg_78;
System_UInt32 vreg_79;
System_Int32 vreg_80;
System_Int32 vreg_83;
Ref<NBodySystem> vreg_85;
RefArr<Ref<Body>> vreg_86;
System_UInt32 vreg_87;
System_Int32 vreg_88;
System_Double vreg_89;
local_3 = 0;
local_4 = 0;
goto label_223;
label_18:
vreg_2 = _this;
vreg_3 = vreg_2->bodies;
vreg_5 = (*vreg_3)[local_4];
local_5 = vreg_5;
vreg_8 = vreg_5;
vreg_9 = vreg_8->mass;
vreg_10 = clr_mul (0.5, vreg_9);
vreg_11 = vreg_5;
vreg_12 = vreg_11->vx;
vreg_13 = vreg_5;
vreg_14 = vreg_13->vx;
vreg_15 = clr_mul (vreg_12, vreg_14);
vreg_16 = vreg_5;
vreg_17 = vreg_16->vy;
vreg_18 = vreg_5;
vreg_19 = vreg_18->vy;
vreg_20 = clr_mul (vreg_17, vreg_19);
vreg_21 = clr_add (vreg_15, vreg_20);
vreg_22 = vreg_5;
vreg_23 = vreg_22->vz;
vreg_24 = vreg_5;
vreg_25 = vreg_24->vz;
vreg_26 = clr_mul (vreg_23, vreg_25);
vreg_27 = clr_add (vreg_21, vreg_26);
vreg_28 = clr_mul (vreg_10, vreg_27);
vreg_29 = clr_add (local_3, vreg_28);
local_3 = vreg_29;
vreg_32 = clr_add (local_4, 1);
local_6 = vreg_32;
goto label_205;
label_105:
vreg_33 = _this;
vreg_34 = vreg_33->bodies;
vreg_36 = (*vreg_34)[local_6];
vreg_37 = local_5;
vreg_38 = vreg_37->x;
vreg_39 = vreg_36;
vreg_40 = vreg_39->x;
vreg_41 = clr_sub (vreg_38, vreg_40);
vreg_42 = local_5;
vreg_43 = vreg_42->y;
vreg_44 = vreg_36;
vreg_45 = vreg_44->y;
vreg_46 = clr_sub (vreg_43, vreg_45);
vreg_47 = local_5;
vreg_48 = vreg_47->z;
vreg_49 = vreg_36;
vreg_50 = vreg_49->z;
vreg_51 = clr_sub (vreg_48, vreg_50);
vreg_54 = clr_mul (vreg_41, vreg_41);
vreg_57 = clr_mul (vreg_46, vreg_46);
vreg_58 = clr_add (vreg_54, vreg_57);
vreg_61 = clr_mul (vreg_51, vreg_51);
vreg_62 = clr_add (vreg_58, vreg_61);
vreg_63 = System_Math_Sqrt(vreg_62);
vreg_65 = local_5;
vreg_66 = vreg_65->mass;
vreg_67 = vreg_36;
vreg_68 = vreg_67->mass;
vreg_69 = clr_mul (vreg_66, vreg_68);
vreg_71 = clr_div (vreg_69, vreg_63);
vreg_72 = clr_sub (local_3, vreg_71);
local_3 = vreg_72;
vreg_75 = clr_add (local_6, 1);
local_6 = vreg_75;
label_205:
vreg_77 = _this;
vreg_78 = vreg_77->bodies;
vreg_79 = vreg_78->size();
vreg_80 = conv_i4 (vreg_79);
if (blt_s(vreg_80)) goto label_105;
vreg_83 = clr_add (local_4, 1);
local_4 = vreg_83;
label_223:
vreg_85 = _this;
vreg_86 = vreg_85->bodies;
vreg_87 = vreg_86->size();
vreg_88 = conv_i4 (vreg_87);
if (blt(vreg_88)) goto label_18;
vreg_89 = local_3;
return vreg_89;
}

Ref<System_String> Texts_FromIndex(System_Int32 index, RefArr<System_Int32> codes, RefArr<System_Int32> startPos, RefArr<System_Int32> lengths, RefArr<System_Byte> data)
{
System_Int32 vreg_2;
System_Int32 vreg_5;
System_Int32 vreg_8;
Ref<System_String> vreg_12;
vreg_2 = (*startPos)[index];
vreg_5 = (*lengths)[index];
vreg_8 = (*codes)[index];
vreg_12 = Texts_BuildSystemString(vreg_8, data, vreg_2, vreg_5);
return vreg_12;
}

Ref<System_String> Texts_BuildSystemString(System_Int32 code, RefArr<System_Byte> data, System_Int32 startPos, System_Int32 len)
{
RefArr<System_Byte> vreg_1;
Ref<System_String> vreg_7;
System_Int32 vreg_10;
Ref<System_String> vreg_11;
RefArr<System_Byte> vreg_13;
vreg_1 = new_arr<System_Byte>(len);
System_Array_Copy(data, startPos, vreg_1, 0, len);
vreg_7 = new_ref<System_String>();
vreg_10 = code;
vreg_7->Coder = vreg_10;
vreg_11 = vreg_7;
vreg_13 = vreg_1;
vreg_7->Data = vreg_13;
return vreg_11;
}

System_Void System_Array_Copy(RefArr<System_Byte> sourceArray, System_Int32 sourceIndex, RefArr<System_Byte> destinationArray, System_Int32 destinationIndex, System_Int32 len)
{
System_Int32 local_0;
System_Int32 vreg_3;
System_Int32 vreg_6;
System_Byte vreg_9;
System_Int32 vreg_15;
local_0 = 0;
goto label_24;
label_4:
vreg_3 = clr_add (sourceIndex, local_0);
vreg_6 = clr_add (destinationIndex, local_0);
vreg_9 = (*sourceArray)[vreg_3];
(*destinationArray)[vreg_6] = vreg_9;
vreg_15 = clr_add (local_0, 1);
local_0 = vreg_15;
label_24:
if (blt_s(len)) goto label_4;
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
