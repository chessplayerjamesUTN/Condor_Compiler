using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
public static Random rand = new Random(DateTime.Now.Year + DateTime.Now.Month * DateTime.Now.Day + DateTime.Now.Hour * (DateTime.Now.Minute - DateTime.Now.Second) + DateTime.Now.Millisecond);
public static Texture2D auto1,auto2,auto3,auto4;
public static int x,y,min,max,direccion;

static int sumarVelocidad(int coordenada,int direccion)
{
if(direccion==1||direccion==2)
{
return coordenada+rand.Next(1,35);
}
else
{
return coordenada-rand.Next(10,15);
}

}
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this); graphics.PreferredBackBufferHeight = 600; graphics.PreferredBackBufferWidth = 600;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
 rand = new Random(DateTime.Now.Year + DateTime.Now.Month * DateTime.Now.Day + DateTime.Now.Hour * (DateTime.Now.Minute - DateTime.Now.Second) + DateTime.Now.Millisecond);


try
{
auto1=Content.Load<Texture2D>("car1");
}
catch
{
Console.WriteLine("Archivo inexistente (error E/S).","505");
Console.WriteLine("auto1=Content.Load<Texture2D>(car1);");
}

try
{
auto2=Content.Load<Texture2D>("car2");
}
catch
{
Console.WriteLine("Archivo inexistente (error E/S).","505");
Console.WriteLine("auto2=Content.Load<Texture2D>(car2);");
}

try
{
auto3=Content.Load<Texture2D>("car3");
}
catch
{
Console.WriteLine("Archivo inexistente (error E/S).","505");
Console.WriteLine("auto3=Content.Load<Texture2D>(car3);");
}

try
{
auto4=Content.Load<Texture2D>("car4");
}
catch
{
Console.WriteLine("Archivo inexistente (error E/S).","505");
Console.WriteLine("auto4=Content.Load<Texture2D>(car4);");
}

x=25;
y=25;
min=25;
max=400;
direccion=1;

            const int framesPerSecond = 10; 
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / framesPerSecond);
        Texture2D tex = Content.Load<Texture2D>("test");
        base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
	spriteBatch.Begin();

if(direccion==1)
{
spriteBatch.Draw(auto1,new Vector2(Convert.ToSingle(x),Convert.ToSingle(y)),Color.White);
x=sumarVelocidad(x,direccion);
if(x>=max)
{
direccion=2;
}

}
else if(direccion==2)
{
spriteBatch.Draw(auto2,new Vector2(Convert.ToSingle(x),Convert.ToSingle(y)),Color.White);
y++;
y=sumarVelocidad(y,direccion);
if(y>=max)
{
direccion=3;
}

}
else if(direccion==3)
{
spriteBatch.Draw(auto3,new Vector2(Convert.ToSingle(x),Convert.ToSingle(y)),Color.White);
x--;
x=sumarVelocidad(x,direccion);
if(x<=min)
{
direccion=4;
}

}
else
{
spriteBatch.Draw(auto4,new Vector2(Convert.ToSingle(x),Convert.ToSingle(y)),Color.White);
y--;
y=sumarVelocidad(y,direccion);
if(y<=min)
{
direccion=1;
}

}


        spriteBatch.End();

            base.Draw(gameTime);
        }
    }
