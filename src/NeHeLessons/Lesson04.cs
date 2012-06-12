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
        public class Lesson04 : GameWindow
        {
            
                float rtri = 0.0f;      // rotation angle for the triangle.
                float rquad = 0.0f;     // rotation angle for the quadrilateral. 
                
                public Lesson04() : base(640, 480, GraphicsMode.Default, "Rotation")
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
                protected override void OnResize(EventArgs e)
                {
                        base.OnResize(e);
                        
                        GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
                        GL.MatrixMode(MatrixMode.Projection);
                        Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, 
                                                                                  Width / (float)Height, 1.0f, 100.0f);
                        GL.LoadMatrix(ref projection);
                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.LoadIdentity();
                }
        
                protected override void OnRenderFrame(FrameEventArgs e)
                {
                        base.OnRenderFrame(e);
                        
                        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                        GL.LoadIdentity();
                        GL.Translate(-1.5f, 0.0f, -6.0f);               // Move Into The Screen And Left
                                
                        GL.Rotate(rtri, 0.0f, 1.0f, 0.0f);              // Rotate The Triangle On The Y axis ( NEW )
                        
                        GL.Begin(BeginMode.Triangles);                  // Start Drawing A Triangle
                        GL.Color3(1.0f,0.0f,0.0f);		        // Set Top Point Of Triangle To Red
                        GL.Vertex3(0.0f, 1.0f, 0.0f);		        // First Point Of The Triangle
                        GL.Color3(0.0f, 1.0f, 0.0f);		        // Set Left Point Of Triangle To Green
                        GL.Vertex3(1.0f, -1.0f, 0.0f);		        // Second Point Of The Triangle
                        GL.Color3(0.0f, 0.0f, 1.0f);		        // Set Right Point Of Triangle To Blue
                        GL.Vertex3(-1.0f, -1.0f, 0.0f);		        // Third Point Of The Triangle
                        GL.End();		                        // Done Drawing The Triangle
                                
                        GL.LoadIdentity();                              // Reset The Current Modelview Matrix
                        
                        GL.Translate(1.5f, 0.0f, -6.0f);                // Move Right 1.5 Units And Into The Screen 6.0
                        GL.Rotate(rquad, 1.0f, 0.0f, 0.0f);            // Rotate The Quad On The X axis ( NEW )
                        
                        
                        GL.Color3(0.5f, 0.5f, 1.0f);                   // Set The Color To A Nice Blue Shade
                        GL.Begin(BeginMode.Quads);			// Start Drawing A Quad	
                        GL.Vertex3(-1.0f, 1.0f, 0.0f);		        // Top Left Of The Quad
                        GL.Vertex3(1.0f, 1.0f, 0.0f);		        // Top Right Of The Quad
                        GL.Vertex3(1.0f, -1.0f, 0.0f);		        // Bottom Right Of The Quad
                        GL.Vertex3(-1.0f, -1.0f, 0.0f);		        // Bottom Left Of The Quad
                        GL.End();				        // Done Drawing The Quad
                        
                        rtri += 0.2f;					// Increase The Rotation Variable For The Triangle
                        rquad -= 0.15f;					// Decrease The Rotation Variable For The Quad 
                                
                        SwapBuffers();
                }
        
                /// <summary>Load resources here.
                /// A general OpenGL initialization function.  Sets all of the initial parameters.
                /// </summary>
                /// <param name="e">Not used.</param>
                protected override void OnLoad(EventArgs e)
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

                

