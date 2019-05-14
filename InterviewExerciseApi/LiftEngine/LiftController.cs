using System;
using System.Collections.Generic;
using System.Linq;

namespace InterviewExerciseApi.LiftEngine
{
    // Responsible for coordinating the individual lifts.
    public class LiftController : ILiftController
    {
        private static ILift _lift1;
        private static ILift _lift2;
        private static ILift _lift3;

        private List<ILift> _lifts = new List<ILift>();

        public LiftController(ILift lift1, ILift lift2, ILift lift3)
        {
            _lift1 = lift1;
            _lift2 = lift2;
            _lift3 = lift3;

            _lifts.Add(_lift1);
            _lifts.Add(_lift2);
            _lifts.Add(_lift3);
        }

        // Returns some status information about each lift
        public object GetStatus()
        {
            var data = new List<LiftDto>();

            foreach (var lift in _lifts)
            {
                data.Add(new LiftDto {
                    Floor = lift.CurrentFloor(),
                    Status = lift.Status
                });
            }

            return data;
        }

        // Calls a lift to current
        public bool Call(int floor)
        {
            // if lifts are all out of order
            //  return false;
            var liftToCall = _lifts.Where(lift => lift.Status != "OutOfService").OrderBy(lift => Math.Abs(lift.CurrentFloor() - floor)).First();
            liftToCall.MoveTo(floor);
            return true;
        }
    }

    public class LiftDto
    {
        public int Floor;
        public string Status;
    }
}
