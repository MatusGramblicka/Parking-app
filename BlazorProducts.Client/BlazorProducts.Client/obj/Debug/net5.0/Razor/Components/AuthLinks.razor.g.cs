#pragma checksum "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\Components\AuthLinks.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b3a28f95effa3772caff817ef2a218fa721fbc09"
// <auto-generated/>
#pragma warning disable 1591
namespace BlazorProducts.Client.Components
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
    public partial class AuthLinks : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenComponent<Microsoft.AspNetCore.Components.Authorization.AuthorizeView>(0);
            __builder.AddAttribute(1, "Authorized", (Microsoft.AspNetCore.Components.RenderFragment<Microsoft.AspNetCore.Components.Authorization.AuthenticationState>)((context) => (__builder2) => {
                __builder2.AddMarkupContent(2, "\r\n        Hello, ");
                __builder2.AddContent(3, 
#nullable restore
#line 3 "F:\ParkingApp2\BlazorProducts.Client\BlazorProducts.Client\Components\AuthLinks.razor"
                context.User.Identity.Name

#line default
#line hidden
#nullable disable
                );
                __builder2.AddMarkupContent(4, "!\r\n        ");
                __builder2.AddMarkupContent(5, "<a href=\"Logout\">Log out</a>");
            }
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(6, "\r\n");
            __builder.OpenComponent<Microsoft.AspNetCore.Components.Authorization.AuthorizeView>(7);
            __builder.AddAttribute(8, "Roles", "Administrator");
            __builder.AddAttribute(9, "Authorized", (Microsoft.AspNetCore.Components.RenderFragment<Microsoft.AspNetCore.Components.Authorization.AuthenticationState>)((context) => (__builder2) => {
                __builder2.AddMarkupContent(10, "<a href=\"Register\">Register</a>");
            }
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(11, "\r\n");
            __builder.OpenComponent<Microsoft.AspNetCore.Components.Authorization.AuthorizeView>(12);
            __builder.AddAttribute(13, "NotAuthorized", (Microsoft.AspNetCore.Components.RenderFragment<Microsoft.AspNetCore.Components.Authorization.AuthenticationState>)((context) => (__builder2) => {
                __builder2.AddMarkupContent(14, "<a href=\"Login\">Log in</a>");
            }
            ));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
