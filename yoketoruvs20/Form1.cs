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


        const int SpeedMax = 10;
        int[] vx = new int[ChrMax];
        int[] vy = new int[ChrMax];
       


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
                chrs[i].Font = tempLabel.Font;
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
            chrs[PlayerIndex].Left= mp.X - chrs[PlayerIndex].Width / 2;
            chrs[PlayerIndex].Top = mp.Y - chrs[PlayerIndex].Height / 2;


            for (int ei = 1; ei < ChrMax; ei++)
            {               

                chrs[ei].Left += vx[ei];
                chrs[ei].Top += vy[ei];

                if (chrs[ei].Left < 0)
                {
                    vx[ei] = Math.Abs(vx[ei]);
                }
                if (chrs[ei].Top < 0)
                {
                    vy[ei] = Math.Abs(vy[ei]);
                }
                if (chrs[ei].Right > ClientSize.Width)
                {
                    vx[ei] = -Math.Abs(vx[ei]);
                }
                if (chrs[ei].Bottom > ClientSize.Height)
                {
                    vy[ei] = -Math.Abs(vy[ei]);
                }

                if ((mp.X >= chrs[ei].Left)
                    && (mp.X < chrs[ei].Right)
                    && (mp.Y >= chrs[ei].Top)
                    && (mp.Y < chrs[ei].Bottom))
                {
                    //MessageBox.Show("重なった！！");

                    //敵か？
                    if (ei < ItemIndex)
                    {
                        nextState = State.GameOver;
                    }
                    else
                    {
                        //アイテム
                        chrs[ei].Visible = false;

                        //for (int itemCount = 10; itemCount >=0; itemCount--)
                        int itemCount = 10;
                        itemCount = itemCount - 1;
                        
                            itemLabel.Text = "★:" + itemCount;

                            if (itemCount <= 0)
                            {
                                nextState = State.Clear;
                            }
                        
                        
                    }
                }
            }

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
                        vx[i] = rand.Next(-SpeedMax, SpeedMax + 1);
                        vy[i] = rand.Next(-SpeedMax, SpeedMax + 1);
                        itemLabel.Text = "★:10";
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
