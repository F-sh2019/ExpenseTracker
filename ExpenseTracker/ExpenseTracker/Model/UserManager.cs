using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.Json;

namespace ExpenseTracker.Model
{
    public static class UserManager
    {
        // This holds the currently logged in User object.
        private static User LoggedInUser;

        public static User Login(string username, string password)
        {
            LoggedInUser = null;
            var returnedstring = FileManager.ReadFileData(username);
            if (!string.IsNullOrEmpty(returnedstring))
            {

                User userobj = JsonSerializer.Deserialize<User>(returnedstring);
                if (userobj.Password == password)
                {
                    LoggedInUser = userobj;
                }
            }
            return LoggedInUser;
        }

        public static User GetLoggedInUser()
        {
            return LoggedInUser;
        }


        public static bool IsCurrentMonthBudgetSet()
        {
            if (LoggedInUser != null)
            {
                var budget = LoggedInUser.Budgets.Where(x => x.BudgetDate.Month == DateTime.Today.Month && x.BudgetDate.Year == DateTime.Today.Year).FirstOrDefault();
                if (budget != null && budget.BudgetGoalAmount > 0)
                {
                    return true;
                }                
            }
            return false;

        }

        public static User CreatenewUser(string username, string password)
        {
            LoggedInUser = null;

            var newUser = new User()
            {
                UserName = username,
                Password = password,
                Budgets = new List<Budget>()
            };

            var userJsonString = JsonSerializer.Serialize(newUser);

            // Save user data to File
            if (FileManager.SaveDataToFile(username, userJsonString))
            {
                LoggedInUser = newUser;
            }

            return LoggedInUser;
        }

        public static void SaveLoggedInUserData()
        {

            if (LoggedInUser != null)
            {
                var serializedJsonstring = JsonSerializer.Serialize<User>(LoggedInUser);
                FileManager.SaveDataToFile(LoggedInUser.UserName, serializedJsonstring);
            }
        }

        public static Budget GetBudgetForBudgetDate(DateTime budgetDate)
        {
            if(LoggedInUser!=null)
            {
                var budget = LoggedInUser.Budgets.Where(x=> x.BudgetDate.Month == budgetDate.Month && x.BudgetDate.Year == budgetDate.Year).FirstOrDefault();
                if(budget!=null )
                {
                   return budget;
                }
            }
            return null;
        }
    }
}