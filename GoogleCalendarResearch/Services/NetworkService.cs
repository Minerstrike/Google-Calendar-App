using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Security.Permissions;
using System.Windows;

namespace GoogleCalendarResearch.Services;

public class NetworkService
{
    #region Properties

    CalendarService service;
    UserCredential credential;

    //const string ApiKey = "AIzaSyA_wie79k__bvhErT6ecaz4BWH2WrHUUbU";
    //const string holidayCalendarIdSA = "en.sa#holiday@group.v.calendar.google.com";

    private const string clientd = "705457219541-3sn73lrf75ockoqg8rp5av0mq09d0q7b.apps.googleusercontent.com";
    private const string clientSecret = "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";

    string calendarId = "micah@farsoft.co.za";
    string username = "micah@farsoft.co.za";

    public string[] scopes =
    {
        CalendarService.Scope.CalendarEvents
    };

    #endregion

    #region Constructors

    public NetworkService(string calendarId, string username)
    {
        this.calendarId = calendarId;
        this.username = username;
        
        service = new CalendarService();

        SignIn(scopes, username);
    }

    #endregion

    #region Methods

    public async Task<List<Event>> GetCalendarEventsAsync()
    {
        var request = service.Events.List(calendarId);

        try
        {
            var response = await request.ExecuteAsync();

            return response.Items.ToList();
        }
        catch (Exception)
        {

            MessageBox.Show("Possible mismatch of accounts", "Attempt failed", MessageBoxButton.OK, MessageBoxImage.Error);
            return [];
        }
    }

    public void PostEvent(Event targetEvent)
    {
        var request = service.Events.Insert(targetEvent, calendarId);
        request.Execute();
    }

    public void UpdateEvent(Event targetEvent)
    {
        var request = service.Events.Update(targetEvent, calendarId, targetEvent.Id);
        request.Execute();
    }

    public void DeleteEvent(Event targetEvent)
    {
        var request = service.Events.Delete(calendarId, targetEvent.Id);
        request.Execute();
    }

    public void SignIn(string[] scopes, string username)
    {
        credential = GoogleWebAuthorizationBroker.AuthorizeAsync
            (
                new ClientSecrets
                {
                    ClientId = clientd,
                    ClientSecret = clientSecret
                },
                scopes,
                username,
                CancellationToken.None
                //new FileDataStore("GOOGLE CALENDAR API ACCESS")
                //new FileDataStore("Daimto.GoogleCalendar.Auth.Store")
            ).Result;

        service = new CalendarService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Google calendar research app"
        });
    }

    public bool SignOut()
    {
        return credential.RevokeTokenAsync(CancellationToken.None).Result;
    }

    #endregion
}
