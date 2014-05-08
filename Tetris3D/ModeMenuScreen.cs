﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris3D
{
   public enum ModeMenuOptions { Classic, Marathon, TimeTrial, Challenges, Back, Ai };

   public class ModeMenuScreen : GameScreen
   {
      //the background used in most of the menu screens
      private Texture2D backGround;
      //the cursor used in most of the menu screens
      private Texture2D cursor;
      //the next Texture2Ds are visuals of the 5 user options presented in this menu
      private Texture2D menuOptionClassic;   //1
      private Texture2D menuOptionChallenges;//2
      private Texture2D menuOptionTimeTrial; //3
      private Texture2D menuOptionMarathon;  //4
      private Texture2D menuOptionAi;  //5
      private Texture2D menuOptionBack;      //6

      //the next 4 textboxes are textual descriptions of 4 of the 5 options (back excluded)
      private TextBox classicDescriptionText;   //1
      private TextBox marathonDescriptionText;  //2
      private TextBox timeTrialDescriptionText; //3
      private TextBox challengesDescriptionText;//4
      private TextBox AiDescriptionText;//5
      //the option that the user is currently highlighting
      private ModeMenuOptions highlightedOption = ModeMenuOptions.Classic;
      //the number of options the user has to choose from
      private int numberOfTetrisMainMenuOptions = 6;

      public ModeMenuScreen(Microsoft.Xna.Framework.Game game)
         : base(game)
      {
      }

      public override void LoadContent()
      {
         this.backGround = this.content.Load<Texture2D>(@"Textures\Title");
         this.cursor = this.content.Load<Texture2D>(@"Textures\Cursor");
         this.menuOptionClassic = this.content.Load<Texture2D>(@"Textures\Menus\Classic");
         this.menuOptionAi = this.content.Load<Texture2D>(@"Textures\Menus\Ai");
         this.menuOptionTimeTrial = this.content.Load<Texture2D>(@"Textures\Menus\TimeTrial");
         this.menuOptionMarathon = this.content.Load<Texture2D>(@"Textures\Menus\Marathon");
         this.menuOptionChallenges = this.content.Load<Texture2D>(@"Textures\Menus\Challenges");
         this.menuOptionBack = this.content.Load<Texture2D>(@"Textures\Menus\Back");

         //initializing the values of the 4 textboxes
         this.classicDescriptionText = new TextBox(this, new Vector2(75, 410f), new Vector2(450, 400), @"Textures\Kootenay",
             "Objective: Get the highest score possible without stacking over the top of the playing field", true);
         this.classicDescriptionText.TextAlign = TextBox.TextAlignOption.TopLeft;
         this.classicDescriptionText.ForeColor = Color.Turquoise;
         this.marathonDescriptionText = new TextBox(this, new Vector2(75, 410f), new Vector2(450, 400), @"Textures\Kootenay",
             "Objective: Keep getting score until you can't play any longer.  Pause and level cap are disabled.  Get comfortable because you may be here for a while", true);
         this.marathonDescriptionText.TextAlign = TextBox.TextAlignOption.TopLeft;
         this.marathonDescriptionText.ForeColor = Color.Thistle;
         this.timeTrialDescriptionText = new TextBox(this, new Vector2(75, 410f), new Vector2(450, 400), @"Textures\Kootenay",
             "Objective: Get the highest score possible with the 3 minutes provided.  Time will be added upon clearing a line", true);
         this.timeTrialDescriptionText.TextAlign = TextBox.TextAlignOption.TopLeft;
         this.timeTrialDescriptionText.ForeColor = Color.Turquoise;
         this.challengesDescriptionText = new TextBox(this, new Vector2(75, 410f), new Vector2(450, 400), @"Textures\Kootenay",
             "Objective: Clear all of the pre-placed blocks on the board to achieve victory", true);
         this.challengesDescriptionText.TextAlign = TextBox.TextAlignOption.TopLeft;
         this.challengesDescriptionText.ForeColor = Color.Thistle;

         this.AiDescriptionText = new TextBox(this, new Vector2(75, 410f), new Vector2(450, 400), @"Textures\Kootenay",
  "Objective: Ai Gameplay", true);
         this.AiDescriptionText.TextAlign = TextBox.TextAlignOption.TopLeft;
         this.AiDescriptionText.ForeColor = Color.Thistle;

         //playing an initial sound upon this screen initializing
         this.screenManager.audio.PlayMenuForwardSound();
      }

      public override void Update(GameTime gameTime)
      {
         //if UP was pressed
         if (this.screenManager.input.KeyboardState.WasKeyPressed(Keys.Up))
         {
            //decrement the highlighted option and play a sound
            this.screenManager.audio.PlayMenuScrollSound();
            this.highlightedOption--;
            this.highlightedOption = (ModeMenuOptions)Math.Max((int)this.highlightedOption, 0);
         }
         //if DOWN was pressed
         else if (this.screenManager.input.KeyboardState.WasKeyPressed(Keys.Down))
         {
            //increment the highlighted option and play a sound
            this.screenManager.audio.PlayMenuScrollSound();
            this.highlightedOption++;
            this.highlightedOption = (ModeMenuOptions)Math.Min((int)this.highlightedOption, this.numberOfTetrisMainMenuOptions - 1);
         }
         //if ENTER was pressed
         else if (this.screenManager.input.KeyboardState.WasKeyPressed(Keys.Enter))
         {
            //depending on which option is highlighted...
            switch (this.highlightedOption)
            {
               //remove this menu screen and add a screen based upon the selection.  Begin playing music if specified.
               //or go back
               case ModeMenuOptions.Classic: this.screenManager.removeScreen(this); this.screenManager.addScreen(new ClassicTetrisScreen(this.screenManager.Game)); break;
               case ModeMenuOptions.Marathon: this.screenManager.removeScreen(this); this.screenManager.addScreen(new MarathonTetrisScreen(this.screenManager.Game)); break;
               case ModeMenuOptions.TimeTrial: this.screenManager.removeScreen(this); this.screenManager.addScreen(new TimeTrialScreen(this.screenManager.Game)); break;
               case ModeMenuOptions.Challenges: this.screenManager.removeScreen(this); this.screenManager.addScreen(new ChallengesMenuScreen(this.screenManager.Game)); break;
               case ModeMenuOptions.Ai: this.screenManager.removeScreen(this); this.screenManager.addScreen(new Ai(this.screenManager.Game)); break;
               case ModeMenuOptions.Back: this.screenManager.removeScreen(this); this.screenManager.addScreen(new MainMenuScreen(this.screenManager.Game)); break;
               default: throw new NotImplementedException();
            }
         }
      }

      public override void Draw(GameTime gameTime)
      {
         //begin drawing
         this.screenManager.batch.Begin();
         //draw the background and the 5 visible options
         this.screenManager.batch.Draw(this.backGround, new Rectangle(0, 0, 1200, 900), Color.White);
         this.screenManager.batch.Draw(this.menuOptionClassic, new Vector2(550, 415), Color.White);
         this.screenManager.batch.Draw(this.menuOptionMarathon, new Vector2(550, 470), Color.White);
         this.screenManager.batch.Draw(this.menuOptionTimeTrial, new Vector2(550, 525), Color.White);
         this.screenManager.batch.Draw(this.menuOptionChallenges, new Vector2(550, 580), Color.White);
         this.screenManager.batch.Draw(this.menuOptionAi, new Vector2(550, 635), Color.White);
         this.screenManager.batch.Draw(this.menuOptionBack, new Vector2(550, 700), Color.White);

         //depending on which option is highlighted...
         switch (this.highlightedOption)
         {
            //draw a cursor indicating the highlighted option and draw a textual description of the highlighted option
            case ModeMenuOptions.Classic: this.screenManager.batch.Draw(this.cursor, new Vector2(515, 417), Color.White);
               this.classicDescriptionText.Draw(this.screenManager.batch); break;
            case ModeMenuOptions.Marathon: this.screenManager.batch.Draw(this.cursor, new Vector2(515, 472), Color.White);
               this.marathonDescriptionText.Draw(this.screenManager.batch); break;
            case ModeMenuOptions.TimeTrial: this.screenManager.batch.Draw(this.cursor, new Vector2(515, 527), Color.White);
               this.timeTrialDescriptionText.Draw(this.screenManager.batch); break;
            case ModeMenuOptions.Challenges: this.screenManager.batch.Draw(this.cursor, new Vector2(515, 582), Color.White);
               this.challengesDescriptionText.Draw(this.screenManager.batch); break;
            case ModeMenuOptions.Ai: this.screenManager.batch.Draw(this.cursor, new Vector2(515, 637), Color.White);
               this.AiDescriptionText.Draw(this.screenManager.batch); break;
            case ModeMenuOptions.Back: this.screenManager.batch.Draw(this.cursor, new Vector2(515, 670), Color.White); break;

            default: throw new NotImplementedException();
         }
         //end drawing
         this.screenManager.batch.End();
      }
   }
}
