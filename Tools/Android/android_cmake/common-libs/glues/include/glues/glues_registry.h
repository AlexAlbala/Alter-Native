/*
 * SGI FREE SOFTWARE LICENSE B (Version 2.0, Sept. 18, 2008)
 * Copyright (C) 1991-2000 Silicon Graphics, Inc. All Rights Reserved.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a
 * copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice including the dates of first publication and
 * either this permission notice or a reference to
 * http://oss.sgi.com/projects/FreeB/
 * shall be included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
 * SILICON GRAPHICS, INC. BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF
 * OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 *
 * Except as contained in this notice, the name of Silicon Graphics, Inc.
 * shall not be used in advertising or otherwise to promote the sale, use or
 * other dealings in this Software without prior written authorization from
 * Silicon Graphics, Inc.
 *
 * OpenGL ES CM 1.0 port of GLU by Mike Gorchak <mike@malva.ua>
 */

#ifndef __GLUES_REGISTRY_H__
#define __GLUES_REGISTRY_H__

#if defined(__USE_SDL_GLES__)
   #include <SDL/SDL_opengles.h>
   #ifndef GLAPI
      #define GLAPI GL_API
   #endif
#elif defined (__QNXNTO__)
   #include <GLES/gl.h>
#elif defined(_WIN32) && (defined(_M_IX86) || defined(_M_X64))
   /* mainly for PowerVR OpenGL ES 1.x win32 emulator */
   #include <GLES\gl.h>
   #undef APIENTRY
   #define APIENTRY
   #if defined(GLUES_EXPORTS)
      #define GLAPI __declspec(dllexport)
   #else
      #define GLAPI __declspec(dllimport)
   #endif
#elif defined(ANDROID)
    #include <GLES/gl.h>
    #define APIENTRY 
    #define GLAPI 
#else
   #error "Platform is unsupported"
#endif

#ifdef __cplusplus
   extern "C" {
#endif

/* Version */
#define GLU_VERSION_1_1 0x00000001
#define GLU_VERSION_1_2 0x00000001
#define GLU_VERSION_1_3 0x00000001

/* StringName */
#define GLU_VERSION     100800
#define GLU_EXTENSIONS  100801

GLAPI const GLubyte* APIENTRY gluGetString(GLenum name);
GLAPI GLboolean APIENTRY gluCheckExtension(const GLubyte* extName, const GLubyte* extString);

#ifdef __cplusplus
}
#endif

#endif /* __GLUES_REGISTRY_H__ */
