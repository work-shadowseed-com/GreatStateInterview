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

        //CR: Comment is not really descriptive of method, nor is it in ///<summary> tags. 
        // Calls a lift to current
        public bool Call(int floor) //CR: "CallLift" might be more descriptive. Do we need checking around the floor value not being silly stuff like -1 or 99999 ?
        {
            // if lifts are all out of order   //CR: Stale code, dont leave commented out code in the solution or old comments
            //  return false;
            var liftToCall = _lifts.Where(lift => lift.Status != "OutOfService").OrderBy(lift => Math.Abs(lift.CurrentFloor() - floor)).First();
            /*CR: all about the above line: 
                Id break up the call statments over seperate lines (Where, orderby, first) to make it more readable
                Dont use magic strings, if use use strings like this, put the in consts somewhere ("OutOfService"). But dont use "OutOfService" anyway, status should really be an enum.
                The whole Math.Abs call is obscure. Seperate out this statement in a small function with a descriptive name
                Use of "First" instead of "FirstOrDefault" : This could cause an error if all lifts are out of service. Refactor with null check on result.
             */

            liftToCall.MoveTo(floor);
            return true; //CR: Returning a hardcoded true when no other result is avalible. With the refactor above, you should return false (and MoveTo not called!) if no lifts are in service (or MoveTo returns a result?)
        }
    }

    public class LiftDto
    {
        public int Floor;
        public string Status;
    }
}
