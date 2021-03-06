using NUnit.Framework;
using Ganzenbord;
using System.Threading;

namespace Ganzenbord.UnitTest
{
    [Apartment(ApartmentState.STA)]
    public class WellTest
    {
        private Player _playerInWell;
        private Player _newPlayer;
        private Well _well;

        [SetUp]
        public void Setup()
        {
            _playerInWell = new Player("", "", PawnColor.BLUE);
            _newPlayer = new Player("", "", PawnColor.GREEN);
            _well = new Well(1, 1, 1);
        }

        [Test]
        public void ReturnMove_WhenCalled_ReturnsBoardPosition()
        {
            //arrange
            _playerInWell.CurrentBoardPosition = 31;

            //act
            int result = _well.ReturnMove(_playerInWell);

            //assert
            Assert.That(31 == result);
        }

        [Test]
        public void ReturnMove_WhenCalled_ChangesSkipTurnTo9999()
        {
            ////arrange
            //int x = _well.ReturnMove(_playerInWell);

            ////act
            //Player result = _well.PlayerInWell;

            ////assert
            //Assert.That(_playerInWell == result);
            Assert.That(false);
            //can't run test because PlayerInWell is a static property
        }

        [Test]
        public void ReturnMove_WhenCalled_ReplacesPlayerInWell()
        {
            ////arrange
            //int x = _well.ReturnMove(_newPlayer);

            ////act
            //Player result = _well.PlayerInWell;

            ////assert
            //Assert.That(_newPlayer == result);
            Assert.That(false);
            //can't run test because PlayerInWell is a static property
        }
    }
}