#pragma once
#include "vect.h"

struct Sampler
{
    virtual vect3d sample(vect3d &p){return 1;}
};
