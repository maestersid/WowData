using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WowData.Model;
using WowData.ViewModels;

using Xamarin.Forms;

namespace WowData.Views
{
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel _viewmodel;
        public LoginPage()
        {
            InitializeComponent();
           
            _viewmodel = new LoginViewModel();
            this.BindingContext = _viewmodel;
            QueryBtn.Clicked += OnQueryData;

        }

        private async void OnQueryData(object sender, EventArgs e)
        {
            CharacterProfileData data;
            try
            {
                loadingIndicator.IsRunning = true;
                //Load Character Details
                data = await _viewmodel.LoadMainCharacterDataAsync(RealmInfo.Text,CharName.Text);

            }
            finally
            {
                loadingIndicator.IsRunning = false;
            }

            if(data == null)
            {
                //early return based on bad data
                await DisplayAlert("Error","Invalid credentials provided, please re-enter and try again", "OK");
                return;
            }

            var page = new CharacterMainView();
            var vm = new CharacterMainViewModel(data);
            page.BindingContext = vm;

            this.Navigation.PushAsync(page);
        }
    }
}
