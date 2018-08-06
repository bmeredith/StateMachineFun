using System;

namespace StateMachineFun
{
    public class JobWorkflow
    {
        private readonly JobState _jobState;

        public Guid Id { get; }
        public DateTime StartDate { get; internal set; }
        public DateTime? EndDate { get; internal set; }
        public string JobType { get; internal set; }
        public string Message { get; internal set; }
        public string ProductId { get; internal set; }
        public JobState.WorkflowState State => _jobState.State;
        public Guid? UserId { get; set; }

        public JobWorkflow(Guid id, string jobType, JobState.WorkflowState state = default(JobState.WorkflowState))
        {
            Id = id;
            JobType = jobType;
            _jobState = new JobState(state);
        }

        public void Start()
        {
            _jobState.ChangeTo(JobState.WorkflowTrigger.Start);
            StartDate = DateTime.UtcNow;
            Message = $"{JobType} started";
        }

        public void Fail(string errorMessage)
        {
            _jobState.ChangeToFailed(errorMessage);
            EndDate = DateTime.UtcNow;
            Message = errorMessage;
        }

        public void Complete()
        {
            _jobState.ChangeTo(JobState.WorkflowTrigger.Complete);
            EndDate = DateTime.UtcNow;
            Message = $"{JobType} completed successfully";
        }
    }
}
