using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Model
{
    public class User : BaseEntity
    {
        private string username;
        private string password;
        private List<Budget> budgets;

        public string UserName
        {
            get { return this.username; }
            set
            {
                if (value != this.username)
                {
                    this.username = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Password
        {
            get { return this.password; }
            set
            {
                if (value != this.password)
                {
                    this.password = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public List<Budget> Budgets
        {
            get { return this.budgets; }
            set
            {
                if (value != this.budgets)
                {
                    this.budgets = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
