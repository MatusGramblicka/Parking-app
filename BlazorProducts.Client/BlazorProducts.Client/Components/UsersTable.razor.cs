using BlazorProducts.Client.Shared;
using Entities.DTO;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorProducts.Client.Components;

public partial class UsersTable
{
    [Parameter] public List<UserLite> Users { get; set; }
    [Parameter] public string LoggedUser { get; set; }
    [Parameter] public EventCallback<UserLite> OnDeleteUser { get; set; }

    [Parameter] public EventCallback<UserLite> OnBookAllDaysForTenant { get; set; }

    private Confirmation _confirmation;
    private UserLite _userToDelete;

    private async Task OnChange(bool? value, UserLite user)
    {
        user.Priviledged = (bool) value;

        await OnBookAllDaysForTenant.InvokeAsync(user);
    }

    private void CallConfirmationModal(UserLite userToDelete)
    {
        _userToDelete = userToDelete;
        _confirmation.Show();
    }

    private async Task DeleteUser()
    {
        _confirmation.Hide();
        await OnDeleteUser.InvokeAsync(_userToDelete);
    }
}