using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
//using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using Microsoft.Xrm.Tooling.Connector;

namespace Lab01
{
    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }

    class Program
    {
        private static string url;
        private static string userName;
        private static string password;

        static void Main(string[] args)
        {

            AppSettingsReader appReader = new AppSettingsReader();

            url = appReader.GetValue("url", typeof(string)).ToString();
            userName = appReader.GetValue("userName", typeof(string)).ToString();
            password = appReader.GetValue("password", typeof(string)).ToString();

            var conn = $@"
    Url = {url};
    AuthType = OAuth;
    UserName = {userName};
    Password = {password};
    AppId = 51f81489-12ee-4a9e-aaae-a2591f45987d;
    RedirectUri = app://58145B91-0C36-4500-8554-080854F2AC97;
    LoginPrompt=Auto;
    RequireNewInstance = True";

            using (var crmSvc = new CrmServiceClient(conn))
            {



                var newProject = new Entity("ava_project");
                newProject["ava_name"] = "My New Project";
                newProject["ava_duedate"] = DateTime.Now;

                var projectId = crmSvc.Create(newProject);

                Console.WriteLine("New project created with Id: " + projectId);

													
													
													
                var query = new QueryExpression("ava_project");

                query.ColumnSet = new ColumnSet("ava_name", "ava_duedate");

                query.Criteria.Conditions.Add(new ConditionExpression("ava_projectid", ConditionOperator.Equal,
                    projectId));

                var results = crmSvc.RetrieveMultiple(query);

                foreach (var result in results.Entities)

                {
                    Console.WriteLine("Retrieved Project: " + result["ava_name"]);
                    				
                    var newDueDate = DateTime.Parse(result["ava_duedate"].ToString()).AddDays(7);

                    result["ava_duedate"] = newDueDate;
                    crmSvc.Update(result);
                    Console.WriteLine("Project " + result["ava_name"] + " has been updated.");

																	  
														 
																	  
                    crmSvc.Delete("ava_project", projectId);
                    Console.WriteLine("Project with Id " + projectId + " has been deleted");

                }

                // -----------------------------------


								   
								   
								   
                var projectList = new List<Entity>();

                using (var sr = new StreamReader("sampleData.txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        var values = line.Split(',');

                        var project = new Entity("ava_project");
                        project["ava_name"] = values[0].Truncate(50);
                        project["ava_startdate"] = DateTime.Parse(values[1]);
                        project["ava_duedate"] = DateTime.Parse(values[2]);
                        project["ava_phase"] = new OptionSetValue(int.Parse(values[3]));
                        project["ava_contracttype"] = new OptionSetValue(int.Parse(values[4]));
                        project["ava_projecttype"] = new OptionSetValue(int.Parse(values[5]));

                        projectList.Add(project);
                    }
                }

                var request = new ExecuteMultipleRequest
                {
                    Settings = new ExecuteMultipleSettings
                    {
                        ContinueOnError = true,
                        ReturnResponses = true
                    },
                    Requests = new OrganizationRequestCollection()
                };

                foreach (var project in projectList)
                {
                    var create = new CreateRequest { Target = project };
                    request.Requests.Add(create);
                }

                var executeMultipleResponses = crmSvc.Execute(request);

																																

																	

								  
							
								  
                Console.WriteLine("Retrieving Projects using FetchXML...");

                //The fetch expression xml can be created in CRM using Advanced Find
													 
                FetchExpression fetchQuery = new FetchExpression("<fetch version=\"1.0\" output-format=\"xml-platform\" mapping=\"logical\" distinct=\"false\">" +
                                                                 "<entity name=\"ava_project\">" +
                                                                 "<attribute name=\"ava_projectid\"/>" +
                                                                 "<attribute name=\"ava_name\"/>" +
                                                                 "<attribute name=\"ava_duedate\"/>" +
                                                                 "<order attribute=\"ava_name\" descending=\"false\"/>" +
                                                                 "<filter type=\"and\">" +
                                                                 "<condition attribute=\"statecode\" operator=\"eq\" value=\"0\"/>" +
                                                                 "</filter>" +
                                                                 "</entity>" +
                                                                 "</fetch>");

                List<Entity> retrievedProjects2 = crmSvc.RetrieveMultiple(fetchQuery).Entities.ToList();

                foreach (Entity project in retrievedProjects2)
                {
                    Console.WriteLine("Project Id: " + project.Id);
                }


                Console.WriteLine("Retrieving Projects using Query Expression...");

                var queryExpression = new QueryExpression("ava_project")
                {
                    ColumnSet = new ColumnSet(true)
                };

                queryExpression.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);

                var retrievedProjects3 = crmSvc.RetrieveMultiple(queryExpression).Entities.ToList();

                foreach (var project in retrievedProjects3)
                {
                    Console.WriteLine("Project Id: " + project.Id);

                }


                foreach (var project in retrievedProjects3)
                {
                    Console.WriteLine("Project Id: " + project.Id);


                    var tempProject = new Entity("ava_project");
                    tempProject.Id = project.Id;
                    tempProject["ava_description"] = "New Description for " + project["ava_name"];

                    if (project.Contains("ava_duedate"))
                        tempProject["ava_duedate"] = DateTime.Parse(project["ava_duedate"].ToString()).AddDays(7);

                    crmSvc.Update(tempProject);
                }

                foreach (var project in retrievedProjects3)
                    crmSvc.Delete("ava_project", project.Id);


                Console.ReadKey();
            }
		 
	 

								 
	 
																					 
		 
																					 
        }
    }
}
