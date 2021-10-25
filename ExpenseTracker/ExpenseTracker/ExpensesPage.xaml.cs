using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpenseTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpensesPage : ContentPage
    {
        public ExpensesPage()
        {
            InitializeComponent();
            
        }
        protected override void OnAppearing()
        {
            ExpensesListView.ItemsSource = null;
            SettingMonth_YearPickerData();
            Setting_SortPickerdata();
            Setting_FilteringPickerData();
            ExpenseListViewByDate(Constants.CurretMonth.Month.ToString(), Constants.CurretMonth.Year.ToString());
        }
        

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void AddNewExpenses_Click(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddExpensePage
            {
                BindingContext = new Expenses()
            });
        }

        private  async void ShowMonthlyBudgetSummary_Click(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new BudgetPage
            {
                BindingContext = new Budget()
            }) ;
        }

        private async void ExpensesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushModalAsync(new AddExpensePage
            {
                BindingContext = (Expenses)e.SelectedItem
            });
        }

        private void Setting_SortPickerdata( )
        {
            List<String> SortingItems = new List<string>() ;
            SortingPicker.ItemsSource = null;
            SortingItems.Add("--");
            SortingItems.Add("Price");
            SortingItems.Add("Expense Date");
            SortingPicker.ItemsSource = SortingItems;

            
        }

        private void SortingSelectedItem_Click(object sender, EventArgs e)
        {
            List<Expenses> ExpenseList = new List<Expenses>();
            List<Expenses> SortExpenseList = new List<Expenses>();
            FileManagement CurrentData = new FileManagement();

           

            ExpenseList = CurrentData.ExpenseList_Bydate(Constants.CurretMonth.Month.ToString(), Constants.CurretMonth.Year.ToString());

            if (SortingPicker.SelectedIndex == 0)
            {
                ExpensesListView.ItemsSource = null;
                ExpensesListView.ItemsSource = ExpenseList;
            }

            else if (SortingPicker.SelectedIndex == 2)
            {
                FilteringPicker.SelectedItem = "--";
                SortExpenseList =ExpenseList.OrderBy(n => n.ExpenseDate).ToList();
                ExpensesListView.ItemsSource = null;
                ExpensesListView.ItemsSource = SortExpenseList;
            }
            else if (SortingPicker.SelectedIndex == 1)
            {
                FilteringPicker.SelectedItem = "--";
                SortExpenseList = ExpenseList.OrderBy(n => n.ExpenseAmount).ToList();
                ExpensesListView.ItemsSource = null;
                ExpensesListView.ItemsSource = SortExpenseList;
            }
        }

        private void Setting_FilteringPickerData()
        {

            FilteringPicker.Items.Clear();

            FilteringPicker.Items.Add("--");
            foreach (string name in Enum.GetNames(typeof(Category)))
            {
                FilteringPicker.Items.Add(name);
            }
        }

        private void SettingMonth_YearPickerData( )
        {

            ExpenseMonthPicker.SelectedIndex = Constants.CurretMonth.Month - 1;
            ExpenseYearPicker.SelectedItem = Constants.CurretMonth.Year.ToString();

        }



        private void FilteringPickerSelectedItem_Click(object sender, EventArgs e)
        {
            List<Expenses> ExpenseList = new List<Expenses>();
            List<Expenses> FilteredExpenseList = new List<Expenses>();
            FileManagement CurrentData = new FileManagement();

            Xamarin.Forms.Picker SelectedCategory = (Xamarin.Forms.Picker)sender;
            string Year;
            string Month;
            if (ExpenseMonthPicker.SelectedIndex == -1) Month = Constants.CurretMonth.Month.ToString(); else Month = (ExpenseMonthPicker.SelectedIndex + 1).ToString();
            if (ExpenseYearPicker.SelectedIndex == -1) Year = Constants.CurretMonth.Year.ToString(); else Year = ExpenseYearPicker.SelectedItem.ToString();

            ExpenseList = CurrentData.ExpenseList_Bydate(Month, Year);



            if (SelectedCategory.SelectedItem == null) return; 
            if (SelectedCategory.SelectedItem.ToString() == "--")
            {
                            
                ExpensesListView.ItemsSource = null;
                ExpensesListView.ItemsSource = ExpenseList;
            }
            else
            {
                SortingPicker.SelectedItem = "--";
                FilteredExpenseList = ExpenseList.Where(n => n.ExpenseCategory.ToString() == SelectedCategory.SelectedItem.ToString()).ToList();
                ExpensesListView.ItemsSource = null;
                ExpensesListView.ItemsSource = FilteredExpenseList;

            }
        }

        private void OnMonthBrows(object sender, EventArgs e)
        {
            FindingDataValueForPicker();
        }

        private void OnYearBrows(object sender, EventArgs e)
        {

            FindingDataValueForPicker();
        }
     
        private void FindingDataValueForPicker()
        {
            string Year;
            string Month;
            if (ExpenseMonthPicker.SelectedIndex == -1) Month = Constants.CurretMonth.Month.ToString(); else Month = (ExpenseMonthPicker.SelectedIndex + 1).ToString();
            if (ExpenseYearPicker.SelectedIndex == -1) Year = Constants.CurretMonth.Year.ToString(); else Year = ExpenseYearPicker.SelectedItem.ToString();
            ExpenseListViewByDate(Month, Year);

        }
       

        public void ExpenseListViewByDate(string Month,string Year)
        {
            List<Expenses> ExpenseList = new List<Expenses>();
            List<Expenses> FilteredExpenseList = new List<Expenses>();
            FileManagement CurrentData = new FileManagement();

            if (Month == "" || Year == "")
            {
                ExpenseList = CurrentData.ExpenseList_Bydate(Constants.CurretMonth.Month.ToString(), Constants.CurretMonth.Year.ToString());
            }
            else
               ExpenseList = CurrentData.ExpenseList_Bydate(Month, Year);
            

            ExpensesListView.ItemsSource = null;
            ExpensesListView.ItemsSource = ExpenseList;
         
           
            decimal CurrentMonthCost = CurrentData.Calculate_MonthlyCost(Month, Year);
            decimal AmonthTOGoal = CurrentData.AmountToGoal(Month, Year);

            AmountLabel.Text = "Current Month Expenses Summary $ " + Convert.ToString(CurrentMonthCost);
            RemainedLable.Text = "Current Month Remaining Balance  $ " + Convert.ToString(AmonthTOGoal);






            if (Month != Constants.CurretMonth.Month.ToString() || Year != Constants.CurretMonth.Year.ToString())
            {
                ExpensesListView.IsEnabled = false;
                ADDExpenseButton.IsEnabled = false;


            }
            else
            {
                ExpensesListView.IsEnabled = true;
                ADDExpenseButton.IsEnabled = true;


            }

        }
    }
}