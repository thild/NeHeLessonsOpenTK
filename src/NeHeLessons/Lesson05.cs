using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;

namespace NeHeLessons
{
        public class Lesson05 : GameWindow
        {

                float rtri = 0.0f;      //rotation angle for the triangle.
                float rquad = 0.0f;     // rotation angle for the quadrilateral.

                public Lesson05 () : base(640, 480, 
                                          GraphicsMode.Default, "3D Shapes")
                {
                        VSync = VSyncMode.On;
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine ("Press:");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine ("F1 to full screen");
                        Console.WriteLine ("Esc to exit");
                        Keyboard.KeyDown += (object sender, KeyboardKeyEventArgs e) => {
                                switch (e.Key) {
                                case  Key.Escape:
                                        Exit ();
                                        break;
                                case Key.F1:
                                        if (WindowState == WindowState.Fullscreen) 
                                                WindowState = WindowState.Normal;
                                        else 
                                                WindowState = WindowState.Fullscreen;
                                        break;
                                default:
                                        //Console.WriteLine("Special key {0} pressed. No action there yet.", e.Key);
                                        break;
                                }
                        };
                }

                
                /// <summary>
                /// Called when your window is resized. Set your viewport here. It is also
                /// a good place to set up your projection matrix (which probably changes
                /// along when the aspect ratio of your window).
                /// </summary>
                /// <param name="e">Not used.</param>
                protected override void OnResize (EventArgs e)
                {
                        base.OnResize (e);
                        
                        GL.Viewport (ClientRectangle.X, ClientRectangle.Y, 
                                     ClientRectangle.Width, ClientRectangle.Height);
                        GL.MatrixMode (MatrixMode.Projection);
                        Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView ((float)Math.PI / 4, 
                                                                                   Width / (float)Height, 
                                                                                   1.0f, 100.0f);
                        GL.LoadMatrix (ref projection);
                        GL.MatrixMode (MatrixMode.Modelview);
                        GL.LoadIdentity ();
                }
                
                protected override void OnRenderFrame (FrameEventArgs e)
                {
                        base.OnRenderFrame (e);
                        
                        GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                        GL.LoadIdentity ();
                        
                        GL.Translate (-1.5f, 0.0f, -6.0f);      // Move Left 1.5 Units And Into The Screen 6.0
                        GL.Rotate (rtri, 0.0f, 1.0f, 0.0f);     // Rotate The Triangle On The Y axis 
                        GL.Begin (BeginMode.Triangles);         // Start Drawing A Triangle
                        GL.Color3 (1.0f, 0.0f, 0.0f);           // Red
                        GL.Vertex3 (0.0f, 1.0f, 0.0f);          // Top Of Triangle (Front)
                        GL.Color3 (0.0f, 1.0f, 0.0f);           // Green
                        GL.Vertex3 (-1.0f, -1.0f, 1.0f);        // Left Of Triangle (Front)
                        GL.Color3 (0.0f, 0.0f, 1.0f);           // Blue
                        GL.Vertex3 (1.0f, -1.0f, 1.0f);         // Right Of Triangle (Front)
                        GL.Color3 (1.0f, 0.0f, 0.0f);           // Red
                        GL.Vertex3 (0.0f, 1.0f, 0.0f);          // Top Of Triangle (Right)
                        GL.Color3 (0.0f, 0.0f, 1.0f);           // Blue
                        GL.Vertex3 (1.0f, -1.0f, 1.0f);         // Left Of Triangle (Right)
                        GL.Color3 (0.0f, 1.0f, 0.0f);           // Green
                        GL.Vertex3 (1.0f, -1.0f, -1.0f);        // Right Of Triangle (Right)
                        GL.Color3 (1.0f, 0.0f, 0.0f);           // Red
                        GL.Vertex3 (0.0f, 1.0f, 0.0f);          // Top Of Triangle (Back)
                        GL.Color3 (0.0f, 1.0f, 0.0f);           // Green
                        GL.Vertex3 (1.0f, -1.0f, -1.0f);        // Left Of Triangle (Back)
                        GL.Color3 (0.0f, 0.0f, 1.0f);           // Blue
                        GL.Vertex3 (-1.0f, -1.0f, -1.0f);       // Right Of Triangle (Back)
                        GL.Color3 (1.0f, 0.0f, 0.0f);           // Red
                        GL.Vertex3 (0.0f, 1.0f, 0.0f);          // Top Of Triangle (Left)
                        GL.Color3 (0.0f, 0.0f, 1.0f);           // Blue
                        GL.Vertex3 (-1.0f, -1.0f, -1.0f);       // Left Of Triangle (Left)
                        GL.Color3 (0.0f, 1.0f, 0.0f);           // Green
                        GL.Vertex3 (-1.0f, -1.0f, 1.0f);        // Right Of Triangle (Left)
                        GL.End ();                              // Done Drawing The Pyramid
                        
                        GL.LoadIdentity ();                     // Reset The Current Modelview Matrix
                        GL.Translate (1.5f, 0.0f, -7.0f);       // Move Right 1.5 Units And Into The Screen 7.0
                        GL.Rotate (rquad, 1.0f, 1.0f, 1.0f);    // Rotate The Quad On The X axis ( NEW )
//                        GL.Begin (BeginMode.Quads);             // Draw A Quad
//                        GL.Color3 (0.0f, 1.0f, 0.0f);           // Set The Color To Green
//                        GL.Vertex3 (1.0f, 1.0f, -1.0f);         // Top Right Of The Quad (Top)
//                        GL.Vertex3 (-1.0f, 1.0f, -1.0f);        // Top Left Of The Quad (Top)
//                        GL.Vertex3 (-1.0f, 1.0f, 1.0f);         // Bottom Left Of The Quad (Top)
//                        GL.Vertex3 (1.0f, 1.0f, 1.0f);          // Bottom Right Of The Quad (Top)
//                        GL.Color3 (1.0f, 0.5f, 0.0f);           // Set The Color To Orange
//                        GL.Vertex3 (1.0f, -1.0f, 1.0f);         // Top Right Of The Quad (Bottom)
//                        GL.Vertex3 (-1.0f, -1.0f, 1.0f);        // Top Left Of The Quad (Bottom)
//                        GL.Vertex3 (-1.0f, -1.0f, -1.0f);       // Bottom Left Of The Quad (Bottom)
//                        GL.Vertex3 (1.0f, -1.0f, -1.0f);        // Bottom Right Of The Quad (Bottom)
//                        GL.Color3 (1.0f, 0.0f, 0.0f);           // Set The Color To Red
//                        GL.Vertex3 (1.0f, 1.0f, 1.0f);          // Top Right Of The Quad (Front)
//                        GL.Vertex3 (-1.0f, 1.0f, 1.0f);         // Top Left Of The Quad (Front)
//                        GL.Vertex3 (-1.0f, -1.0f, 1.0f);        // Bottom Left Of The Quad (Front)
//                        GL.Vertex3 (1.0f, -1.0f, 1.0f);         // Bottom Right Of The Quad (Front)
//                        GL.Color3 (1.0f, 1.0f, 0.0f);           // Set The Color To Yellow
//                        GL.Vertex3 (1.0f, -1.0f, -1.0f);        // Top Right Of The Quad (Back)
//                        GL.Vertex3 (-1.0f, -1.0f, -1.0f);       // Top Left Of The Quad (Back)
//                        GL.Vertex3 (-1.0f, 1.0f, -1.0f);        // Bottom Left Of The Quad (Back)
//                        GL.Vertex3 (1.0f, 1.0f, -1.0f);         // Bottom Right Of The Quad (Back)
//                        GL.Color3 (0.0f, 0.0f, 1.0f);           // Set The Color To Blue
//                        GL.Vertex3 (-1.0f, 1.0f, 1.0f);         // Top Right Of The Quad (Left)
//                        GL.Vertex3 (-1.0f, 1.0f, -1.0f);        // Top Left Of The Quad (Left)
//                        GL.Vertex3 (-1.0f, -1.0f, -1.0f);       // Bottom Left Of The Quad (Left)
//                        GL.Vertex3 (-1.0f, -1.0f, 1.0f);        // Bottom Right Of The Quad (Left)
//                        GL.Color3 (1.0f, 0.0f, 1.0f);           // Set The Color To Violet
//                        GL.Vertex3 (1.0f, 1.0f, -1.0f);         // Top Right Of The Quad (Right)
//                        GL.Vertex3 (1.0f, 1.0f, 1.0f);          // Top Left Of The Quad (Right)
//                        GL.Vertex3 (1.0f, -1.0f, 1.0f);         // Bottom Left Of The Quad (Right)
//                        GL.Vertex3 (1.0f, -1.0f, -1.0f);        // Bottom Right Of The Quad (Right)
//                        GL.End ();                              // Done Drawing The Quad
                        
                        var c = new Color4[] {
                                new Color4 (0.0f, 1.0f, 0.0f, 1.0f),           // Set The Color To Green
                                new Color4 (1.0f, 0.5f, 0.0f, 1.0f),           // Set The Color To Orange
                                new Color4 (1.0f, 0.0f, 0.0f, 1.0f),           // Set The Color To Red
                                new Color4 (1.0f, 1.0f, 0.0f, 1.0f),           // Set The Color To Yellow
                                new Color4 (0.0f, 0.0f, 1.0f, 1.0f),           // Set The Color To Blue
                                new Color4 (1.0f, 0.0f, 1.0f, 1.0f)           // Set The Color To Violet
                        };
                        Primitives.DrawCube(Vector3.One, c);
                        rtri += 0.2f;                           // Increase The Rotation Variable For The Triangle ( NEW )
                        rquad -= 0.15f;                         // Decrease The Rotation Variable For The Quad ( NEW )
                        SwapBuffers ();
                }
                
                /// <summary>Load resources here.
                /// A general OpenGL initialization function.  Sets all of the initial parameters.
                /// </summary>
                /// <param name="e">Not used.</param>
                protected override void OnLoad (EventArgs e)
                {
                        base.OnLoad (e);
                        
                        GL.ClearColor (0.0f, 0.0f, 0.0f, 0.0f);         // This Will Clear The Background Color To Black
                        GL.ClearDepth (1.0);                            // Enables Clearing Of The Depth Buffer
                        GL.DepthFunc (DepthFunction.Lequal);            // The Type Of Depth Test To Do
                        GL.Enable (EnableCap.DepthTest);                // Enables Depth Testing
                        GL.ShadeModel (ShadingModel.Smooth);            // Enables Smooth Color Shading
                        GL.Hint (HintTarget.PerspectiveCorrectionHint, // Really Nice Perspective Calculations
                                HintMode.Nicest);
                }
        }
}




