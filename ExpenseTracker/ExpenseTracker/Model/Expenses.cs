using System;

namespace ExpenseTracker.Model
{ 
    public enum Category
    {
        Home,
        Shopping,
        Travel,
        Food,
        Entertainment,
        Education,
        Bills,
        Gift
    }

    public class Expenses : BaseEntity
    {
        private int expenseid;
        private string description;
        private decimal expenseamount;
        private DateTime expensedate;
        private Category expensecategory;


        public int ExpenseId
        {
            get { return this.expenseid; }
            set {
                if (value != this.expenseid)
                {
                    this.expenseid = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Description
        {
            get { return this.description; }
            set {
                if (value != this.description)
                {
                    this.description = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public decimal ExpenseAmount
        {
            get { return this.expenseamount; }
            set {
                if (value != this.expenseamount)
                {
                    this.expenseamount = value;
                    NotifyPropertyChanged();
                }
            }
        }
        
        public DateTime ExpenseDate
        {
            get { return this.expensedate; }
            set {
                if (value != this.expensedate)
                {
                    this.expensedate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Category ExpenseCategory
        {
            get { return this.expensecategory; }
            set {
                if (value != this.expensecategory)
                {
                    this.expensecategory = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Expenses()
        {

        }

       
        public Expenses(string name, decimal amount, DateTime date, Category category)
        {
            Description = name;
            ExpenseAmount = amount;
            ExpenseDate = date;
            ExpenseCategory = category;
        }
    }

}
