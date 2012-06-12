using System;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;

namespace NeHeLessons
{
        public class Primitives
        {
                public Primitives ()
                {
                }
                
                public static void DrawCircle (float radius)
                {
                        GL.Begin (BeginMode.TriangleFan);
                        for (int i = 0; i < 360; i++) {
                                double degInRad = i * Math.PI / 180;
                                GL.Vertex2 (Math.Cos (degInRad) * radius, Math.Sin (degInRad) * radius);
                        }
                        GL.End ();
                }
                
                public static void DrawQuad (float x, float y)
                {
                        DrawQuad (new Vector2 (x, y));
                }
                
                public static void DrawQuad (Vector2 v)
                {
                        GL.Begin (BeginMode.Quads);             // Start Drawing Quads 
                        GL.Vertex3 (-v.X, v.Y, 0.0f);         // Left And Up 1 Unit (Top Left)
                        GL.Vertex3 (v.X, v.Y, 0.0f);          // Right And Up 1 Unit (Top Right)
                        GL.Vertex3 (v.X, -v.Y, 0.0f);         // Right And Down One Unit (Bottom Right)
                        GL.Vertex3 (-v.X, -v.Y, 0.0f);        // Left And Down One Unit (Bottom Left)
                        GL.End ();                              // Done Drawing A Quad                         
                }
                
                public static void DrawCube (float x, float y, float z)
                {
                        DrawCube (new Vector3 (x, y, z));
                }
                
                public static void DrawCube (Vector3 v)
                {
                        GL.Begin (BeginMode.Quads);             // Draw A Quad
                        
                        GL.Vertex3 (v.X, v.Y, -v.Z);         // Top Right Of The Quad (Top)
                        GL.Vertex3 (-v.X, v.Y, -v.Z);        // Top Left Of The Quad (Top)
                        GL.Vertex3 (-v.X, v.Y, v.Z);         // Bottom Left Of The Quad (Top)
                        GL.Vertex3 (v.X, v.Y, v.Z);          // Bottom Right Of The Quad (Top)
                        
                        GL.Vertex3 (v.X, -v.Y, v.Z);         // Top Right Of The Quad (Bottom)
                        GL.Vertex3 (-v.X, -v.Y, v.Z);        // Top Left Of The Quad (Bottom)
                        GL.Vertex3 (-v.X, -v.Y, -v.Z);       // Bottom Left Of The Quad (Bottom)
                        GL.Vertex3 (v.X, -v.Y, -v.Z);        // Bottom Right Of The Quad (Bottom)
                        
                        GL.Vertex3 (v.X, v.Y, v.Z);          // Top Right Of The Quad (Front)
                        GL.Vertex3 (-v.X, v.Y, v.Z);         // Top Left Of The Quad (Front)
                        GL.Vertex3 (-v.X, -v.Y, v.Z);        // Bottom Left Of The Quad (Front)
                        GL.Vertex3 (v.X, -v.Y, v.Z);         // Bottom Right Of The Quad (Front)
                        
                        GL.Vertex3 (v.X, -v.Y, -v.Z);        // Top Right Of The Quad (Back)
                        GL.Vertex3 (-v.X, -v.Y, -v.Z);       // Top Left Of The Quad (Back)
                        GL.Vertex3 (-v.X, v.Y, -v.Z);        // Bottom Left Of The Quad (Back)
                        GL.Vertex3 (v.X, v.Y, -v.Z);         // Bottom Right Of The Quad (Back)
                        
                        GL.Vertex3 (-v.X, v.Y, v.Z);         // Top Right Of The Quad (Left)
                        GL.Vertex3 (-v.X, v.Y, -v.Z);        // Top Left Of The Quad (Left)
                        GL.Vertex3 (-v.X, -v.Y, -v.Z);       // Bottom Left Of The Quad (Left)
                        GL.Vertex3 (-v.X, -v.Y, v.Z);        // Bottom Right Of The Quad (Left)
                        
                        GL.Vertex3 (v.X, v.Y, -v.Z);         // Top Right Of The Quad (Right)
                        GL.Vertex3 (v.X, v.Y, v.Z);          // Top Left Of The Quad (Right)
                        GL.Vertex3 (v.X, -v.Y, v.Z);         // Bottom Left Of The Quad (Right)
                        GL.Vertex3 (v.X, -v.Y, -v.Z);        // Bottom Right Of The Quad (Right)
                        
                        GL.End ();                              // Done Drawing The Quad
                }
                
                public static void DrawCube (Vector3 v, Color4[] c)
                {
                        GL.Begin (BeginMode.Quads);             // Draw A Quad
                        
                        GL.Color4(c[0]);
                        GL.Vertex3 (v.X, v.Y, -v.Z);         // Top Right Of The Quad (Top)
                        GL.Vertex3 (-v.X, v.Y, -v.Z);        // Top Left Of The Quad (Top)
                        GL.Vertex3 (-v.X, v.Y, v.Z);         // Bottom Left Of The Quad (Top)
                        GL.Vertex3 (v.X, v.Y, v.Z);          // Bottom Right Of The Quad (Top)
                        
                        GL.Color4(c[1]);
                        GL.Vertex3 (v.X, -v.Y, v.Z);         // Top Right Of The Quad (Bottom)
                        GL.Vertex3 (-v.X, -v.Y, v.Z);        // Top Left Of The Quad (Bottom)
                        GL.Vertex3 (-v.X, -v.Y, -v.Z);       // Bottom Left Of The Quad (Bottom)
                        GL.Vertex3 (v.X, -v.Y, -v.Z);        // Bottom Right Of The Quad (Bottom)
                        
                        GL.Color4(c[2]);
                        GL.Vertex3 (v.X, v.Y, v.Z);          // Top Right Of The Quad (Front)
                        GL.Vertex3 (-v.X, v.Y, v.Z);         // Top Left Of The Quad (Front)
                        GL.Vertex3 (-v.X, -v.Y, v.Z);        // Bottom Left Of The Quad (Front)
                        GL.Vertex3 (v.X, -v.Y, v.Z);         // Bottom Right Of The Quad (Front)
                        
                        GL.Color4(c[3]);
                        GL.Vertex3 (v.X, -v.Y, -v.Z);        // Top Right Of The Quad (Back)
                        GL.Vertex3 (-v.X, -v.Y, -v.Z);       // Top Left Of The Quad (Back)
                        GL.Vertex3 (-v.X, v.Y, -v.Z);        // Bottom Left Of The Quad (Back)
                        GL.Vertex3 (v.X, v.Y, -v.Z);         // Bottom Right Of The Quad (Back)
                        
                        GL.Color4(c[4]);
                        GL.Vertex3 (-v.X, v.Y, v.Z);         // Top Right Of The Quad (Left)
                        GL.Vertex3 (-v.X, v.Y, -v.Z);        // Top Left Of The Quad (Left)
                        GL.Vertex3 (-v.X, -v.Y, -v.Z);       // Bottom Left Of The Quad (Left)
                        GL.Vertex3 (-v.X, -v.Y, v.Z);        // Bottom Right Of The Quad (Left)
                        
                        GL.Color4(c[5]);
                        GL.Vertex3 (v.X, v.Y, -v.Z);         // Top Right Of The Quad (Right)
                        GL.Vertex3 (v.X, v.Y, v.Z);          // Top Left Of The Quad (Right)
                        GL.Vertex3 (v.X, -v.Y, v.Z);         // Bottom Left Of The Quad (Right)
                        GL.Vertex3 (v.X, -v.Y, -v.Z);        // Bottom Right Of The Quad (Right)
                        
                        GL.End ();                              // Done Drawing The Quad
                }
//                
//                public static void DrawTriangle (float x, float y, float z)
//                {
//                        DrawTriangle (new Vector3 (x, y, z));
//                }
//
//                public static void DrawTriangle (Vector3 v)
//                {
//                        GL.Begin (BeginMode.Triangles);         /* Begin Drawing Triangles */
//                        GL.Vertex3 (0.0f, 1.0f, 0.0f);          // Move Up One Unit From Center (Top Point)
//                        GL.Vertex3 (1.0f, -1.0f, 0.0f);         // Left And Down One Unit (Bottom Left)
//                        GL.Vertex3 (-1.0f, -1.0f, 0.0f);        // Right And Down One Unit (Bottom Right)
//                        GL.End ();                              // Done Drawing A Triangle
//                }
        }
}

