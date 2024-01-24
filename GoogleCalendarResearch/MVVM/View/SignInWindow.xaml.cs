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

    public SignInWindow()
    {
        InitializeComponent();
    }
    
    public SignInWindow(NetworkService networkService)
    {
        InitializeComponent();

        this.networkService = networkService;
        activeNetworkService = true;
    }

    #endregion

    #region Methods

    public async Task InitialiseNetworkService()
    {
        if (calendarId == string.Empty)
        {
            networkService = await new NetworkService().BuildAsync(username, username);
        }
        else
        {
            networkService = await new NetworkService().BuildAsync(username, username);
        }
    }

    #endregion

    #region Button Actions

    private async void ButtonSignIn_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            MessageBox.Show("Please enter a username.", "Missing username.", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        await InitialiseNetworkService();

        new MainWindow(networkService).Show();
        Close();
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

    #region Window events
    
    private void ObservableWindow_Closed(object sender, EventArgs e)
    {
        if (networkService is not null)
        {
            networkService.SignOut(); 
        }
    }

    #endregion
}
