using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;

namespace yoketoruvs20
{
    public partial class Form1 : Form        
    {

        const bool isDebug = true;
        
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        const int PlayerMax = 1;
        const int EnemyMax = 10;
        const int ItemMax = 10;
        const int ChrMax = PlayerMax + EnemyMax + ItemMax;
        Label[] chrs = new Label[ChrMax];
        const int PlayerIndex = 0;
        const int EnemyIndex = PlayerIndex + PlayerMax;
        const int ItemIndex= EnemyIndex + EnemyMax;


        const string PlayerText = "( ｀ー´)ノ";
        const string EnemyText = "♦";
        const string ItemText = "★";

        static Random rand = new Random();
        

        enum State
        {
            None = -1,      //無効
            Title,          //タイトル
            Game,           //ゲーム
            GameOver,       //ゲームオーバー
            Clear           //クリア
        }
        State currentState = State.None;
        State nextState = State.Title;

        const int SpeedMax = 20;
        int[] vx = new int[ChrMax];
        int[] vy = new int[ChrMax];

        public Form1()
        {
            InitializeComponent();

            for(int i=0; i < ChrMax; i++)
            {
                chrs[i] = new Label();
                chrs[i].AutoSize = true;
                if(i==PlayerIndex)
                {
                    chrs[i].Text = PlayerText;
                }
                else if(i<ItemIndex)
                {
                    chrs[i].Text = EnemyText;
                }
                else
                {
                    chrs[i].Text = ItemText;
                }
                Controls.Add(chrs[i]);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (nextState != State.None)
            {
                initProc();
            }

            if (isDebug)
            {
                if (GetAsyncKeyState((int)Keys.O)<0)            //O=Game"O"ver
                {
                    nextState = State.GameOver;
                }
                else if (GetAsyncKeyState((int)Keys.C)<0)       //C="C"lear
                {
                    nextState = State.Clear;
                }
            }

            if(currentState==State.Game)
            {
                UpdateGame();
            }
        }

        void UpdateGame()
        {
            Point mp = PointToClient(MousePosition);
            //chrs[0].Text.Left = mp.X - chrs[0].Text.Width / 2;
            //chrs[0].Text.Top = mp.Y - chrs[0].Text.Height / 2;


        }

        void initProc()
        {
            currentState = nextState;
            nextState = State.None;

            switch(currentState)
            {
                case State.Title:
                    titleLabel.Visible = true;
                    startButton.Visible = true;
                    copyrightLabel.Visible = true;
                    hiLabel.Visible = true;
                    gameOverLabel.Visible = false;
                    titleButton.Visible = false;
                    clearLabel.Visible = false;
                    break;

                case State.Game:
                    titleLabel.Visible = false;
                    startButton.Visible = false;
                    copyrightLabel.Visible = false;
                    hiLabel.Visible = false;
                    

                    for(int i=EnemyIndex;i<ChrMax;i++)
                    {
                        chrs[i].Left = rand.Next(ClientSize.Width - chrs[i].Width);
                        chrs[i].Top = rand.Next(ClientSize.Height - chrs[i].Height);
                    }

                    break;

                case State.GameOver:
                    gameOverLabel.Visible = true;
                    titleButton.Visible = true;
                    break;

                case State.Clear:
                    clearLabel.Visible = true;
                    titleButton.Visible = true;
                    hiLabel.Visible = true;
                    break;
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            nextState = State.Game;
        }

        private void titleButton_Click(object sender, EventArgs e)
        {
            nextState = State.Title;
        }
    }
}
