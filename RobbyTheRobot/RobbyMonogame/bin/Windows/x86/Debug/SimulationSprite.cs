using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RobbyGeneticAlgo;


namespace RobbyMonogame
{
    public class SimulationSprite: DrawableGameComponent
    {
        private double millisecondsPerFrame = 250;//Update every half second
        private double timeSinceLastUpdate = 0; //Accumulate the elapsed time

        private SpriteFont spriteFont;

        private string[] fileNames;

        private int currentFileIndex = 0;
        private string fitness;
        private int moves = 0;
        private string generationNumber;

        private int score;


        private Chromosome chromosome;
        private Contents[,] grid;
        private int x;
        private int y;

        private SpriteBatch spriteBatch;
        private Texture2D canImage;
        private Texture2D tileImage;
        private Texture2D robbyImage;
        private Game1 game;


        public SimulationSprite(Game1 game)
            :base(game)
        {
            this.game = game;
        }
        public override void Initialize()
        {
            fileNames = new string[6];
            fileNames[0] = "../../../../../RobbyGeneticAlgo/bin/Debug/Gen_1.txt";
            fileNames[1] = "../../../../../RobbyGeneticAlgo/bin/Debug/Gen_20.txt";
            fileNames[2] = "../../../../../RobbyGeneticAlgo/bin/Debug/Gen_100.txt";
            fileNames[3] = "../../../../../RobbyGeneticAlgo/bin/Debug/Gen_200.txt";
            fileNames[4] = "../../../../../RobbyGeneticAlgo/bin/Debug/Gen_500.txt";
            fileNames[5] = "../../../../../RobbyGeneticAlgo/bin/Debug/Gen_1000.txt";

           
            string[] lines = File.ReadAllLines(fileNames[5]);
            
            
            generationNumber = lines[0];
            fitness = lines[1];
            string[] alleles = lines[2].Split(',');
            Allele[] alleleArr = new Allele[alleles.Length];
            for (int i = 0; i < alleleArr.Length; i++)
            {
                alleleArr[i] = (Allele)Enum.Parse(typeof(Allele), alleles[i]);
            }
            chromosome = new Chromosome(alleleArr);
       
    




            grid = Helpers.GenerateRandomTestGrid(10);
            x = Helpers.rand.Next(0, grid.GetLength(0));
            y = Helpers.rand.Next(0, grid.GetLength(1));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            robbyImage = game.Content.Load<Texture2D>("robby");
            canImage = game.Content.Load<Texture2D>("can");
            tileImage = game.Content.Load<Texture2D>("tile");
            this.spriteFont = this.game.Content.Load<SpriteFont>("scoreFont");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //code to slow down update tick taken from stackoverflow: https://stackoverflow.com/questions/4418209/decrease-run-speed-in-xna
            timeSinceLastUpdate += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeSinceLastUpdate >= millisecondsPerFrame)
            {
                timeSinceLastUpdate = 0;
                if (moves < 200)
                {
                    score += Helpers.ScoreForAllele(chromosome, grid, ref x, ref y);
                }
                moves++;

            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            for(int i = 0;i < grid.GetLength(0);i++)
            {
                for(int j = 0; j < grid.GetLength(1); j++)
                {
                    if(i == x && j == y)
                    {
                        spriteBatch.Draw(robbyImage, new Rectangle(i * 32, j * 32, 32, 32), Color.White);
                        continue;
                    }
                    else if(grid[i,j] == Contents.Can)
                    {
                        spriteBatch.Draw(canImage, new Rectangle(i * 32, j * 32, 32, 32), Color.White);
                    }
                    else if(grid[i, j] == Contents.Empty)
                    {
                        spriteBatch.Draw(tileImage, new Rectangle(i * 32, j * 32, 32, 32), Color.White);
                    }  
                }
            }
            string scoreText = "Score : " + score;
            string movesText = "Moves : " + moves;
            string generationText = "Generation : " + generationNumber;
            string fitnessText = "Average Fitness : " + fitness;

            this.spriteBatch.DrawString(this.spriteFont, generationText, new Vector2(350, 20), Color.Black);
            this.spriteBatch.DrawString(this.spriteFont, fitnessText, new Vector2(350, 40), Color.Black);
            this.spriteBatch.DrawString(this.spriteFont, scoreText, new Vector2(350,60), Color.Black);
            this.spriteBatch.DrawString(this.spriteFont, movesText, new Vector2(350, 80), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }


    }

}
