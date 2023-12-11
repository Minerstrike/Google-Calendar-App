using Google.Apis.Calendar.v3.Data;
using GoogleCalendarResearch.Core;
using GoogleCalendarResearch.Services;
using System.Windows;

namespace GoogleCalendarResearch.MVVM.View;


/// <summary>
/// Interaction logic for UpdateEventWindow.xaml
/// </summary>
public partial class UpdateEventWindow : ObservableWindow
{
    #region Properties

    NetworkService networkService;

    private Event _targetEvent;
    public Event targetEvent
    {
        get => _targetEvent;
        set
        {
            _targetEvent = value;
            OnPropertyChanged();
        }
    }


    readonly DateTime today = DateTime.Today;
    readonly string timeZone = TimeZoneInfo.Local.ToString();

    #endregion

    #region Constructor

    public UpdateEventWindow(NetworkService networkService, Event targetEvent)
    {
        InitializeComponent();

        this.networkService = networkService;
        this.targetEvent = targetEvent;
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

    public void UpdateEvent()
    {
        if (string.IsNullOrEmpty(summary) 
            && startDateTimeOffset == new DateTimeOffset() 
            && endDateTimeOffset == new DateTimeOffset())
        {
            MessageBox.Show(
                messageBoxText  : "Too little input to update the event",
                caption         : "Invalid input",
                button          : MessageBoxButton.OK,
                icon            : MessageBoxImage.Exclamation);

            return;
        }

        if (string.IsNullOrEmpty(summary))
        {
            summary = targetEvent.Summary;
        }

        if (startDateTimeOffset == new DateTimeOffset())
        {
            startDateTimeOffset = targetEvent.Start.DateTimeDateTimeOffset.Value;
        }

        if (endDateTimeOffset == new DateTimeOffset())
        {
            endDateTimeOffset = targetEvent.End.DateTimeDateTimeOffset.Value;
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

        targetEvent.Summary = summary;

        targetEvent.Start = new EventDateTime()
        {
            DateTimeDateTimeOffset = startDateTimeOffset,
            TimeZone = timeZone
        };

        targetEvent.End = new EventDateTime()
        {
            DateTimeDateTimeOffset = endDateTimeOffset,
            TimeZone = timeZone
        };

        networkService.UpdateEvent(targetEvent);

        Close();
    }

    #endregion

    #region Button Actions

    private void ButtonUpdateEvent_Click(object sender, RoutedEventArgs e)
    {
        UpdateEvent();
    }

    #endregion
}
