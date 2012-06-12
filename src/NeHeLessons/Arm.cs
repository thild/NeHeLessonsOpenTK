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
        public class Arm : GameWindow
        {
  
                private float sholderPitch, sholderRoll, sholderYaw, elbowPitch, elbowRoll, elbowYaw, wristPitch, wristRoll, wristYaw;
                
                public Arm () : base(640, 480, 
                                          GraphicsMode.Default, 
                        "Your First Polygon")
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
                                                                                   Width / (float)Height, 1.0f, 100.0f);
                        GL.LoadMatrix (ref projection);
                        GL.MatrixMode (MatrixMode.Modelview);
                        GL.LoadIdentity ();
                }

                /// <summary>
                /// Called when it is time to render the next frame. Add your rendering code here.
                /// </summary>
                /// <param name="e">Contains timing information.</param>
                protected override void OnRenderFrame (FrameEventArgs e)
                {
                        base.OnRenderFrame (e);
                        
                        GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                        GL.PushMatrix();
                                GL.Translate (0.0f, 0.0f, -6.0f);      // Left 1.5 Then Into Screen Six Units
                        
                                GL.Translate (0.0f, -0.5f, 0.0f);
                                GL.Rotate(sholderPitch, 0, 0, 1);
                                GL.Rotate(sholderRoll, 0, 1, 0);
                                GL.Rotate(sholderYaw, 1, 0, 0);
                                GL.Translate (0.0f, 0.5f, 0.0f);
                                
                                GL.PushMatrix();
                                        GL.Scale (0.2f, 0.5f, 0.2f);
                                        Primitives.DrawCube (Vector3.One, new Color4[]{Color4.Red, Color4.Blue, Color4.Green, 
                                                                              Color4.Yellow, Color4.Cyan, Color4.Magenta});
                                GL.PopMatrix();
                                               
                                GL.Translate (0.0f, 0.5f, 0.0f);
                                GL.Rotate(elbowPitch, 0, 0, 1);
                                GL.Rotate(elbowRoll, 0, 1, 0);
                                GL.Rotate(elbowYaw, 1, 0, 0);
                                GL.Translate (0.0f, 0.5f, 0.0f);
                                GL.PushMatrix();
                                        GL.Scale (0.2f, 0.5f, 0.2f);
                                        Primitives.DrawCube (Vector3.One, new Color4[]{Color4.Red, Color4.Blue, Color4.Green, 
                                                                              Color4.Yellow, Color4.Cyan, Color4.Magenta});
                                GL.PopMatrix();
                        
                                GL.Translate (0.0f, 0.5f, 0.0f);
                                GL.Rotate(wristPitch, 0, 0, 1);
                                GL.Rotate(wristRoll, 0, 1, 0);
                                GL.Rotate(wristYaw, 1, 0, 0);
                                GL.Translate (0.0f, 0.3f, 0.0f);
                                
                                GL.PushMatrix();
                                        GL.Scale (0.2f, 0.3f, 0.2f);
                                        Primitives.DrawCube (Vector3.One, new Color4[]{Color4.Red, Color4.Blue, Color4.Green, 
                                                                              Color4.Yellow, Color4.Cyan, Color4.Magenta});
                                GL.PopMatrix();
                        
                        GL.PopMatrix();
                        
                        SwapBuffers ();
                }
                
                protected override void OnUpdateFrame (FrameEventArgs e)
                {
                        base.OnUpdateFrame (e);
                        HandleKeyPressed ();
                }


                /* The function called whenever a key is pressed. */
                void HandleKeyPressed ()
                {
                                
                        if (Keyboard [Key.Q]) {
                                sholderPitch -= 5f;
                        }
                        if (Keyboard [Key.W]) {
                                sholderPitch += 5f;
                        }
                        // walk forward (bob head)
                        if (Keyboard [Key.A]) {
                                elbowPitch -= 5f;
                        }
                        // walk back (bob head)
                        if (Keyboard [Key.S]) {
                                elbowPitch += 5f;
                                
                        }
                        // look left
                        if (Keyboard [Key.Z]) { 
                                wristPitch -= 5f;
                        }
                        // look right
                        if (Keyboard [Key.X]) { 
                                wristPitch += 5f; 
                        }
                        
                        
                        if (Keyboard [Key.E]) {
                                sholderRoll -= 5f;
                        }
                        if (Keyboard [Key.R]) {
                                sholderRoll += 5f;
                        }
                        // walk forward (bob head)
                        if (Keyboard [Key.D]) {
                                elbowRoll -= 5f;
                        }
                        // walk back (bob head)
                        if (Keyboard [Key.F]) {
                                elbowRoll += 5f;
                                
                        }
                        // look left
                        if (Keyboard [Key.C]) { 
                                wristRoll -= 5f;
                        }
                        // look right
                        if (Keyboard [Key.V]) { 
                                wristRoll += 5f; 
                        }


                        if (Keyboard [Key.T]) {
                                sholderYaw -= 5f;
                        }
                        if (Keyboard [Key.Y]) {
                                sholderYaw += 5f;
                        }
                        // walk forward (bob head)
                        if (Keyboard [Key.G]) {
                                elbowYaw -= 5f;
                        }
                        // walk back (bob head)
                        if (Keyboard [Key.H]) {
                                elbowYaw += 5f;
                                
                        }
                        // look left
                        if (Keyboard [Key.B]) { 
                                wristYaw -= 5f;
                        }
                        // look right
                        if (Keyboard [Key.N]) { 
                                wristYaw += 5f; 
                        }

                }                
                
                /// <summary>Load resources here.
                /// A general OpenGL initialization function.  Sets all of the initial parameters.
                /// </summary>
                /// <param name="e">Not used.</param>
                protected override void OnLoad (EventArgs e)
                {
                        base.OnLoad (e);
                        
                        GL.ClearColor (0.0f, 0.0f, 0.0f, 0.5f);         // This Will Clear The Background Color To Black
                        GL.ClearDepth (1.0);                            // Enables Clearing Of The Depth Buffer
                        GL.DepthFunc (DepthFunction.Lequal);            // The Type Of Depth Test To Do
                        GL.Enable (EnableCap.DepthTest);                // Enables Depth Testing
                        GL.ShadeModel (ShadingModel.Smooth);            // Enables Smooth Color Shading
                        GL.Hint (HintTarget.PerspectiveCorrectionHint, // Really Nice Perspective Calculations
                                HintMode.Nicest);
                }
        }
}


