using Blazored.LocalStorage;
using BlazorProducts.Client.Features;
using BlazorProducts.Client.HttpInterceptor;
using BlazorProducts.Client.HttpRepository;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
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

        const int nuberOfDaysToShow = 31;

        protected async override Task OnInitializedAsync()
        {
            Interceptor.RegisterEvent();
            Interceptor.RegisterBeforeSendEvent();            

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
        }        

        private async Task BookOrFreeDay(string day)
        {
            var tenantsDays = await TenantDayRepository.GetTenantDays(LoggedUserName);
            
            if (tenantsDays.Contains(day))
                await TenantDayRepository.FreeDay(new TenantDay { DayId = day.ToString(), TenantId = LoggedUserName });
            else
                await TenantDayRepository.BookDay(new TenantDay { DayId = day.ToString(), TenantId = LoggedUserName });

            tenantsDaysForUI = await TenantDayRepository.GetTenantDays(LoggedUserName);

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
            var calendarButtontype = "btn-info";
            if (count >= 2)
            {
                if (tenantsDays.Contains(dayStringRepr))
                {
                    calendarButtontype = "btn-outline-info";
                }
                else
                    calendarButtontype = "btn-outline-danger";
            }
            else
            {
                if (tenantsDays.Contains(dayStringRepr))
                {
                    calendarButtontype = "btn-outline-info";
                }
                else
                    calendarButtontype = "btn-info";
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

        public void Dispose() => Interceptor.DisposeEvent();
    }
}
