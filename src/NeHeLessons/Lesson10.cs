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
using System.IO;
using System.Drawing.Imaging;
using PFormat = System.Drawing.Imaging.PixelFormat;

namespace NeHeLessons
{
        public class Lesson10 : GameWindow
        {

                int[] texture = new int[3]; //Storage for 3 textures.
                
                int loop;  // general loop variable
                
                
                bool light = true;      // lighting on/off
                bool blend = true;      // blending on/off
                float yrot;             // y rotation 
                float walkbias = 0;
                float walkbiasangle = 0;

                float lookupdown = 0.0f;
                const float piover180 = 0.0174532925f;

                float xpos, zpos;

                float z = -5.0f;                // depth into the screen.
                
                // white ambient light at half intensity (rgba)
                float[] LightAmbient = { 0.5f, 0.5f, 0.5f, 1.0f };

                // super bright, full intensity diffuse light.
                float[] LightDiffuse = { 1.0f, 1.0f, 1.0f, 1.0f };

                // position of light (x, y, z, (position of light))
                float[] LightPosition = { 0.0f, 0.0f, 2.0f, 1.0f };

                uint filter;                // Which Filter To Use (nearest/linear/mipmapped)
                
                struct Vertex  // vertex coordinates - 3d and texture
                {
                       
                        public float x, y, z;                   // 3d coords.
                        public float u, v;                      // texture coords.
                }

                struct Triangle
                {                        // vertex coordinates - 3d and texture
                        public Vertex[] vertex;                        // 3 vertices array
                }

                struct Sector
                {
                        public Triangle[] triangles;
                        // pointer to array of triangles.
                        public Sector (int numtriangles)
                        {
                                triangles = new Triangle[numtriangles];
                                for (int i = 0; i < numtriangles; i++) {
                                        triangles[i] = new Triangle ();
                                }
                        }
                }

                Sector sector1;

                // degrees to radians...2 PI radians = 360 degrees
                float Rad (float angle)
                {
                        return angle * piover180;
                }

                public Lesson10 () : base(640, 480, GraphicsMode.Default, "Loading And Moving Through A 3D World")
                {
                        VSync = VSyncMode.On;
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine ("Press:");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine ("L to enable light");
                        Console.WriteLine ("F to change filter");
                        Console.WriteLine ("B to enable blending");
                        Console.WriteLine ("Page Up/Down to look Up/Down");
                        Console.WriteLine ("Arrows to walk");
                        Console.WriteLine ("F1 to full screen");
                        Console.WriteLine ("Esc to exit");                        
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
                                case Key.L:
                                        light = !light;
                                        // switch the current value of light, between 0 and 1.
                                        Console.WriteLine ("Light is now: {0}", light);
                                        if (!light) {
                                                GL.Disable (EnableCap.Lighting);
                                        } else {
                                                GL.Enable (EnableCap.Lighting);
                                        }
                                        break;
                        
                                case Key.F :
                                        filter += 1;
                                        if (filter > 2) {
                                                filter = 0;
                                        }
                                        Console.WriteLine ("Filter is now: {0}", filter);
                                        break;
                                        
                                case Key.B :
                                        blend = !blend;
                                        // switch the current value of blend, between 0 and 1.
                                        Console.WriteLine ("Blend is now: {0}", blend);
                                        if (!blend) {
                                                GL.Disable (EnableCap.Blend);           // Turn Blending Off
                                                GL.Enable (EnableCap.DepthTest);        // Turn Depth Testing On
                                        } else {
                                                GL.Enable (EnableCap.Blend);            // Turn Blending On
                                                GL.Disable (EnableCap.DepthTest);       // Turn Depth Testing Off
                                        }
                                        break;
                                default:
                                        
                                        break;
                                }
                        };
                }
                
                
                // loads the world from a text file.
                void SetupWorld ()
                {
                        System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo ();
                        nfi.NumberDecimalSeparator = ".";
                        using (StreamReader sr = File.OpenText ("data/lesson10/world.txt")) {
                                string line = sr.ReadLine ();
                                sector1 = new Sector (int.Parse (line.Substring (line.IndexOf (" ") + 1)));
                                while (!sr.EndOfStream) {
                                        for (loop = 0; loop < sector1.triangles.Length; loop++) {
                                                line = sr.ReadLine ();
                                                while (line == "" || line.Contains ("//")) {
                                                        line = sr.ReadLine ();
                                                }
                                                sector1.triangles[loop].vertex = new Vertex[3];
                                                for (int vert = 0; vert < 3; vert++) {
                                                        
                                                        string[] c = line.Split (new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                                        sector1.triangles[loop].vertex[vert].x = float.Parse (c[0], nfi);
                                                        sector1.triangles[loop].vertex[vert].y = float.Parse (c[1], nfi);
                                                        sector1.triangles[loop].vertex[vert].z = float.Parse (c[2], nfi);
                                                        sector1.triangles[loop].vertex[vert].u = float.Parse (c[3], nfi);
                                                        sector1.triangles[loop].vertex[vert].v = float.Parse (c[4], nfi);
                                                        if (vert < 2) {
                                                                line = sr.ReadLine ();
                                                        }
                                                }
                                                
                                        }
                                }
                        }
                }


                // Load Bitmaps And Convert To Textures
                void LoadGLTextures ()
                {
                        string file = "data/lesson10/mud.bmp";
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
                        GL.TexImage2D (TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, 
                                       image.Width, image.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, 
                                       PixelType.UnsignedByte, bitmapData.Scan0);
                        GL.GenerateMipmap (GenerateMipmapTarget.Texture2D);
                        
                        // Unlock The Pixel Data From Memory
                        image.UnlockBits (bitmapData);
                        // Dispose The Bitmap
                        image.Dispose ();
                        
                }

              /* A general OpenGL initialization function.  Sets all of the initial parameters. */
                // We call this right after our OpenGL window is created.
                protected override void OnLoad (EventArgs e)
                {
                        base.OnLoad (e);
                        
                        LoadGLTextures ();                              // Load The Texture(s) 
                        GL.Enable (EnableCap.Texture2D);                // Enable Texture Mapping
                        GL.ShadeModel (ShadingModel.Smooth);            // Enable Smooth Shading
                        GL.ClearColor (0, 0, 0, 0.0f);                  // Black Background
                        GL.ClearDepth (1.0);                              // Depth Buffer Setup
                        GL.Enable (EnableCap.DepthTest);                // Enables Depth Testing
                        GL.DepthFunc (DepthFunction.Less);            // The Type Of Depth Testing To Do
                        GL.Hint (HintTarget.PerspectiveCorrectionHint,  // Really Nice Perspective Calculations
                                 HintMode.Nicest);
                        
                        /* setup blending */
                        GL.BlendFunc(BlendingFactorSrc.SrcAlpha, 
                                     BlendingFactorDest.One);           // Set The Blending Function For Translucency
                        GL.Enable (EnableCap.Blend);                    // Enables Depth Testing
                        
                        // set up light number 1.
                        GL.Light (LightName.Light1, LightParameter.Ambient, LightAmbient);      // add lighting. (ambient)
                        GL.Light (LightName.Light1, LightParameter.Diffuse, LightDiffuse);      // add lighting. (diffuse).
                        GL.Light (LightName.Light1, LightParameter.Position, LightPosition);    // set light position.
                        GL.Enable (EnableCap.Light1);                                           // turn light 1 on.
                        GL.Enable (EnableCap.Lighting);                         // Enable Lighting
                        
                        SetupWorld();
                }

                protected override void OnResize (EventArgs e)
                {
                          base.OnResize (e);
                        
                        GL.Viewport(ClientRectangle.X, ClientRectangle.Y, 
                                    ClientRectangle.Width, ClientRectangle.Height);
                        GL.MatrixMode(MatrixMode.Projection);
                        Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, 
                                                                                  Width / (float)Height, 0.1f, 100.0f);
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
                        GL.LoadIdentity ();
                        
                        // calculate translations and rotations.
                        float x_m, y_m, z_m, u_m, v_m;
                        float xtrans = -xpos;
                        float ztrans = -zpos;
                        float ytrans = -walkbias - 0.25f;
                        float sceneroty = 360.0f - yrot;
                        
                        int numtriangles;
                        
                        GL.Rotate (lookupdown, 1.0f, 0, 0);
                        GL.Rotate (sceneroty, 0, 1.0f, 0);
                        
                        GL.Translate (xtrans, ytrans, ztrans);
                        
                        GL.BindTexture (TextureTarget.Texture2D,        // Select A Texture Based On filter
                                        texture[filter]);
                        // pick the texture.
                        numtriangles = sector1.triangles.Length;
                        
                        for (loop = 0; loop < numtriangles; loop++) {
                                // loop through all the triangles
                                GL.Begin (BeginMode.Triangles);
                                GL.Normal3 (0.0f, 0.0f, 1.0f);
                                
                                x_m = sector1.triangles[loop].vertex[0].x;
                                y_m = sector1.triangles[loop].vertex[0].y;
                                z_m = sector1.triangles[loop].vertex[0].z;
                                u_m = sector1.triangles[loop].vertex[0].u;
                                v_m = sector1.triangles[loop].vertex[0].v;
                                GL.TexCoord2 (u_m, v_m);
                                GL.Vertex3 (x_m, y_m, z_m);
                                
                                x_m = sector1.triangles[loop].vertex[1].x;
                                y_m = sector1.triangles[loop].vertex[1].y;
                                z_m = sector1.triangles[loop].vertex[1].z;
                                u_m = sector1.triangles[loop].vertex[1].u;
                                v_m = sector1.triangles[loop].vertex[1].v;
                                GL.TexCoord2 (u_m, v_m);
                                GL.Vertex3 (x_m, y_m, z_m);
                                
                                x_m = sector1.triangles[loop].vertex[2].x;
                                y_m = sector1.triangles[loop].vertex[2].y;
                                z_m = sector1.triangles[loop].vertex[2].z;
                                u_m = sector1.triangles[loop].vertex[2].u;
                                v_m = sector1.triangles[loop].vertex[2].v;
                                GL.TexCoord2 (u_m, v_m);
                                GL.Vertex3 (x_m, y_m, z_m);
                                
                                GL.End ();
                        }
                        
                        // since this is double buffered, swap the buffers to display what just got drawn.
                        SwapBuffers ();
                }
                
                
                protected override void OnUpdateFrame (FrameEventArgs e)
                {
                        base.OnUpdateFrame (e);
                        HandleCharacterMovement ();
                }
                
                
                /* The function called whenever a key is pressed. */
                void HandleCharacterMovement () {
                        
                        if (Keyboard[Key.PageUp])
                        {
                                z -= 0.02f;
                                lookupdown -= 1.0f;
                        }
                        if (Keyboard[Key.PageDown])
                        {
                                z += 0.02f;
                                lookupdown += 1.0f;
                        }

                        if (Keyboard[Key.Up])// walk forward (bob head)
                        {
                                xpos -= (float)Math.Sin(yrot * piover180) * 0.05f;
                                zpos -= (float)Math.Cos(yrot * piover180) * 0.05f;
                                if (walkbiasangle >= 359.0f)
                                        walkbiasangle = 0.0f;
                                else
                                        walkbiasangle += 10;
                                walkbias = (float)Math.Sin(walkbiasangle * piover180) / 20.0f;
                        }
                        if (Keyboard[Key.Down])// walk back (bob head)
                        {
                                xpos += (float)Math.Sin(yrot * piover180) * 0.05f;
                                zpos += (float)Math.Cos(yrot * piover180) * 0.05f;
                                if (walkbiasangle <= 1.0f)
                                        walkbiasangle = 359.0f;
                                else
                                        walkbiasangle -= 10;
                                walkbias = (float)Math.Sin(walkbiasangle * piover180) / 20.0f;
                        }

                        if (Keyboard[Key.Left]) // look left
                        {
                                yrot += 1.5f;
                        }

                        if (Keyboard[Key.Right])
                        { // look right
                                yrot -= 1.5f;
                        }
                }
        }
}


