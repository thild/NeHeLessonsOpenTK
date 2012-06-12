#region BSD License
/*
 BSD License
Copyright (c) 2002, Randy Ridge, The CsGL Development Team
http://csgl.sourceforge.net/
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions
are met:

1. Redistributions of source code must retain the above copyright notice,
   this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

3. Neither the name of The CsGL Development Team nor the names of its
   contributors may be used to endorse or promote products derived from this
   software without specific prior written permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
   FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
   COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
   INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
   BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
   LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
   CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
   LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
   ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
   POSSIBILITY OF SUCH DAMAGE.
 */
#endregion BSD License

#region Original Credits / License
/* Copyright (c) Mark J. Kilgard, 1994, 1997. */

/**
(c) Copyright 1993, Silicon Graphics, Inc.

ALL RIGHTS RESERVED

Permission to use, copy, modify, and distribute this software
for any purpose and without fee is hereby granted, provided
that the above copyright notice appear in all copies and that
both the copyright notice and this permission notice appear in
supporting documentation, and that the name of Silicon
Graphics, Inc. not be used in advertising or publicity
pertaining to distribution of the software without specific,
written prior permission.

THE MATERIAL EMBODIED ON THIS SOFTWARE IS PROVIDED TO YOU
"AS-IS" AND WITHOUT WARRANTY OF ANY KIND, EXPRESS, IMPLIED OR
OTHERWISE, INCLUDING WITHOUT LIMITATION, ANY WARRANTY OF
MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE.  IN NO
EVENT SHALL SILICON GRAPHICS, INC.  BE LIABLE TO YOU OR ANYONE
ELSE FOR ANY DIRECT, SPECIAL, INCIDENTAL, INDIRECT OR
CONSEQUENTIAL DAMAGES OF ANY KIND, OR ANY DAMAGES WHATSOEVER,
INCLUDING WITHOUT LIMITATION, LOSS OF PROFIT, LOSS OF USE,
SAVINGS OR REVENUE, OR THE CLAIMS OF THIRD PARTIES, WHETHER OR
NOT SILICON GRAPHICS, INC.  HAS BEEN ADVISED OF THE POSSIBILITY
OF SUCH LOSS, HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
ARISING OUT OF OR IN CONNECTION WITH THE POSSESSION, USE OR
PERFORMANCE OF THIS SOFTWARE.

US Government Users Restricted Rights

Use, duplication, or disclosure by the Government is subject to
restrictions set forth in FAR 52.227.19(c)(2) or subparagraph
(c)(1)(ii) of the Rights in Technical Data and Computer
Software clause at DFARS 252.227-7013 and/or in similar or
successor clauses in the FAR or the DOD or NASA FAR
Supplement.  Unpublished-- rights reserved under the copyright
laws of the United States.  Contractor/manufacturer is Silicon
Graphics, Inc., 2011 N.  Shoreline Blvd., Mountain View, CA
94039-7311.

OpenGL(TM) is a trademark of Silicon Graphics, Inc.
*/
#endregion Original Credits / License

using System;

namespace OpenTK.Graphics.OpenGL {
 /// <summary>
 /// Implements GLUT shapes.
 /// </summary>
 [Serializable]
 public abstract class GLUT {
         // --- Fields ---
         #region Private Fields
         private static GLUquadric quadObj;

         // Box Data:
         private static float[/*6*/, /*3*/] n = {
                 {-1.0f,  0.0f,  0.0f},
                 { 0.0f,  1.0f,  0.0f},
                 { 1.0f,  0.0f,  0.0f},
                 { 0.0f, -1.0f,  0.0f},
                 { 0.0f,  0.0f,  1.0f},
                 { 0.0f,  0.0f, -1.0f}
         };

         private static int[/*6*/, /*4*/] faces = {
                 {0, 1, 2, 3},
                 {3, 2, 6, 7},
                 {7, 6, 5, 4},
                 {4, 5, 1, 0},
                 {5, 6, 2, 1},
                 {7, 4, 0, 3}
         };

         private const float M_PI = 3.14159265358979323846f;
         private static float[ , ] dodec = new float[20, 3];
         private static bool dodecinited = false;

         // Octahedron Data: The Octahedron Produced Is Centered At The Origin And Has A Radius Of 1.0f
         private static float[ , ] odata = {
                 {1.0f, 0.0f, 0.0f},
                 {-1.0f, 0.0f, 0.0f},
                 {0.0f, 1.0f, 0.0f},
                 {0.0f, -1.0f, 0.0f},
                 {0.0f, 0.0f, 1.0f},
                 {0.0f, 0.0f, -1.0f}
         };

         private static int[ , ] ondex = {
                 {0, 4, 2},
                 {1, 2, 4},
                 {0, 3, 4},
                 {1, 4, 3},
                 {0, 2, 5},
                 {1, 5, 2},
                 {0, 5, 3},
                 {1, 3, 5}
         };

         // Icosahedron Data: These Numbers Are Rigged To Make An Icosahedron Of Radius 1.0f
         private const float X = 0.525731112119133606f;
         private const float Z = 0.850650808352039932f;

         private static float[ , ] idata = {
                 {-X, 0, Z},
                 {X, 0, Z},
                 {-X, 0, -Z},
                 {X, 0, -Z},
                 {0, Z, X},
                 {0, Z, -X},
                 {0, -Z, X},
                 {0, -Z, -X},
                 {Z, X, 0},
                 {-Z, X, 0},
                 {Z, -X, 0},
                 {-Z, -X, 0}
         };

         private static int[ , ] index = {
                 {0, 4, 1},
                 {0, 9, 4},
                 {9, 5, 4},
                 {4, 5, 8},
                 {4, 8, 1},
                 {8, 10, 1},
                 {8, 3, 10},
                 {5, 3, 8},
                 {5, 2, 3},
                 {2, 7, 3},
                 {7, 10, 3},
                 {7, 6, 10},
                 {7, 11, 6},
                 {11, 0, 6},
                 {0, 1, 6},
                 {6, 1, 10},
                 {9, 0, 11},
                 {9, 11, 2},
                 {9, 2, 5},
                 {7, 2, 11},
         };

         // Tetrahedron Data:
         private const float T = 1.73205080756887729f;

         private static float[ , ] tdata = {
                 {T, T, T},
                 {T, -T, -T},
                 {-T, T, -T},
                 {-T, -T, T}
         };

         private static int[ , ] tndex = {
                 {0, 1, 3},
                 {2, 1, 0},
                 {3, 2, 0},
                 {1, 2, 3}
         };

         // Teapot Data:
         private static int[/*10*/ , /*16*/ ] patchdata = {
                 // Rim
                 {102, 103, 104, 105, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15},
                 // Body
                 {12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27},
                 {24, 25, 26, 27, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40},
                 // Lid
                 {96, 96, 96, 96, 97, 98, 99, 100, 101, 101, 101, 101, 0, 1, 2, 3,},
                 {0, 1, 2, 3, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117},
                 // Bottom
                 {118, 118, 118, 118, 124, 122, 119, 121, 123, 126, 125, 120, 40, 39, 38, 37},
                 // Handle
                 {41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56},
                 {53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 28, 65, 66, 67},
                 // Spout
                 {68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83},
                 {80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95}
         };

         private static float[/*126*/, /*3*/] cpdata = {
                 {0.2f, 0.0f, 2.7f},
                 {0.2f, -0.112f, 2.7f},
                 {0.112f, -0.2f, 2.7f},
                 {0.0f, -0.2f, 2.7f},
                 {1.3375f, 0.0f, 2.53125f},
                 {1.3375f, -0.749f, 2.53125f},
                 {0.749f, -1.3375f, 2.53125f},
                 {0.0f, -1.3375f, 2.53125f},
                 {1.4375f, 0.0f, 2.53125f},
                 {1.4375f, -0.805f, 2.53125f},
                 {0.805f, -1.4375f, 2.53125f},
                 {0.0f, -1.4375f, 2.53125f},
                 {1.5f, 0.0f, 2.4f},
                 {1.5f, -0.84f, 2.4f},
                 {0.84f, -1.5f, 2.4f},
                 {0.0f, -1.5f, 2.4f},
                 {1.75f, 0.0f, 1.875f},
                 {1.75f, -0.98f, 1.875f},
                 {0.98f, -1.75f, 1.875f},
                 {0.0f, -1.75f, 1.875f},
                 {2.0f, 0.0f, 1.35f},
                 {2.0f, -1.12f, 1.35f},
                 {1.12f, -2.0f, 1.35f},
                 {0.0f, -2.0f, 1.35f},
                 {2.0f, 0.0f, 0.9f},
                 {2.0f, -1.12f, 0.9f},
                 {1.12f, -2.0f, 0.9f},
                 {0.0f, -2.0f, 0.9f},
                 {-2.0f, 0.0f, 0.9f},
                 {2.0f, 0.0f, 0.45f},
                 {2.0f, -1.12f, 0.45f},
                 {1.12f, -2.0f, 0.45f},
                 {0.0f, -2.0f, 0.45f},
                 {1.5f, 0.0f, 0.225f},
                 {1.5f, -0.84f, 0.225f},
                 {0.84f, -1.5f, 0.225f},
                 {0.0f, -1.5f, 0.225f},
                 {1.5f, 0.0f, 0.15f},
                 {1.5f, -0.84f, 0.15f},
                 {0.84f, -1.5f, 0.15f},
                 {0.0f, -1.5f, 0.15f},
                 {-1.6f, 0.0f, 2.025f},
                 {-1.6f, -0.3f, 2.025f},
                 {-1.5f, -0.3f, 2.25f},
                 {-1.5f, 0.0f, 2.25f},
                 {-2.3f, 0.0f, 2.025f},
                 {-2.3f, -0.3f, 2.025f},
                 {-2.5f, -0.3f, 2.25f},
                 {-2.5f, 0.0f, 2.25f},
                 {-2.7f, 0.0f, 2.025f},
                 {-2.7f, -0.3f, 2.025f},
                 {-3.0f, -0.3f, 2.25f},
                 {-3.0f, 0.0f, 2.25f},
                 {-2.7f, 0.0f, 1.8f},
                 {-2.7f, -0.3f, 1.8f},
                 {-3.0f, -0.3f, 1.8f},
                 {-3.0f, 0.0f, 1.8f},
                 {-2.7f, 0.0f, 1.575f},
                 {-2.7f, -0.3f, 1.575f},
                 {-3.0f, -0.3f, 1.35f},
                 {-3.0f, 0.0f, 1.35f},
                 {-2.5f, 0.0f, 1.125f},
                 {-2.5f, -0.3f, 1.125f},
                 {-2.65f, -0.3f, 0.9375f},
                 {-2.65f, 0.0f, 0.9375f},
                 {-2.0f, -0.3f, 0.9f},
                 {-1.9f, -0.3f, 0.6f},
                 {-1.9f, 0.0f, 0.6f},
                 {1.7f, 0.0f, 1.425f},
                 {1.7f, -0.66f, 1.425f},
                 {1.7f, -0.66f, 0.6f},
                 {1.7f, 0.0f, 0.6f},
                 {2.6f, 0.0f, 1.425f},
                 {2.6f, -0.66f, 1.425f},
                 {3.1f, -0.66f, 0.825f},
                 {3.1f, 0.0f, 0.825f},
                 {2.3f, 0.0f, 2.1f},
                 {2.3f, -0.25f, 2.1f},
                 {2.4f, -0.25f, 2.025f},
                 {2.4f, 0.0f, 2.025f},
                 {2.7f, 0.0f, 2.4f},
                 {2.7f, -0.25f, 2.4f},
                 {3.3f, -0.25f, 2.4f},
                 {3.3f, 0.0f, 2.4f},
                 {2.8f, 0.0f, 2.475f},
                 {2.8f, -0.25f, 2.475f},
                 {3.525f, -0.25f, 2.49375f},
                 {3.525f, 0.0f, 2.49375f},
                 {2.9f, 0.0f, 2.475f},
                 {2.9f, -0.15f, 2.475f},
                 {3.45f, -0.15f, 2.5125f},
                 {3.45f, 0.0f, 2.5125f},
                 {2.8f, 0.0f, 2.4f},
                 {2.8f, -0.15f, 2.4f},
                 {3.2f, -0.15f, 2.4f},
                 {3.2f, 0.0f, 2.4f},
                 {0.0f, 0.0f, 3.15f},
                 {0.8f, 0.0f, 3.15f},
                 {0.8f, -0.45f, 3.15f},
                 {0.45f, -0.8f, 3.15f},
                 {0.0f, -0.8f, 3.15f},
                 {0.0f, 0.0f, 2.85f},
                 {1.4f, 0.0f, 2.4f},
                 {1.4f, -0.784f, 2.4f},
                 {0.784f, -1.4f, 2.4f},
                 {0.0f, -1.4f, 2.4f},
                 {0.4f, 0.0f, 2.55f},
                 {0.4f, -0.224f, 2.55f},
                 {0.224f, -0.4f, 2.55f},
                 {0.0f, -0.4f, 2.55f},
                 {1.3f, 0.0f, 2.55f},
                 {1.3f, -0.728f, 2.55f},
                 {0.728f, -1.3f, 2.55f},
                 {0.0f, -1.3f, 2.55f},
                 {1.3f, 0.0f, 2.4f},
                 {1.3f, -0.728f, 2.4f},
                 {0.728f, -1.3f, 2.4f},
                 {0.0f, -1.3f, 2.4f},
                 {0.0f, 0.0f, 0.0f},
                 {1.425f, -0.798f, 0.0f},
                 {1.5f, 0.0f, 0.075f},
                 {1.425f, 0.0f, 0.0f},
                 {0.798f, -1.425f, 0.0f},
                 {0.0f, -1.5f, 0.075f},
                 {0.0f, -1.425f, 0.0f},
                 {1.5f, -0.84f, 0.075f},
                 {0.84f, -1.5f, 0.075f}
         };

         private static float[/*2*/, /*2*/, /*2*/] tex = {
                 {
                         {0, 0},
                         {1, 0}
                 },
                 {
                         {0, 1},
                         {1, 1}
                 }
         };
         #endregion Private Fields

         // --- Private Methods ---
         #region DrawBox(float size, uint type)
         /// <summary>
         /// Draws a box.
         /// </summary>
         /// <param name="size">The size of the box.</param>
         /// <param name="type">The type of drawing to do.</param>
         private static void DrawBox(float size, uint type) {
                 float[ , ] v = new float[8, 3];

                 v[0, 0] = v[1, 0] = v[2, 0] = v[3, 0] = -size / 2;
                 v[4, 0] = v[5, 0] = v[6, 0] = v[7, 0] =  size / 2;
                 v[0, 1] = v[1, 1] = v[4, 1] = v[5, 1] = -size / 2;
                 v[2, 1] = v[3, 1] = v[6, 1] = v[7, 1] =  size / 2;
                 v[0, 2] = v[3, 2] = v[4, 2] = v[7, 2] = -size / 2;
                 v[1, 2] = v[2, 2] = v[5, 2] = v[6, 2] =  size / 2;

                 for(int i = 5; i >= 0; i--) {
                         GL.glBegin(type);
                                 GL.glNormal3f(n[i, 0], n[i, 1], n[i, 2]);
                                 GL.glVertex3f(v[faces[i, 0], 0], v[faces[i, 0], 1], v[faces[i, 0], 2]);
                                 GL.glVertex3f(v[faces[i, 1], 0], v[faces[i, 1], 1], v[faces[i, 1], 2]);
                                 GL.glVertex3f(v[faces[i, 2], 0], v[faces[i, 2], 1], v[faces[i, 2], 2]);
                                 GL.glVertex3f(v[faces[i, 3], 0], v[faces[i, 3], 1], v[faces[i, 3], 2]);
                         GL.glEnd();
                 }
         }
         #endregion DrawBox(float size, uint type)

         #region InitQuadObj()
         /// <summary>
         /// Initializes a new quadric.
         /// </summary>
         private static void InitQuadObj() {
                 quadObj = GL.gluNewQuadric();
         }
         #endregion InitQuadObj()

         #region QUAD_OBJ_INIT()
         /// <summary>
         /// Ensures we have a valid quadric.
         /// </summary>
         private static void QUAD_OBJ_INIT() {
                 InitQuadObj();
         }
         #endregion QUAD_OBJ_INIT()

         #region Doughnut(float r, float R, int nsides, int rings)
         /// <summary>
         /// Draws a doughnut.
         /// </summary>
         /// <param name="r">The inner radius.</param>
         /// <param name="R">The outer radius.</param>
         /// <param name="nsides">The number of sides.</param>
         /// <param name="rings">The number of rings.</param>
         private static void Doughnut(float r, float R, int nsides, int rings) {
                 float theta, phi, theta1;
                 float cosTheta, sinTheta;
                 float cosTheta1, sinTheta1;
                 float ringDelta, sideDelta;

                 ringDelta = 2.0f * M_PI / rings;
                 sideDelta = 2.0f * M_PI / nsides;

                 theta = 0.0f;
                 cosTheta = 1.0f;
                 sinTheta = 0.0f;
                 for(int i = rings - 1; i >= 0; i--) {
                         theta1 = theta + ringDelta;
                         cosTheta1 = (float) Math.Cos(theta1);
                         sinTheta1 = (float) Math.Sin(theta1);
                         GL.glBegin(GL.GL_QUAD_STRIP);
                         phi = 0.0f;
                         for(int j = nsides; j >= 0; j--) {
                                 float cosPhi, sinPhi, dist;

                                 phi += sideDelta;
                                 cosPhi = (float) Math.Cos(phi);
                                 sinPhi = (float) Math.Sin(phi);
                                 dist = R + r * cosPhi;

                                 GL.glNormal3f(cosTheta1 * cosPhi, -sinTheta1 * cosPhi, sinPhi);
                                 GL.glVertex3f(cosTheta1 * dist, -sinTheta1 * dist, r * sinPhi);
                                 GL.glNormal3f(cosTheta * cosPhi, -sinTheta * cosPhi, sinPhi);
                                 GL.glVertex3f(cosTheta * dist, -sinTheta * dist,  r * sinPhi);
                         }
                         GL.glEnd();
                         theta = theta1;
                         cosTheta = cosTheta1;
                         sinTheta = sinTheta1;
                 }
         }
         #endregion Doughnut(float r, float R, int nsides, int rings)

         #region InitDodecahedron()
         /// <summary>
         /// Initializes the dodecahedron array.
         /// </summary>
         private static void InitDodecahedron() {
                 float alpha, beta;

                 alpha = (float) (Math.Sqrt(2.0f / (3.0f + (float) (Math.Sqrt(5.0f)))));
                 beta = 1.0f + (float) (Math.Sqrt(6.0f / (3.0f + (float) (Math.Sqrt(5.0f)))) - 2.0f + 2.0f * (float) (Math.Sqrt(2.0 / (3.0 + (float) Math.Sqrt(5.0)))));
                 dodec[0, 0] = -alpha; dodec[0, 1] = 0; dodec[0, 2] = beta;
                 dodec[1, 0] = alpha; dodec[1, 1] = 0; dodec[1, 2] = beta;
                 dodec[2, 0] = -1; dodec[2, 1] = -1; dodec[2, 2] = -1;
                 dodec[3, 0] = -1; dodec[3, 1] = -1; dodec[3, 2] = 1;
                 dodec[4, 0] = -1; dodec[4, 1] = 1; dodec[4, 2] = -1;
                 dodec[5, 0] = -1; dodec[5, 1] = 1; dodec[5, 2] = 1;
                 dodec[6, 0] = 1; dodec[6, 1] = -1; dodec[6, 2] = -1;
                 dodec[7, 0] = 1; dodec[7, 1] = -1; dodec[7, 2] = 1;
                 dodec[8, 0] = 1; dodec[8, 1] = 1; dodec[8, 2] = -1;
                 dodec[9, 0] = 1; dodec[9, 1] = 1; dodec[9, 2] = 1;
                 dodec[10, 0] = beta; dodec[10, 1] = alpha; dodec[10, 2] = 0;
                 dodec[11, 0] = beta; dodec[11, 1] = -alpha; dodec[11, 2] = 0;
                 dodec[12, 0] = -beta; dodec[12, 1] = alpha; dodec[12, 2] = 0;
                 dodec[13, 0] = -beta; dodec[13, 1] = -alpha; dodec[13, 2] = 0;
                 dodec[14, 0] = -alpha; dodec[14, 1] = 0; dodec[14, 2] = -beta;
                 dodec[15, 0] = alpha; dodec[15, 1] = 0; dodec[15, 2] = -beta;
                 dodec[16, 0] = 0; dodec[16, 1] = beta; dodec[16, 2] = alpha;
                 dodec[17, 0] = 0; dodec[17, 1] = beta; dodec[17, 2] = -alpha;
                 dodec[18, 0] = 0; dodec[18, 1] = -beta; dodec[18, 2] = alpha;
                 dodec[19, 0] = 0; dodec[19, 1] = -beta; dodec[19, 2] = -alpha;
         }
         #endregion InitDodecahedron()

         #region Octahedron(uint shadeType)
         /// <summary>
         /// Draws an octahedron.
         /// </summary>
         /// <param name="shadeType">The type of shading for OpenGL to perform.</param>
         private static void Octahedron(uint shadeType) {
                 for(int i = 7; i >= 0; i--) {
                         DrawTriangle(i, odata, ondex, shadeType);
                 }
         }
         #endregion Octahedron(uint shadeType)

         private static void DIFF3(float[] a, float[] b, float[] c) {
                 c[0] = a[0] - b[0];
                 c[1] = a[1] - b[1];
                 c[2] = a[2] - b[2];
         }

         private static void CrossProduct(float[] v1, float[] v2, float[] prod) {
                 float[] p = new float[3];
                 p[0] = v1[1] * v2[2] - v2[1] * v1[2];
                 p[1] = v1[2] * v2[0] - v2[2] * v1[0];
                 p[2] = v1[0] * v2[1] - v2[0] * v1[1];
                 prod[0] = p[0];
                 prod[1] = p[1];
                 prod[2] = p[2];
         }

         private static void Normalize(float[] v) {
                 float d;
                 d = (float) (Math.Sqrt(v[0] * v[0] + v[1] * v[1] + v[2] * v[2]));
                 if(d == 0.0f) {
                         v[0] = d = 1.0f;
                 }
                 d = 1 / d;
                 v[0] *= d;
                 v[1] *= d;
                 v[2] *= d;
         }


         private static void Pentagon(int a, int b, int c, int d, int e, uint shadeType) {
                 float[] n0 = new float[3];
                 float[] d1 = new float[3];
                 float[] d2 = new float[3];
                 float[] da = {dodec[a, 0], dodec[a, 1], dodec[a, 2]};
                 float[] db = {dodec[b, 0], dodec[b, 1], dodec[b, 2]};
                 float[] dc = {dodec[c, 0], dodec[c, 1], dodec[c, 2]};

                 DIFF3(da, db, d1);
                 DIFF3(db, dc, d2);
                 CrossProduct(d1, d2, n0);
                 Normalize(n0);

                 GL.glBegin(shadeType);
                         GL.glNormal3f(n0[0], n0[1], n0[2]);
                         GL.glVertex3f(dodec[a, 0], dodec[a, 1], dodec[a, 2]);
                         GL.glVertex3f(dodec[b, 0], dodec[b, 1], dodec[b, 2]);
                         GL.glVertex3f(dodec[c, 0], dodec[c, 1], dodec[c, 2]);
                         GL.glVertex3f(dodec[d, 0], dodec[d, 1], dodec[d, 2]);
                         GL.glVertex3f(dodec[e, 0], dodec[e, 1], dodec[e, 2]);
                 GL.glEnd();
         }

         private static void Dodecahedron(uint type) {
                 if(!dodecinited) {
                         dodecinited = true;
                         InitDodecahedron();
                 }
                 Pentagon(0, 1, 9, 16, 5, type);
                 Pentagon(1, 0, 3, 18, 7, type);
                 Pentagon(1, 7, 11, 10, 9, type);
                 Pentagon(11, 7, 18, 19, 6, type);
                 Pentagon(8, 17, 16, 9, 10, type);
                 Pentagon(2, 14, 15, 6, 19, type);
                 Pentagon(2, 13, 12, 4, 14, type);
                 Pentagon(2, 19, 18, 3, 13, type);
                 Pentagon(3, 0, 5, 12, 13, type);
                 Pentagon(6, 15, 8, 10, 11, type);
                 Pentagon(4, 17, 8, 15, 14, type);
                 Pentagon(4, 12, 5, 16, 17, type);
         }

         private static void RecordItem(float[] n1, float[] n2, float[] n3, uint shadeType) {
                 float[] q0 = new float[3];
                 float[] q1 = new float[3];

                 DIFF3(n1, n2, q0);
                 DIFF3(n2, n3, q1);
                 CrossProduct(q0, q1, q1);
                 Normalize(q1);

                 GL.glBegin(shadeType);
                         GL.glNormal3fv(q1);
                         GL.glVertex3fv(n1);
                         GL.glVertex3fv(n2);
                         GL.glVertex3fv(n3);
                 GL.glEnd();
         }

         private static void Subdivide(float[] v0, float[] v1, float[] v2, uint shadeType) {
                 int depth;
                 float[] w0 = new float[3];
                 float[] w1 = new float[3];
                 float[] w2 = new float[3];
                 float l;
                 int i, j, k, n;

                 depth = 1;
                 for(i = 0; i < depth; i++) {
                         for(j = 0; i + j < depth; j++) {
                                 k = depth - i - j;
                                 for(n = 0; n < 3; n++) {
                                         w0[n] = (i * v0[n] + j * v1[n] + k * v2[n]) / depth;
                                         w1[n] = ((i + 1) * v0[n] + j * v1[n] + (k - 1) * v2[n]) / depth;
                                         w2[n] = (i * v0[n] + (j + 1) * v1[n] + (k - 1) * v2[n]) / depth;
                                 }
                                 l = (float) (Math.Sqrt(w0[0] * w0[0] + w0[1] * w0[1] + w0[2] * w0[2]));
                                 w0[0] /= l;
                                 w0[1] /= l;
                                 w0[2] /= l;
                                 l = (float) (Math.Sqrt(w1[0] * w1[0] + w1[1] * w1[1] + w1[2] * w1[2]));
                                 w1[0] /= l;
                                 w1[1] /= l;
                                 w1[2] /= l;
                                 l = (float) (Math.Sqrt(w2[0] * w2[0] + w2[1] * w2[1] + w2[2] * w2[2]));
                                 w2[0] /= l;
                                 w2[1] /= l;
                                 w2[2] /= l;
                                 RecordItem(w1, w0, w2, shadeType);
                         }
                 }
         }

         private static void DrawTriangle(int i, float[ , ] data, int[ , ] ndx, uint shadeType) {
                 float[] x0 = {data[ndx[i, 0], 0], data[ndx[i, 0], 1], data[ndx[i, 0], 2]};
                 float[] x1 = {data[ndx[i, 1], 0], data[ndx[i, 1], 1], data[ndx[i, 1], 2]};
                 float[] x2 = {data[ndx[i, 2], 0], data[ndx[i, 2], 1], data[ndx[i, 2], 2]};
                 Subdivide(x0, x1, x2, shadeType);
         }

         private static void Icosahedron(uint shadeType) {
                 for(int i = 19; i >= 0; i--) {
                         DrawTriangle(i, idata, index, shadeType);
                 }
         }

         private static void Tetrahedron(uint shadeType) {
                 for(int i = 3; i >= 0; i--) {
                         DrawTriangle(i, tdata, tndex, shadeType);
                 }
         }

         private static void DrawTeapot(int grid, double scale, uint type) {
                 float[ , , ] p = new float[4, 4, 3];
                 float[ , , ] q = new float[4, 4, 3];
                 float[ , , ] r = new float[4, 4, 3];
                 float[ , , ] s = new float[4, 4, 3];
                 long i, j, k, l;

                 GL.glPushAttrib(GL.GL_ENABLE_BIT | GL.GL_EVAL_BIT);
                 GL.glEnable(GL.GL_AUTO_NORMAL);
                 GL.glEnable(GL.GL_NORMALIZE);
                 GL.glEnable(GL.GL_MAP2_VERTEX_3);
                 GL.glEnable(GL.GL_MAP2_TEXTURE_COORD_2);
                 GL.glPushMatrix();
                 GL.glRotatef(270.0f, 1.0f, 0.0f, 0.0f);
                 GL.glScalef(0.5f * (float) scale, 0.5f * (float) scale, 0.5f * (float) scale);
                 GL.glTranslatef(0.0f, 0.0f, -1.5f);
                 for(i = 0; i < 10; i++) {
                         for(j = 0; j < 4; j++) {
                                 for(k = 0; k < 4; k++) {
                                         for(l = 0; l < 3; l++) {
                                                 p[j, k, l] = cpdata[patchdata[i, j * 4 + k], l];
                                                 q[j, k, l] = cpdata[patchdata[i, j * 4 + (3 - k)], l];
                                                 if(l == 1) {
                                                         q[j, k, l] *= -1.0f;
                                                 }
                                                 if(i < 6) {
                                                         r[j, k, l] = cpdata[patchdata[i, j * 4 + (3 - k)], l];
                                                         if(l == 0) {
                                                                 r[j, k, l] *= -1.0f;
                                                         }
                                                         s[j, k, l] = cpdata[patchdata[i, j * 4 + k], l];
                                                         if(l == 0) {
                                                                 s[j, k, l] *= -1.0f;
                                                         }
                                                         if(l == 1) {
                                                                 s[j, k, l] *= -1.0f;
                                                         }
                                                 }
                                         }
                                 }
                         }

                         int cnt = 0;
                         float[] tex1 = new float[8];
                         for(int d = 0; d < 2; d++) {
                                 for(int e = 0; e < 2; e++) {
                                         for(int f = 0; f < 2; f++) {
                                                 tex1[cnt] = tex[d, e, f];
                                                 cnt++;
                                         }
                                 }
                         }
                         GL.glMap2f(GL.GL_MAP2_TEXTURE_COORD_2, 0, 1, 2, 2, 0, 1, 4, 2, tex1);

                         cnt = 0;
                         float[] p1 = new float[48];
                         for(int d = 0; d < 4; d++) {
                                 for(int e = 0; e < 4; e++) {
                                         for(int f = 0; f < 3; f++) {
                                                 p1[cnt] = p[d, e, f];
                                                 cnt++;
                                         }
                                 }
                         }
                         GL.glMap2f(GL.GL_MAP2_VERTEX_3, 0, 1, 3, 4, 0, 1, 12, 4, p1);
                         GL.glMapGrid2f(grid, 0.0f, 1.0f, grid, 0.0f, 1.0f);
                         GL.glEvalMesh2(type, 0, grid, 0, grid);

                         cnt = 0;
                         int cnt1 = 0;
                         int cnt2 = 0;
                         float[] q1 = new float[48];
                         float[] r1 = new float[48];
                         float[] s1 = new float[48];
                         for(int d = 0; d < 4; d++) {
                                 for(int e = 0; e < 4; e++) {
                                         for(int f = 0; f < 3; f++) {
                                                 q1[cnt] = q[d, e, f];
                                                 cnt++;
                                         }
                                 }
                         }
                         GL.glMap2f(GL.GL_MAP2_VERTEX_3, 0, 1, 3, 4, 0, 1, 12, 4, q1);
                         GL.glEvalMesh2(type, 0, grid, 0, grid);
                         if(i < 6) {
                                 for(int d = 0; d < 4; d++) {
                                         for(int e = 0; e < 4; e++) {
                                                 for(int f = 0; f < 3; f++) {
                                                         r1[cnt1] = r[d, e, f];
                                                         cnt1++;
                                                 }
                                         }
                                 }
                                 GL.glMap2f(GL.GL_MAP2_VERTEX_3, 0, 1, 3, 4, 0, 1, 12, 4, r1);
                                 GL.glEvalMesh2(type, 0, grid, 0, grid);

                                 for(int d = 0; d < 4; d++) {
                                         for(int e = 0; e < 4; e++) {
                                                 for(int f = 0; f < 3; f++) {
                                                         s1[cnt2] = s[d, e, f];
                                                         cnt2++;
                                                 }
                                         }
                                 }
                                 GL.glMap2f(GL.GL_MAP2_VERTEX_3, 0, 1, 3, 4, 0, 1, 12, 4, s1);
                                 GL.glEvalMesh2(type, 0, grid, 0, grid);
                         }
                 }
                 GL.glPopMatrix();
                 GL.glPopAttrib();
         }






         // --- Public Methods ---
         public static void glutWireSphere(double radius, int slices, int stacks) {
                 QUAD_OBJ_INIT();
                 GL.gluQuadricDrawStyle(quadObj, GL.GLU_LINE);
                 GL.gluQuadricNormals(quadObj, GL.GLU_SMOOTH);
                 // If we ever changed/used the texture or orientation state
                 // of quadObj, we'd need to change it to the defaults here
                 // with gluQuadricTexture and/or gluQuadricOrientation.
                 GL.gluSphere(quadObj, radius, slices, stacks);
         }

         public static void glutSolidSphere(double radius, int slices, int stacks) {
                 QUAD_OBJ_INIT();
                 GL.gluQuadricDrawStyle(quadObj, GL.GLU_FILL);
                 GL.gluQuadricNormals(quadObj, GL.GLU_SMOOTH);
                 // If we ever changed/used the texture or orientation state
                 // of quadObj, we'd need to change it to the defaults here
                 // with gluQuadricTexture and/or gluQuadricOrientation.
                 GL.gluSphere(quadObj, radius, slices, stacks);
         }

         public static void glutWireCone(double conebase, double height, int slices, int stacks) {
                 QUAD_OBJ_INIT();
                 GL.gluQuadricDrawStyle(quadObj, GL.GLU_LINE);
                 GL.gluQuadricNormals(quadObj, GL.GLU_SMOOTH);
                 // If we ever changed/used the texture or orientation state
                 // of quadObj, we'd need to change it to the defaults here
                 // with gluQuadricTexture and/or gluQuadricOrientation.
                 GL.gluCylinder(quadObj, conebase, 0.0, height, slices, stacks);
         }

         public static void glutSolidCone(double conebase, double height, int slices, int stacks) {
                 QUAD_OBJ_INIT();
                 GL.gluQuadricDrawStyle(quadObj, GL.GLU_FILL);
                 GL.gluQuadricNormals(quadObj, GL.GLU_SMOOTH);
                 // If we ever changed/used the texture or orientation state
                 // of quadObj, we'd need to change it to the defaults here
                 // with gluQuadricTexture and/or gluQuadricOrientation.
                 GL.gluCylinder(quadObj, conebase, 0.0, height, slices, stacks);
         }

         public static void glutWireCube(double size) {
                 DrawBox((float) size, GL.GL_LINE_LOOP);
         }

         public static void glutSolidCube(double size) {
                 DrawBox((float) size, GL.GL_QUADS);
         }


         public static void glutWireTorus(double innerRadius, double outerRadius, int nsides, int rings) {
                 GL.glPushAttrib(GL.GL_POLYGON_BIT);
                         GL.glPolygonMode(GL.GL_FRONT_AND_BACK, GL.GL_LINE);
                         Doughnut((float) innerRadius, (float) outerRadius, nsides, rings);
                 GL.glPopAttrib();
         }

         public static void glutSolidTorus(double innerRadius, double outerRadius, int nsides, int rings) {
                 Doughnut((float) innerRadius, (float) outerRadius, nsides, rings);
         }

         public static void glutWireDodecahedron() {
                 Dodecahedron(GL.GL_LINE_LOOP);
         }

         public static void glutSolidDodecahedron() {
                 Dodecahedron(GL.GL_TRIANGLE_FAN);
         }

         public static void glutWireOctahedron() {
                 Octahedron(GL.GL_LINE_LOOP);
         }

         public static void glutSolidOctahedron() {
                 Octahedron(GL.GL_TRIANGLES);
         }

         public static void glutWireIcosahedron() {
                 Icosahedron(GL.GL_LINE_LOOP);
         }

         public static void glutSolidIcosahedron() {
                 Icosahedron(GL.GL_TRIANGLES);
         }

         public static void glutWireTetrahedron() {
                 Tetrahedron(GL.GL_LINE_LOOP);
         }

         public static void glutSolidTetrahedron() {
                 Tetrahedron(GL.GL_TRIANGLES);
         }

         public static void glutSolidTeapot(double scale) {
                 DrawTeapot(14, scale, GL.GL_FILL);
         }

         public static void glutWireTeapot(double scale) {
                 DrawTeapot(10, scale, GL.GL_LINE);
         }
 }
}
