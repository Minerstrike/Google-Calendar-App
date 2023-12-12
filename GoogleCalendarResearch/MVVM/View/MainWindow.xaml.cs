using Google.Apis.Calendar.v3.Data;
using GoogleCalendarResearch.Core;
using GoogleCalendarResearch.MVVM.View;
using GoogleCalendarResearch.Services;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Input;

namespace GoogleCalendarResearch;


/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : ObservableWindow
{
    #region Properties

    NetworkService networkService;

    private List<Event> _events = [];
    public List<Event> events
    {
        get => _events;
        set 
        { 
            _events = value; 
            OnPropertyChanged();
        }
    }

    private Event _selectedEvent = new Event();
    public Event selectedEvent
    {
        get => _selectedEvent;
        set 
        { 
            _selectedEvent = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Constructors

    public MainWindow(NetworkService networkService)
    {
        InitializeComponent();

        this.networkService = networkService;
    }

    #endregion

    #region Network requests

    public async Task<List<Event>> GetEventsAsync()
    {
        return await networkService.GetCalendarEventsAsync();
    }

    public void DeleteEvent(Event targetEvent)
    {
        networkService.DeleteEvent(targetEvent);
    }

    #endregion

    #region Button actions

    private async void ButtonAdd_Click(object sender, RoutedEventArgs e)
    {
        await CreateEventHelper();
    }

    private async void ButtonShowEvents_Click(object sender, RoutedEventArgs e)
    {
        await ReadEventsHelper();
    }

    private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
    {
        UpdateEventsHelper();
    }

    private async void ButtonDelete_Click(object sender, RoutedEventArgs e)
    {
        await DeleteEventHelper();
    }

    private void ButtonSignIn_Click(object sender, RoutedEventArgs e)
    {
        new SignInWindow(networkService).Show();
        this.Close();
    }

    #endregion

    #region Methods

    public async Task CreateEventHelper()
    {
        try
        {
            new AddEventWindow(networkService).ShowDialog();
            events = await GetEventsAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task ReadEventsHelper()
    {
        try
        {
            events = await GetEventsAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void UpdateEventsHelper()
    {
        if (selectedEvent is not null)
        {
            new UpdateEventWindow(networkService, selectedEvent).ShowDialog();
        }
        else
        {
            MessageBox.Show(
                    messageBoxText  : "Please select an Event",
                    caption         : "Missing Event",
                    button          : MessageBoxButton.OK,
                    icon            : MessageBoxImage.Exclamation);
        }
    }

    public async Task DeleteEventHelper()
    {
        try
        {
            DeleteEvent(selectedEvent);
            events = await GetEventsAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

    #region Window Events

    private async void ObservableWindow_Activated(object sender, EventArgs e)
    {
        events = await GetEventsAsync();
    }

    #endregion

    #region Mouse Events

    private void Menu_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            this.DragMove();
        }
    } 

    #endregion
}