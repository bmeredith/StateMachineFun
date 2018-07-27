using System;
using System.Threading;

namespace StateMachineFun
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var jobWorkflowService = new JobWorkflowService(new JobWorkflowRepository());
            var id = jobWorkflowService.Create("Populate Stuff Job");

            try
            {
                jobWorkflowService.Start(id);

                DoWork();

                jobWorkflowService.Complete(id);
            }
            catch (Exception ex)
            {
                jobWorkflowService.Fail(id, ex.Message);
            }

            Console.ReadLine();
        }

        private static void DoWork()
        {
            Console.WriteLine("Doing lots of work here. Please wait...");
            Thread.Sleep(3000);
        }
    }
}
