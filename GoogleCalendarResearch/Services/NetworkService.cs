using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using GoogleCalendarResearch.Authentication;
using System.Threading;
using System.Windows;

namespace GoogleCalendarResearch.Services;

public class NetworkService
{
    #region Properties

    CalendarService service;
    UserCredential credential;

    private const string clientd        = AuthConstants.CLIENT_ID;
    private const string clientSecret   = AuthConstants.CLIENT_SECRET;

    string calendarId   = string.Empty;
    string username     = string.Empty;

    public string[] scopes =
    {
        CalendarService.Scope.CalendarEvents
    };

    #endregion

    #region Constructors

    public NetworkService()
    {

    }

    #endregion

    #region Methods

    public async Task<NetworkService> BuildAsync(string calendarId, string username)
    {
        this.calendarId = calendarId;
        this.username = username;

        service = new CalendarService();

        await SignInAsync(scopes, username);

        return this;
    }

    public async Task<NetworkService> BuildAsync(string calendarId, string username, CancellationToken cancellationToken)
    {
        this.calendarId = calendarId;
        this.username = username;

        service = new CalendarService();

        await SignInAsync(scopes, username, cancellationToken);
        return this;
    }

    public async Task<List<Event>> GetCalendarEventsAsync()
    {
        var request = service.Events.List(calendarId);

        try
        {
            var response = await request.ExecuteAsync();

            if (response is not null)
            {
                return response.Items.ToList();
            }

            return [];
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Attempt failed", MessageBoxButton.OK, MessageBoxImage.Error);
            return [];
        }
    }

    public async Task<List<Event>> GetCalendarEventsAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested is false)
        {
            var request = service.Events.List(calendarId);

            try
            {
                var response = await request.ExecuteAsync(cancellationToken);

                return response.Items.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    message         : ex.Message,
                    innerException  : ex.InnerException
                );
            } 
        }

        return [];
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
            ).Result;

        service = new CalendarService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Google calendar research app"
        });
    }

    public async Task SignInAsync(string[] scopes, string username)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        credential = await GoogleWebAuthorizationBroker.AuthorizeAsync
            (
                new ClientSecrets
                {
                    ClientId = clientd,
                    ClientSecret = clientSecret
                },
                scopes,
                username,
                cancellationTokenSource.Token
            );

        service = new CalendarService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Google calendar research app"
        });
    }

    public async Task SignInAsync(string[] scopes, string username, CancellationToken cancellationToken)
    {
        credential = await GoogleWebAuthorizationBroker.AuthorizeAsync
            (
                new ClientSecrets
                {
                    ClientId = clientd,
                    ClientSecret = clientSecret
                },
                scopes,
                username,
                cancellationToken
            );

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
