using System;
using System.Collections.Generic;
using System.Text;

namespace NeHeLessons
{
        class Program
        {
                static void Main (string[] args)
                {
                        bool exit = false;
                        int op = 0;
                        Console.Title = "Nehe Lessons";                        

                        do {
                                do {
                                        WriteMenu ();
                                        int.TryParse (Console.ReadLine(), out op);
                                } while (op == 0);
                                
                                try {
                                        switch (op) {
                                        case 1 :
                                                exit = true;
                                                break;
                                        case 2:
                                                // The 'using' idiom guarantees proper resource cleanup.
                                                // We request 30 UpdateFrame evnts per second, and unlimited
                                                // RenderFrame events (as fast as the computer can handle).
                                                using (Lesson02 lesson02 = new Lesson02 ()) {
                                                        lesson02.Run (30.0);
                                                }
                                                break;
                                        
                                        case 3:
                                                using (Lesson03 lesson03 = new Lesson03 ()) {
                                                        lesson03.Run (30.0);
                                                }
                                                break;
                                        
                                        case 4:
                                                using (Lesson04 lesson04 = new Lesson04 ()) {
                                                        lesson04.Run (30.0);
                                                }
                                                break;
                                        
                                        case 5:
                                                using (Lesson05 lesson05 = new Lesson05 ()) {
                                                        lesson05.Run (30.0);
                                                }
                                                break;
                                        
                                        case 6:
                                                using (Lesson06 lesson06 = new Lesson06 ()) {
                                                        lesson06.Run (30.0);
                                                }
                                                break;
                                        
                                        case 7:
                                                using (Lesson07 lesson07 = new Lesson07 ()) {
                                                        lesson07.Run (30.0);
                                                }
                                                break;
                                        
                                        case 8:
                                                using (Lesson08 lesson08 = new Lesson08 ()) {
                                                        lesson08.Run (30.0);
                                                }
                                                break;
                                        
                                        case 9:
                                                using (Lesson09 lesson09 = new Lesson09 ()) {
                                                        lesson09.Run (30.0);
                                                }
                                                break;
        
                                        case 10:
                                                using (Lesson10 lesson10 = new Lesson10 ()) {
                                                        lesson10.Run (30.0);
                                                }
                                                break;
                                                
                                        case 11:
                                                using (Lesson11 lesson11 = new Lesson11 ()) {
                                                        lesson11.Run (30.0);
                                                }
                                                break;
                                        
                                        case 12:
                                                using (Lesson12 lesson12 = new Lesson12 ()) {
                                                        lesson12.Run (30.0);
                                                }
                                                break;
                                                
                                        case 13:
                                                using (Arm arm = new Arm ()) {
                                                        arm.Run (30.0);
                                                }
                                                break;
                                                

                                        default:
                                                break;
                                        }
                                } catch (Exception ex) {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine(ex.Message);
                                }
                        } while (!exit);
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine ("Bye!!");
                }
                
                static void WriteMenu ()
                {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine ("Nehe Lessons");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine ("1 - Exit");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Out.Flush();
                        Console.WriteLine ("2 - Your First Polygon");
                        Console.WriteLine ("3 - Adding Color");
                        Console.WriteLine ("4 - Rotation");
                        Console.WriteLine ("5 - 3D Shapes");
                        Console.WriteLine ("6 - Texture Mapping");
                        Console.WriteLine ("7 - Texture Filters, Lighting & Keyboard Control");
                        Console.WriteLine ("8 - Blending");
                        Console.WriteLine ("9 - Moving Bitmaps In 3D Space");
                        Console.WriteLine ("10 - Loading And Moving Through A 3D World");
                        Console.WriteLine ("11 - Waving Texture");
                        Console.WriteLine ("12 - Display List");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write ("Chose one option and press enter: ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                }
        }
}
