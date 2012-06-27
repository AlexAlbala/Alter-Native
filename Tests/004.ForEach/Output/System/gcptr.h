

#if !defined(_GCPTR_H_68F9569A_2697_45DD_B71D_E1F77D95E40E_INCLUDED_)
#define _GCPTR_H_68F9569A_2697_45DD_B71D_E1F77D95E40E_INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

#include <assert.h>
#include <stddef.h>

#ifdef LINUX
#include <pthread.h>
#endif

namespace gc_detail
{
	struct gc_t
	{
	};

	template <typename T>
	struct destructor
	{
		static void destroy(void* obj) { delete static_cast<T*>(obj); }
	};

	struct pointer_base
	{
	protected:
		pointer_base();
		~pointer_base();

		void reset_node(void* obj, void (*destroy)(void*));
		template <typename T>
			void reset_node(T* obj) { reset_node(obj, destructor<T>::destroy); }
	};
};	// namespace gc_detail

namespace
{
	gc_detail::gc_t gc;
};

void* operator new(size_t size, const gc_detail::gc_t&);
void operator delete(void* obj, const gc_detail::gc_t&);

void gc_collect();
void gc_set_threshold(size_t bytes);

template <typename T>
class gc_ptr : public gc_detail::pointer_base
{
public:
	typedef T element_type;

	explicit gc_ptr(T* obj=0) : ptr(0) { reset(obj); }
	~gc_ptr() { }

	template <typename U>
		gc_ptr(const gc_ptr<U>& other) : ptr(0) { reset(other.get()); }
	gc_ptr(const gc_ptr& other) : ptr(0) { reset(other.get()); }
	gc_ptr(const int other) : ptr((T*)other) {assert(other == 0);} 

	template <typename U>
	gc_ptr& operator=(const gc_ptr<U>& other) { reset(other.get()); return *this; }
	gc_ptr& operator=(const gc_ptr& other) { reset(other.get()); return *this; }

	void reset(T* obj=0) { reset_node(obj); ptr = obj; }

	T& operator*() const { return *get(); }
	T* operator->() const { return get(); }
	

	bool operator!=(gc_ptr<T> pointer) const { return get()!=pointer->get(); }
	bool operator!=(const T* pointer) const { return get()!=pointer; }
	
	
	T* get() const { return ptr; }

	void swap(gc_ptr& other) { T* temp = ptr; reset(other.get()); other.reset(temp); }

private:
	// If you're using VC++ 6 you may want to uncomment this line to generate an
	// error when someone mistakenly tries to assign to a "real pointer".  This
	// is only needed to work around a bug in the VC++ 6 compiler.
//	operator=(T*);
	T* ptr;
};

#endif // !defined(_GCPTR_H_68F9569A_2697_45DD_B71D_E1F77D95E40E_INCLUDED_)
