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
using System.Drawing.Imaging;
using PFormat = System.Drawing.Imaging.PixelFormat;


namespace NeHeLessons
{
        public class Lesson08 : GameWindow
        {
                
                bool light = true; // lighting on/off
                bool blend = true; // blending on/off
                
                float xrot;   // x rotation 
                float yrot;   // y rotation 
                float zrot;   // z rotation 
                float xspeed; // x rotation speed
                float yspeed; // y rotation speed
                float zspeed; // z rotation speed

                float z = -10.0f; // depth into the screen.

                float[] LightAmbient = { 0.5f, 0.5f, 0.5f, 1.0f };      // white ambient light at half intensity (rgba) */
                float[] LightDiffuse = { 1.0f, 1.0f, 1.0f, 1.0f };      // super bright, full intensity diffuse light.
                float[] LightPosition = { 0.0f, 0.0f, 2.0f, 1.0f };     //position of light (x, y, z, (position of light))

                uint filter;			                        // Which Filter To Use (nearest/linear/mipmapped) 
                int[] texture = new int[3];		                // Storage for 3 textures.


                public Lesson08() : base (640, 480, GraphicsMode.Default, "Blending")
                {
                        VSync = VSyncMode.On;
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine ("Press:");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine ("L to enable light");
                        Console.WriteLine ("F to change filter");
                        Console.WriteLine ("B to enable blending");
                        Console.WriteLine ("Page Up/Down to zoom");
                        Console.WriteLine ("Arrows to rotate X and Y axis");
                        Console.WriteLine ("Shit+Right/Left to rotate Z axis");
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
                                        
                                case Key.B :
                                        blend = !blend;              // switch the current value of blend, between 0 and 1.
                                        Console.WriteLine("Blend is now: {0}", blend);
                                        if (!blend)
                                        {
                                                GL.Disable(EnableCap.Blend);            // Turn Blending Off
                                                GL.Enable(EnableCap.DepthTest);         // Turn Depth Testing On
                                        }
                                        else
                                        {
                                                GL.Enable(EnableCap.Blend);              // Turn Blending On
                                                GL.Disable(EnableCap.DepthTest);         // Turn Depth Testing Off
                                        }        
                                        break;
                                case Key.L :
                                        light = !light;
                                        // switch the current value of light, between 0 and 1.
                                        Console.WriteLine ("Light is now: {0}", light);
                                        if (!light) 
                                                GL.Disable (EnableCap.Lighting);
                                        else 
                                                GL.Enable (EnableCap.Lighting);
                                        break;
                                        
                                case Key.F : 
                                        filter += 1;
                                        if (filter > 2) 
                                                filter = 0;
                                        Console.WriteLine ("Filter is now: {0}", filter);
                                        break;
                                }
                        };
                }
                
                
                // Load Bitmaps And Convert To Textures
                void LoadGLTextures()
                {
                        string file = "data/lesson08/glass.bmp";
                        if (!System.IO.File.Exists (file)) {
                                throw new System.IO.FileNotFoundException ();
                        }
                        
                        GL.GenTextures (3, texture);                            // Create Texture       
                        Bitmap image = new Bitmap (file);                       // Load Texture
                        image.RotateFlip (RotateFlipType.RotateNoneFlipY);      // Flip The Bitmap Along The Y-Axis
                        Rectangle rectangle = new Rectangle (0, 0,              // Rectangle For Locking The Bitmap In Memory
                                                             image.Width, 
                                                             image.Height);
                        
                        BitmapData bitmapData = image.LockBits (rectangle,      // Get The Bitmap's Pixel Data From The Locked Bitmap
                                                                ImageLockMode.ReadOnly, 
                                                                PFormat.Format24bppRgb);
                        
                        
                        // Create Nearest Filtered Texture
                        // cheap scaling when image bigger than texture
                        GL.BindTexture (TextureTarget.Texture2D, texture[0]);
                        GL.TexParameter (TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 
                                         (int)TextureMinFilter.Nearest);
                        GL.TexParameter (TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 
                                         (int)TextureMinFilter.Nearest);
                        GL.TexImage2D (TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, 
                                       image.Width, image.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, 
                                       PixelType.UnsignedByte, bitmapData.Scan0);
                        
                        
                        // Create Linear Filtered Texture
                        GL.BindTexture (TextureTarget.Texture2D, texture[1]);
                        GL.TexParameter (TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 
                                         (int)TextureMinFilter.Linear);
                        GL.TexParameter (TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 
                                         (int)TextureMinFilter.Linear);
                        GL.TexImage2D (TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, 
                                       image.Width, image.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, 
                                       PixelType.UnsignedByte, bitmapData.Scan0);
                        
                        
                        GL.BindTexture (TextureTarget.Texture2D, texture[2]);
                        GL.TexParameter (TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 
                                         (int)TextureMinFilter.Linear);
                        GL.TexParameter (TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 
                                         (int)TextureMinFilter.LinearMipmapNearest);
                        
                        // Requieres OpenGL >= 1.4
                        GL.TexImage2D (TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 
                                       image.Width, image.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, 
                                       PixelType.UnsignedByte, bitmapData.Scan0);
                        GL.GenerateMipmap (GenerateMipmapTarget.Texture2D);
                        
                        // Unlock The Pixel Data From Memory
                        image.UnlockBits (bitmapData);
                        // Dispose The Bitmap
                        image.Dispose ();
                }


                
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


                /// <summary>
                /// The main drawing function. 
                /// </summary>
                /// <param name="e">
                /// A <see cref="FrameEventArgs"/>
                /// </param>
                protected override void OnRenderFrame (FrameEventArgs e)
                {
                        base.OnRenderFrame (e);
                        
                        // Clear The Screen And The Depth Buffer
                        GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                        GL.LoadIdentity();                               // Reset The View

                        GL.Translate(0.0f, 0.0f, z);                  // move z units out from the screen.

                        GL.Rotate(xrot, 1.0f, 0.0f, 0.0f);             // Rotate On The X Axis
                        GL.Rotate(yrot, 0.0f, 1.0f, 0.0f);             // Rotate On The Y Axis
                        GL.Rotate(zrot, 0.0f, 0.0f, 1.0f);             // Rotate On The Y Axis

                        GL.Translate(0.0f, 1.0f, 0.0f);                  // move z units out from the screen.
                        GL.Rotate(zrot, 0.0f, 0.0f, 1.0f);             // Rotate On The Y Axis

                        GL.Translate(1.0f, 0.0f, 0.0f);                  // move z units out from the screen.
                        GL.Rotate(yrot, 0.0f, 1.0f, 0.0f);             // Rotate On The Y Axis
                        
                        GL.BindTexture (TextureTarget.Texture2D,        // Select A Texture Based On filter
                                        texture[filter]);                       
                        
                        GL.Begin (BeginMode.Quads);                     // Start Drawing Quads
                        
                        // Front Face
                        GL.Normal3( 0.0f, 0.0f, 1.0f);                                  // Normal Pointing Towards Viewer
                        GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f,  1.0f);      // Point 1 (Front)
                        GL.TexCoord2(1.0f, 0.0f); GL.Vertex3( 1.0f, -1.0f,  1.0f);      // Point 2 (Front)
                        GL.TexCoord2(1.0f, 1.0f); GL.Vertex3( 1.0f,  1.0f,  1.0f);      // Point 3 (Front)
                        GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f,  1.0f,  1.0f);      // Point 4 (Front)
                        // Back Face
                        GL.Normal3( 0.0f, 0.0f,-1.0f);                                  // Normal Pointing Away From Viewer
                        GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, -1.0f);      // Point 1 (Back)
                        GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.0f,  1.0f, -1.0f);      // Point 2 (Back)
                        GL.TexCoord2(0.0f, 1.0f); GL.Vertex3( 1.0f,  1.0f, -1.0f);      // Point 3 (Back)
                        GL.TexCoord2(0.0f, 0.0f); GL.Vertex3( 1.0f, -1.0f, -1.0f);      // Point 4 (Back)
                        // Top Face
                        GL.Normal3( 0.0f, 1.0f, 0.0f);                                  // Normal Pointing Up
                        GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f,  1.0f, -1.0f);      // Point 1 (Top)
                        GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f,  1.0f,  1.0f);      // Point 2 (Top)
                        GL.TexCoord2(1.0f, 0.0f); GL.Vertex3( 1.0f,  1.0f,  1.0f);      // Point 3 (Top)
                        GL.TexCoord2(1.0f, 1.0f); GL.Vertex3( 1.0f,  1.0f, -1.0f);      // Point 4 (Top)
                        // Bottom Face
                        GL.Normal3( 0.0f,-1.0f, 0.0f);                                  // Normal Pointing Down
                        GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.0f, -1.0f, -1.0f);      // Point 1 (Bottom)
                        GL.TexCoord2(0.0f, 1.0f); GL.Vertex3( 1.0f, -1.0f, -1.0f);      // Point 2 (Bottom)
                        GL.TexCoord2(0.0f, 0.0f); GL.Vertex3( 1.0f, -1.0f,  1.0f);      // Point 3 (Bottom)
                        GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f,  1.0f);      // Point 4 (Bottom)
                        // Right face
                        GL.Normal3( 1.0f, 0.0f, 0.0f);                                  // Normal Pointing Right
                        GL.TexCoord2(1.0f, 0.0f); GL.Vertex3( 1.0f, -1.0f, -1.0f);      // Point 1 (Right)
                        GL.TexCoord2(1.0f, 1.0f); GL.Vertex3( 1.0f,  1.0f, -1.0f);      // Point 2 (Right)
                        GL.TexCoord2(0.0f, 1.0f); GL.Vertex3( 1.0f,  1.0f,  1.0f);      // Point 3 (Right)
                        GL.TexCoord2(0.0f, 0.0f); GL.Vertex3( 1.0f, -1.0f,  1.0f);      // Point 4 (Right)
                        // Left Face
                        GL.Normal3(-1.0f, 0.0f, 0.0f);                                  // Normal Pointing Left
                        GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, -1.0f);      // Point 1 (Left)
                        GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f,  1.0f);      // Point 2 (Left)
                        GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.0f,  1.0f,  1.0f);      // Point 3 (Left)
                        GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f,  1.0f, -1.0f);      // Point 4 (Left)
                        GL.End();                                                       // Done Drawing Quads

                        xrot += xspeed;                                 // X Axis Rotation      
                        yrot += yspeed;                                 // Y Axis Rotation
                        zrot += zspeed;                                 // Z Axis Rotation
                        
                        // since this is double buffered, swap the buffers to display what just got drawn.
                        SwapBuffers ();
                }
                
                
                protected override void OnUpdateFrame (FrameEventArgs e)
                {
                        base.OnUpdateFrame (e);
                        HandleKeyPressed ();
                }

                /// <summary>
                /// Update parameters changed by key press in each frame. 
                /// </summary>
                void HandleKeyPressed()
                {
                        if (Keyboard[Key.PageUp])
                        {
                                z -= 0.02f;
                        }
                        
                        if (Keyboard[Key.PageDown])
                        {
                                z += 0.02f;
                        }

                        // walk forward (bob head)
                        if (Keyboard[Key.Up])
                        {
                                xspeed -= 0.01f;
                        }
                        
                        // walk back (bob head)
                        if (Keyboard[Key.Down])
                        {
                                xspeed += 0.01f;
                        }
                        
                        bool shift = Keyboard[Key.ShiftLeft] || Keyboard[Key.ShiftRight];
                        // look left
                        if (Keyboard[Key.Left] && !shift) 
                        {
                                yspeed -= 0.01f;
                        }

                        // look right
                        if (Keyboard[Key.Right] && !shift)
                        { 
                                yspeed += 0.01f;
                        }

                        // look left
                        if (Keyboard[Key.Left] && shift) 
                        {
                                zspeed -= 0.01f;
                        }

                        // look right
                        if (Keyboard[Key.Right] && shift)
                        { 
                                zspeed += 0.01f;
                        }
                        
                }
		
 		/// <summary>Load resources here.
                /// A general OpenGL initialization function.  Sets all of the initial parameters.
                /// </summary>
                /// <param name="e">Not used.</param>
                protected override void OnLoad (EventArgs e)
                {
                        base.OnLoad (e);
                        
                        LoadGLTextures ();                              // Load The Texture(s) 
                        GL.Enable (EnableCap.Texture2D);                // Enable Texture Mapping
                        GL.ShadeModel (ShadingModel.Smooth);            // Enable Smooth Shading
                        GL.ClearColor (0, 0, 0, 0.5f);                  // Black Background
                        GL.ClearDepth (1.0);                              // Depth Buffer Setup
                        GL.Enable (EnableCap.DepthTest);                // Enables Depth Testing
                        GL.DepthFunc (DepthFunction.Lequal);            // The Type Of Depth Testing To Do
                        GL.Hint (HintTarget.PerspectiveCorrectionHint,  // Really Nice Perspective Calculations
                                 HintMode.Nicest);
 

                        /* setup blending */
                        GL.Enable (EnableCap.Blend);                    // Enables Depth Testing
                        GL.BlendFunc(BlendingFactorSrc.SrcAlpha, 
                                     BlendingFactorDest.One);            // Set The Blending Function For Translucency
                        GL.Color4(1.0f, 1.0f, 1.0f, 0.5f);                        
                        
                        // set up light number 1.
                        GL.Light (LightName.Light1, LightParameter.Ambient, LightAmbient);      // add lighting. (ambient)
                        GL.Light (LightName.Light1, LightParameter.Diffuse, LightDiffuse);      // add lighting. (diffuse).
                        GL.Light (LightName.Light1, LightParameter.Position, LightPosition);    // set light position.
                        GL.Enable (EnableCap.Light1);                                           // turn light 1 on.
                        GL.Enable (EnableCap.Lighting);                                           // turn light 1 on.
                }

        }
}


