﻿using ReactiveUI;
using RoutingExample.ViewModels;
using RoutingExample.Views;
using System;

namespace RoutingExample
{
    public class AppViewLocator : IViewLocator
    {
        public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch
        {
            FirstViewModel context => new FirstView { ViewModel = context },
            LoginViewModel context => new LoginWindow { ViewModel = context },
            RegistrationViewModel context => new RegistrationWindow { ViewModel = context },
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
        };
    }
}
