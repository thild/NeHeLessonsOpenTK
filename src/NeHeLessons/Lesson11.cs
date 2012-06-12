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
using System.Drawing.Imaging;
using PFormat = System.Drawing.Imaging.PixelFormat;


namespace NeHeLessons
{

        class Lesson11 : GameWindow
        {
                
                 
                float xrot, yrot, zrot;         // floats for x rotation, y rotation, z rotation
                
                float[][][] points;             // the array for the points on the grid of our "wave"
                
                int wiggle_count = 0;
                
                int[] texture = new int[1];     // Storage for 1 texture.
                
                
                public Lesson11 () : base(640, 480, GraphicsMode.Default, "Waving Texture")
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
                /// Called when it is time to render the next frame. Add your rendering code here.
                /// </summary>
                /// <param name="e">Contains timing information.</param>
                protected override void OnRenderFrame (FrameEventArgs e)
                {
                        base.OnRenderFrame (e);
                        int x, y;
                        float float_x, float_y, float_xb, float_yb;
                        
                        // Clear The Screen And The Depth Buffer
                        GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                        
                        GL.LoadIdentity();                           // Reset The View
                        GL.Translate(0.0f,0.0f,-12.0f);              // move 12 units into the screen.
                        
                        GL.BindTexture (TextureTarget.Texture2D,        // Select A Texture
                                        texture[0]);
                        
                        GL.Begin(BeginMode.Quads);
                        
                        for (x=0; x<44; x++) {
                                for (y=0; y<44; y++) {
                                    float_x  = (float) (x)/44;
                                    float_y  = (float) (y)/44;
                                    float_xb = (float) (x+1)/44;
                                    float_yb = (float) (y+1)/44;
                                
                                    GL.TexCoord2( float_x, float_y);
                                    GL.Vertex3( points[x][y][0], points[x][y][1], points[x][y][2] );
                                                
                                    GL.TexCoord2( float_x, float_yb );
                                    GL.Vertex3( points[x][y+1][0], points[x][y+1][1], points[x][y+1][2] );
                                                
                                    GL.TexCoord2( float_xb, float_yb );
                                    GL.Vertex3( points[x+1][y+1][0], points[x+1][y+1][1], points[x+1][y+1][2] );
                                                
                                    GL.TexCoord2( float_xb, float_y );
                                    GL.Vertex3( points[x+1][y][0], points[x+1][y][1], points[x+1][y][2] );
                                }
                        }
                        GL.End();
                        
                        if (wiggle_count == 2) { // cycle the sine values
                                for (y = 0; y < 45; y++) {
                                    points[44][y][2] = points[0][y][2];
                                }
                                
                                for( x = 0; x < 44; x++ ) {
                                    for( y = 0; y < 45; y++) {
                                        points[x][y][2] = points[x+1][y][2];
                                    }           
                                }               
                                wiggle_count = 0;
                        }    
                        
                        wiggle_count++;
                        
                        xrot +=0.3f;
                        yrot +=0.2f;
                        zrot +=0.4f;
                        
                        // since this is double buffered, swap the buffers to display what just got drawn.
                        SwapBuffers();
                }

                /// <summary>Load resources here.
                /// A general OpenGL initialization function.  Sets all of the initial parameters.
                /// </summary>
                /// <param name="e">Not used.</param>
                protected override void OnLoad (EventArgs e)
                {
                        base.OnLoad (e);
                        LoadGLTextures ();
                        GL.Enable(EnableCap.Texture2D);                                 // Enable Texture Mapping
                        GL.ShadeModel(ShadingModel.Smooth);                             // Enable Smooth Shading
                        GL.ClearColor(0.0f, 0.0f, 0.0f, 0.5f);                          // Black Background
                        GL.ClearDepth(1.0);                                            // Depth Buffer Setup
                        GL.Enable(EnableCap.DepthTest);                                 // Enables Depth Testing
                        GL.DepthFunc(DepthFunction.Lequal);                             // The Type Of Depth Testing To Do
                        GL.Hint (HintTarget.PerspectiveCorrectionHint,                  // Really Nice Perspective Calculations
                                 HintMode.Nicest);

                        GL.PolygonMode(MaterialFace.Back, PolygonMode.Fill);            // Back Face Is Solid
                        GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);           // Front Face Is Made Of Lines
                        
                        this.points = new float[45][][];
                        for (int i=0; i < this.points.Length; i++)
                        {
                                this.points[i] = new float[45][];
                                for (int j=0; j < this.points[i].Length; j++)
                                {
                                        this.points[i][j] = new float[3];
                                        this.points[i][j][0] = (float)((i / 5.0f) - 4.5f);
                                        this.points[i][j][1] = (float)((j / 5.0f) - 4.5f);
                                        this.points[i][j][2] = (float)(Math.Sin((((i / 5.0f) * 40.0f) / 360.0f) * Math.PI * 2.0f));
                                }
                        }
                }
                
                void LoadGLTextures ()
                {
                        string file = "data/lesson11/Tim.bmp";
                        if (!System.IO.File.Exists (file)) {
                                throw new System.IO.FileNotFoundException ();
                        }
                        
                        GL.GenTextures (1, texture);                            // Create Texture       
                        Bitmap image = new Bitmap (file);                       // Load Texture
                        image.RotateFlip (RotateFlipType.RotateNoneFlipY);      // Flip The Bitmap Along The Y-Axis
                        Rectangle rectangle = new Rectangle (0, 0,              // Rectangle For Locking The Bitmap In Memory
                                                             image.Width, 
                                                             image.Height);
                        
                        BitmapData bitmapData = image.LockBits (rectangle,      // Get The Bitmap's Pixel Data From The Locked Bitmap
                                                                ImageLockMode.ReadOnly, 
                                                                PFormat.Format24bppRgb);
                        
                        // Create Linear Filtered Texture
                        GL.BindTexture (TextureTarget.Texture2D, texture[0]);
                        GL.TexParameter (TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 
                                         (int)TextureMinFilter.Linear);
                        GL.TexParameter (TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 
                                         (int)TextureMinFilter.Linear);
                        GL.TexImage2D (TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, 
                                       image.Width, image.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, 
                                       PixelType.UnsignedByte, bitmapData.Scan0);

                        
                        // Unlock The Pixel Data From Memory
                        image.UnlockBits (bitmapData);
                        // Dispose The Bitmap
                        image.Dispose ();
                }
                
                
                
        }
}






















