﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Ganzenbord
{
    public class Game
    {
        public PlayerFactory _playerFactory;
        private readonly Dice _dice;
        private readonly BoardData boardData;
        public Board Board;
        private DispatcherTimer makeMoveDelay;
        private DispatcherTimer makeSpecialMoveDelay;
        private Player cP;

        private int currentPlayer = 0;
        private bool specialIsHit = true;

        public List<Player> PlayerList { get; set; }

        public Game(Grid boardGrid)
        {
            boardData = BoardData.GetBoardData();
            _playerFactory = new PlayerFactory();
            PlayerList = _playerFactory.GetPlayerList();
            _dice = new Dice();
            Board = new Board(boardGrid);

            makeMoveDelay = new DispatcherTimer();
            makeMoveDelay.Interval = new TimeSpan(0, 0, 2);
            makeMoveDelay.Tick += MakeMove;

            makeSpecialMoveDelay = new DispatcherTimer();
            makeSpecialMoveDelay.Interval = new TimeSpan(0, 0, 2);
            makeSpecialMoveDelay.Tick += GooseMove;
        }

        private void RollDice()
        {
            PlayerList[currentPlayer].Dice1 = _dice.Roll();
            PlayerList[currentPlayer].Dice2 = _dice.Roll();
        }

        public bool Run()
        {
            cP = PlayerList[currentPlayer];
            cP.IsReversed = false;

            RollDice();
            string DiceRolled = $"Dice:  {cP.Dice1}  |  {cP.Dice2}";

            boardData.PlaySidebar.UpdateDisplay(DiceRolled, BindedProp.DICEROLLED);

            if (cP.SkipTurn > 0 || cP == Well.PlayerInWell)
            {
                boardData.PlaySidebar.UpdateDisplay($"Sorry {cP.Name}, je moet een beurt overslaan!", BindedProp.FIELDMESSAGE);

                cP.SkipTurn--;

                currentPlayer = currentPlayer == PlayerList.Count - 1 ? 0 : currentPlayer + 1; // select next player in list
                boardData.PlaySidebar.ImagePath = PlayerList[currentPlayer].AvatarPath;

                boardData.PlaySidebar.UpdateDisplay(PlayerList[currentPlayer].Name, BindedProp.CURRENTTURN);
            }
            else
            {
                int newFieldPos = cP.CurrentBoardPosition + cP.Dice1 + cP.Dice2;
                cP.Move(newFieldPos);

                boardData.PlaySidebar.UpdateDisplay($"{cP.Name} heeft {cP.Dice1 + cP.Dice2} geworpen, en zet aan", BindedProp.FIELDMESSAGE);

                makeMoveDelay.Start();
            }

            if (cP.CurrentBoardPosition == 63)
            {
                boardData.PlaySidebar.UpdateDisplay($"{cP.Name} has won the game !!!", BindedProp.FIELDMESSAGE);
                return true;
            }

            return false;
        }

        public void MakeMove(object sender, EventArgs e)
        {
            Board.UpdateField(cP);

            makeMoveDelay.Stop();
            makeSpecialMoveDelay.Start();
        }

        public void GooseMove(object sender, EventArgs e)
        {
            Field currentField = Board.BoardList.FirstOrDefault(x => x.Number == cP.CurrentBoardPosition);
            int desiredPosition = currentField.ReturnMove(cP);

            specialIsHit = desiredPosition != cP.CurrentBoardPosition;
            boardData.PlaySidebar.UpdateDisplay(currentField.ToString(), BindedProp.FIELDMESSAGE);

            cP.Move(desiredPosition);
            Board.UpdateField(cP);

            if (specialIsHit)
            {
                makeSpecialMoveDelay.Stop();
                makeSpecialMoveDelay.Start();
            }
            else
            {
                makeSpecialMoveDelay.Stop();
            }
            currentPlayer = currentPlayer == PlayerList.Count - 1 ? 0 : currentPlayer + 1; // select next player in list
            boardData.PlaySidebar.ImagePath = PlayerList[currentPlayer].AvatarPath;

            boardData.PlaySidebar.UpdateDisplay(PlayerList[currentPlayer].Name, BindedProp.CURRENTTURN);
        }
    }
}