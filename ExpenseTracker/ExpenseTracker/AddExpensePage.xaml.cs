using ExpenseTracker.Model;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.Json;


namespace ExpenseTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddExpensePage : ContentPage
    {
        public AddExpensePage()
        {
            InitializeComponent();
        }

        private User currentUser = UserManager.GetLoggedInUser();
        private string name;
        private decimal amount;
        private DateTime date;
        private Category category;

        protected override void OnAppearing()
        {
            var expense = (Expenses)BindingContext;
            if (expense is object && !string.IsNullOrEmpty(expense.Description))
            {
                ExpenseLabel.Text = "Update Expense";
                ExpenseName.Text = expense.Description;
                ExpenseAmount.Text = expense.ExpenseAmount.ToString();
                ExpenseDate.Date = expense.ExpenseDate;
                ExpenseCategory.Text = $"Category: {expense.ExpenseCategory}";
                SetCategoryIcon(expense.ExpenseCategory);
                DeleteButton.IsVisible = true;
                UpdateButton.IsVisible = true;
                AddSaveButton.IsVisible = false;
            }
            else
            {
                SetCategoryIcon(Category.Home);
            }
        }

        private async void OnAddButtonClicked(object sender, EventArgs e)
        {
            if (validateNewExpenseData())
            {
                Budget matchingBudget = Budget.getMatchingBudget(date, currentUser);
                AddExpenseInBudget(matchingBudget);
                UserManager.SaveLoggedInUserData();
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Alert", "One or more required fields are empty. Please try again.", "OK");
            }
        }

        private async void OnUpdateButtonClicked(object sender, EventArgs e)
        {
            var expense = (Expenses)BindingContext;
            if (validateNewExpenseData())
            {  
                Budget matchingBudget = Budget.getMatchingBudget(date, currentUser);
                AddExpenseInBudget(matchingBudget);
                deleteExpense(expense.ExpenseDate, expense.ExpenseId);
                UserManager.SaveLoggedInUserData();
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Alert", "One or more required fields are empty. Please try again.", "OK");
            }
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var expense = (Expenses)BindingContext;
            deleteExpense(expense.ExpenseDate, expense.ExpenseId);
            UserManager.SaveLoggedInUserData();
            await Navigation.PopModalAsync();
        }

        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private bool validateNewExpenseData()
        {
            bool validateFields = true;

            if (string.IsNullOrWhiteSpace(ExpenseName.Text)
                || string.IsNullOrWhiteSpace(ExpenseAmount.Text)
                || string.IsNullOrWhiteSpace(category.ToString()))
            {
                validateFields = false;
            }
            else
            {
                name = ExpenseName.Text;
                amount = Convert.ToDecimal(ExpenseAmount.Text);
            }
            
            if (date == DateTime.MinValue)
            {
                date = DateTime.Now;
            }

            return validateFields;
        }
        
        private void AddExpenseInBudget(Budget budget)
        {
            Expenses newExpense = new Expenses(name, amount, date, category);
            newExpense.ExpenseId = budget.getNextId(budget.ListOfExpenses);
            budget.AddExpense(newExpense);
        }

        public void deleteExpense(DateTime budgetDate, int expId)
        {
            foreach (Budget budget in currentUser.Budgets)
            {
                if (budget.BudgetDate.Month == budgetDate.Month)
                {
                    budget.ListOfExpenses.RemoveAll(x => x.ExpenseId == expId);
                    break;
                }
            }
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            // Read the value that user has selected
            date = (DateTime)e.NewDate;
            if (date.Month != DateTime.Now.Month)
            {
                DisplayAlert("Warning!", "This item will not be visible under the current month's list", "OK");
            }
        }

        private void HomeIcon_Clicked(object sender, EventArgs e)
        {
            SetCategoryIcon(Category.Home);
        }

        private void ShoppingIcon_Clicked(object sender, EventArgs e)
        {
            SetCategoryIcon(Category.Shopping);
        }

        private void TravelIcon_Clicked(object sender, EventArgs e)
        {
            SetCategoryIcon(Category.Travel);
        }

        private void FoodIcon_Clicked(object sender, EventArgs e)
        {
            SetCategoryIcon(Category.Food);
        }

        private void EntertainmentIcon_Clicked(object sender, EventArgs e)
        {
            SetCategoryIcon(Category.Entertainment);
        }
                
        private void EducationIcon_Clicked(object sender, EventArgs e)
        {
            SetCategoryIcon(Category.Education);
        }

        private void BillsIcon_Clicked(object sender, EventArgs e)
        {
            SetCategoryIcon(Category.Bills);
        }

        private void GiftIcon_Clicked(object sender, EventArgs e)
        {
            SetCategoryIcon(Category.Gift);
        }

        private void SetCategoryIcon(Category selectedCategory)
        {
            category = selectedCategory;
            ExpenseCategory.Text = $"Category: {selectedCategory}";
            DisableAllIcons();
            switch (selectedCategory)
                {
                case Category.Home:
                    HomeIcon.BackgroundColor = Color.Aqua;
                    break;
                case Category.Shopping:
                    ShoppingIcon.BackgroundColor = Color.Aqua;
                    break;
                case Category.Travel:
                    TravelIcon.BackgroundColor = Color.Aqua;
                    break;
                case Category.Food:
                    FoodIcon.BackgroundColor = Color.Aqua;
                    break;
                case Category.Entertainment:
                    EntertainmentIcon.BackgroundColor = Color.Aqua;
                    break;
                case Category.Education:
                    EducationIcon.BackgroundColor = Color.Aqua;
                    break;
                case Category.Bills:
                    BillsIcon.BackgroundColor = Color.Aqua;
                    break;
                case Category.Gift:
                    GiftIcon.BackgroundColor = Color.Aqua;
                    break;
                }
        }
        
        private void DisableAllIcons()
        {
            HomeIcon.BackgroundColor = Color.LightGray;
            ShoppingIcon.BackgroundColor = Color.LightGray;
            TravelIcon.BackgroundColor = Color.LightGray;
            FoodIcon.BackgroundColor = Color.LightGray;
            EntertainmentIcon.BackgroundColor = Color.LightGray;
            EducationIcon.BackgroundColor = Color.LightGray;
            BillsIcon.BackgroundColor = Color.LightGray;
            GiftIcon.BackgroundColor = Color.LightGray;
        }
    }
}