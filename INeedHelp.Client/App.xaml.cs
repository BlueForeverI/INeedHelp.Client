﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Callisto.Controls;
using INeedHelp.Client.Data;
using INeedHelp.Client.Helpers;
using INeedHelp.Client.ViewModels;
using INeedHelp.Client.Views;
using ParseStarterProject.Services;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace INeedHelp.Client
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private ProfileSettingsView settingsView;
        private ProfileSettingsViewModel settingsViewModel;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            settingsView = new ProfileSettingsView();
            settingsViewModel = new ProfileSettingsViewModel();
            settingsView.DataContext = settingsViewModel;
        }

        private void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsCommand profileSettingsCommand = new SettingsCommand("profile-settings", "Profile Settings",
                                                          HandleProfileSettingsCommandRequest);
            args.Request.ApplicationCommands.Add(profileSettingsCommand);

            SettingsCommand privaciPolicyCommand = new SettingsCommand("privacy-policy", "Privacy Policy",
                                                          HandlePrivacyPolocySettingsCommand);
            args.Request.ApplicationCommands.Add(privaciPolicyCommand);
        }

        private void HandlePrivacyPolocySettingsCommand(IUICommand command)
        {
            var settings = new SettingsFlyout();
            settings.Content = new PrivacyPolicySettingsView();
            settings.HeaderText = "Privacy Policy";
            settings.IsOpen = true;
        }

        private void HandleProfileSettingsCommandRequest(IUICommand command)
        {
            var settings = new SettingsFlyout();
            settings.Content = settingsView;
            settingsView.DataContext = settingsViewModel;
            settingsViewModel.LoadProperties();
            settingsViewModel.PictureReceived += (sender, args) => HandleProfileSettingsCommandRequest(command);
            settings.HeaderText = "Profile Settings";
            settings.IsOpen = true;

        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(Views.HomeView), args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            if (args.TileId != "App")
            {
                int requestId = int.Parse(args.Arguments);
                var request = await HelpRequestsPersister.GetRequestById(requestId,
                    AccountManager.CurrentUser.SessionKey);
                NavigationService.Navigate(ViewType.EditRequest, request);
            }

            // Ensure the current window is active
            Window.Current.Activate();


            SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        /// <summary>
        /// Invoked when the application is activated to display search results.
        /// </summary>
        /// <param name="args">Details about the activation request.</param>
        protected async override void OnSearchActivated(Windows.ApplicationModel.Activation.SearchActivatedEventArgs args)
        {
            // TODO: Register the Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().QuerySubmitted
            // event in OnWindowCreated to speed up searches once the application is already running

            // If the Window isn't already using Frame navigation, insert our own Frame
            var previousContent = Window.Current.Content;
            var frame = previousContent as Frame;

            // If the app does not contain a top-level frame, it is possible that this 
            // is the initial launch of the app. Typically this method and OnLaunched 
            // in App.xaml.cs can call a common method.
            if (frame == null)
            {
                // Create a Frame to act as the navigation context and associate it with
                // a SuspensionManager key
                frame = new Frame();
                INeedHelp.Client.Common.SuspensionManager.RegisterFrame(frame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await INeedHelp.Client.Common.SuspensionManager.RestoreAsync();
                    }
                    catch (INeedHelp.Client.Common.SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }
            }

            frame.Navigate(typeof(SearchRequestsView), args.QueryText);
            Window.Current.Content = frame;

            // Ensure the current window is active
            Window.Current.Activate();
        }

    }
}
