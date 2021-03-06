﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
        private readonly DispatcherTimer makeMoveDelay;
        private readonly DispatcherTimer makeSpecialMoveDelay;
        private Player cP;

        private int currentPlayer = 0;
        private bool specialIsHit = true;

        public IList<Player> PlayerList { get; set; }

        public Game(Grid boardGrid)
        {
            boardData = BoardData.GetBoardData();
            _playerFactory = new PlayerFactory();
            PlayerList = _playerFactory.GetPlayerList();
            _dice = new Dice();
            Board = new Board(boardGrid);

            makeMoveDelay = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 500)
            };
            makeMoveDelay.Tick += MakeMove;

            makeSpecialMoveDelay = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 2, 500)
            };
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
            string DiceRolled = $"{cP.Dice1}    |    {cP.Dice2}";

            boardData.PlaySidebar.UpdateDisplay(DiceRolled, BindedProp.DICEROLLED);

            if (cP.SkipTurn > 0 || cP == Well.PlayerInWell)
            {
                boardData.PlaySidebar.UpdateDisplay($"{cP.Name} has to skip this turn. Roll the dice to start!", BindedProp.FIELDMESSAGE);

                int NextPlayer = currentPlayer == PlayerList.Count - 1 ? 0 : currentPlayer + 1;
                boardData.PlaySidebar.UpdateDisplay(PlayerList[NextPlayer].Name, BindedProp.CURRENTTURN);

                cP.SkipTurn--;

                _playerFactory.SetNextPlayerFirst();
                MainWindow.EnableDiceButton();
            }
            else
            {
                boardData.PlaySidebar.UpdateDisplay($"{cP.Name} Rolled \"{cP.Dice1 + cP.Dice2}\" and moves to position {cP.CurrentBoardPosition + cP.Dice1 + cP.Dice2}.", BindedProp.FIELDMESSAGE);

                makeMoveDelay.Start();
            }

            currentPlayer = currentPlayer == PlayerList.Count - 1 ? 0 : currentPlayer + 1;
            return false;
        }

        public void MakeMove(object sender, EventArgs e)
        {
            int newFieldPos = cP.CurrentBoardPosition + cP.Dice1 + cP.Dice2;
            cP.Move(newFieldPos);
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
                MainWindow.EnableDiceButton();
            }

            if (!specialIsHit)
            {
                _playerFactory.SetNextPlayerFirst();
                boardData.PlaySidebar.UpdateDisplay(PlayerList[currentPlayer].Name, BindedProp.CURRENTTURN);
            }
            if (desiredPosition == 63)
            {
                boardData.PlaySidebar.UpdateDisplay($"{cP.Name} won", BindedProp.CURRENTTURN);
                MainWindow.SetGameOver();
                makeSpecialMoveDelay.Stop();
                specialIsHit = false;
            }
        }
    }
}