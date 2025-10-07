#include "native_sharp.hpp"
// headers imported by native methods
#include <iostream>
#include <cmath>

struct System_String;
struct nbody;
struct NBodySystem;
struct Body;
struct Texts;
struct System_String {
  int32_t Coder;
  RefArr<uint8_t> Data;
};
struct nbody {};
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
void nbody_Main();
void System_Console_WriteLine(double value);
void NBodySystem_ctor(NBodySystem* _this);
Ref<Body> Body_jupiter();
Ref<Body> Body_saturn();
Ref<Body> Body_uranus();
Ref<Body> Body_neptune();
Ref<Body> Body_offsetMomentum(Ref<Body> _this, double px, double py, double pz);
double NBodySystem_energy(NBodySystem* _this);
double System_Math_Sqrt(double val);
void NBodySystem_advance(NBodySystem* _this, double dt);
void NBodySystem_AdvanceTwoLoops(NBodySystem* _this, double dt);
void NBodySystem_advanceInnerLoop(NBodySystem* _this, double dt, Body* iBody, int32_t j);
void NBodySystem_AdvanceBodiesEnergy(NBodySystem* _this, double dt);
Ref<System_String> Texts_FromIndex(int32_t index, Arr<int32_t>* codes, Arr<int32_t>* startPos, Arr<int32_t>* lengths, Arr<uint8_t>* data);
Ref<System_String> Texts_BuildSystemString(int32_t code, Arr<uint8_t>* data, int32_t startPos, int32_t len);
void System_Array_Copy(Arr<uint8_t>* sourceArray, int32_t sourceIndex, Arr<uint8_t>* destinationArray, int32_t destinationIndex, int32_t len);
#include "native_sharp.cpp"
int main(int argc, char**argv) {
timeItMilliseconds(nbody_Main);
return 0;
}
void nbody_Main()
{
  int32_t local_0,local_2;
  NBodySystem* local_1,*vreg_1;
  double vreg_3,vreg_e;
  bool vreg_c;

  local_0 = 10000000;
   NBodySystem vreg_1_instance;
   vreg_1 = &vreg_1_instance;  
  NBodySystem_ctor(vreg_1);
  local_1 = vreg_1;
  vreg_3 = NBodySystem_energy(vreg_1);
  System_Console_WriteLine(vreg_3);
  local_2 = 0;
  goto label_K;
  label_r:
  NBodySystem_advance(local_1, 0.01);
  local_2 = add (local_2, 1);
  label_K:
  vreg_c = blt_s (local_0, local_2);
  if (brtrue(vreg_c)) goto label_r;
  vreg_e = NBodySystem_energy(local_1);
  System_Console_WriteLine(vreg_e);
}

void System_Console_WriteLine(double value) {
std::cout<<value<<'\n';
}

void NBodySystem_ctor(NBodySystem* _this)
{
  double local_0,local_1,local_2,vreg_x,vreg_C,vreg_D,vreg_K,vreg_P,vreg_Q,vreg_X,vreg_12,vreg_13;
  int32_t local_3,vreg_1c;
  RefArr<Ref<Body>> vreg_3;
  Ref<Body> vreg_b,vreg_f,vreg_j,vreg_n,vreg_1h,vreg_1l,vreg_1m;
  Arr<Ref<Body>>* vreg_u,*vreg_z,*vreg_H,*vreg_M,*vreg_U,*vreg_Z,*vreg_1a,*vreg_1f;
  Body* vreg_w,*vreg_B,*vreg_J,*vreg_O,*vreg_W,*vreg_11;
  uint32_t vreg_1b;
  bool vreg_1d;

  vreg_3 = new_arr<Ref<Body>>(5);
  vreg_1m = new_ref<Body>(0);
  vreg_1m->Mass = 39.47841760435743;
  (*vreg_3)[0] = vreg_1m;
  vreg_b = Body_jupiter();
  (*vreg_3)[1] = vreg_b;
  vreg_f = Body_saturn();
  (*vreg_3)[2] = vreg_f;
  vreg_j = Body_uranus();
  (*vreg_3)[3] = vreg_j;
  vreg_n = Body_neptune();
  (*vreg_3)[4] = vreg_n;
  _this->bodies = vreg_3;
  local_0 = 0;
  local_1 = 0;
  local_2 = 0;
  local_3 = 0;
  goto label_30;
  label_1u:
  vreg_u = _this->bodies.get();
  vreg_w = ((*vreg_u)[local_3]).get();
  vreg_x = vreg_w->Vx;
  vreg_z = _this->bodies.get();
  vreg_B = ((*vreg_z)[local_3]).get();
  vreg_C = vreg_B->Mass;
  vreg_D = mul (vreg_x, vreg_C);
  local_0 = add (local_0, vreg_D);
  vreg_H = _this->bodies.get();
  vreg_J = ((*vreg_H)[local_3]).get();
  vreg_K = vreg_J->Vy;
  vreg_M = _this->bodies.get();
  vreg_O = ((*vreg_M)[local_3]).get();
  vreg_P = vreg_O->Mass;
  vreg_Q = mul (vreg_K, vreg_P);
  local_1 = add (local_1, vreg_Q);
  vreg_U = _this->bodies.get();
  vreg_W = ((*vreg_U)[local_3]).get();
  vreg_X = vreg_W->Vz;
  vreg_Z = _this->bodies.get();
  vreg_11 = ((*vreg_Z)[local_3]).get();
  vreg_12 = vreg_11->Mass;
  vreg_13 = mul (vreg_X, vreg_12);
  local_2 = add (local_2, vreg_13);
  local_3 = add (local_3, 1);
  label_30:
  vreg_1a = _this->bodies.get();
  vreg_1b = vreg_1a->size();
  vreg_1c = conv_i4 (vreg_1b);
  vreg_1d = blt_s (vreg_1c, local_3);
  if (brtrue(vreg_1d)) goto label_1u;
  vreg_1f = _this->bodies.get();
  vreg_1h = ((*vreg_1f)[0]);
  vreg_1l = Body_offsetMomentum(vreg_1h, local_0, local_1, local_2);
}

Ref<Body> Body_jupiter()
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

Ref<Body> Body_saturn()
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

Ref<Body> Body_uranus()
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

Ref<Body> Body_neptune()
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

Ref<Body> Body_offsetMomentum(Ref<Body> _this, double px, double py, double pz)
{
  double vreg_2,vreg_4,vreg_7,vreg_9,vreg_c,vreg_e;

  vreg_2 = neg (px);
  vreg_4 = mul (vreg_2, 0.025330295910584444);
  _this->Vx = vreg_4;
  vreg_7 = neg (py);
  vreg_9 = mul (vreg_7, 0.025330295910584444);
  _this->Vy = vreg_9;
  vreg_c = neg (pz);
  vreg_e = mul (vreg_c, 0.025330295910584444);
  _this->Vz = vreg_e;
  return _this;
}

double NBodySystem_energy(NBodySystem* _this)
{
  double local_3,vreg_9,vreg_a,vreg_c,vreg_e,vreg_f,vreg_h,vreg_j,vreg_k,vreg_l,vreg_n,vreg_p,vreg_q,vreg_r,vreg_s,vreg_F,vreg_K,vreg_P,vreg_S,vreg_V,vreg_W,vreg_Z,vreg_10,vreg_11,vreg_14,vreg_16,vreg_17,vreg_19,vreg_1u,vreg_1w,vreg_1y,vreg_1A,vreg_1C,vreg_1E;
  int32_t local_4,local_6,vreg_1i,vreg_1r;
  Body* local_5,*vreg_5,*vreg_A;
  Arr<Ref<Body>>* vreg_3,*vreg_y,*vreg_1g,*vreg_1p;
  uint32_t vreg_1h,vreg_1q;
  bool vreg_1j,vreg_1s;

  local_3 = 0;
  local_4 = 0;
  goto label_3B;
  label_i:
  vreg_3 = _this->bodies.get();
  vreg_5 = ((*vreg_3)[local_4]).get();
  local_5 = vreg_5;
  vreg_9 = vreg_5->Mass;
  vreg_a = mul (0.5, vreg_9);
  vreg_c = vreg_5->Vx;
  vreg_e = vreg_5->Vx;
  vreg_f = mul (vreg_c, vreg_e);
  vreg_h = vreg_5->Vy;
  vreg_j = vreg_5->Vy;
  vreg_k = mul (vreg_h, vreg_j);
  vreg_l = add (vreg_f, vreg_k);
  vreg_n = vreg_5->Vz;
  vreg_p = vreg_5->Vz;
  vreg_q = mul (vreg_n, vreg_p);
  vreg_r = add (vreg_l, vreg_q);
  vreg_s = mul (vreg_a, vreg_r);
  local_3 = add (local_3, vreg_s);
  local_6 = add (local_4, 1);
  goto label_3j;
  label_1H:
  vreg_y = _this->bodies.get();
  vreg_A = ((*vreg_y)[local_6]).get();
  vreg_1u = local_5->_X_k__BackingField;
  vreg_1w = vreg_A->_X_k__BackingField;
  vreg_F = sub (vreg_1u, vreg_1w);
  vreg_1y = local_5->_Y_k__BackingField;
  vreg_1A = vreg_A->_Y_k__BackingField;
  vreg_K = sub (vreg_1y, vreg_1A);
  vreg_1C = local_5->_Z_k__BackingField;
  vreg_1E = vreg_A->_Z_k__BackingField;
  vreg_P = sub (vreg_1C, vreg_1E);
  vreg_S = mul (vreg_F, vreg_F);
  vreg_V = mul (vreg_K, vreg_K);
  vreg_W = add (vreg_S, vreg_V);
  vreg_Z = mul (vreg_P, vreg_P);
  vreg_10 = add (vreg_W, vreg_Z);
  vreg_11 = System_Math_Sqrt(vreg_10);
  vreg_14 = local_5->Mass;
  vreg_16 = vreg_A->Mass;
  vreg_17 = mul (vreg_14, vreg_16);
  vreg_19 = div (vreg_17, vreg_11);
  local_3 = sub (local_3, vreg_19);
  local_6 = add (local_6, 1);
  label_3j:
  vreg_1g = _this->bodies.get();
  vreg_1h = vreg_1g->size();
  vreg_1i = conv_i4 (vreg_1h);
  vreg_1j = blt_s (vreg_1i, local_6);
  if (brtrue(vreg_1j)) goto label_1H;
  local_4 = add (local_4, 1);
  label_3B:
  vreg_1p = _this->bodies.get();
  vreg_1q = vreg_1p->size();
  vreg_1r = conv_i4 (vreg_1q);
  vreg_1s = blt (vreg_1r, local_4);
  if (brtrue(vreg_1s)) goto label_i;
  return local_3;
}

double System_Math_Sqrt(double val) {
return std::sqrt(val);
}

void NBodySystem_advance(NBodySystem* _this, double dt)
{
  NBodySystem_AdvanceTwoLoops(_this, dt);
  NBodySystem_AdvanceBodiesEnergy(_this, dt);
}

void NBodySystem_AdvanceTwoLoops(NBodySystem* _this, double dt)
{
  int32_t local_0,local_2,vreg_j,vreg_s;
  Body* local_1;
  Arr<Ref<Body>>* vreg_2,*vreg_h,*vreg_q;
  uint32_t vreg_i,vreg_r;
  bool vreg_k,vreg_t;

  local_0 = 0;
  goto label_L;
  label_4:
  vreg_2 = _this->bodies.get();
  local_1 = ((*vreg_2)[local_0]).get();
  local_2 = add (local_0, 1);
  goto label_w;
  label_j:
  NBodySystem_advanceInnerLoop(_this, dt, local_1, local_2);
  local_2 = add (local_2, 1);
  label_w:
  vreg_h = _this->bodies.get();
  vreg_i = vreg_h->size();
  vreg_j = conv_i4 (vreg_i);
  vreg_k = blt_s (vreg_j, local_2);
  if (brtrue(vreg_k)) goto label_j;
  local_0 = add (local_0, 1);
  label_L:
  vreg_q = _this->bodies.get();
  vreg_r = vreg_q->size();
  vreg_s = conv_i4 (vreg_r);
  vreg_t = blt_s (vreg_s, local_0);
  if (brtrue(vreg_t)) goto label_4;
}

void NBodySystem_advanceInnerLoop(NBodySystem* _this, double dt, Body* iBody, int32_t j)
{
  Arr<Ref<Body>>* vreg_3,*vreg_b,*vreg_j,*vreg_M,*vreg_10,*vreg_1e,*vreg_1n,*vreg_1B,*vreg_1P;
  Body* vreg_5,*vreg_d,*vreg_l,*vreg_O,*vreg_12,*vreg_1g,*vreg_1p,*vreg_1D,*vreg_1R;
  double vreg_7,vreg_f,vreg_n,vreg_q,vreg_t,vreg_u,vreg_x,vreg_y,vreg_A,vreg_E,vreg_F,vreg_J,vreg_P,vreg_Q,vreg_S,vreg_T,vreg_X,vreg_13,vreg_14,vreg_16,vreg_17,vreg_1b,vreg_1h,vreg_1i,vreg_1k,vreg_1l,vreg_1s,vreg_1v,vreg_1w,vreg_1y,vreg_1z,vreg_1G,vreg_1J,vreg_1K,vreg_1M,vreg_1N,vreg_1U,vreg_1X,vreg_1Y,vreg_20,vreg_21,vreg_23,vreg_25,vreg_27,vreg_29,vreg_2b,vreg_2d;

  vreg_23 = iBody->_X_k__BackingField;
  vreg_3 = _this->bodies.get();
  vreg_5 = ((*vreg_3)[j]).get();
  vreg_25 = vreg_5->_X_k__BackingField;
  vreg_7 = sub (vreg_23, vreg_25);
  vreg_27 = iBody->_Y_k__BackingField;
  vreg_b = _this->bodies.get();
  vreg_d = ((*vreg_b)[j]).get();
  vreg_29 = vreg_d->_Y_k__BackingField;
  vreg_f = sub (vreg_27, vreg_29);
  vreg_2b = iBody->_Z_k__BackingField;
  vreg_j = _this->bodies.get();
  vreg_l = ((*vreg_j)[j]).get();
  vreg_2d = vreg_l->_Z_k__BackingField;
  vreg_n = sub (vreg_2b, vreg_2d);
  vreg_q = mul (vreg_7, vreg_7);
  vreg_t = mul (vreg_f, vreg_f);
  vreg_u = add (vreg_q, vreg_t);
  vreg_x = mul (vreg_n, vreg_n);
  vreg_y = add (vreg_u, vreg_x);
  vreg_A = System_Math_Sqrt(vreg_y);
  vreg_E = mul (vreg_y, vreg_A);
  vreg_F = div (dt, vreg_E);
  vreg_J = iBody->Vx;
  vreg_M = _this->bodies.get();
  vreg_O = ((*vreg_M)[j]).get();
  vreg_P = vreg_O->Mass;
  vreg_Q = mul (vreg_7, vreg_P);
  vreg_S = mul (vreg_Q, vreg_F);
  vreg_T = sub (vreg_J, vreg_S);
  iBody->Vx = vreg_T;
  vreg_X = iBody->Vy;
  vreg_10 = _this->bodies.get();
  vreg_12 = ((*vreg_10)[j]).get();
  vreg_13 = vreg_12->Mass;
  vreg_14 = mul (vreg_f, vreg_13);
  vreg_16 = mul (vreg_14, vreg_F);
  vreg_17 = sub (vreg_X, vreg_16);
  iBody->Vy = vreg_17;
  vreg_1b = iBody->Vz;
  vreg_1e = _this->bodies.get();
  vreg_1g = ((*vreg_1e)[j]).get();
  vreg_1h = vreg_1g->Mass;
  vreg_1i = mul (vreg_n, vreg_1h);
  vreg_1k = mul (vreg_1i, vreg_F);
  vreg_1l = sub (vreg_1b, vreg_1k);
  iBody->Vz = vreg_1l;
  vreg_1n = _this->bodies.get();
  vreg_1p = ((*vreg_1n)[j]).get();
  vreg_1s = vreg_1p->Vx;
  vreg_1v = iBody->Mass;
  vreg_1w = mul (vreg_7, vreg_1v);
  vreg_1y = mul (vreg_1w, vreg_F);
  vreg_1z = add (vreg_1s, vreg_1y);
  vreg_1p->Vx = vreg_1z;
  vreg_1B = _this->bodies.get();
  vreg_1D = ((*vreg_1B)[j]).get();
  vreg_1G = vreg_1D->Vy;
  vreg_1J = iBody->Mass;
  vreg_1K = mul (vreg_f, vreg_1J);
  vreg_1M = mul (vreg_1K, vreg_F);
  vreg_1N = add (vreg_1G, vreg_1M);
  vreg_1D->Vy = vreg_1N;
  vreg_1P = _this->bodies.get();
  vreg_1R = ((*vreg_1P)[j]).get();
  vreg_1U = vreg_1R->Vz;
  vreg_1X = iBody->Mass;
  vreg_1Y = mul (vreg_n, vreg_1X);
  vreg_20 = mul (vreg_1Y, vreg_F);
  vreg_21 = add (vreg_1U, vreg_20);
  vreg_1R->Vz = vreg_21;
}

void NBodySystem_AdvanceBodiesEnergy(NBodySystem* _this, double dt)
{
  Arr<Ref<Body>>* local_0,*vreg_B;
  int32_t local_1,vreg_D;
  Body* vreg_5;
  double vreg_c,vreg_d,vreg_e,vreg_l,vreg_m,vreg_n,vreg_u,vreg_v,vreg_w,vreg_G,vreg_I,vreg_K;
  uint32_t vreg_C;
  bool vreg_E;

  local_0 = _this->bodies.get();
  local_1 = 0;
  goto label_1k;
  label_b:
  vreg_5 = ((*local_0)[local_1]).get();
  vreg_G = vreg_5->_X_k__BackingField;
  vreg_c = vreg_5->Vx;
  vreg_d = mul (dt, vreg_c);
  vreg_e = add (vreg_G, vreg_d);
  vreg_5->_X_k__BackingField = vreg_e;
  vreg_I = vreg_5->_Y_k__BackingField;
  vreg_l = vreg_5->Vy;
  vreg_m = mul (dt, vreg_l);
  vreg_n = add (vreg_I, vreg_m);
  vreg_5->_Y_k__BackingField = vreg_n;
  vreg_K = vreg_5->_Z_k__BackingField;
  vreg_u = vreg_5->Vz;
  vreg_v = mul (dt, vreg_u);
  vreg_w = add (vreg_K, vreg_v);
  vreg_5->_Z_k__BackingField = vreg_w;
  local_1 = add (local_1, 1);
  label_1k:
  vreg_B = local_0;
  vreg_C = vreg_B->size();
  vreg_D = conv_i4 (vreg_C);
  vreg_E = blt_s (vreg_D, local_1);
  if (brtrue(vreg_E)) goto label_b;
}

Ref<System_String> Texts_FromIndex(int32_t index, Arr<int32_t>* codes, Arr<int32_t>* startPos, Arr<int32_t>* lengths, Arr<uint8_t>* data)
{
  int32_t vreg_2,vreg_5,vreg_8;
  Ref<System_String> vreg_c;

  vreg_2 = ((*startPos)[index]);
  vreg_5 = ((*lengths)[index]);
  vreg_8 = ((*codes)[index]);
  vreg_c = Texts_BuildSystemString(vreg_8, data, vreg_2, vreg_5);
  return vreg_c;
}

Ref<System_String> Texts_BuildSystemString(int32_t code, Arr<uint8_t>* data, int32_t startPos, int32_t len)
{
  RefArr<uint8_t> vreg_1;
  Ref<System_String> vreg_7;

  vreg_1 = new_arr<uint8_t>(len);
  System_Array_Copy(data, startPos, vreg_1.get(), 0, len);
  vreg_7 = new_ref<System_String>(0);
  vreg_7->Coder = code;
  vreg_7->Data = vreg_1;
  return vreg_7;
}

void System_Array_Copy(Arr<uint8_t>* sourceArray, int32_t sourceIndex, Arr<uint8_t>* destinationArray, int32_t destinationIndex, int32_t len)
{
  int32_t local_0,vreg_3,vreg_6;
  uint8_t vreg_9;
  bool vreg_i;

  local_0 = 0;
  goto label_o;
  label_4:
  vreg_3 = add (sourceIndex, local_0);
  vreg_6 = add (destinationIndex, local_0);
  vreg_9 = ((*sourceArray)[vreg_3]);
  (*destinationArray)[vreg_6] = vreg_9;
  local_0 = add (local_0, 1);
  label_o:
  vreg_i = blt_s (len, local_0);
  if (brtrue(vreg_i)) goto label_4;
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
