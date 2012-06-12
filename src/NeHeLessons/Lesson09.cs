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
        public class Lesson09 : GameWindow
        {

                const int STAR_NUM = 50;        // number of stars to have */
                bool twinkle;                   // twinkle on/off (true = on, false = off)

                // Star structure
                struct Star
                {         
                        public byte r, g, b;            // stars' color
                        public float dist;              // stars' distance from center
                        public float angle;             // stars' current angle
                }              

                Star[] star = new Star[STAR_NUM];    // make 'star' array of STAR_NUM size using info from the structure 'stars'

                float zoom = -15.0f;   // viewing distance from stars.
                float tilt = 90.0f;    // tilt the view
                float spin;            // spin twinkling stars

                uint loop;             // general loop variable

                int[] texture = new int[1];		// Storage for 1 texture.

                Random rnd = new Random();

                public Lesson09() : base (640, 480, GraphicsMode.Default, "Moving Bitmaps In 3D Space")
                {
                        VSync = VSyncMode.On;
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine ("Press:");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine ("T to enable twinkle");
                        Console.WriteLine ("Page Up/Down to zoom");
                        Console.WriteLine ("Arrows Up/Down to tilt");
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
                                case  Key.T:
                                        twinkle = !twinkle;              // switch the current value of twinkle, between 0 and 1.
                                        Console.WriteLine("Twinkle is now: {0}", twinkle);
                                        break;
        
                                        }
                        };

                }
                
                
                // Load Bitmaps And Convert To Textures
                void LoadGLTextures()
                {
                        Console.WriteLine ("Loading textures...");
                        string file = "data/lesson09/Star.bmp";
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


		
		/// <summary>Load resources here.
                /// A general OpenGL initialization function.  Sets all of the initial parameters.
                /// </summary>
                /// <param name="e">Not used.</param>
                protected override void OnLoad (EventArgs e)
                {
                        base.OnLoad (e);
                        Console.WriteLine ("Initializing...");
                        LoadGLTextures ();                              // Load The Texture(s) 
                        GL.Enable (EnableCap.Texture2D);                // Enable Texture Mapping
                        GL.ShadeModel (ShadingModel.Smooth);            // Enable Smooth Shading
                        GL.ClearColor (0, 0, 0, 0.5f);                  // Black Background
                        GL.ClearDepth (1.0);                              // Depth Buffer Setup
                        GL.Hint (HintTarget.PerspectiveCorrectionHint,  // Really Nice Perspective Calculations
                                 HintMode.Nicest);
 

                        /* setup blending */
                        GL.BlendFunc(BlendingFactorSrc.SrcAlpha, 
                                     BlendingFactorDest.One);           // Set The Blending Function For Translucency
                        GL.Enable (EnableCap.Blend);                    // Enables Depth Testing
                        
			/* set up the stars */
                        for (loop = 0; loop < STAR_NUM; loop++)
                        {
                                star[loop].angle = 0.0f;                		// initially no rotation.
                                star[loop].dist = loop * 1.0f / STAR_NUM * 5.0f;        // calculate distance form the center
                                star[loop].r = (byte)rnd.Next(0, 255);            	// random red intensity;
                                star[loop].g = (byte)rnd.Next(0, 255);            	// random green intensity;
                                star[loop].b = (byte)rnd.Next(0, 255);            	// random blue intensity;
                        }
                }
		
		protected override void OnResize (EventArgs e)
                {
                        base.OnResize (e);
                        
                        GL.Viewport(ClientRectangle.X, ClientRectangle.Y, 
                                    ClientRectangle.Width, ClientRectangle.Height);
                        GL.MatrixMode(MatrixMode.Projection);
                        Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 
                                                                                  Width / (float)Height, 0.1f, 100.0f);
                        GL.LoadMatrix(ref projection);
                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.LoadIdentity();
                }


                /* The main drawing function. */
                protected override void OnRenderFrame (FrameEventArgs e)
                {
                        GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                        GL.BindTexture (TextureTarget.Texture2D,        // Select A Texture Based On filter
                                        texture[0]);         

                        for (loop = 0; loop < STAR_NUM; ++loop)
                        {        // loop through all the stars.
                                GL.LoadIdentity();                        // reset the view before we draw each star.
                                GL.Translate(0.0f, 0.0f, zoom);          // zoom into the screen.
                                GL.Rotate(tilt, 1.0f, 0.0f, 0.0f);       // tilt the view.

                                GL.Rotate(star[loop].angle, 0.0f, 1.0f, 0.0f); // rotate to the current star's angle.
                                GL.Translate(star[loop].dist, 0.0f, 0.0f); // move forward on the X plane (the star's x plane).

                                GL.Rotate(-star[loop].angle, 0.0f, 1.0f, 0.0f); // cancel the current star's angle.
                                GL.Rotate(-tilt, 1.0f, 0.0f, 0.0f);      // cancel the screen tilt.

                                if (twinkle)
                                {       // twinkling stars enabled ... draw an additional star.
                                        // assign a color using bytes
                                        uint idx = STAR_NUM - loop - 1;
                                        GL.Color4(star[idx].r, 
                                                  star[idx].g, 
                                                  star[idx].b, 
                                                  (Byte)255);
                                           
                                        GL.Begin(BeginMode.Quads);                   // begin drawing the textured quad.
                                        GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 0.0f);
                                        GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 0.0f);
                                        GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 0.0f);
                                        GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, 0.0f);
                                        GL.End();                             // done drawing the textured quad.
                                }

                                // main star
                                GL.Rotate(spin, 0.0f, 0.0f, 1.0f);       // rotate the star on the z axis.

                                // Assign A Color Using Bytes
                                GL.Color4(star[loop].r, star[loop].g, star[loop].b, (Byte)255);
                                
                                GL.Begin(BeginMode.Quads);                      // Begin Drawing The Textured Quad
                                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 0.0f);
                                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 0.0f);
                                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 0.0f);
                                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, 0.0f);
                                GL.End();                                       // Done Drawing The Textured Quad

                                spin += 0.01f;                                          // used to spin the stars.
                                star[loop].angle += loop * 1.0f / STAR_NUM * 1.0f;    // change star angle.
                                star[loop].dist -= 0.01f;                               // bring back to center.

                                // star hit the center
                                if (star[loop].dist < 0.0f)
                                {             
                                        star[loop].dist += 5.0f;                        // move 5 units from the center.
                                        star[loop].r = (byte)rnd.Next(0, 255);          // new red color.
                                        star[loop].g = (byte)rnd.Next(0, 255);          // new green color.
                                        star[loop].b = (byte)rnd.Next(0, 255);          // new blue color.
                                }
                        }

                        // since this is double buffered, swap the buffers to display what just got drawn.
                        SwapBuffers();
                }                
                
                /// <summary>
                /// Called when it is time to setup the next frame. Add you game logic here.
                /// </summary>
                /// <param name="e">Contains timing information for framerate independent logic.</param>
                protected override void OnUpdateFrame (FrameEventArgs e)
                {
                        base.OnUpdateFrame (e);
                        
                        if(Keyboard[Key.PageUp]) {
                                zoom -= 0.2f;
                        }
                        
                        if(Keyboard[Key.PageDown]) {
                                zoom += 0.2f;
                        }
                        
                        if(Keyboard[Key.Up]) {
                                tilt += 0.5f;
                        }
                        
                        if(Keyboard[Key.Down]) {
                                tilt -= 0.5f;
                        }
                }                
        }
}


