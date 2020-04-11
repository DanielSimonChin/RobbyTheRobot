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
    //THE FILES WRITTEN IN THE PRINT METHOD MUST BE WRITTEN IN ORDER FOR THE MONOGAME PORTION TO FUNCTION ie: Gen_1.txt,Gen_20.txt,Gen_100.txt,Gen_200.txt,Gen_500.txt,Gen_1000.txt
    //THE FILES ARE LOCATED AT teamg\RobbyTheRobot\RobbyGeneticAlgo\bin\Debug
    /// <summary>
    /// This class is the visual representation of Robby's progression as he runs on a grid
    /// </summary>
    public class SimulationSprite: DrawableGameComponent
    {
        //increase this number to slow down the process, lower to increase speed
        private double millisecondsPerFrame = 100;//Update every half second
        private double timeSinceLastUpdate = 0; //Accumulate the elapsed time

        private SpriteFont spriteFont;

        //The file names to retrive info from
        private string[] fileNames;
        //The file we are currently reading from
        private int currentFileIndex = 0;
        private string fitness;
        private int moves = 0;
        private string generationNumber;

        private int score;

        //a chromosome that takes the alleles from the file
        private Chromosome chromosome;
        //the grid from which the chromosome is run on
        private Contents[,] grid;
        //current position of robby
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
        /// <summary>
        /// Setup backend logic that is not visible on the screen
        /// </summary>
        public override void Initialize()
        {
            fileNames = new string[6];
            fileNames[0] = "../../../../../RobbyGeneticAlgo/bin/Debug/Gen_1.txt";
            fileNames[1] = "../../../../../RobbyGeneticAlgo/bin/Debug/Gen_20.txt";
            fileNames[2] = "../../../../../RobbyGeneticAlgo/bin/Debug/Gen_100.txt";
            fileNames[3] = "../../../../../RobbyGeneticAlgo/bin/Debug/Gen_200.txt";
            fileNames[4] = "../../../../../RobbyGeneticAlgo/bin/Debug/Gen_500.txt";
            fileNames[5] = "../../../../../RobbyGeneticAlgo/bin/Debug/Gen_1000.txt";

            setupFileInfo();

            base.Initialize();
        }
        /// <summary>
        /// Helper method that initializes the private fields
        /// </summary>
        public void setupFileInfo()
        {
            string[] lines = File.ReadAllLines(fileNames[currentFileIndex]);

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
        }

        /// <summary>
        /// Loading the images to display on screen
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            robbyImage = game.Content.Load<Texture2D>("robby");
            canImage = game.Content.Load<Texture2D>("can");
            tileImage = game.Content.Load<Texture2D>("tile");
            this.spriteFont = this.game.Content.Load<SpriteFont>("scoreFont");

            base.LoadContent();
        }

        /// <summary>
        /// Updates every event tick
        /// </summary>
        /// <param name="gameTime">current gameTime</param>
        public override void Update(GameTime gameTime)
        {
            //code to slow down update tick taken from stackoverflow: https://stackoverflow.com/questions/4418209/decrease-run-speed-in-xna
            timeSinceLastUpdate += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeSinceLastUpdate >= millisecondsPerFrame)
            {
                timeSinceLastUpdate = 0;
                if (moves < 200)
                {
                    //perform the move and change robby's location
                    score += Helpers.ScoreForAllele(chromosome, grid, ref x, ref y);
                    moves++;
                }
                else
                {
                    //if robby has done 200 moves, then move on to the next file
                    moves = 0;
                    score = 0;
                    currentFileIndex++;
                    //exit once all files have been visually displayed
                    if(currentFileIndex == 6)
                    {
                        game.Exit();
                    }
                    else
                    {
                        setupFileInfo();
                    }
                }
            }
            base.Update(gameTime);
        }
        /// <summary>
        /// Draw the grid and robby's location as well as the score, moves,generation,fitness
        /// </summary>
        /// <param name="gameTime">Game time</param>
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
            string fitnessText = "Generation's Best Fitness : " + fitness;

            this.spriteBatch.DrawString(this.spriteFont, generationText, new Vector2(350, 20), Color.Black);
            this.spriteBatch.DrawString(this.spriteFont, fitnessText, new Vector2(350, 40), Color.Black);
            this.spriteBatch.DrawString(this.spriteFont, scoreText, new Vector2(350,60), Color.Black);
            this.spriteBatch.DrawString(this.spriteFont, movesText, new Vector2(350, 80), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
