﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ganzenbord
{
    internal class Field9 : Goose
    {
        private int returnValue { get; set; }
        private bool firstRound { get; set; } = true;

        public Field9(int number, int x, int y)
            : base(number, x, y)
        {
            returnValue = 0;
        }

        public override int ReturnMove(Player player)
        {
            if (!player.HasDied)
            {
                if (player.Dice1 == 5 || player.Dice1 == 4 && player.Dice2 == 5 || player.Dice2 == 4)
                {
                    returnValue = 26;
                    return 26;
                }
                else if (player.Dice1 == 6 || player.Dice1 == 3 && player.Dice2 == 6 || player.Dice2 == 3)
                {
                    returnValue = 53;
                    return 53;
                }
                returnValue = 0;
            }
            firstRound = false;
            return base.ReturnMove(player);
        }

        public override string ToString()
        {
            if (returnValue == 0)
            {
                return base.ToString();
            }
            if (firstRound)
            {
                return $"You arrived 9 in first move,  you move to {returnValue}.";
            }
            return "You died, revived and threw nine. You where born lucky!";
        }
    }
}