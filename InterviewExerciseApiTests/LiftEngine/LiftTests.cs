using InterviewExerciseApi.LiftEngine;
using Moq;
using NUnit.Framework;
using System.Runtime.InteropServices.ComTypes;

namespace InterviewExerciseApiTests.LiftEngine
{
    public class LiftTests
    {
        private static readonly string OutOfServiceStatusKey = "OutOfService";
        private Mock<ILift> _liftOne;
        private Mock<ILift> _liftTwo;
        private Mock<ILift> _liftThree;
        private int _moveToCallCount;

        [SetUp]
        public void Setup()
        {
            _liftOne = new Mock<ILift>();
            _liftTwo = new Mock<ILift>();
            _liftThree = new Mock<ILift>();
            _moveToCallCount = 0;
        }

        private void SetMockLiftState(Mock<ILift> liftMock, bool liftInService)
        {
            liftMock.Setup(lift => lift.Status)
                .Returns(liftInService ? string.Empty : OutOfServiceStatusKey);
        }

        private void SetMockLiftCurrentFloor(Mock<ILift> liftMock, int currentFloor)
        {
            liftMock.Setup(lift => lift.CurrentFloor()).Returns(currentFloor);
        }

        private void SetUpMockLiftCallCount(Mock<ILift> liftMock)
        {
            liftMock.Setup(life => life.MoveTo(It.IsAny<int>())).Callback(() =>
            {
                _moveToCallCount++;
            });
        }




        [TestCase(false, false, false)]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(true, true, false)]
        [TestCase(false, true, true)]
        public void GivenOneLiftInService_MoveToIsCalledOnce(bool liftOneInService, bool liftTwoInService, bool liftThreeInService)
        {
            SetMockLiftState(_liftOne, liftOneInService);
            SetMockLiftState(_liftTwo, liftTwoInService);
            SetMockLiftState(_liftThree, liftThreeInService);
            SetUpMockLiftCallCount(_liftOne);
            SetUpMockLiftCallCount(_liftTwo);
            SetUpMockLiftCallCount(_liftThree);
            var liftController = new LiftController(_liftOne.Object, _liftTwo.Object, _liftThree.Object);

            liftController.Call(1);

            Assert.That(_moveToCallCount, Is.EqualTo(1));
        }

        [TestCase(false, false, false)]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(true, true, false)]
        [TestCase(false, true, true)]
        public void GivenOneLiftInService_CallReturnsTrues(bool liftOneInService, bool liftTwoInService, bool liftThreeInService)
        {
            SetMockLiftState(_liftOne, liftOneInService);
            SetMockLiftState(_liftTwo, liftTwoInService);
            SetMockLiftState(_liftThree, liftThreeInService);
            var liftController = new LiftController(_liftOne.Object, _liftTwo.Object, _liftThree.Object);

            var result = liftController.Call(1);

            Assert.That(result, Is.True);
        }

        [Test]
        public void GivenAllLiftsAreOutOfInService_MoveToIsNotCalled()
        {
            SetMockLiftState(_liftOne, false);
            SetMockLiftState(_liftTwo, false);
            SetMockLiftState(_liftThree, false);
            var liftController = new LiftController(_liftOne.Object, _liftTwo.Object, _liftThree.Object);

            liftController.Call(1);

            _liftOne.Verify(lift => lift.MoveTo(It.IsAny<int>()), Times.Never);
            _liftTwo.Verify(lift => lift.MoveTo(It.IsAny<int>()), Times.Never);
            _liftThree.Verify(lift => lift.MoveTo(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void GivenAllLiftsAreOutOfInService_CallReturnsFalse()
        {
            SetMockLiftState(_liftOne, false);
            SetMockLiftState(_liftTwo, false);
            SetMockLiftState(_liftThree, false);
            var liftController = new LiftController(_liftOne.Object, _liftTwo.Object, _liftThree.Object);

            var result = liftController.Call(1);

            Assert.That(result, Is.False);
        }

        [Test]
        public void GivenOneLiftsIsOutOfInServiceAndClosest_MoveToIsCalledForIt()
        {
            SetMockLiftState(_liftOne, true);
            SetMockLiftState(_liftTwo, false);
            SetMockLiftState(_liftThree, false);
            SetMockLiftCurrentFloor(_liftOne, 100);
            var liftController = new LiftController(_liftOne.Object, _liftTwo.Object, _liftThree.Object);

            liftController.Call(99);

            _liftOne.Verify(lift => lift.MoveTo(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void GivenOneLiftsIsOutOfInServiceAndClosest_MoveToIsCalledForAnotherLift()
        {
            SetMockLiftState(_liftOne, true);
            SetMockLiftState(_liftTwo, false);
            SetMockLiftState(_liftThree, false);
            SetUpMockLiftCallCount(_liftOne);
            SetUpMockLiftCallCount(_liftTwo);
            SetUpMockLiftCallCount(_liftThree);
            SetMockLiftCurrentFloor(_liftOne, 100);
            var liftController = new LiftController(_liftOne.Object, _liftTwo.Object, _liftThree.Object);

            liftController.Call(99);

            Assert.That(_moveToCallCount, Is.EqualTo(1));
        }


        [Test]
        public void GivenLiftOneIsClosest_CorrectFloorIsCalled()
        {
            SetMockLiftState(_liftOne, false);
            SetMockLiftState(_liftTwo, false);
            SetMockLiftState(_liftThree, false);
            SetMockLiftCurrentFloor(_liftOne, 100);

            var liftController = new LiftController(_liftOne.Object, _liftTwo.Object, _liftThree.Object);

            liftController.Call(99);

            _liftOne.Verify(lift => lift.MoveTo(99), Times.Once);
        }

        [TestCase(10, 9, 11)]
        [TestCase(10, 11, 9)]
        [TestCase(10, 11, 9)]

        public void GivenLiftOneIsClosest_MoveToIsCalledForLiftOne(int liftOneFloor, int otherLiftsFloor, int callFloor)
        {
            SetMockLiftState(_liftOne, false);
            SetMockLiftState(_liftTwo, false);
            SetMockLiftState(_liftThree, false);
            SetMockLiftCurrentFloor(_liftOne, liftOneFloor);
            SetMockLiftCurrentFloor(_liftTwo, otherLiftsFloor);
            SetMockLiftCurrentFloor(_liftThree, otherLiftsFloor);

            var liftController = new LiftController(_liftOne.Object, _liftTwo.Object, _liftThree.Object);

            liftController.Call(callFloor);

            _liftOne.Verify(lift => lift.MoveTo(It.IsAny<int>()), Times.Once);
        }



    }
}