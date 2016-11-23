using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.Consts
{
    public enum TaskStatus
    {
        Created,
        Assigned,
        InProgress,
        CurrentlyInProgress,
        AwaitingFeedback,
        Finished
    }
}