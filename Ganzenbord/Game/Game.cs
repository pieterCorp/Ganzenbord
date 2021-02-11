﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Ganzenbord
{
    internal class Game
    {
        private readonly List<Player> PlayerList = new List<Player>();

        public Board _board;

        public int counter = 0;

        private Player PlayerPlaying { get; set; }

        public Game()
        {
            _board = new Board();

            MakeNewPlayer("Dries", null, new BitmapImage(new Uri("/Images/playerBlue.png", UriKind.Relative)));
            MakeNewPlayer("Kobe", null, new BitmapImage(new Uri("/Images/playerRed.png", UriKind.Relative)));
            MakeNewPlayer("Pieter", null, new BitmapImage(new Uri("/Images/playerYellow.png", UriKind.Relative)));
        }

        private void MakeNewPlayer(string name, Image avatar, BitmapImage pion)
        {
            Player player = new Player(name, avatar, pion);
            PlayerList.Add(player);
        }

        public void TestRun()
        {
            if (PlayerPlaying == null)
            {
                PlayerPlaying = PlayerList[0];
            }
            else
            {
                //PlayerPlaying = PlayerList.Where(x => x.)
            }

            for (int i = 0; i < PlayerList.Count; i++)
            {
                int rolled1 = PlayerList[i].RollDice();

                PlayerList[i].Move(rolled1);
                _board.UpdateField(PlayerList[i]);

                //for (int j = 0; j < rolled1; j++)
                //{
                //    PlayerList[i].OldBoardPosition = PlayerList[i].NewBoardPosition;
                //    PlayerList[i].NewBoardPosition++;

                //    _board.UpdateField(PlayerList[i]);
                //    Thread.Sleep(100);
                //}
            }
        }
    }
}