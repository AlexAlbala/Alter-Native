#pragma once
#define null (0)

template < class T, class U > 
bool isinstof(U u) {
   return dynamic_cast< T >(u) != nullptr;
}

template < class T, class U > 
T ascast(U u) {
   return dynamic_cast< T >(u);
}