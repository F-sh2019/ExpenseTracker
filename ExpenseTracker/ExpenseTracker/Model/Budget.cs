using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseTracker.Model
{
    public class Budget : BaseEntity
    {
        private decimal budgetgoalamount;
        private List<Expenses> listofexpenses;
        private DateTime budgetdate;

        public decimal BudgetGoalAmount
        {
            get { return this.budgetgoalamount; }
            set
            {
                if (value != this.budgetgoalamount)
                {
                    this.budgetgoalamount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public List<Expenses> ListOfExpenses
        {
            get { return this.listofexpenses; }
            set
            {
                if (value != this.listofexpenses)
                {
                    this.listofexpenses = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DateTime BudgetDate
        {
            get { return this.budgetdate; }
            set
            {
                if (value != this.budgetdate)
                {
                    this.budgetdate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Budget()
        {
            BudgetGoalAmount = 0;
            ListOfExpenses = new List<Expenses>();
            BudgetDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        public Budget(decimal setBudget)
        {
            BudgetGoalAmount = setBudget;
            ListOfExpenses = new List<Expenses>();
            BudgetDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        public List<Expenses> AddExpense(Expenses expense)
        {
            ListOfExpenses.Add(expense);

            return ListOfExpenses;
        }

        public List<Expenses> DeleteExpense(Expenses expense)
        {
            ListOfExpenses.Remove(expense);
            return null;
        }


        public static Budget getMatchingBudget(DateTime date, User currentUser)
        {
            bool isBudgetAvailable = false;
            Budget targetBudget = new Budget();

            foreach (Budget budget in currentUser.Budgets)
            {
                if (budget.BudgetDate.Month == date.Month && budget.BudgetDate.Year == date.Year)
                {
                    isBudgetAvailable = true;
                    targetBudget = budget;
                    break;
                }
            }

            if (isBudgetAvailable == false)
            {
                targetBudget.BudgetGoalAmount = 0;
                targetBudget.BudgetDate = date;
                currentUser.Budgets.Add(targetBudget);
            }

            return targetBudget;
        }
        public static Budget loadMatchingBudgetData(DateTime date, User currentUser)
        {
            bool isBudgetAvailable = false;
            Budget targetBudget = new Budget();

            foreach (Budget budget in currentUser.Budgets)
            {
                if (budget.BudgetDate.Month == date.Month && budget.BudgetDate.Year == date.Year)
                {
                    isBudgetAvailable = true;
                    targetBudget = budget;
                    break;
                }
            }

            if (isBudgetAvailable == false)
            {
                targetBudget.BudgetGoalAmount = 0;
                targetBudget.BudgetDate = date;
            }

            return targetBudget;
        }

        public int getNextId(List<Expenses> expenses)
        {
            int nextId;
            Expenses lastExpense = expenses.LastOrDefault();
            if (lastExpense == null)
            {
                nextId = 0;
            }
            else
            {
                nextId = lastExpense.ExpenseId + 1;
            }
            return nextId;
        }
    }
}