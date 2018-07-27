using System;

namespace StateMachineFun
{
    public interface IJobWorkflowService
    {
        Guid Create(string jobType, string productId = null);
        void Start(Guid jobId);
        void Complete(Guid jobId);
        void Fail(Guid jobId, string errorMessage);
    }

    public class JobWorkflowService : IJobWorkflowService
    {
        private readonly IJobWorkflowRepository _jobWorkflowRepository;

        public JobWorkflowService(IJobWorkflowRepository jobWorkflowRepository)
        {
            _jobWorkflowRepository = jobWorkflowRepository;
        }

        public Guid Create(string jobType, string productId = null)
        {
            return _jobWorkflowRepository.Create(new JobWorkflow(Guid.NewGuid(), jobType)
            {
                ProductId = productId
            });
        }

        public void Start(Guid jobWorkflowId)
        {
            Update(jobWorkflowId, x => x.Start());
        }

        public void Complete(Guid jobWorkflowId)
        {
            Update(jobWorkflowId, x => x.Complete());
        }

        public void Fail(Guid jobWorkflowId, string errorMessage)
        {
            Update(jobWorkflowId, x => x.Fail(errorMessage));
        }

        private void Update(Guid jobWorkflowId, Action<JobWorkflow> update)
        {
            var jobWorkflow = _jobWorkflowRepository.Get(jobWorkflowId);
            if (jobWorkflow == null)
            {
                return;
            }

            update(jobWorkflow);
            Console.WriteLine(jobWorkflow.Message);
            _jobWorkflowRepository.Update(jobWorkflow);
        }
    }
}
