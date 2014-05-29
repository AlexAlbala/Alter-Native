#pragma once

#include "vect.h"

bool rayBoxIntersectDist(vect3d &ray_pos, vect3d &ray_dir, vect3d box_low, vect3d box_high, double &dist, bool &start_in_box);
bool rayBoxIntersectPos(vect3d &ray_pos, vect3d &ray_dir, vect3d box_low, vect3d box_high, double &dist, bool &start_in_box, vect3d &pos);

bool rayTriangleIntersect(vect3d &ray_pos, vect3d &ray_dir, vect3d &v0, vect3d &v1, vect3d &v2, double &dist);

bool intersect_aabb_tri(vect3d &mine, vect3d &maxe, vect3d &v0, vect3d &v1, vect3d &v2);

double dist2_aabb_pt(vect3d &mine, vect3d &maxe, vect3d &pt);
float dist2_aabb_pt(vect3f &mine, vect3f &maxe, vect3f &pt);
