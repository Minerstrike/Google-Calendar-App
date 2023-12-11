using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using GoogleCalendarResearch.Core;
using GoogleCalendarResearch.Services;
using System.Windows;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;

namespace GoogleCalendarResearch.MVVM.View;

/// <summary>
/// Interaction logic for AddEventWindow.xaml
/// </summary>
public partial class AddEventWindow : ObservableWindow
{

    #region Properties

    NetworkService networkService;

    readonly DateTime today = DateTime.Today;
    readonly string timeZone = TimeZoneInfo.Local.ToString();

    #endregion

    #region Constructors

    public AddEventWindow(NetworkService networkService)
    {
        InitializeComponent();

        this.networkService = networkService;
    }

    #endregion

    #region Bindings

    /// <summary>
    /// The summary of the event
    /// </summary>
    private string _summary;
    public string summary
    {
        get => _summary;
        set
        {
            _summary = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Start date and time of the event
    /// </summary>
    private DateTimeOffset _startDateTimeOffset;
    public DateTimeOffset startDateTimeOffset
    {
        get => _startDateTimeOffset;
        set
        {
            _startDateTimeOffset = value;
            OnPropertyChanged();
        }
    }


    /// <summary>
    /// End date and time of the event
    /// </summary>
    private DateTimeOffset _endDateTimeOffset;
    public DateTimeOffset endDateTimeOffset
    {
        get => _endDateTimeOffset;
        set
        {
            _endDateTimeOffset = value;
            OnPropertyChanged();
        }
    }


    #endregion

    #region Network Requests

    public void AddEvent()
    {
        if (string.IsNullOrEmpty(summary))
        {
            MessageBox.Show(
                messageBoxText  : "Please provide a summary for the event",
                caption         : "Missing summary",
                button          : MessageBoxButton.OK,
                icon            : MessageBoxImage.Exclamation);

            return;
        }

        if (startDateTimeOffset == new DateTimeOffset())
        {
            MessageBox.Show(
                messageBoxText  : "Please provide a start date and time for the event",
                caption         : "Missing start date",
                button          : MessageBoxButton.OK,
                icon            : MessageBoxImage.Exclamation);

            return;
        }

        if (endDateTimeOffset == new DateTimeOffset())
        {
            MessageBox.Show(
                messageBoxText  : "Please provide an end date and time for the event",
                caption         : "Missing end date",
                button          : MessageBoxButton.OK,
                icon            : MessageBoxImage.Exclamation);

            return;
        }

        if (startDateTimeOffset > endDateTimeOffset)
        {
            MessageBox.Show(
                messageBoxText  : "An event cannot begin after it has ended",
                caption         : "Invalid dates",
                button          : MessageBoxButton.OK,
                icon            : MessageBoxImage.Exclamation);

            return;
        }

        Event targetEvent = new Event()
        {
            Summary = summary,
            Start = new EventDateTime()
            {
                DateTimeDateTimeOffset = startDateTimeOffset,
                TimeZone = timeZone
            },
            End = new EventDateTime()
            {
                DateTimeDateTimeOffset = endDateTimeOffset,
                TimeZone = timeZone
            }
        };

        networkService.PostEvent(targetEvent);

        this.Close();
    }

    #endregion

    #region Button Actions

    private void ButtonCreateEvent_Click(object sender, RoutedEventArgs e)
    {
        AddEvent();
    }

    #endregion
}
