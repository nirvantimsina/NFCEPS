using Microsoft.AspNetCore.Components;
using MudBlazor;
using NFCEPS.UI.Components.Dropdown.Managers.Interface;
using NFCEPS.UI.Components.Dropdown.Models.RequestModel;
using NFCEPS.UI.Shared.Security;

namespace NFCEPS.UI.Components.Dropdown.Pages;

public partial class FlagDropdown<TValue>(IFlagDropdownManager dropdownManager, ISnackbar snackbar) : PermissionAwareBase
{
    CascadingTypeParameterAttribute TId;
    protected DropdownRequestModel request = new();
    protected string? error;
    protected bool IsLoading;

    [Parameter] public string Flag { get; set; } = string.Empty;
    [Parameter] public string Label { get; set; } = "Select Item";

    [Parameter] public TValue? Value { get; set; }
    [Parameter] public EventCallback<TValue> ValueChanged { get; set; }

    [Parameter] public bool ShowDefaultOption { get; set; } = true;
    [Parameter] public string DefaultOptionText { get; set; } = "-- Select --";

    protected List<UISelectItem> items = new();

    protected override async Task OnParametersSetAsync()
    {
        if (!string.IsNullOrEmpty(Flag))
        {
            try
            {
                IsLoading = true;

                request.Flag = Flag;

                var result = await dropdownManager.DropdownListAsync(request);

                if (result?.Success != true)
                {
                    error = result?.Message ?? $"Error fetching dropdown items for {request.Flag}";
                    snackbar.Add(error, Severity.Error);
                    items = new();
                    return;
                }
                items = result.Data.Select(d => new UISelectItem
                {
                    Value = d.Value ?? string.Empty,
                    Text = d.Text ?? string.Empty
                }).ToList();

            }
            catch (Exception ex)
            {
                snackbar.Add(ex.Message, Severity.Error);
                items = new();
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    private async Task OnValueChanged(string stringId)
    {
        TValue parsedValue = ParseValue(stringId);
        Value = parsedValue;
        await ValueChanged.InvokeAsync(Value);
    }

    private TValue ParseValue(string id)
    {
        if (string.IsNullOrEmpty(id)) return default!;

        if (typeof(TValue) == typeof(int) && int.TryParse(id, out var intVal))
            return (TValue)(object)intVal;

        if (typeof(TValue) == typeof(Guid) && Guid.TryParse(id, out var guidVal))
            return (TValue)(object)guidVal;

        return (TValue)(object)id;
    }

    public class UISelectItem
    {
        public string Value { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}
