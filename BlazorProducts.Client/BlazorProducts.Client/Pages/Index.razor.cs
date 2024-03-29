﻿using Blazored.LocalStorage;
using BlazorProducts.Client.Features;
using BlazorProducts.Client.HttpInterceptor;
using BlazorProducts.Client.HttpRepository;
using Entities.Configuration;
using Entities.DTO;
using Entities.Enums;
using Entities.WebSocket;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorProducts.Client.Pages;

public partial class Index
{
    [Inject] public IOptions<WebSocketConfiguration> WebSocketSettings { get; set; }
    [Inject] public ITenantDayRepo TenantDayRepository { get; set; }
    [Inject] public HttpInterceptorService Interceptor { get; set; }
    [Inject] public ILocalStorageService LocalStorageService { get; set; }

    public string LoggedUserName { get; set; }
    public int DayOfWeekLocalNumber { get; set; }
    public DateTime Today { get; set; }
    public List<string> TenantsDaysForUi { get; set; } = new();
    public List<TenantsForDay> TenantsForDay { get; set; } = new();
    public IEnumerable<string> TenantsDayActualSelection { get; set; } = new List<string>();
    public WebSocketConfiguration WebSocketConfiguration { get; set; }

    private const int NumberOfDaysToShow = 31;

    //https://gist.github.com/SteveSandersonMS/5aaff6b010b0785075b0a08cc1e40e01
    private CancellationTokenSource disposalTokenSource = new();
    private ClientWebSocket _webSocket = new();

    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        Interceptor.RegisterBeforeSendEvent();

        WebSocketConfiguration = WebSocketSettings.Value;
        await _webSocket.ConnectAsync(new Uri(WebSocketConfiguration.Connection), disposalTokenSource.Token);
        _ = ReceiveLoop();

        Today = DateTime.Today;

        var currentDay = DateTime.Now.DayOfWeek.ToString();
        _ = Enum.TryParse(currentDay, out DayOfWeekLocal dayOfWeekLocal);
        DayOfWeekLocalNumber = (int) dayOfWeekLocal;

        var token = await LocalStorageService.GetItemAsync<string>("authToken");
        if (!string.IsNullOrWhiteSpace(token))
        {
            LoggedUserName = JwtParser.GetLoggedUserName(token);
        }

        TenantsDaysForUi = await TenantDayRepository.GetTenantDays(LoggedUserName);

        TenantsForDay = await GetCalendarMap();

        TenantsDayActualSelection = TenantsForDay.Where(w => w.TenantIds.Contains(LoggedUserName)).Select(d => d.DayId);
    }

    private async Task BookOrFreeDay(string day)
    {
        var tenantsDays = await TenantDayRepository.GetTenantDays(LoggedUserName);

        if (tenantsDays.Contains(day))
        {
            await TenantDayRepository.FreeDay(new TenantDay {DayId = day, TenantId = LoggedUserName});
            TenantsForDay.Single(w => w.DayId == day).TenantIds.Remove(LoggedUserName);
        }
        else
        {
            var tenantsForConcreteDay = await TenantDayRepository.GetDaysForTenant(day);
            if (tenantsForConcreteDay.Count < 2)
            {
                await TenantDayRepository.BookDay(new TenantDay {DayId = day, TenantId = LoggedUserName});
                TenantsForDay.Single(w => w.DayId == day).TenantIds.Add(LoggedUserName);
            }
            else
                TenantsForDay =
                    await GetCalendarMap(); // if simultaneous 2 user booked the same day, one of them cant booked and then UI must be reload with updated days
        }

        TenantsDaysForUi = await TenantDayRepository.GetTenantDays(LoggedUserName);
        TenantsDayActualSelection = TenantsForDay.Where(w => w.TenantIds.Contains(LoggedUserName)).Select(d => d.DayId);

        StateHasChanged();
    }

    private static string AddDotsAndRemoveZeros(string nextDayStringRepr)
    {
        var nextDayStringReprToShowOnUi = nextDayStringRepr.Insert(4, ".").Insert(2, ".");

        if (nextDayStringReprToShowOnUi.Substring(3, 1) == "0")
            nextDayStringReprToShowOnUi = nextDayStringReprToShowOnUi.Remove(3, 1);

        if (nextDayStringReprToShowOnUi.Substring(0, 1) == "0")
            nextDayStringReprToShowOnUi = nextDayStringReprToShowOnUi.Remove(0, 1);

        return nextDayStringReprToShowOnUi;
    }

    private string GetTypeOfCalendarDay(int count, List<string> tenantsDays, string dayStringRepr)
    {
        var calendarButtontype = "btn-success";
        if (count >= 2)
        {
            if (tenantsDays.Contains(dayStringRepr))
            {
                calendarButtontype = "btn-warning";
            }
            else
                calendarButtontype = "btn-danger";
        }
        else
        {
            if (tenantsDays.Contains(dayStringRepr))
            {
                calendarButtontype = "btn-warning";
            }
            else
                calendarButtontype = "btn-success";
        }

        return calendarButtontype;
    }

    private async Task<List<TenantsForDay>> GetCalendarMap()
    {
        var days = new List<string>();

        for (var i = 0; i < 7; i++)
        {
            if (i >= DayOfWeekLocalNumber)
            {
                Today = DateTime.Today;
                Today = Today.AddDays(i - DayOfWeekLocalNumber);
                var nextDayStringReprCs = Today.ToString("ddMM");
                days.Add(nextDayStringReprCs);
            }
        }

        for (var i = 7; i < 21; i++)
        {
            Today = DateTime.Today;
            Today = Today.AddDays(i - DayOfWeekLocalNumber);
            var nextDayStringReprCs = Today.ToString("ddMM");
            days.Add(nextDayStringReprCs);
        }

        for (var i = 21; i < 42; i++)
        {
            if (i - DayOfWeekLocalNumber < NumberOfDaysToShow)
            {
                Today = DateTime.Today;
                Today = Today.AddDays(i - DayOfWeekLocalNumber);
                var nextDayStringReprCs = Today.ToString("ddMM");
                days.Add(nextDayStringReprCs);
            }
        }

        var multipleDaysForTenant = await TenantDayRepository.GetMultipleDaysForTenant(days);

        return multipleDaysForTenant;
    }

    async Task ReceiveLoop()
    {
        var buffer = new ArraySegment<byte>(new byte[1024]);
        while (!disposalTokenSource.IsCancellationRequested)
        {
            // Note that the received block might only be part of a larger message. If this applies in your scenario, check the received.EndOfMessage
            // and consider buffering the blocks until that property is true. Or use a higher-level library such as SignalR.
            var received = await _webSocket.ReceiveAsync(buffer, disposalTokenSource.Token);
            var receivedAsText = Encoding.UTF8.GetString(buffer.Array, 0, received.Count);

            WebSocketMessageDayChange webSocketMessage;
            try
            {
                webSocketMessage = JsonConvert.DeserializeObject<WebSocketMessageDayChange>(receivedAsText);
            }
            catch (JsonReaderException ex)
            {
                continue;
            }

            if (webSocketMessage.Message == WebSocketMessage.ParkingPlaceChange.ToString() &&
                webSocketMessage.TenantId != LoggedUserName)
            {
                TenantsDaysForUi = await TenantDayRepository.GetTenantDays(LoggedUserName);
                TenantsForDay = await GetCalendarMap();
                TenantsDayActualSelection =
                    TenantsForDay.Where(w => w.TenantIds.Contains(LoggedUserName)).Select(d => d.DayId);

                StateHasChanged();
            }
        }
    }

    public void Dispose()
    {
        Interceptor.DisposeEvent();

        disposalTokenSource.Cancel();
        _ = _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
    }
}