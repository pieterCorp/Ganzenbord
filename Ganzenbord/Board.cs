﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Ganzenbord
{
    public class Board
    {
        public ObservableCollection<Field> BoardList { get; set; }
        private readonly MainWindow _frontend;

        public Board(MainWindow frontend)
        {
            _frontend = frontend;
            BoardList = new ObservableCollection<Field>();
        }

        public void SetUpBoard()
        {
            CreateBoardList();
            SetSpiral();
            SetCorners();
            SetSpecials();
            FillBoardGrid();
        }

        private void CreateBoardList()
        {
            int counter = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Field field = new Field(counter, i, j);

                    BoardList.Add(field);

                    counter++;
                }
            }
        }

        private void SetSpiral()
        {
            int counter = 0;
            int a = 8;
            int b = 0;

            while (counter < 64)
            {
                for (int i = b; i < a; i++)
                {
                    Field currentField = BoardList.Where(x => x.X == i).Where(x => x.Y == b).FirstOrDefault();

                    currentField.Number = counter;

                    currentField.Background.Source = new BitmapImage(new Uri("/Images/vertical.jpg", UriKind.Relative));

                    counter++;
                }
                for (int i = b + 1; i < a - 1; i++)
                {
                    Field currentField = BoardList.Where(x => x.X == a - 1).Where(x => x.Y == i).FirstOrDefault();
                    currentField.Number = counter;
                    currentField.Background.Source = new BitmapImage(new Uri("/Images/horizontal.jpg", UriKind.Relative));

                    counter++;
                }
                for (int i = a - 1; i > b; i--)
                {
                    Field currentField = BoardList.Where(x => x.X == i).Where(x => x.Y == a - 1).FirstOrDefault();
                    currentField.Number = counter;
                    currentField.Background.Source = new BitmapImage(new Uri("/Images/vertical.jpg", UriKind.Relative));

                    counter++;
                }
                for (int i = a - 1; i > b; i--)
                {
                    Field currentField = BoardList.Where(x => x.X == b).Where(x => x.Y == i).FirstOrDefault();
                    currentField.Number = counter;
                    currentField.Background.Source = new BitmapImage(new Uri("/Images/horizontal.jpg", UriKind.Relative));

                    counter++;
                }
                a--;
                b++;
            }
        }

        private void SetCorners()
        {
            foreach (Field field in BoardList)
            {
                switch (field.Number)
                {
                    case 0:
                        field.Background.Source = new BitmapImage(new Uri("/Images/0.jpg", UriKind.Relative));
                        break;

                    case 63:
                        field.Background.Source = new BitmapImage(new Uri("/Images/63.jpg", UriKind.Relative));
                        break;

                    case 7:
                    case 33:
                    case 51:
                    case 61:
                        field.Background.Source = new BitmapImage(new Uri("/Images/leftunder.jpg", UriKind.Relative));
                        break;

                    case 14:
                    case 38:
                    case 54:
                    case 62:
                        field.Background.Source = new BitmapImage(new Uri("/Images/rightunder.jpg", UriKind.Relative));
                        break;

                    case 21:
                    case 43:
                    case 57:
                        field.Background.Source = new BitmapImage(new Uri("/Images/righttop.jpg", UriKind.Relative));
                        break;

                    case 27:
                    case 47:
                    case 59:
                        field.Background.Source = new BitmapImage(new Uri("/Images/lefttop.jpg", UriKind.Relative));
                        break;
                }
            }
        }

        private void SetSpecials()
        {
            foreach (Field field in BoardList)
            {
                switch (field.Number)
                {
                    case 5:
                    case 9:
                    case 14:
                    case 18:
                    case 23:
                    case 27:
                    case 32:
                    case 36:
                    case 41:
                    case 45:
                    case 50:
                    case 54:
                    case 59:
                        field.SpecialImage.Source = new BitmapImage(new Uri("/Images/goose.png", UriKind.Relative));
                        field.Special = SpecialFields.Goose;
                        break;
                }
            }
        }

        private void FillBoardGrid()
        {
            foreach (var field in BoardList)
            {
                field.FieldNumber.Content = field.Number;

                _frontend.BordGrid.Children.Add(field.Grid);

                Grid.SetRow(field.Grid, field.X);
                Grid.SetColumn(field.Grid, field.Y);
            }
        }

        public void UpdateField(Player player)
        {
            BoardList.Where(x => x.Number == player.OldBoardPosition).FirstOrDefault().GamePiece.Source = null;
            BoardList.Where(x => x.Number == player.NewBoardPosition).FirstOrDefault().GamePiece.Source = player.Pion;
        }
    }
}