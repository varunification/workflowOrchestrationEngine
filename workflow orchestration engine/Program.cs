using System;
using System.Collections.Generic;
using System.Xml;
namespace workflow_orchestration_engine
{
    class Program
    {
        static async Task Main(String[] args)
        {
            var workflowParser = new WorkflowParser();
            Stack<Activity> stack = new Stack<Activity>();    
            List<Activity> activities = workflowParser.ParseActivities(@"C:\Users\lette\Desktop\Frontend\Javascript hands-on\workflow.xml");
            foreach (var activity in activities)
            {
                bool flag =  await activity.Execute();
                if (flag)
                {
                    stack.Push(activity);
                }
                else
                {
                    while (stack.Any())
                    {
                        Activity rollback = stack.Pop();

                        rollback.RollBack();
                        
                    }
                    break;
                }
            }

        }
    }
}