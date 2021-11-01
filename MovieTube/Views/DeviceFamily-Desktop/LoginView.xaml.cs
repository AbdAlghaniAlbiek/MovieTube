using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.Connectivity;
using Microsoft.Toolkit.Extensions;
using MovieTube.Functions.Models;
using MovieTube.UI.Services.AppSettings;
using System.Net.NetworkInformation;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieTube.UI.Views.DeviceFamily_Desktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginView : Page
    {
        public LoginView()
        {
            this.InitializeComponent();
        }

        private DependencyObject FindChildControl<T>(DependencyObject control, string ctrlName)
        {
            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childNumber; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);
                FrameworkElement fe = child as FrameworkElement;
                // Not a framework element or is null
                if (fe == null) return null;

                if (child is T && fe.Name == ctrlName)
                {
                    // Found the control so return
                    return child;
                }
                else
                {
                    // Not found it - search children
                    DependencyObject nextLevel = FindChildControl<T>(child, ctrlName);
                    if (nextLevel != null)
                        return nextLevel;
                }
            }
            return null;
        }


        private void Page_Loading(FrameworkElement sender, object args)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                //Login pivot
                if (mainPivot.SelectedIndex == 0)
                {
                    loginIconToastNotif.Glyph = (string)this.Resources["NetworkIssueIcon"];
                    loginTextToastNotif.Text = "You aren't connected to internet";

                    loginToastNotif.Show(6000);
                }
                //Register pivot
                else
                {
                    registerIconToastNotif.Glyph = (string)this.Resources["NetworkIssueIcon"];
                    registerTextToastNotif.Text = "You aren't connected to internet";

                    registerToastNotif.Show(6000);
                }
            }
        }

        #region Sign in process

        private void txtEmailLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            loginErrorIconEmail.Visibility = Visibility.Collapsed;
            loginErrorTextEmail.Visibility = Visibility.Collapsed;
        }
        public bool LoginCheckConstrains()
        {
            bool canPass = true;
            
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (string.IsNullOrEmpty(txtEmailLogin.Text) || string.IsNullOrEmpty(txtPasswordLogin.Password))
                {
                    loginIconToastNotif.Glyph = (string)this.Resources["ErrorIcon"];
                    loginTextToastNotif.Text = "Please fill all textboxes before continue";

                    loginToastNotif.Show(6000);

                    canPass = false;
                }

                if (!txtEmailLogin.Text.IsEmail())
                {
                    loginErrorIconEmail.Visibility = Visibility.Visible;
                    loginErrorTextEmail.Visibility = Visibility.Visible;
                    canPass = false;
                }
                else
                {
                    loginErrorIconEmail.Visibility = Visibility.Collapsed;
                    loginErrorTextEmail.Visibility = Visibility.Collapsed;
                }

                return canPass;
            }
            else
            {
                loginIconToastNotif.Glyph = (string)this.Resources["NetworkIssueIcon"];
                loginTextToastNotif.Text = "You aren't connected to internet";

                loginToastNotif.Show(6000);

                canPass = false;
                return canPass;
            }
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            progLoginLoading.IsIndeterminate = true;

            if (LoginCheckConstrains())
            {
               HttpResponse response =
                    await Functions.Repositories.RemoteRepo.UsersRepo.LoginAsync(
                        new User()
                        {
                            Email = txtEmailLogin.Text,
                            Password = txtPasswordLogin.Password
                        });

                //the process is successful (user has email and password in database)
                if (response.Result != null)
                {
                    this.Frame.Navigate(typeof(CategoriesView), null, new DrillInNavigationTransitionInfo());
                }

                // user doesn't have an account in database
                else
                {
                    loginIconToastNotif.Glyph = (string)this.Resources["ErrorIcon"];
                    loginTextToastNotif.Text = response.ErrorMessage;

                    loginToastNotif.Show(6000);
                }
            }

            progLoginLoading.IsIndeterminate = false;
        }

        #endregion

        #region Sign up process

        //Registeration
        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            registerErrorIconName.Visibility = Visibility.Collapsed;
            registerErrorTextName.Visibility = Visibility.Collapsed;
        }
        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            registerErrorIconEmail.Visibility = Visibility.Collapsed;
            registerErrorTextEmail.Visibility = Visibility.Collapsed;
        }
        private bool RegisterCheckConstrains()
        {
            bool canPass = true;

            //Check if the user connect to internet
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                //Check if any textbox is empty and tell user to fill all of them
                if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Password))
                {
                    registerIconToastNotif.Glyph = (string)this.Resources["ErrorIcon"];
                    registerTextToastNotif.Text = "Please fill all textboxes before continue";

                    registerToastNotif.Show(6000);

                    canPass = false;
                }

                //I make this way beacause IsCharacterString() method if there is spaces ot tabs between words in 
                // txtName.Text then it return false state to me
                //to solve that I made this 'for' to split the text of txtName to subStrings and check each of them once by once
                string[] subTxtName = txtName.Text.Split(" ");

                for (int i = 0; i < subTxtName.Length; i++)
                {
                    if (subTxtName[i] != "")
                    {
                        //Check if there is Unvalid Name format
                        if (!subTxtName[i].IsCharacterString())
                        {
                            registerErrorIconName.Visibility = Visibility.Visible;
                            registerErrorTextName.Visibility = Visibility.Visible;
                            canPass = false;
                            break;
                        }
                    }
                }

                //Check if there is Unvalid Email format
                if (!txtEmail.Text.IsEmail())
                {
                    registerErrorIconEmail.Visibility = Visibility.Visible;
                    registerErrorTextEmail.Visibility = Visibility.Visible;
                    canPass = false;
                }

                return canPass;
            }
            else
            {
                registerIconToastNotif.Glyph = (string)this.Resources["NetworkIssueIcon"];
                registerTextToastNotif.Text = "You aren't connected to internet";

                registerToastNotif.Show(6000);

                canPass = false;
                return canPass;
            }
        }

        private async void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            progRegisterLoading.IsIndeterminate = true;

            if (RegisterCheckConstrains())
            {
                HttpResponse response =
                    await Functions.Repositories.RemoteRepo.UsersRepo.RegisterAsync(
                        new User()
                        {
                            Email = txtEmail.Text,
                            Password = txtPassword.Password,
                            Name = txtName.Text
                        });

                //the process is successful (user has email and password in database)
                if (response.Result != null)
                {
                    mainPivot.SelectedIndex = 2;
                }
                // user doesn't have an account in database
                else
                {
                    registerIconToastNotif.Glyph = (string)this.Resources["ErrorIcon"];
                    registerTextToastNotif.Text = response.ErrorMessage;

                    registerToastNotif.Show(6000);
                }
            }

            progRegisterLoading.IsIndeterminate = false;
        }


        //Verification
        private bool VerifyrCheckConstrains()
        {
            //Check if the user connect to internet
            if (NetworkInterface.GetIsNetworkAvailable())
            {

                //Check if any textbox is empty and tell user to fill all of them
                if (string.IsNullOrEmpty(txtCode.Text))
                {
                    verifyIconToastNotif.Glyph = (string)this.Resources["ErrorIcon"];
                    verifyTextToastNotif.Text = "Please fill texbox before continue";
 
                    verifyToastNotif.Show(6000);

                    return false;
                }

                return true;
            }
            else
            {
                verifyIconToastNotif.Glyph = (string)this.Resources["NetworkIssueIcon"];
                verifyTextToastNotif.Text = "You aren't connected to internet";

                verifyToastNotif.Show(6000);

                return false;
            }
        }
        private async void BtnVerify_Click(object sender, RoutedEventArgs e)
        {
            progVerifyLoading.IsIndeterminate = true;

            if (VerifyrCheckConstrains())
            {
                HttpResponse response =
                    await Functions.Repositories.RemoteRepo.UsersRepo.VerifyAsync(txtCode.Text);

                //the process is successful (user has email and password in database)
                if (response.Result != null)
                {
                    mainPivot.SelectedIndex = 3;
                }
                // user doesn't have an account in database
                else
                {
                    verifyIconToastNotif.Glyph = (string)this.Resources["ErrorIcon"];
                    verifyTextToastNotif.Text = response.ErrorMessage;

                    verifyToastNotif.Show(6000);
                }
            }

            progVerifyLoading.IsIndeterminate = false;

        }


        //Success and contrinue to main page
        private void BtnRegSuccess_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CategoriesView), null, new DrillInNavigationTransitionInfo());
        }


        #endregion

        private void backToSignInBtn_Click(object sender, RoutedEventArgs e)
        {
            mainPivot.SelectedIndex = 0;
        }

        private void hyplinkGoToSignUp_Click(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            mainPivot.SelectedIndex = 1;
        }
    }
}
