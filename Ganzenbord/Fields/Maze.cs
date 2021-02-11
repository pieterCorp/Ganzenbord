﻿using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace Ganzenbord
{
    class Maze : Field
    {
        public Image SpecialImage { get; set; }

        public Maze(int number, int x, int y)
            : base(number, x, y)
        {
            SpecialImage = new Image();
            SpecialImage.Source = new BitmapImage(new Uri($"/Images/maze.png", UriKind.Relative));
            Grid.Children.Insert(1, SpecialImage);
        }
        public override int ReturnMove(Player player)
        {
            return 0;
        }

        public override void UpdateBoardPosition(Player player)
        {
            throw new System.NotImplementedException();
        }

    }
}