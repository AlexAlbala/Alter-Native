/*
 * Copyright (c) 2003 Hewlett-Packard Development Company, L.P.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

/*
 * Initialized data and out-of-line functions to support atomic_ops.h
 * go here.  Currently this is needed only for pthread-based atomics
 * emulation, or for compare-and-swap emulation.
 * Pthreads emulation isn't useful on a native Windows platform, and
 * cas emulation is not needed.  Thus we skip this on Windows.
 */

#if defined(HAVE_CONFIG_H)
# include "config.h"
#endif

#if defined(__native_client__) && !defined(AO_USE_NO_SIGNALS) \
    && !defined(AO_USE_NANOSLEEP)
  /* Since NaCl is not recognized by configure yet, we do it here.      */
# define AO_USE_NO_SIGNALS
# define AO_USE_NANOSLEEP
#endif

#if defined(AO_USE_WIN32_PTHREADS) && !defined(AO_USE_NO_SIGNALS)
# define AO_USE_NO_SIGNALS
#endif

#if !defined(_MSC_VER) && !defined(__MINGW32__) && !defined(__BORLANDC__) \
    || defined(AO_USE_NO_SIGNALS)

#undef AO_REQUIRE_CAS

#include <pthread.h>

#ifndef AO_USE_NO_SIGNALS
# include <signal.h>
#endif

#ifdef AO_USE_NANOSLEEP
  /* This requires _POSIX_TIMERS feature. */
# include <sys/time.h>
# include <time.h>
#elif defined(AO_USE_WIN32_PTHREADS)
# include <windows.h> /* for Sleep() */
#elif defined(_HPUX_SOURCE)
# include <sys/time.h>
#else
# include <sys/select.h>
#endif

#include "atomic_ops.h"  /* Without cas emulation! */

#ifndef AO_HAVE_double_t
# include "atomic_ops/sysdeps/standard_ao_double_t.h"
#endif

/*
 * Lock for pthreads-based implementation.
 */

pthread_mutex_t AO_pt_lock = PTHREAD_MUTEX_INITIALIZER;

/*
 * Out of line compare-and-swap emulation based on test and set.
 *
 * We use a small table of locks for different compare_and_swap locations.
 * Before we update perform a compare-and-swap, we grab the corresponding
 * lock.  Different locations may hash to the same lock, but since we
 * never acquire more than one lock at a time, this can't deadlock.
 * We explicitly disable signals while we perform this operation.
 *
 * FIXME: We should probably also support emulation based on Lamport
 * locks, since we may not have test_and_set either.
 */
#define AO_HASH_SIZE 16

#define AO_HASH(x) (((unsigned long)(x) >> 12) & (AO_HASH_SIZE-1))

AO_TS_t AO_locks[AO_HASH_SIZE] = {
  AO_TS_INITIALIZER, AO_TS_INITIALIZER, AO_TS_INITIALIZER, AO_TS_INITIALIZER,
  AO_TS_INITIALIZER, AO_TS_INITIALIZER, AO_TS_INITIALIZER, AO_TS_INITIALIZER,
  AO_TS_INITIALIZER, AO_TS_INITIALIZER, AO_TS_INITIALIZER, AO_TS_INITIALIZER,
  AO_TS_INITIALIZER, AO_TS_INITIALIZER, AO_TS_INITIALIZER, AO_TS_INITIALIZER,
};

static AO_T dummy = 1;

/* Spin for 2**n units. */
void AO_spin(int n)
{
  int i;
  AO_T j = AO_load(&dummy);

  for (i = 0; i < (2 << n); ++i)
    {
       j *= 5;
       j -= 4;
    }
  AO_store(&dummy, j);
}

void AO_pause(int n)
{
  if (n < 12)
    AO_spin(n);
  else
    {
#     ifdef AO_USE_NANOSLEEP
        struct timespec ts;
        ts.tv_sec = 0;
        ts.tv_nsec = (n > 28 ? 100000 * 1000 : 1 << (n - 2));
        nanosleep(&ts, 0);
#     elif defined(AO_USE_WIN32_PTHREADS)
        Sleep(n > 28 ? 100 : 1 << (n - 22)); /* in millis */
#     else
        struct timeval tv;
        /* Short async-signal-safe sleep. */
        tv.tv_sec = 0;
        tv.tv_usec = n > 28 ? 100000 : 1 << (n - 12);
        select(0, 0, 0, 0, &tv);
#     endif
    }
}

static void lock_ool(volatile AO_TS_t *l)
{
  int i = 0;

  while (AO_test_and_set_acquire(l) == AO_TS_SET)
    AO_pause(++i);
}

AO_INLINE void lock(volatile AO_TS_t *l)
{
  if (AO_test_and_set_acquire(l) == AO_TS_SET)
    lock_ool(l);
}

AO_INLINE void unlock(volatile AO_TS_t *l)
{
  AO_CLEAR(l);
}

#ifndef AO_USE_NO_SIGNALS
  static sigset_t all_sigs;
  static volatile AO_t initialized = 0;
  static volatile AO_TS_t init_lock = AO_TS_INITIALIZER;
#endif

int AO_compare_and_swap_emulation(volatile AO_t *addr, AO_t old,
                                  AO_t new_val)
{
  AO_TS_t *my_lock = AO_locks + AO_HASH(addr);
  int result;

# ifndef AO_USE_NO_SIGNALS
    sigset_t old_sigs;
    if (!AO_load_acquire(&initialized))
    {
      lock(&init_lock);
      if (!initialized) sigfillset(&all_sigs);
      unlock(&init_lock);
      AO_store_release(&initialized, 1);
    }
    sigprocmask(SIG_BLOCK, &all_sigs, &old_sigs);
        /* Neither sigprocmask nor pthread_sigmask is 100%      */
        /* guaranteed to work here.  Sigprocmask is not         */
        /* guaranteed be thread safe, and pthread_sigmask       */
        /* is not async-signal-safe.  Under linuxthreads,       */
        /* sigprocmask may block some pthreads-internal         */
        /* signals.  So long as we do that for short periods,   */
        /* we should be OK.                                     */
# endif
  lock(my_lock);
  if (*addr == old)
    {
      *addr = new_val;
      result = 1;
    }
  else
    result = 0;
  unlock(my_lock);
# ifndef AO_USE_NO_SIGNALS
    sigprocmask(SIG_SETMASK, &old_sigs, NULL);
# endif
  return result;
}

int AO_compare_double_and_swap_double_emulation(volatile AO_double_t *addr,
                                                AO_t old_val1, AO_t old_val2,
                                                AO_t new_val1, AO_t new_val2)
{
  AO_TS_t *my_lock = AO_locks + AO_HASH(addr);
  int result;

# ifndef AO_USE_NO_SIGNALS
    sigset_t old_sigs;
    if (!AO_load_acquire(&initialized))
    {
      lock(&init_lock);
      if (!initialized) sigfillset(&all_sigs);
      unlock(&init_lock);
      AO_store_release(&initialized, 1);
    }
    sigprocmask(SIG_BLOCK, &all_sigs, &old_sigs);
        /* Neither sigprocmask nor pthread_sigmask is 100%      */
        /* guaranteed to work here.  Sigprocmask is not         */
        /* guaranteed be thread safe, and pthread_sigmask       */
        /* is not async-signal-safe.  Under linuxthreads,       */
        /* sigprocmask may block some pthreads-internal         */
        /* signals.  So long as we do that for short periods,   */
        /* we should be OK.                                     */
# endif
  lock(my_lock);
  if (addr -> AO_val1 == old_val1 && addr -> AO_val2 == old_val2)
    {
      addr -> AO_val1 = new_val1;
      addr -> AO_val2 = new_val2;
      result = 1;
    }
  else
    result = 0;
  unlock(my_lock);
# ifndef AO_USE_NO_SIGNALS
    sigprocmask(SIG_SETMASK, &old_sigs, NULL);
# endif
  return result;
}

void AO_store_full_emulation(volatile AO_t *addr, AO_t val)
{
  AO_TS_t *my_lock = AO_locks + AO_HASH(addr);
  lock(my_lock);
  *addr = val;
  unlock(my_lock);
}

#else /* Non-posix platform */

extern int AO_non_posix_implementation_is_entirely_in_headers;

#endif
