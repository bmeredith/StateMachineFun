using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachineFun
{
    public interface IJobWorkflowRepository
    {
        Guid Create(JobWorkflow jobWorkflow);
        JobWorkflow Get(Guid id);
        void Update(JobWorkflow jobWorkflow);
    }

    public class JobWorkflowRepository : IJobWorkflowRepository
    {
        private static readonly List<JobWorkflowRecordSet> Db = new List<JobWorkflowRecordSet>();

        public Guid Create(JobWorkflow jobWorkflow)
        {
            Db.Add(ToRecordSet(jobWorkflow));

            return jobWorkflow.Id;
        }

        public JobWorkflow Get(Guid id)
        {
            var recordSet = Db.FirstOrDefault(x => x.Id == id);
            if (recordSet == null)
            {
                return null;
            }

            return ToModel(recordSet);
        }

        public void Update(JobWorkflow jobWorkflow)
        {
            Db.RemoveAll(x => x.Id == jobWorkflow.Id);
            Db.Add(ToRecordSet(jobWorkflow));
        }

        private static JobWorkflow ToModel(JobWorkflowRecordSet recordSet)
        {
            return new JobWorkflow(recordSet.Id, recordSet.JobType, recordSet.State)
            {
                EndDate = recordSet.EndDate,
                Message = recordSet.Message,
                ProductId = recordSet.ProductId,
                StartDate = recordSet.StartDate,
                UserId = recordSet.UserId
            };
        }

        private static JobWorkflowRecordSet ToRecordSet(JobWorkflow jobWorkflow)
        {
            return new JobWorkflowRecordSet
            {
                EndDate = jobWorkflow.EndDate,
                Id = jobWorkflow.Id,
                JobType = jobWorkflow.JobType,
                Message = jobWorkflow.Message,
                ProductId = jobWorkflow.ProductId,
                StartDate = jobWorkflow.StartDate,
                State = jobWorkflow.State,
                UserId = jobWorkflow.UserId
            };
        }
    }

    public class JobWorkflowRecordSet
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Message { get; set; }
        public string ProductId { get; set; }
        public JobState.WorkflowState State { get; set; }
        public string JobType { get; set; }
        public Guid? UserId { get; set; }
    }
}
