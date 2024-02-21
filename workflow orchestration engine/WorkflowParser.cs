using System;
using System.Collections.Generic;
using System.Xml;
using workflow_orchestration_engine;

public class WorkflowParser { 
    public List<Activity> ParseActivities(string xmlFilePath) { 
        var activities = new List<Activity>(); 
        XmlDocument doc = new XmlDocument(); 
        doc.Load(xmlFilePath); 
        XmlNodeList activityNodes = 
            doc.SelectNodes("//Activity"); 
        foreach (XmlNode node in activityNodes) { 
            var activity = new Activity { Name = node.Attributes["Name"]?.InnerText,
                Description = node.Attributes["Description"]?.InnerText, 
                Critical = bool.Parse(node.Attributes["Critical"]?.InnerText ?? "false"),
                Enabled = bool.Parse(node.Attributes["Enabled"]?.InnerText ?? "true"), 
                Rollback = bool.Parse(node.Attributes["Rollback"]?.InnerText ?? "true"), 
                SleepPeriodInMilliseconds = int.Parse(node.SelectSingleNode("Parameters/SleepPeriodInMilliseconds")?.InnerText ?? "0") }; 
            activities.Add(activity); 
        } 
        return activities; 
    } 
}

