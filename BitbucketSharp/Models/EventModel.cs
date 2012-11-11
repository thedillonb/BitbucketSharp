using System.Collections.Generic;
using System;

namespace BitbucketSharp.Models
{
    public class EventsModel
    {
        public int Count { get; set; }
        public List<EventModel> Events { get; set; } 
    }

    public class EventModel
    {
        public string Node { get; set; }
        public string Description { get; set; }
        public RepositoryDetailedModel Repository { get; set; }
        public string CreatedOn { get; set; }
        public UserModel User { get; set; }
        public DateTime UtcCreatedOn { get; set; }
        public string Event { get; set; }

        public static class Type
        {
            public static readonly string 
                Commit = "commit", CreateRepo = "create",
                DeleteRepo = "delete", 
                WikiCreated = "wiki_created", WikiUpdated = "wiki_updated",
                StartFollowUser = "start_follow_user", StopFollowUser = "stop_follow_user",
                StartFollowRepo = "start_follow_repo", StopFollowRepo = "stop_follow_repo",
                IssueReported = "report_issue", IssueUpdated = "issue_update",
                IssueComment = "issue_comment";
        }
    }
}
