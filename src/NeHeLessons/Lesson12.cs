using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;


namespace NeHeLessons
{

        class Lesson12 : GameWindow
        {

                
                int[] texture = new int[1];     // Storage For One Texture
                int box;                        // Storage For The Display List
                int top;                        // Storage For The Second Display List
                float xrot;                     // Rotates Cube On The Y Axis
                float yrot;                     // Rotates ModelView On The Y Axis
  
                // Array For Box Colors
                // Bright:  Red, Orange, Yellow, Green, Blue
                static float[,] boxcol = new float[5, 3] { 
                                                                { 1.0f, 0.0f, 0.0f }, 
                                                                { 1.0f, 0.5f, 0.0f }, 
                                                                { 1.0f, 1.0f, 0.0f }, 
                                                                { 0.0f, 1.0f, 0.0f }, 
                                                                { 0.0f, 1.0f, 1.0f }
                                                         };

                // Array For Top Colors
                // Dark:  Red, Orange, Yellow, Green, Blue
                static float[,] topcol = new float[5, 3] { 
                                                                { 0.5f, 0.0f,  0.0f }, 
                                                                { 0.5f, 0.25f, 0.0f }, 
                                                                { 0.5f, 0.5f,  0.0f }, 
                                                                { 0.0f, 0.5f,  0.0f }, 
                                                                { 0.0f, 0.5f,  0.5f } 
                                                        };

                public Lesson12 () : base(640, 480, GraphicsMode.Default, "Display List")
                {
                        VSync = VSyncMode.On;
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine ("Press:");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine ("Arrows to rotate");
                        Console.WriteLine ("F1 to full screen");
                        Console.WriteLine ("Esc to exit");
                        //lambda delegate
                        Keyboard.KeyDown += (object sender, KeyboardKeyEventArgs e) => {
                                switch (e.Key) {
                                case Key.Escape :
                                        Exit ();
                                        break;
                                case Key.F1:
                                        if (WindowState == WindowState.Fullscreen) 
                                                WindowState = WindowState.Normal;
                                        else 
                                                WindowState = WindowState.Fullscreen;
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
                        Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView (MathHelper.PiOver4, 
                                                                                   Width / (float)Height, 0.1f, 100f);
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
                        // Clear The Screen And The Depth Buffer
                        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); 

                        // Select The Texture
                        GL.BindTexture(TextureTarget.Texture2D, 
                                       texture[0]); 

                        for (int yloop = 1; yloop < 6; yloop++) {                                           // Loop Through The Y Plane
                                for (int xloop = 0; xloop < yloop; xloop++) {                               // Loop Through The X Plane
                                        GL.LoadIdentity();
                                        GL.Translate (1.4f + ((xloop) * 2.8f) - ((yloop) * 1.4f),       // Position The Cubes On The Screen //
                                                      ((6f - (yloop)) * 2.4f) - 7f, -20f);
                                        GL.Rotate (45f - (2f * yloop) + xrot, 1f, 0f, 0f);              // Tilt The Cubes Up And Down
                                        GL.Rotate (45f + yrot, 0f, 1f, 0f);                             // Spin Cubes Left And Right
                                        GL.Color3 (ref boxcol[(yloop - 1), 2]);                         // Select A Box Color
                                        GL.CallList (box);                                              // Draw The Box
                                        GL.Color3 (ref topcol[yloop - 1, 2]);                           // Select The Top Color 
                                        GL.CallList (top);                                              // Draw The Top 
                                }
                        }
                        this.SwapBuffers ();
                }

                /// <summary>
                /// Called when it is time to setup the next frame. Add you game logic here.
                /// </summary>
                /// <param name="e">Contains timing information for framerate independent logic.</param>
                protected override void OnUpdateFrame (FrameEventArgs e)
                {
                        base.OnUpdateFrame (e);
                        
                        if(Keyboard[Key.Up]) {
                                xrot -= 0.5f;
                        }
                        
                        if(Keyboard[Key.Down]) {
                                xrot += 0.5f;
                        }
                        
                        if(Keyboard[Key.Left]) {
                                yrot -= 0.5f;
                        }
                        
                        if(Keyboard[Key.Right]) {
                                yrot += 0.5f;
                        }
                }

                
                /// <summary>Load resources here.
                /// A general OpenGL initialization function.  Sets all of the initial parameters.
                /// </summary>
                /// <param name="e">Not used.</param>
                protected override void OnLoad (EventArgs e)
                {
                        base.OnLoad (e);
                        LoadGLTextures ();
                        BuildLists ();
                        GL.Enable (EnableCap.Texture2D);                        // Enable Texture Mapping 
                        GL.ShadeModel (ShadingModel.Smooth);                    // Enable Smooth Shading
                        GL.ClearColor (0.0f, 0.0f, 0.0f, 0.5f);                            // Black Background
                        GL.ClearDepth (1.0);                                     // Depth Buffer Setup
                        GL.Enable (EnableCap.DepthTest);                        // Enables Depth Testing
                        GL.DepthFunc (DepthFunction.Lequal);                    // The Type Of Depth Testing To Do
                        GL.Hint (HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
                        GL.Enable (EnableCap.Light0);                           // Quick And Dirty Lighting (Assumes Light0 Is Set Up)
                        GL.Enable (EnableCap.Lighting);                         // Enable Lighting
                        GL.Enable (EnableCap.ColorMaterial);                    // Enable Material Coloring
                }

                // Build Box Display List
                public void BuildLists ()
                {
                        
                        box = GL.GenLists (2);
                        GL.NewList (box, ListMode.Compile);
                        GL.Begin (BeginMode.Quads);     // Start Drawing Quads
                        
                        // Bottom Face
                        GL.TexCoord2 (1f, 1f);
                        GL.Vertex3 (-1f, -1f, -1f);             // Top Right Of The Texture and Quad
                        GL.TexCoord2 (0f, 1f);
                        GL.Vertex3 (1f, -1f, -1f);              // Top Left Of The Texture and Quad
                        GL.TexCoord2 (0f, 0f);
                        GL.Vertex3 (1f, -1f, 1f);               // Bottom Left Of The Texture and Quad
                        GL.TexCoord2 (1f, 0f);
                        GL.Vertex3 (-1f, -1f, 1f);              // Bottom Right Of The Texture and Quad
                        
                        // Front Face
                        GL.TexCoord2 (0f, 0f);
                        GL.Vertex3 (-1f, -1f, 1f);              // Bottom Left Of The Texture and Quad
                        GL.TexCoord2 (1f, 0f);
                        GL.Vertex3 (1f, -1f, 1f);               // Bottom Right Of The Texture and Quad
                        GL.TexCoord2 (1f, 1f);
                        GL.Vertex3 (1f, 1f, 1f);                // Top Right Of The Texture and Quad
                        GL.TexCoord2 (0f, 1f);
                        GL.Vertex3 (-1f, 1f, 1f);               // Top Left Of The Texture and Quad
                        
                        // Back Face
                        GL.TexCoord2 (1f, 0f);
                        GL.Vertex3 (-1f, -1f, -1f);             // Bottom Right Of The Texture and Quad
                        GL.TexCoord2 (1f, 1f);
                        GL.Vertex3 (-1f, 1f, -1f);              // Top Right Of The Texture and Quad
                        GL.TexCoord2 (0f, 1f);
                        GL.Vertex3 (1f, 1f, -1f);               // Top Left Of The Texture and Quad
                        GL.TexCoord2 (0f, 0f);
                        GL.Vertex3 (1f, -1f, -1f);              // Bottom Left Of The Texture and Quad
                        
                        // Right face
                        GL.TexCoord2 (1f, 0f);
                        GL.Vertex3 (1f, -1f, -1f);              // Bottom Right Of The Texture and Quad
                        GL.TexCoord2 (1f, 1f);
                        GL.Vertex3 (1f, 1f, -1f);               // Top Right Of The Texture and Quad
                        GL.TexCoord2 (0f, 1f);
                        GL.Vertex3 (1f, 1f, 1f);                // Top Left Of The Texture and Quad
                        GL.TexCoord2 (0f, 0f);
                        GL.Vertex3 (1f, -1f, 1f);               // Bottom Left Of The Texture and Quad
                        
                        // Left Face
                        GL.TexCoord2 (0f, 0f);
                        GL.Vertex3 (-1f, -1f, -1f);             // Bottom Left Of The Texture and Quad
                        GL.TexCoord2 (1f, 0f);
                        GL.Vertex3 (-1f, -1f, 1f);              // Bottom Right Of The Texture and Quad
                        GL.TexCoord2 (1f, 1f);
                        GL.Vertex3 (-1f, 1f, 1f);               // Top Right Of The Texture and Quad
                        GL.TexCoord2 (0f, 1f);
                        GL.Vertex3 (-1f, 1f, -1f);              // Top Left Of The Texture and Quad
                        GL.End ();                              // Done Drawing Quads
                        GL.EndList ();                          // Done Building The box List
                        
                        top = box + 1;                          // top List Value Is box List Value +1
                        GL.NewList (top, ListMode.Compile);     // New Compiled top Display List
                        GL.Begin (BeginMode.Quads);             // Start Drawing Quad
                        
                        // Top Face
                        GL.TexCoord2 (0f, 1f);
                        GL.Vertex3 (-1f, 1f, -1f);              // Top Left Of The Texture and Quad
                        GL.TexCoord2 (0f, 0f);
                        GL.Vertex3 (-1f, 1f, 1f);               // Bottom Left Of The Texture and Quad
                        GL.TexCoord2 (1f, 0f);
                        GL.Vertex3 (1f, 1f, 1f);                // Bottom Right Of The Texture and Quad
                        GL.TexCoord2 (1f, 1f);
                        GL.Vertex3 (1f, 1f, -1f);               // Top Right Of The Texture and Quad
                        GL.End ();                              // Done Drawing Quad
                        GL.EndList ();
                        
                }


                void LoadGLTextures ()
                {
                        string file = "data/lesson12/cube.bmp";
                        if (!System.IO.File.Exists (file)) {
                                throw new System.IO.FileNotFoundException ();
                        }
                        Bitmap image = new Bitmap (file);
                        image.RotateFlip (RotateFlipType.RotateNoneFlipY);
                        System.Drawing.Imaging.BitmapData bitmapData;
                        Rectangle rect = new Rectangle (0, 0, image.Width, image.Height);
                        
                        bitmapData = image.LockBits (rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, 
                                                     System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                        
                        GL.GenTextures (1, texture);
                        
                        // Create Linear Filtered Texture
                        GL.BindTexture (TextureTarget.Texture2D, texture[0]);
                        
                        GL.TexParameter (TextureTarget.Texture2D, 
                                         TextureParameterName.TextureMagFilter, (int)All.Linear);
                        GL.TexParameter (TextureTarget.Texture2D, 
                                         TextureParameterName.TextureMinFilter, (int)All.Linear);
                        GL.TexImage2D (TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, 
                                       image.Width, image.Height, 0, 
                                       PixelFormat.Rgb, PixelType.UnsignedByte, bitmapData.Scan0);
                        
                        // Unlock The Pixel Data From Memory
                        image.UnlockBits (bitmapData);
                        // Dispose The Bitmap
                        image.Dispose ();
                }
                
                
                
        }
}






















