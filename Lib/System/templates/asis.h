/*********************************************************************************************************/
/*********************************************** AS IS CASTS  ********************************************/
/*********************************************************************************************************/
/*
Macro for C# feature is operator
*/
template < class T, class U > 
bool is_inst_of(U u) {
   return dynamic_cast< T >(u) != (T)0;
};
/*
Macro for C# feature as cast 
*/
template < class T, class U > 
T as_cast(U u) {
   return dynamic_cast< T >(u);
};