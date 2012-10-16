/*
 * Copyright (c) 2004 Hewlett-Packard Development Company, L.P.
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
 * Definitions for architectures on which loads and stores of unsigned short
 * are atomic for all legal alignments.
 */

AO_INLINE unsigned short
AO_short_load(const volatile unsigned short *addr)
{
  assert(((size_t)addr & (sizeof(unsigned short) - 1)) == 0);
  /* Cast away the volatile for architectures like IA64 where   */
  /* volatile adds barrier semantics.                           */
  return (*(unsigned short *)addr);
}
#define AO_HAVE_short_load

AO_INLINE void
AO_short_store(volatile unsigned short *addr, unsigned short new_val)
{
  assert(((size_t)addr & (sizeof(unsigned short) - 1)) == 0);
  (*(unsigned short *)addr) = new_val;
}
#define AO_HAVE_short_store
