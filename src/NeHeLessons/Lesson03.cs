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
        public class Lesson03 : GameWindow
        {

                public Lesson03 () : base(640, 480, 
                                          GraphicsMode.Default, "Adding Color")
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
                                        Exit();
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
                        Matrix4 projection = 
                                Matrix4.CreatePerspectiveFieldOfView ((float)Math.PI / 4, 
                                                                       Width / (float)Height, 1.0f, 100.0f);
                        GL.LoadMatrix (ref projection);
                        GL.MatrixMode (MatrixMode.Modelview);
                        GL.LoadIdentity ();
                }

                protected override void OnRenderFrame (FrameEventArgs e)
                {
                        base.OnRenderFrame (e);
                        
                        GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                        GL.LoadIdentity ();
                        
                        
                        GL.Translate (-1.5f, 0.0f, -6.0f);      // Left 1.5 Then Into Screen Six Units
                        
                        GL.Begin (BeginMode.Triangles);         /* Begin Drawing Triangles */
                        GL.Color3 (1.0f, 0.0f, 0.0f);           // Set The Color To Red //
                        GL.Vertex3 (0.0f, 1.0f, 0.0f);          // Move Up One Unit From Center (Top Point)
                        GL.Color3 (0.0f, 1.0f, 0.0f);           // Set The Color To Green
                        GL.Vertex3 (1.0f, -1.0f, 0.0f);         // Left And Down One Unit (Bottom Left)
                        GL.Color3 (0.0f, 0.0f, 1.0f);           // Set The Color To Blue
                        GL.Vertex3 (-1.0f, -1.0f, 0.0f);        // Right And Down One Unit (Bottom Right)
                        GL.End ();                              // Done Drawing A Triangle

                        GL.Translate (3.0f, 0.0f, 0.0f);        // Move Right 3 Units

                        GL.Color3 (0.5f, 0.5f, 1.0f);           // Set The Color To Blue One Time Only

                        GL.Begin (BeginMode.Quads);             // Start Drawing Quads 
                        GL.Vertex3 (-1.0f, 1.0f, 0.0f);         // Left And Up 1 Unit (Top Left)
                        GL.Vertex3 (1.0f, 1.0f, 0.0f);          // Right And Up 1 Unit (Top Right)
                        GL.Vertex3 (1.0f, -1.0f, 0.0f);         // Right And Down One Unit (Bottom Right)
                        GL.Vertex3 (-1.0f, -1.0f, 0.0f);        // Left And Down One Unit (Bottom Left)
                        GL.End ();                              // Done Drawing A Quad
                        
                        SwapBuffers (); // Flush the buffer
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
                        GL.Hint(HintTarget.PerspectiveCorrectionHint,   // Really Nice Perspective Calculations
                                HintMode.Nicest);
                }
        }
}


