#pragma once
/*
where T: struct
The type argument must be a value type. Any value type except Nullable can be specified. See Using Nullable Types (C# Programming Guide) for more information.

where T : class
The type argument must be a reference type, including any class, interface, delegate, or array type. (See note below.)

where T : new()
The type argument must have a public parameterless constructor. When used in conjunction with other constraints, the new() constraint must be specified last.

where T : <base class name>
The type argument must be or derive from the specified base class.

where T : <interface name>
The type argument must be or implement the specified interface. Multiple interface constraints can be specified. The constraining interface can also be generic.

where T : U
The type argument supplied for T must be or derive from the argument supplied for U. This is called a naked type constraint.
*/

/*#define Where_T_is_Class(T) WhereIsNew<T>
#define Where_T_is_Struct(T) WhereIsNew<T>
#define Where_T_is_U(T,U) WhereIs<T,U>*/

#define Where_T_is_Class(T)
#define Where_T_is_Struct(T)

#define Where_T_is_New(T) __Internal__::WhereIsNew<T>()
#define Where_T_is(T,Base) __Internal__::WhereIs<T,Base>()
#define Where_T_is_Interface(T,Interface) __Internal__::WhereIs<T,Interface>()

namespace __Internal__ {
    template<class T> struct WhereIsNew {
        static void constraints()
        {
        	T* c = new T();	
        }

        WhereIsNew()
        { 
        	void(*p) () = constraints;
        }
    };

    template<class T1, class T2> struct WhereIs {
        static void constraints(T1* a, T2* b)
        {
        	T2* c = a;
        	b = a;
        }

        WhereIs()
        { 
        	void(*p)(T1*,T2*) = constraints;
        }
    };
}