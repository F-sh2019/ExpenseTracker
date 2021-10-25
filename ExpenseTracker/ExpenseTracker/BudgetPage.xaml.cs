using ExpenseTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpenseTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BudgetPage : ContentPage
    {
        public BudgetPage()
        {
            InitializeComponent();
        }

        private DateTime budgetDate = DateTime.Now;
        private User currentUser = UserManager.GetLoggedInUser();
        private Budget currentBudget;

        protected override void OnAppearing()
        {
            Budget thisBudget = Budget.getMatchingBudget(budgetDate, currentUser);
            budgetDate = thisBudget.BudgetDate;

            // populate data into Pickers and BudgetInput
            BudgetInput.Text = thisBudget.BudgetGoalAmount.ToString();
            BudgetStatusReport.Text = $"Spent ${getTotalExpensesForMonth()} of ${thisBudget.BudgetGoalAmount}";

            int monthId = budgetDate.Month;
            int yearId = budgetDate.Year;
            BudgetMonthPicker.SelectedIndex = monthId - 1;
            BudgetYearPicker.SelectedIndex = yearId - 2021;
            currentBudget = thisBudget;

            getSummaryData(currentBudget);
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            Budget nextBudget = Budget.getMatchingBudget(budgetDate, currentUser);
            budgetDate = nextBudget.BudgetDate;
            nextBudget.BudgetGoalAmount = decimal.Parse(BudgetInput.Text.Replace("$", string.Empty));
            BudgetInput.Text = nextBudget.BudgetGoalAmount.ToString();
            nextBudget.BudgetDate = budgetDate;
            currentBudget = nextBudget;
            Constants.CurretMonth = budgetDate;
            Console.WriteLine("look at user: ");
            Console.WriteLine(currentUser);
            UserManager.SaveLoggedInUserData();
            await Navigation.PushModalAsync(new ExpensesPage());
        }

        private async void OnViewExpensesButtonClicked(object sender, EventArgs e)
        {
            Constants.CurretMonth = budgetDate;
            await Navigation.PushModalAsync(new ExpensesPage { BindingContext = new Expenses() });
            return;
        }
        public void OnMonthChosen(object sender, EventArgs e)
        {
            BudgetInput.Text = "0";
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                budgetDate = new DateTime(BudgetYearPicker.SelectedIndex + 2021, selectedIndex + 1, 1);
            }
            
            this.SetBudgetGoalAmount();

            getSummaryData(currentBudget);
        }
        public void OnYearChosen(object sender, EventArgs e)
        {
            BudgetInput.Text = "0";
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                budgetDate = new DateTime(selectedIndex + 2021, BudgetMonthPicker.SelectedIndex + 1, 1);
            }

            this.SetBudgetGoalAmount();

            getSummaryData(currentBudget);
        }

        private void SetBudgetGoalAmount()
        {
            var currentBudget = UserManager.GetBudgetForBudgetDate(budgetDate);
            if (currentBudget != null)
            {
                BudgetInput.Text = currentBudget.BudgetGoalAmount.ToString();
            }
        }

        public void getSummaryData(Budget currentBudget)
        {
            decimal totalHomeExpense = 0;
            decimal totalShoppingExpense = 0;
            decimal totalTravelExpense = 0;
            decimal totalFoodExpense = 0;
            decimal totalEntertainmentExpense = 0;
            decimal totalEducationExpense = 0;
            decimal totalBillsExpense = 0;
            decimal totalGiftExpense = 0;
            decimal totalExpenses = 0;

            Budget tempCurrentBudget = Budget.loadMatchingBudgetData(budgetDate, currentUser);

            foreach (Expenses expense in tempCurrentBudget.ListOfExpenses)
            {
                if (expense.ExpenseCategory == Category.Home)
                {
                    totalHomeExpense = totalHomeExpense + expense.ExpenseAmount;
                }

                if (expense.ExpenseCategory == Category.Shopping)
                {
                    totalShoppingExpense = totalShoppingExpense + expense.ExpenseAmount;
                }

                if (expense.ExpenseCategory == Category.Travel)
                {
                    totalTravelExpense = totalTravelExpense + expense.ExpenseAmount;
                }

                if (expense.ExpenseCategory == Category.Food)
                {
                    totalFoodExpense = totalFoodExpense + expense.ExpenseAmount;
                }

                if (expense.ExpenseCategory == Category.Entertainment)
                {
                    totalEntertainmentExpense = totalEntertainmentExpense + expense.ExpenseAmount;
                }

                if (expense.ExpenseCategory == Category.Education)
                {
                    totalEducationExpense = totalEducationExpense + expense.ExpenseAmount;
                }

                if (expense.ExpenseCategory == Category.Bills)
                {
                    totalBillsExpense = totalBillsExpense + expense.ExpenseAmount;
                }

                if (expense.ExpenseCategory == Category.Gift)
                {
                    totalGiftExpense = totalGiftExpense + expense.ExpenseAmount;
                }
            }
            HomeTotal.Text = "$" + totalHomeExpense.ToString();
            ShoppingTotal.Text = "$" + totalShoppingExpense.ToString();
            TravelTotal.Text = "$" + totalTravelExpense.ToString();
            FoodTotal.Text = "$" + totalFoodExpense.ToString();
            EntertainmentTotal.Text = "$" + totalEntertainmentExpense.ToString();
            EducationTotal.Text = "$" + totalEducationExpense.ToString();
            BillsTotal.Text = "$" + totalBillsExpense.ToString();
            GiftTotal.Text = "$" + totalGiftExpense.ToString();
            BudgetStatusReport.Text = $"Spent ${getTotalExpensesForMonth()} of ${tempCurrentBudget.BudgetGoalAmount}";
        }

        public decimal getTotalExpensesForMonth()
        {
            decimal totalMonthlyExpenses = 0;

            currentBudget = Budget.loadMatchingBudgetData(budgetDate, currentUser);
            foreach (Expenses expense in currentBudget.ListOfExpenses)
            {
                totalMonthlyExpenses += expense.ExpenseAmount;
            }
            return totalMonthlyExpenses;
        }
    }
}