using ExpenseTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpenseTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()

        {
            InitializeComponent();
        }


        private async void SignInButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(UsernameEntry.Text) && !string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                var Loginobj = UserManager.Login(UsernameEntry.Text, PasswordEntry.Text);
                if (Loginobj != null)
                {
                    if (UserManager.IsCurrentMonthBudgetSet())
                    {
                        Constants.CurretMonth = DateTime.Today;
                        await Navigation.PushModalAsync(new ExpensesPage());
                        return;
                    }

                    await Navigation.PushModalAsync(new BudgetPage());
                    return;
                }
            }

            await DisplayAlert("Authentication error", "Please enter a valid username and password", "Ok");
        }

        private async void SignUpButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(UsernameEntry.Text) && !string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                var checkuser = FileManager.ReadFileData(UsernameEntry.Text);
                if(!string.IsNullOrEmpty(checkuser))
                {
                    await DisplayAlert("Validation error", "Username already exists, please choose a different username.", "Ok");
                    return;
                }
                var Loginobj = UserManager.CreatenewUser(UsernameEntry.Text, PasswordEntry.Text);
                if (Loginobj != null)
                {
                    await Navigation.PushModalAsync(new BudgetPage());
                    return;
                }
            }

            await DisplayAlert("Authentication error", "Please enter a valid username and password", "Ok");
        }
    }
}