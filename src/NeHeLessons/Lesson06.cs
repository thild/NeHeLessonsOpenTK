using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;


namespace NeHeLessons
{
        public class Lesson06 : GameWindow
        {

                float xrot, yrot, zrot;         // floats for x rotation, y rotation, z rotation 
                int[] texture = new int[1];     // storage for one texture
                
                public Lesson06 () : base(640, 480, GraphicsMode.Default, "Texture Mapping")
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

                // Load Bitmaps And Convert To Textures
                void LoadGLTextures ()
                {
                        string file = "data/lesson06/NeHe.bmp";
                        if (!System.IO.File.Exists (file)) {
                                throw new System.IO.FileNotFoundException ();
                        }
                        Bitmap image = new Bitmap (file);
                        image.RotateFlip (RotateFlipType.RotateNoneFlipY);
                        System.Drawing.Imaging.BitmapData bitmapData;
                        Rectangle rect = new Rectangle (0, 0, image.Width, image.Height);
                        
                        bitmapData = image.LockBits (rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, 
                                                     System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                        
                        GL.GenTextures (1, this.texture);
                        
                        // Create Linear Filtered Texture
                        GL.BindTexture (TextureTarget.Texture2D, this.texture[0]);
                        GL.TexParameter (TextureTarget.Texture2D, 
                                         TextureParameterName.TextureMagFilter, (int)All.Linear);
                        GL.TexParameter (TextureTarget.Texture2D, 
                                         TextureParameterName.TextureMinFilter, (int)All.Linear);
                        GL.TexImage2D (TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 
                                       image.Width, image.Height, 0, 
                                       PixelFormat.Bgr, PixelType.UnsignedByte, bitmapData.Scan0);
                        
                        // Unlock The Pixel Data From Memory
                        image.UnlockBits (bitmapData);
                        // Dispose The Bitmap
                        image.Dispose ();
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
                        
                        GL.Viewport(ClientRectangle.X, ClientRectangle.Y, 
                                    ClientRectangle.Width, ClientRectangle.Height);
                        GL.MatrixMode(MatrixMode.Projection);
                        Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, 
                                                                                  Width / (float)Height, 1.0f, 100.0f);
                        GL.LoadMatrix(ref projection);
                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.LoadIdentity();
                }

                protected override void OnRenderFrame (FrameEventArgs e)
                {
                        base.OnRenderFrame (e);
                        
                        GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                        GL.LoadIdentity ();
                        
                        GL.Translate (0.0f, 0.0f, -5.0f);                       // move 5 units into the screen.
                        GL.Rotate (xrot, 1.0f, 0.0f, 0.0f);                     // Rotate On The X Axis
                        GL.Rotate (yrot, 0.0f, 1.0f, 0.0f);                     // Rotate On The Y Axis
                        GL.Rotate (zrot, 0.0f, 0.0f, 1.0f);                     // Rotate On The Z Axis
                        GL.BindTexture (TextureTarget.Texture2D, texture[0]);   // choose the texture to use.
                        GL.Begin (BeginMode.Quads);                        // begin drawing a cube
                        
                        // Front Face (note that the texture's corners have to match the quad's corners)
                        GL.TexCoord2 (0.0f, 0.0f);       
                        GL.Vertex3 (-1.0f, -1.0f, 1.0f);        // Bottom Left Of The Texture and Quad
                        GL.TexCoord2 (1.0f, 0.0f);
                        GL.Vertex3 (1.0f, -1.0f, 1.0f);         // Bottom Right Of The Texture and Quad
                        GL.TexCoord2 (1.0f, 1.0f);
                        GL.Vertex3 (1.0f, 1.0f, 1.0f);          // Top Right Of The Texture and Quad
                        GL.TexCoord2 (0.0f, 1.0f);
                        GL.Vertex3 (-1.0f, 1.0f, 1.0f);         // Top Left Of The Texture and Quad
                        
                        // Back Face
                        GL.TexCoord2 (1.0f, 0.0f);
                        GL.Vertex3 (-1.0f, -1.0f, -1.0f);       // Bottom Right Of The Texture and Quad
                        GL.TexCoord2 (1.0f, 1.0f);
                        GL.Vertex3 (-1.0f, 1.0f, -1.0f);        // Top Right Of The Texture and Quad
                        GL.TexCoord2 (0.0f, 1.0f);
                        GL.Vertex3 (1.0f, 1.0f, -1.0f);         // Top Left Of The Texture and Quad
                        GL.TexCoord2 (0.0f, 0.0f);
                        GL.Vertex3 (1.0f, -1.0f, -1.0f);        // Bottom Left Of The Texture and Quad
                        
                        // Top Face
                        GL.TexCoord2 (0.0f, 1.0f);
                        GL.Vertex3 (-1.0f, 1.0f, -1.0f);        // Top Left Of The Texture and Quad
                        GL.TexCoord2 (0.0f, 0.0f);
                        GL.Vertex3 (-1.0f, 1.0f, 1.0f);         // Bottom Left Of The Texture and Quad
                        GL.TexCoord2 (1.0f, 0.0f);
                        GL.Vertex3 (1.0f, 1.0f, 1.0f);          // Bottom Right Of The Texture and Quad
                        GL.TexCoord2 (1.0f, 1.0f);
                        GL.Vertex3 (1.0f, 1.0f, -1.0f);         // Top Right Of The Texture and Quad
                        
                        // Bottom Face       
                        GL.TexCoord2 (1.0f, 1.0f);
                        GL.Vertex3 (-1.0f, -1.0f, -1.0f);       // Top Right Of The Texture and Quad
                        GL.TexCoord2 (0.0f, 1.0f);
                        GL.Vertex3 (1.0f, -1.0f, -1.0f);        // Top Left Of The Texture and Quad
                        GL.TexCoord2 (0.0f, 0.0f);
                        GL.Vertex3 (1.0f, -1.0f, 1.0f);         // Bottom Left Of The Texture and Quad
                        GL.TexCoord2 (1.0f, 0.0f);
                        GL.Vertex3 (-1.0f, -1.0f, 1.0f);        // Bottom Right Of The Texture and Quad
                        
                        // Right face
                        GL.TexCoord2 (1.0f, 0.0f);
                        GL.Vertex3 (1.0f, -1.0f, -1.0f);        // Bottom Right Of The Texture and Quad
                        GL.TexCoord2 (1.0f, 1.0f);
                        GL.Vertex3 (1.0f, 1.0f, -1.0f);         // Top Right Of The Texture and Quad
                        GL.TexCoord2 (0.0f, 1.0f);
                        GL.Vertex3 (1.0f, 1.0f, 1.0f);          // Top Left Of The Texture and Quad
                        GL.TexCoord2 (0.0f, 0.0f);
                        GL.Vertex3 (1.0f, -1.0f, 1.0f);         // Bottom Left Of The Texture and Quad
                        
                        // Left Face
                        GL.TexCoord2 (0.0f, 0.0f);
                        GL.Vertex3 (-1.0f, -1.0f, -1.0f);       // Bottom Left Of The Texture and Quad
                        GL.TexCoord2 (1.0f, 0.0f);
                        GL.Vertex3 (-1.0f, -1.0f, 1.0f);        // Bottom Right Of The Texture and Quad
                        GL.TexCoord2 (1.0f, 1.0f);
                        GL.Vertex3 (-1.0f, 1.0f, 1.0f);         // Top Right Of The Texture and Quad
                        GL.TexCoord2 (0.0f, 1.0f);
                        GL.Vertex3 (-1.0f, 1.0f, -1.0f);        // Top Left Of The Texture and Quad
                        GL.End ();                              // done with the polygon.
                        
                        xrot += 0.3f;                           // X Axis Rotation      
                        yrot += 0.2f;                           // Y Axis Rotation
                        zrot += 0.4f;                           // Z Axis Rotation
                        SwapBuffers ();
                }

                /// <summary>Load resources here.
                /// A general OpenGL initialization function.  Sets all of the initial parameters.
                /// </summary>
                /// <param name="e">Not used.</param>
                protected override void OnLoad (EventArgs e)
                {
                        base.OnLoad (e);
                        LoadGLTextures();
                        GL.Enable (EnableCap.Texture2D);                // Enable Texture Mapping
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



