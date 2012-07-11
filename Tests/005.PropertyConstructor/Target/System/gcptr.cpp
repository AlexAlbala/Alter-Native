#include "gcptr.h"

#define	NOMINMAX

#ifdef _WIN32
#include <windows.h>
#endif

#pragma warning(disable : 4786)
#include <set>
#include <map>
#include <stdexcept>
#include <limits>

namespace
{
	struct node_t
	{
		node_t(void* obj, int n);
		~node_t();
		bool contains(void* obj);
		void release();

		void* base;
		size_t size;
		bool mark;
		void* object;
		void (*destroy)(void*);
	};

	struct node_less
	{
		bool operator()(const node_t* x, const node_t* y) const
		{
			return (static_cast<char*>(x->base) + x->size) <= static_cast<char*>(y->base);
		}
	};

	typedef std::map<gc_detail::pointer_base*, node_t*> ptr_map;
	typedef std::set<node_t*, node_less> node_set;

	struct data_t
	{
		data_t() : threshold(1024), allocated(0), collecting(false), current_mark(false)
		{ 
#ifdef _WIN32
			InitializeCriticalSection(&cs);
#elif LINUX
			pthread_mutexattr_t mAttr;
			pthread_mutexattr_settype(&mAttr, PTHREAD_MUTEX_RECURSIVE_NP);
			pthread_mutex_init(&m,&mAttr);
			pthread_mutexattr_destroy(&mAttr);
#endif
		}
		~data_t() { gc_collect(); 
#ifdef _WIN32
		DeleteCriticalSection(&cs); 
#elif LINUX
		pthread_mutex_destroy(&m);
#endif
		}

		ptr_map pointers;
		node_set nodes;
		size_t threshold;
		size_t allocated;
		bool collecting;
		bool current_mark;
#ifdef _WIN32
		CRITICAL_SECTION cs;
#elif LINUX
		pthread_mutex_t m;
#endif
	};

	data_t& data() { static data_t instance; return instance; }

	struct data_lock
	{
#ifdef _WIN32
		data_lock() { EnterCriticalSection(&data().cs); }
		~data_lock() { LeaveCriticalSection(&data().cs); }
#elif LINUX
		data_lock() { pthread_mutex_lock(&data().m);
		~data_lock() { pthread_mutex_unlock(&data().m);
#endif
	};

	node_t::node_t(void* obj, int node)
		: base(obj), size(node), object(0), destroy(0), mark(data().current_mark)
	{
	}

	node_t::~node_t()
	{
		if (object && destroy)
			destroy(object);
	}

	void node_t::release()
	{
		if (object && destroy)
			destroy(object);
		object = 0;
		base = 0;
		size = 0;
	}

	bool node_t::contains(void* obj)
	{
		char* begin = static_cast<char*>(base);
		char* end = begin + size;
		return begin <= obj && obj < end;
	}

	node_set::iterator find_node(void* obj)
	{
		// Construct a temporary node in order to search for the object's node.
		node_t temp(obj, 0);

		// Use lower_bound to search for the "insertion point" and determine
		// if the node at this point contains the specified object.  If the
		// insertion point is at the end or does not contain the object then
		// we've failed to find the node and return an iterator to the end.
		node_set::iterator i = data().nodes.lower_bound(&temp);
		if (i == data().nodes.end() || !(*i)->contains(obj))
			return data().nodes.end();

		// Return the found iterator.
		return i;
	}

	node_t* get_node(void* obj, void (*destroy)(void*))
	{
		if (!obj)
			return 0;

		data_lock lock;

		node_set::iterator i = find_node(obj);
		if (i == data().nodes.end())
			throw std::invalid_argument("Object was not created with new(gc)");

		node_t* node = *i;

		if (destroy && node->destroy == 0)
		{
			node->destroy = destroy;
			node->object = obj;
		}

		return node;
	}

	void mark(ptr_map::iterator i)
	{
		// Mark the node associated with the pointer and then recursively
		// mark all pointers contained by the object pointed to.
		node_t* node = i->second;
		if (node && node->mark != data().current_mark)
		{
			node->mark = data().current_mark;
			for (ptr_map::iterator j = data().pointers.begin(); j != data().pointers.end(); ++j)
			{
				if (node->contains(j->first))
					mark(j);
			}
		}
	}
};

namespace gc_detail
{
	pointer_base::pointer_base()
	{
		data_lock lock;
		data().pointers.insert(ptr_map::value_type(this,(node_t*)0));
	}

	pointer_base::~pointer_base()
	{
		data_lock lock;
		data().pointers.erase(this);
	}

	void pointer_base::reset_node(void* obj, void (*destroy)(void*))
	{
		data_lock lock;
		data().pointers[this] = get_node(obj, destroy);
	}
};

void* operator new(size_t size, const gc_detail::gc_t&)
{
	data_lock lock;

	bool collected = false;

	if (data().threshold != std::numeric_limits<size_t>::max())
	{
		// Determine if we've exceeded the threshold and so should collect.
		data().allocated += size;
		if (data().allocated > data().threshold)
		{
			gc_collect();
			collected = true;
		}
	}

	// Attempt the first allocation.  The standard requires new to throw
	// on failure but user code may change this behavior and VC++ by default
	// only returns 0.  We'll catch exceptions and if we've already collected
	// re-throw the exception.
	void* obj = 0;
	try { obj = ::operator new(size); } catch(...) { if (collected) throw; }

	// If we've failed to allocate but new didn't throw an exception and we've
	// not collected yet we'll collect and then re-try calling new.  If new
	// throws at this point we'll ignore it and let the caller handle it.
	if (obj == 0 && !collected)
	{
		gc_collect();
		obj = ::operator new(size);
	}

	// If we actually allocated memory with new then we need to add it to
	// the node map.
	try
	{
		if (obj != 0)
			data().nodes.insert(new node_t(obj, size));
	}
	catch (...)
	{
		// If inserting into the map failed clean up and simply throw
		// a bad_alloc exception.
		::operator delete(obj);
		throw std::bad_alloc();
	}

	return obj;
}

void operator delete(void* obj, const gc_detail::gc_t&)
{
	data_lock lock;

	// Theoretically, none of this code will throw an exception.
	// If an exception occurs the best we can do is assume that
	// everything worked any way and ignore the exception.
	try
	{
		node_set::iterator i = find_node(obj);
		node_t* node = *i;
		if (node)
			data().nodes.erase(i);

		// This operator really should only be called when
		// construction of an object fails, in which case
		// there won't be a "registered destructor" and the
		// following will only delete the node.
		delete node;
	}
	catch (...)
	{
	}

	// Because there was no "registered destructor" we'll still
	// need to delete the memory allocated by operator new(gc).
	::operator delete(obj);
}

void gc_collect()
{
	data_lock lock;

	// During the sweep phase we'll be deleting objects that could cause
	// a recursive call to 'collect' which would cause invalid results.  So
	// we prevent recursion here.
	if (data().collecting)
		return;

	data().collecting = true;

	// Toggle the 'current_mark' so that we can start over.
	data().current_mark = !data().current_mark;

	{	// Mark phase
		// Loop through all of the pointers looking for 'root' pointers.  A 'root'
		// pointer is a pointer that's not contained within the object pointed
		// to by any other pointer.  When a 'root' pointer is found it is
		// marked, and all the pointers referenced through the 'root' pointer
		// are also marked.
		for (ptr_map::iterator i = data().pointers.begin(); i != data().pointers.end(); ++i)
		{
			gc_detail::pointer_base* ptr = i->first;
			node_t* node = i->second;
			if (!node || node->mark == data().current_mark)
				continue;	// Don't need to check pointers that are marked.

			// If we can't find a node that contains the pointer it's a root pointer
			// and should be marked.
			node_set::iterator j = find_node(ptr);
			if (j == data().nodes.end())
				mark(i);
		}
	}

	{	// Sweep phase
		// Step through all of the nodes and delete any that are not marked.
		for (node_set::iterator i = data().nodes.begin(); i != data().nodes.end(); /*nothing*/)
		{
			node_t* node = *i;
			if (node->mark != data().current_mark)
			{
				if (node->destroy == 0)
				{
					// We must be constructing this object, so we'll just mark it.
					// This prevents premature collection of objects that call
					// gc_collect during the construction phase before any gc_ptr<>'s
					// are assigned to the object.
					node->mark = data().current_mark;
				}
				else
				{
					delete node;
#if _WIN32
					i = data().nodes.erase(i);
#else
					data().nodes.erase(i++);
#endif
					continue;
				}
			}
			++i;
		}
	}

	data().collecting = false;
	data().allocated = 0;
}

void gc_set_threshold(size_t bytes)
{
	data_lock lock;
	data().threshold = bytes;
}
