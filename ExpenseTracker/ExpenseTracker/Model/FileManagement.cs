using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;

namespace ExpenseTracker.Model
{
    class FileManagement
    {


        public Budget CurrentMonthBudget()
        {
            List<Expenses> CurrentMonthExpense = new List<Expenses>();
            List<Budget> Budgets = new List<Budget>();

            User CurrentListdata = UserManager.GetLoggedInUser();
            Budgets = CurrentListdata.Budgets;

            var Budget = CurrentListdata.Budgets.Find(n => n.BudgetDate.Month == Constants.CurretMonth.Month && n.BudgetDate.Year == Constants.CurretMonth.Year);
            return Budget;
        }


        public List<Expenses> ExpenseList_Bydate(string Month ,string Year)
        {
         

            List<Expenses> CurrentMonthExpense = new List<Expenses>();
            List<Budget> Budgets = new List<Budget>();

            User CurrentListdata = UserManager.GetLoggedInUser();
           
            Budgets = CurrentListdata.Budgets;
          
            var Budget = CurrentListdata.Budgets.Find(n => n.BudgetDate.Month.ToString() == Month && n.BudgetDate.Year.ToString() == Year);
            if (Budget != null)
            {
                CurrentMonthExpense = Budget.ListOfExpenses;
            }
                return (CurrentMonthExpense);
            
            
        }

        public decimal Calculate_MonthlyCost(string Month, string Year)
        {
            List<Expenses> CurrentMonthExpense = new List<Expenses>();
            CurrentMonthExpense = ExpenseList_Bydate(Month,Year);

            decimal  monthlyCost= 0.0m;

            foreach( var Expense in CurrentMonthExpense)
            {
                if (Expense != null)
                {
                    monthlyCost = monthlyCost + Expense.ExpenseAmount;
                }
               }
            return monthlyCost;
        }


        public decimal AmountToGoal(string Month, string Year)
        {
            List<Expenses> CurrentMonthExpense = new List<Expenses>();
            List<Budget> Budgets = new List<Budget>();

            User CurrentListdata = UserManager.GetLoggedInUser();
            Budgets = CurrentListdata.Budgets;
            var Budget = CurrentListdata.Budgets.Find(n => n.BudgetDate.Month.ToString() == Month && n.BudgetDate.Year.ToString() == Year);

            if (Budget != null)
            {
                decimal CurrentBudgetExpencess = Calculate_MonthlyCost(Month, Year);

                return (Budget.BudgetGoalAmount - CurrentBudgetExpencess);
            }
            return 0;
        }

    }
}
