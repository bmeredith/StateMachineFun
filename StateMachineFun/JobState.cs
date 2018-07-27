using System;
using Stateless;

namespace StateMachineFun
{
    public class JobState
    {
        public enum WorkflowState { NotStarted, InProgress, Completed, Failed }
        public enum WorkflowTrigger { Start, Complete, Fail }

        private readonly StateMachine<WorkflowState, WorkflowTrigger> _stateMachine;
        private readonly StateMachine<WorkflowState, WorkflowTrigger>.TriggerWithParameters<string> _failTrigger;

        public WorkflowState State => _stateMachine.State;

        public JobState(WorkflowState workflowState = default(WorkflowState))
        {
            _stateMachine = new StateMachine<WorkflowState, WorkflowTrigger>(workflowState);
            _failTrigger = _stateMachine.SetTriggerParameters<string>(WorkflowTrigger.Fail);

            _stateMachine.Configure(WorkflowState.NotStarted)
                .Permit(WorkflowTrigger.Start, WorkflowState.InProgress);

            _stateMachine.Configure(WorkflowState.InProgress)
                .Permit(WorkflowTrigger.Fail, WorkflowState.Failed)
                .Permit(WorkflowTrigger.Complete, WorkflowState.Completed);

            _stateMachine.OnUnhandledTrigger((state, trigger) =>
            {
                Console.WriteLine($"Unhandled: '{state}' state, '{trigger}' trigger!");
            });
        }

        public void ChangeTo(WorkflowTrigger trigger)
        {
            _stateMachine.Fire(trigger);
        }

        public void ChangeToFailed(string errorMessage)
        {
            _stateMachine.Fire(_failTrigger, errorMessage);
        }
    }
}
