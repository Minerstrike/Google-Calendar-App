using GoogleCalendarResearch.Core;
using GoogleCalendarResearch.Services;
using System.Windows;

namespace GoogleCalendarResearch.MVVM.View;


/// <summary>
/// Interaction logic for SignInWindow.xaml
/// </summary>
public partial class SignInWindow : ObservableWindow
{
    #region Properties

    NetworkService networkService;

    private string _calendarId = string.Empty;
    public string calendarId
    {
        get => _calendarId;
        set 
        { 
            _calendarId = value;
            OnPropertyChanged();
        }
    }

    private string _username = string.Empty;
    public string username 
    {
        get => _username;
        set 
        { 
            _username = value;
            OnPropertyChanged();
        }
    }

    private bool _activeNetworkService;

    public bool activeNetworkService
    {
        get => _activeNetworkService;
        set 
        { 
            _activeNetworkService = value;
            OnPropertyChanged();
        }
    }


    #endregion

    #region Constructor

    public SignInWindow(NetworkService networkService)
    {
        InitializeComponent();

        this.networkService = networkService;
        activeNetworkService = true;
    }

    #endregion

    #region Methods

    public void InitialiseNetworkService()
    {
        if (calendarId == string.Empty)
        {
            networkService = new NetworkService(username, username);
        }
        else
        {
            networkService = new NetworkService(calendarId, username);
        }
    }

    #endregion

    #region Button Actions

    private void ButtonSignIn_Click(object sender, RoutedEventArgs e)
    {
        InitialiseNetworkService();
        
        new MainWindow(networkService).Show();
        this.Close();
    }

    private void ButtonSignOut_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            activeNetworkService = !networkService.SignOut();

            MessageBox.Show("Successfully signed out", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception)
        {

            MessageBox.Show("Failed to sign out", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion
}
