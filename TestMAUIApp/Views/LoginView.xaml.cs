﻿using TestMAUIApp.ViewModels;

namespace TestMAUIApp.Views
{
    public partial class LoginView : ContentPage
    {

        public LoginView(LoginViewModel loginViewModel)
        {
            InitializeComponent();
            BindingContext = loginViewModel;
        }
    }

}
