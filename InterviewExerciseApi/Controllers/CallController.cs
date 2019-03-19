using InterviewExerciseApi.LiftEngine;
using Microsoft.AspNetCore.Mvc;

namespace InterviewExerciseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallController : ControllerBase
    {
        // Responsible for controlling all the lifts
        ILiftController _liftController;

        public CallController(ILiftController liftController)
        {
            _liftController = liftController;
        }

        // Endpoint that provides the status of all the lifts.
        // GET: /api/call
        [HttpGet]
        public object Get()
        {
            // Get the lift statuses
            var status = _liftController.GetStatus();
            return status;
        }

        // Endpoint that summons a lift to the current floor.
        // POST: /api/call
        [HttpPost]
        public void Post([FromBody] int currentFloor)
        {
            _liftController.Call(currentFloor);
        }
    }
}
