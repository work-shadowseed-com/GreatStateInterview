using Stateless;

namespace InterviewExerciseApi.LiftEngine
{
    public interface ILift
    {
        string Status { get; }
        int CurrentFloor();
        bool AreDoorsOpen();
        void MoveTo(int floor);
    }
}
