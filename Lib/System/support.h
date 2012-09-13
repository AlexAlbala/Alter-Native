#pragma once
#define null (0)

template < class T, class U > 
bool is_inst_of(U u) {
   return dynamic_cast< T >(u) != nullptr;
}

template < class T, class U > 
T as_cast(U u) {
   return dynamic_cast< T >(u);
}