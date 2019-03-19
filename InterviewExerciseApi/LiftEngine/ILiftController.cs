using System;
namespace InterviewExerciseApi.LiftEngine
{
    public interface ILiftController
    {
        bool Call(int floor);
        object GetStatus();
    }
}
