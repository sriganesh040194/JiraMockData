using System;
using JiraMockData.Model;
using JiraMockData.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Globalization;
using System.Threading.Tasks;

namespace JiraMockData
{
    public class BoardMetrics
    {
        readonly string[] boardNames = new string[] { "bur_board" };

        readonly string[] taskType = new string[] { "User Story", "Task", "Sub Task" };

        readonly string[] taskStatus = new string[] { "new", "inProgress", "done" };

        public const string boardIndexname = "dev.logevent.anypoint-jira-boardmetrics.";
        public const string boardUserIssueIndexName = "dev.logevent.anypoint-jira-board-user-issue-metrics.";
        public const string userIndexName = "dev.logevent.anypoint-jira-usermetrics.";

        readonly List<KeyValuePair<string, string>> assignees = new List<KeyValuePair<string, string>>() {
            new KeyValuePair<string, string>("777992529209lsndsnd", "sriganesh"),
            new KeyValuePair<string, string>("0987654illms", "rafi"),
            new KeyValuePair<string, string>("2342352345bgggg", "piyush")

        };

        List<KeyValuePair<BoardMetricsModel, List<BoardUserIssueModel>>> boardAndIssueMetrics = new List<KeyValuePair<BoardMetricsModel, List<BoardUserIssueModel>>>();

        int sprintcycle = 14;
        int noOfDays = 60;

        Random random;

        ElasticSearch elasticSearch;
        public BoardMetrics()
        {
            random = new Random();
            elasticSearch = new ElasticSearch();
        }

        public async Task GenerateData()
        {

            for (int day = 0; day <= noOfDays; day++)
            {
                foreach (var boardName in boardNames)
                {
                    var sprintNo = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(day / sprintcycle)));
                    var sprint = new Sprint()
                    {
                        id = sprintNo,
                        name = "Sprint " + sprintNo,
                        state = "active",
                        startDate = GetDate(sprintNo),
                        endDate = GetDate(sprintNo + sprintcycle),
                        goal = "Sprint " + sprintNo + "goals"
                    };
                   

                    var issueAssigneeStripped = new List<IssueAssignee>();
                    var issueAssigned = new List<BoardUserIssueModel>();
                    foreach (var assignee in assignees)
                    {
                        issueAssigneeStripped.Add(new IssueAssignee() { accountId = assignee.Key, displayName = assignee.Value });


                        //create a random issue
                        var issue = new List<IssueWithoutAssignee>();

                        for (var issueId = 1; issueId <= random.Next(1, 15); issueId++)
                        {
                            issue.Add(new IssueWithoutAssignee()
                            {

                                id = random.Next(),
                                summary = "Summary of " + issueId,
                                created = sprint.startDate,
                                updated = sprint.startDate,
                                customField = new CustomField() { storyPoints = random.Next(1, 16) },
                                status = new Status() { key = taskStatus[random.Next(0, taskStatus.Length)] },
                                issuetype = new Issuetype(taskType[random.Next(0, taskType.Length)]),

                            });
                        }


                        var taskCount = new Metrics(GetTaskCounts(issue, taskStatus[0]), GetTaskCounts(issue, taskStatus[1]), GetTaskCounts(issue, taskStatus[2]));

                        var storyPoints = new Metrics(GetStoryPointsSum(issue, taskStatus[0]), GetStoryPointsSum(issue, taskStatus[1]), GetStoryPointsSum(issue, taskStatus[2]));



                        issueAssigned.Add(new BoardUserIssueModel()
                        {
                            boardAndUserName = boardName + "_" + assignee.Value,
                            boardName = boardName,
                            displayName = assignee.Value,
                            emailAddress = "",
                            //processedDate = GetDate(day),
                            sprint = sprint,
                            project = null,
                            taskStatusCount = taskCount,
                            storyPoints = storyPoints,
                            issue = issue
                        });
                    }


                    var boardMetricsModel = new BoardMetricsModel()
                    {
                        id = 4,
                        name = boardName,
                        projectType = "scrum",
                        //processedDate = GetDate(day),
                        sprint = sprint,
                        issues = issueAssigneeStripped,
                        taskStatusCount = new MetricsStatusCount(GetBoardTaskCount(issueAssigned)),
                        storyPoints = new MetricsStatusCount(GetBoardStoryPoints(issueAssigned))
                    };

                    Console.WriteLine("Board User Metrics ######################################");
                    Console.WriteLine(JsonSerializer.Serialize(issueAssigned));
                    Console.WriteLine("Board Metrics ######################################");
                    Console.WriteLine(JsonSerializer.Serialize(boardMetricsModel));


                    boardAndIssueMetrics.Add(new KeyValuePair<BoardMetricsModel, List<BoardUserIssueModel>>(boardMetricsModel, issueAssigned));

                    //await elasticSearch.PostAsync(boardIndexname + GetYMDPatternDate(day), JsonSerializer.Serialize(boardMetricsModel));

                    //foreach(var issue in issueAssigned)
                    //{
                    //    await elasticSearch.PostAsync(boardUserIssueIndexName + GetYMDPatternDate(day), JsonSerializer.Serialize(issue));
                    //}
                    

                    //await elasticSearch.GetAsync();
                }
            }
        }

        private static int GetTaskCounts(List<IssueWithoutAssignee> issue, string key = "new")
        {
            return issue.Where(a => a.status.key == key).Count();
        }

        private static int GetStoryPointsSum(List<IssueWithoutAssignee> issue, string key = "new")
        {
            return issue.Where(a => a.status.key == key).Sum(a => a.customField.storyPoints);
        }

        private static Metrics GetBoardTaskCount(List<BoardUserIssueModel> boardIssue)
        {
            Metrics metrics = new Metrics();
            foreach (var issue in boardIssue)
            {
                metrics.toDo += issue.taskStatusCount.toDo;
                metrics.inProgress += issue.taskStatusCount.inProgress;
                metrics.done += issue.taskStatusCount.done;
            }
            return metrics;
        }

        private static Metrics GetBoardStoryPoints(List<BoardUserIssueModel> boardIssue)
        {
            Metrics metrics = new Metrics();
            foreach (var issue in boardIssue)
            {
                metrics.toDo += issue.storyPoints.toDo;
                metrics.inProgress += issue.storyPoints.inProgress;
                metrics.done += issue.storyPoints.done;
            }
            return metrics;
        }

        private static string GetDate(int addDays=0)
        {
            return String.Format("{0:O}", DateTime.Now.AddDays(addDays).ToUniversalTime());
        }

        public static string GetYMDPatternDate(int addDays=0)
        {
            return DateTime.Now.AddDays(addDays).ToUniversalTime().ToString("yyyy.MM.dd", CultureInfo.InvariantCulture);
        }

        public static string GetYMDPatternDate(string datetime)
        {
            return DateTime.Parse(datetime).ToString("yyyy.MM.dd", CultureInfo.InvariantCulture);
        }

    }
}
