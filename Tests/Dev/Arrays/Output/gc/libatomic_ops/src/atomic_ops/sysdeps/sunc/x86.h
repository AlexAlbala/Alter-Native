/*
 * Copyright (c) 1991-1994 by Xerox Corporation.  All rights reserved.
 * Copyright (c) 1996-1999 by Silicon Graphics.  All rights reserved.
 * Copyright (c) 1999-2003 by Hewlett-Packard Company. All rights reserved.
 *
 *
 * THIS MATERIAL IS PROVIDED AS IS, WITH ABSOLUTELY NO WARRANTY EXPRESSED
 * OR IMPLIED.  ANY USE IS AT YOUR OWN RISK.
 *
 * Permission is hereby granted to use or copy this program
 * for any purpose,  provided the above notices are retained on all copies.
 * Permission to modify the code and to distribute modified code is granted,
 * provided the above notices are retained, and a notice that the code was
 * modified is included with the above copyright notice.
 *
 * Some of the machine specific code was borrowed from our GC distribution.
 */

/* The following really assume we have a 486 or better.                 */

#include "../all_aligned_atomic_load_store.h"

/* Real X86 implementations, except for some old WinChips, appear       */
/* to enforce ordering between memory operations, EXCEPT that a later   */
/* read can pass earlier writes, presumably due to the visible          */
/* presence of store buffers.                                           */
/* We ignore both the WinChips, and the fact that the official specs    */
/* seem to be much weaker (and arguably too weak to be usable).         */

#include "../ordered_except_wr.h"

#include "../test_and_set_t_is_char.h"

#include "../standard_ao_double_t.h"

#if defined(AO_USE_PENTIUM4_INSTRS)
AO_INLINE void
AO_nop_full(void)
{
  __asm__ __volatile__ ("mfence" : : : "memory");
}
#define AO_HAVE_nop_full

#else

/* We could use the cpuid instruction.  But that seems to be slower     */
/* than the default implementation based on test_and_set_full.  Thus    */
/* we omit that bit of misinformation here.                             */

#endif

/* As far as we can tell, the lfence and sfence instructions are not    */
/* currently needed or useful for cached memory accesses.               */

/* Really only works for 486 and later */
AO_INLINE AO_t
AO_fetch_and_add_full (volatile AO_t *p, AO_t incr)
{
  AO_t result;

  __asm__ __volatile__ ("lock; xaddl %0, %1" :
                        "=r" (result), "=m" (*p) : "0" (incr) /* , "m" (*p) */
                        : "memory");
  return result;
}
#define AO_HAVE_fetch_and_add_full

AO_INLINE unsigned char
AO_char_fetch_and_add_full (volatile unsigned char *p, unsigned char incr)
{
  unsigned char result;

  __asm__ __volatile__ ("lock; xaddb %0, %1" :
                        "=q" (result), "=m" (*p) : "0" (incr) /* , "m" (*p) */
                        : "memory");
  return result;
}
#define AO_HAVE_char_fetch_and_add_full

AO_INLINE unsigned short
AO_short_fetch_and_add_full (volatile unsigned short *p, unsigned short incr)
{
  unsigned short result;

  __asm__ __volatile__ ("lock; xaddw %0, %1" :
                        "=r" (result), "=m" (*p) : "0" (incr) /* , "m" (*p) */
                        : "memory");
  return result;
}
#define AO_HAVE_short_fetch_and_add_full

/* Really only works for 486 and later */
AO_INLINE void
AO_or_full (volatile AO_t *p, AO_t incr)
{
  __asm__ __volatile__ ("lock; orl %1, %0" :
                        "=m" (*p) : "r" (incr) /* , "m" (*p) */
                        : "memory");
}
#define AO_HAVE_or_full

AO_INLINE AO_TS_VAL_t
AO_test_and_set_full (volatile AO_TS_t *addr)
{
  AO_TS_t oldval;
  /* Note: the "xchg" instruction does not need a "lock" prefix */
  __asm__ __volatile__ ("xchg %b0, %1"
                        : "=q"(oldval), "=m"(*addr)
                        : "0"(0xff) /* , "m"(*addr) */
                        : "memory");
  return (AO_TS_VAL_t)oldval;
}
#define AO_HAVE_test_and_set_full

/* Returns nonzero if the comparison succeeded. */
AO_INLINE int
AO_compare_and_swap_full (volatile AO_t *addr, AO_t old, AO_t new_val)
{
  char result;
  __asm__ __volatile__ ("lock; cmpxchgl %2, %0; setz %1"
                        : "=m"(*addr), "=a"(result)
                        : "r" (new_val), "a"(old) : "memory");
  return (int) result;
}
#define AO_HAVE_compare_and_swap_full

#if 0
/* FIXME: not tested (and probably wrong). Besides,     */
/* it tickles a bug in Sun C 5.10 (when optimizing).    */
/* Returns nonzero if the comparison succeeded. */
/* Really requires at least a Pentium.          */
AO_INLINE int
AO_compare_double_and_swap_double_full(volatile AO_double_t *addr,
                                       AO_t old_val1, AO_t old_val2,
                                       AO_t new_val1, AO_t new_val2)
{
  char result;
#if __PIC__
  /* If PIC is turned on, we can't use %ebx as it is reserved for the
     GOT pointer.  We can save and restore %ebx because GCC won't be
     using it for anything else (such as any of the m operands) */
  __asm__ __volatile__("pushl %%ebx;"   /* save ebx used for PIC GOT ptr */
                       "movl %6,%%ebx;" /* move new_val1 to %ebx */
                       "lock; cmpxchg8b %0; setz %1;"
                       "pop %%ebx;"     /* restore %ebx */
                       : "=m"(*addr), "=a"(result)
                       : "m"(*addr), "d" (old_val2), "a" (old_val1),
                         "c" (new_val2), "m" (new_val1) : "memory");
#else
  /* We can't just do the same thing in non-PIC mode, because GCC
   * might be using %ebx as the memory operand.  We could have ifdef'd
   * in a clobber, but there's no point doing the push/pop if we don't
   * have to. */
  __asm__ __volatile__("lock; cmpxchg8b %0; setz %1;"
                       : "=m"(*addr), "=a"(result)
                       : /* "m"(*addr), */ "d" (old_val2), "a" (old_val1),
                         "c" (new_val2), "b" (new_val1) : "memory");
#endif
  return (int) result;
}
#define AO_HAVE_compare_double_and_swap_double_full
#endif

#include "../ao_t_is_int.h"
