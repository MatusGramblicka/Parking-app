#pragma checksum "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\App.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0b3d20c65b464c4ab698e5b7614eeb61255fe0dd"
// <auto-generated/>
#pragma warning disable 1591
namespace BlazorProducts.Client
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\_Imports.razor"
using System.Net.Http.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\_Imports.razor"
using Microsoft.AspNetCore.Components.WebAssembly.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\_Imports.razor"
using BlazorProducts.Client;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\_Imports.razor"
using BlazorProducts.Client.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\_Imports.razor"
using Blazored.Toast;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\_Imports.razor"
using Blazored.Toast.Services;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\_Imports.razor"
using Blazored.Toast.Configuration;

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\_Imports.razor"
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

#line default
#line hidden
#nullable disable
#nullable restore
#line 16 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\App.razor"
using BlazorProducts.Client.Pages;

#line default
#line hidden
#nullable disable
    public partial class App : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenComponent<Microsoft.AspNetCore.Components.Authorization.CascadingAuthenticationState>(0);
            __builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Routing.Router>(2);
                __builder2.AddAttribute(3, "AppAssembly", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Reflection.Assembly>(
#nullable restore
#line 4 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\App.razor"
                          typeof(Program).Assembly

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(4, "PreferExactMatches", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Boolean>(
#nullable restore
#line 4 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\App.razor"
                                                                         true

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(5, "Found", (Microsoft.AspNetCore.Components.RenderFragment<Microsoft.AspNetCore.Components.RouteData>)((routeData) => (__builder3) => {
                    __builder3.OpenComponent<Microsoft.AspNetCore.Components.Authorization.AuthorizeRouteView>(6);
                    __builder3.AddAttribute(7, "RouteData", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.RouteData>(
#nullable restore
#line 6 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\App.razor"
                                            routeData

#line default
#line hidden
#nullable disable
                    ));
                    __builder3.AddAttribute(8, "DefaultLayout", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Type>(
#nullable restore
#line 7 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\App.razor"
                                                typeof(MainLayout)

#line default
#line hidden
#nullable disable
                    ));
                    __builder3.AddAttribute(9, "Authorizing", (Microsoft.AspNetCore.Components.RenderFragment)((__builder4) => {
                        __builder4.AddMarkupContent(10, "<text>Please wait, we are authorizing the user.</text>");
                    }
                    ));
                    __builder3.CloseComponent();
                }
                ));
                __builder2.AddAttribute(11, "NotFound", (Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                    __builder3.OpenComponent<Microsoft.AspNetCore.Components.LayoutView>(12);
                    __builder3.AddAttribute(13, "Layout", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Type>(
#nullable restore
#line 14 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\App.razor"
                                 typeof(MainLayout)

#line default
#line hidden
#nullable disable
                    ));
                    __builder3.AddAttribute(14, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder4) => {
                        __builder4.OpenComponent<BlazorProducts.Client.Pages.CustomNotFound>(15);
                        __builder4.CloseComponent();
                    }
                    ));
                    __builder3.CloseComponent();
                }
                ));
                __builder2.CloseComponent();
            }
            ));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
