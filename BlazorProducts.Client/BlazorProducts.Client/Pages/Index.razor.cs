using Blazored.LocalStorage;
using BlazorProducts.Client.Features;
using BlazorProducts.Client.HttpInterceptor;
using BlazorProducts.Client.HttpRepository;
using Entities.DataTransferObjects;
using Entities.Enums;
using Entities.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorProducts.Client.Pages
{
    public partial class Index
    {
        [Inject]
        public ITenantDayRepo TenantDayRepository { get; set; }
        [Inject]
        public HttpInterceptorService Interceptor { get; set; }
        [Inject]
        public ILocalStorageService LocalStorageService { get; set; } 

        public string LoggedUserName { get; set; }
        public DayOfWeekLocal dayOfWeekLocal { get; set; }
        public int dayOfWeekLocalNumber { get; set; }
        public DateTime today { get; set; }
        public List<string> tenantsDaysForUI { get; set; } = new List<string>();
        public List<string> tenantsInDay { get; set; } = new List<string>();
        public List<List<TenantsForDay>> tenantsForDay { get; set; } = new List<List<TenantsForDay>>();
        public List<string> tenantsDayActualSelection { get; set; } = new List<string>();

        const int nuberOfDaysToShow = 31;

        //https://gist.github.com/SteveSandersonMS/5aaff6b010b0785075b0a08cc1e40e01
        public CancellationTokenSource disposalTokenSource = new CancellationTokenSource();
        public ClientWebSocket webSocket = new ClientWebSocket();
        public string message = "Hello, websocket!";
        public string log = "";

        protected async override Task OnInitializedAsync()
        {
            Interceptor.RegisterEvent();
            Interceptor.RegisterBeforeSendEvent();

            await webSocket.ConnectAsync(new Uri("wss://localhost:5011/socket"), disposalTokenSource.Token);
            _ = ReceiveLoop();

            today = DateTime.Today;

            var currentday = DateTime.Now.DayOfWeek.ToString();
            Enum.TryParse(currentday, out DayOfWeekLocal dayOfWeekLocal);
            dayOfWeekLocalNumber = (int)dayOfWeekLocal;            

            var token = await LocalStorageService.GetItemAsync<string>("authToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                LoggedUserName = JwtParser.GetLoggedUserName(token);
            }
           
            tenantsDaysForUI = await TenantDayRepository.GetTenantDays(LoggedUserName);

            tenantsForDay = await GetCalendarMap();

            tenantsDayActualSelection = tenantsForDay.SelectMany(s => s).Where(w => w.TenantId.Contains(LoggedUserName)).Select(d => d.DayId).ToList();
        }        

        private async Task BookOrFreeDay(string day)
        {
            var tenantsDays = await TenantDayRepository.GetTenantDays(LoggedUserName);           
            
            if (tenantsDays.Contains(day))
            { 
                await TenantDayRepository.FreeDay(new TenantDay { DayId = day.ToString(), TenantId = LoggedUserName });
                tenantsForDay.SelectMany(s => s).Where(w => w.DayId == day).Single().TenantId.Remove(LoggedUserName);
            }
            else
            {
                var tenantsForConcreteDay = await TenantDayRepository.GetDaysForTenant(day);
                if (tenantsForConcreteDay.Count < 2)
                { 
                    await TenantDayRepository.BookDay(new TenantDay { DayId = day.ToString(), TenantId = LoggedUserName });
                    tenantsForDay.SelectMany(s => s).Where(w => w.DayId == day).Single().TenantId.Add(LoggedUserName);
                }
                else
                    tenantsForDay = await GetCalendarMap(); // if simultaneous 2 user booked the same day, one of them cant booked and then UI must be reload with updated days
            }

            tenantsDaysForUI = await TenantDayRepository.GetTenantDays(LoggedUserName);
            tenantsDayActualSelection = tenantsForDay.SelectMany(s => s).Where(w => w.TenantId.Contains(LoggedUserName)).Select(d => d.DayId).ToList();

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

        private async Task<List<List<TenantsForDay>>> GetCalendarMap()
        {
            List<List<TenantsForDay>> completeTenantsForDay = new List<List<TenantsForDay>>();
            List<TenantsForDay> tenantsForDay1 = new List<TenantsForDay>();
            List<TenantsForDay> tenantsForDay2 = new List<TenantsForDay>();
            List<TenantsForDay> tenantsForDay3 = new List<TenantsForDay>();
            List<TenantsForDay> tenantsForDay4 = new List<TenantsForDay>();
            List<TenantsForDay> tenantsForDay5 = new List<TenantsForDay>();
            List<TenantsForDay> tenantsForDay6 = new List<TenantsForDay>();
            // 1. line
            for (int i = 0; i < 7; i++)
            {
                if (i >= dayOfWeekLocalNumber)
                {
                    today = DateTime.Today;
                    today = today.AddDays(i - dayOfWeekLocalNumber);
                    var nextDayStringReprCs = today.ToString("ddMM");
                    tenantsInDay = await TenantDayRepository.GetDaysForTenant(nextDayStringReprCs);
                    tenantsForDay1.Add(new TenantsForDay
                    {
                        DayId = nextDayStringReprCs,
                        TenantId = tenantsInDay
                    });
                }
            }
            completeTenantsForDay.Add(tenantsForDay1);
            
            // 2. line
            for (int i = 7; i < 14; i++)
            {                
                today = DateTime.Today;
                today = today.AddDays(i - dayOfWeekLocalNumber);
                var nextDayStringReprCs = today.ToString("ddMM");
                tenantsInDay = await TenantDayRepository.GetDaysForTenant(nextDayStringReprCs);
                tenantsForDay2.Add(new TenantsForDay
                {
                    DayId = nextDayStringReprCs,
                    TenantId = tenantsInDay
                });                
            }
            completeTenantsForDay.Add(tenantsForDay2);
            
            // 3. line
            for (int i = 14; i < 21; i++)
            {
                today = DateTime.Today;
                today = today.AddDays(i - dayOfWeekLocalNumber);
                var nextDayStringReprCs = today.ToString("ddMM");
                tenantsInDay = await TenantDayRepository.GetDaysForTenant(nextDayStringReprCs);
                tenantsForDay3.Add(new TenantsForDay
                {
                    DayId = nextDayStringReprCs,
                    TenantId = tenantsInDay
                });
            }
            completeTenantsForDay.Add(tenantsForDay3);
            
            // 4. day
            for (int i = 21; i < 28; i++)
            {
                if (i - dayOfWeekLocalNumber < nuberOfDaysToShow)
                {
                    today = DateTime.Today;
                    today = today.AddDays(i - dayOfWeekLocalNumber);
                    var nextDayStringReprCs = today.ToString("ddMM");
                    tenantsInDay = await TenantDayRepository.GetDaysForTenant(nextDayStringReprCs);
                    tenantsForDay4.Add(new TenantsForDay
                    {
                        DayId = nextDayStringReprCs,
                        TenantId = tenantsInDay
                    });
                }
            }
            completeTenantsForDay.Add(tenantsForDay4);
            
            // 5. line
            for (int i = 28; i < 35; i++)
            {
                if (i - dayOfWeekLocalNumber < nuberOfDaysToShow)
                {
                    today = DateTime.Today;
                    today = today.AddDays(i - dayOfWeekLocalNumber);
                    var nextDayStringReprCs = today.ToString("ddMM");
                    tenantsInDay = await TenantDayRepository.GetDaysForTenant(nextDayStringReprCs);
                    tenantsForDay5.Add(new TenantsForDay
                    {
                        DayId = nextDayStringReprCs,
                        TenantId = tenantsInDay
                    });
                }
            }
            completeTenantsForDay.Add(tenantsForDay5);
            
            // 6. line
            for (int i = 35; i < 42 - dayOfWeekLocalNumber + 1; i++)
            {
                if (i - dayOfWeekLocalNumber < nuberOfDaysToShow)
                {
                    today = DateTime.Today;
                    today = today.AddDays(i - dayOfWeekLocalNumber);
                    var nextDayStringReprCs = today.ToString("ddMM");
                    tenantsInDay = await TenantDayRepository.GetDaysForTenant(nextDayStringReprCs);
                    tenantsForDay6.Add(new TenantsForDay
                    {
                        DayId = nextDayStringReprCs,
                        TenantId = tenantsInDay
                    });
                }
            }
            completeTenantsForDay.Add(tenantsForDay6);

            return completeTenantsForDay;
        }

        async Task ReceiveLoop()
        {
            var buffer = new ArraySegment<byte>(new byte[1024]);
            while (!disposalTokenSource.IsCancellationRequested)
            {
                // Note that the received block might only be part of a larger message. If this applies in your scenario,
                // check the received.EndOfMessage and consider buffering the blocks until that property is true.
                // Or use a higher-level library such as SignalR.
                var received = await webSocket.ReceiveAsync(buffer, disposalTokenSource.Token);
                var receivedAsText = Encoding.UTF8.GetString(buffer.Array, 0, received.Count);

                // todo check message           
                if (receivedAsText == WebSocketMessage.ParkingPlaceChange.ToString())
                { 
                    tenantsDaysForUI = await TenantDayRepository.GetTenantDays(LoggedUserName);
                    tenantsForDay = await GetCalendarMap();
                    tenantsDayActualSelection = tenantsForDay.SelectMany(s => s).Where(w => w.TenantId.Contains(LoggedUserName)).Select(d => d.DayId).ToList();

                    StateHasChanged();
                }
            }
        }

        public void Dispose()
        { 
            Interceptor.DisposeEvent();

            disposalTokenSource.Cancel();
            _ = webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bye", CancellationToken.None);
        }        
    }
}
