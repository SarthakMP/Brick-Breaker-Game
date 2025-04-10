using Raylib_cs;
using System.Numerics;
using Physics_LIb;
using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;

class Program
{
    //static Rectangle bottom_line, right_line, left_line, top_line;
    static List<Rectangle> bricks;


    public static bool paddle_coll(Rectangle rect,Rectangle r_circle)
    {

        if(Raylib.CheckCollisionRecs(rect,r_circle))
        {
            return true;
        }

        return false;
    }

    private static void Draw_vector(MVector v1,MVector v2)
    {
        Raylib.DrawLine((int)(v1.X), (int)v1.Y, (int)v2.X, (int)v2.Y, Color.Red);
    }

    static int nScreenWidth = 800;
    static int nScreenHeight = 400;
    
    private static void intialize()
    {
        

        Raylib.InitWindow(nScreenWidth, nScreenHeight, "Brick Breaker");
        Raylib.SetTargetFPS(60);

       Program.bricks =new List<Rectangle>();            //create a list of rectangles
                                                                   //Create a grid of bricks
        for (int i = 0; i < nScreenWidth; i += 30)
        {
            for (int j = 0; j <= nScreenHeight / 2 - 30; j += 30)
            {
                Rectangle brick = new Rectangle(i, j, 29, 29);
                bricks.Add(brick);

            }
        }

        

    }


    public static void Main()
    {

        int p_h = 25;
        int p_w = 100;
        int offeset = 50;
        int p_speed =10;
        int c_radius = 10;

        int threshold = 200;
        bool toggle = false;
        int b_hspeed = 5;
        int b_vspeed = -5;

        double inc_angle = 0;

        int v_offset=0;
        int h_offset =0;

        int c_col = 0;
        //coordinates of the center
        //for rec
        int r_y = nScreenHeight - 25;
        int r_x = nScreenWidth / 2;

        //for circle
        int c_x = nScreenWidth / 2;
        int c_y = nScreenHeight / 2;

        

        MGravity mGravity = new MGravity(c_x,c_y);
        MVector vector = new MVector(-10, 5);
        MVector Zero = new MVector(c_x, c_y);
        MVector velocity = new MVector(b_hspeed, b_vspeed);

        float gravity = 9.81f;
        int g_mul = 10;
        Color color = new Color(31, 31, 31, 255);
        intialize();
        while (!Raylib.WindowShouldClose())                                                 //updates till the windows closes
        {
    
            //starts drawing
            Raylib.BeginDrawing();
            
            
            Raylib.ClearBackground(new Color(31, 31, 31, 255));                                            //set background to black
            

            
            Rectangle rect = new Rectangle(Math.Abs(nScreenWidth/2 - offeset), nScreenHeight-25,p_w,p_h);                   //creates a rectangle
            Rectangle top_line = new Rectangle(0, 0, nScreenWidth, 0);                   //top col rect                                                        
            Rectangle left_line = new Rectangle(0, 0, 0, nScreenHeight);               //left col rect
            Rectangle right_line = new Rectangle(nScreenWidth, 0, 0, nScreenHeight); //right col rect
            Rectangle bottom_line = new Rectangle(0, nScreenHeight, nScreenWidth, 0); //bottom col rect



            //applies velocity to the circle
            (vector, velocity) = mGravity.ApplyGravity(vector, velocity, new MVector(0, -gravity*g_mul)); //applies gravity to the circle

            
            int x_offset = (int)vector.X;   //sets the x offset of the circle
            int y_offset = (int)vector.Y;   //sets the y offset of the circle

            Raylib.DrawCircle(c_x + x_offset, c_y - y_offset, c_radius, Color.White);                  //draws the circle

            Raylib.DrawRectangleGradientEx(rect, Color.White, color, color, Color.White);   //sets the color of rectangle
            Raylib.DrawRectangleLinesEx(top_line,1f,Color.Red); //draws the rectangle


            if (Raylib.CheckCollisionCircleRec(new Vector2(c_x + x_offset, c_y - y_offset), c_radius, rect))
            {
                if (Math.Abs(velocity.Y) > threshold)
                {
                    
                    velocity = new MVector(velocity.X, threshold);
                }else
                velocity = new MVector(velocity.X+5, -velocity.Y*2);
                
                (vector, velocity) = mGravity.ApplyGravity(vector, velocity, new MVector(0, -gravity * g_mul)); //applies gravity to the circle

            } else if (Raylib.CheckCollisionCircleRec(new Vector2(c_x + x_offset, c_y - y_offset), c_radius, top_line))
            {
                
                if (Math.Abs(velocity.Y) > threshold)
                {
                    velocity = new MVector(velocity.X, threshold);
                }
                else
                    velocity = new MVector(velocity.X + 5, -velocity.Y);

                (vector, velocity) = mGravity.ApplyGravity(vector, velocity, new MVector(0, -gravity * g_mul));
            
            }else if (Raylib.CheckCollisionCircleRec(new Vector2(c_x + x_offset, c_y - y_offset), c_radius, right_line) ||
                Raylib.CheckCollisionCircleRec(new Vector2(c_x + x_offset, c_y - y_offset), c_radius, left_line)){
                
                
                if (Math.Abs(velocity.X) > threshold)
                {
                    velocity = new MVector(threshold, velocity.Y);
                }
                else
                    velocity = new MVector(-velocity.X, velocity.Y);

                (vector, velocity) = mGravity.ApplyGravity(vector, velocity, new MVector(0, -gravity * g_mul));
             }
            else if(Raylib.CheckCollisionCircleRec(new Vector2(c_x + x_offset, c_y - y_offset), c_radius, bottom_line))
            {
                Console.WriteLine("Game over");
            }

            if (Raylib.IsKeyDown(KeyboardKey.A))                        //check if key down is A checks if the key is down not juts pressed
            {
                offeset += p_speed;                                     //moves the rectangle to the left
                //color = new Color(255, 0, 0, 255);                      //for debug set color to red
            }
            else if (Raylib.IsKeyDown(KeyboardKey.D))                  //check if key down is D
            {
                offeset -= p_speed;                                      //moves the rectangle to the right
                //color = new Color(0, 255, 0, 255);                      //for debug set color to green
            }
            else
            {
                color = new Color(31, 31, 31, 255);                      //for debug set color to grey
            }
            

                                                                         
            //for checking the paddle doesnt go out of the screen        
            if(offeset > nScreenWidth / 2)
            {
                offeset = nScreenWidth / 2;
            }else if(offeset <= -nScreenWidth/2 + p_w)
            {
                offeset = -nScreenWidth / 2 + p_w;
            }

            int ran_x = Raylib.GetRandomValue(-10, 10);
            int ran_y = Raylib.GetRandomValue(-10, 10);

            //collision between the cirlce and the bricks
            foreach (Rectangle b in bricks)
                {
                    Raylib.DrawRectangleRec(b, Color.White); //draws the bricks
                    if (Raylib.CheckCollisionCircleRec(new Vector2(c_x + x_offset, c_y - y_offset), c_radius, b))
                    {

                        if (Math.Abs(velocity.Y) > threshold)
                        {
                            velocity = new MVector(velocity.Y, threshold);
                        }
                        else
                            velocity = new MVector(velocity.X  + ran_x, -velocity.Y + ran_y);

                        (vector, velocity) = mGravity.ApplyGravity(vector, velocity, new MVector(0, -gravity * g_mul));

                        bricks.Remove(b);
                        //velocity = new MVector(velocity.X+50, -velocity.Y);
                        break;
                    }
                }



            //ends drawing
            Raylib.EndDrawing();

    }

        Raylib.CloseWindow();


       


    }
}