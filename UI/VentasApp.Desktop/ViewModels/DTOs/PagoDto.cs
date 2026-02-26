using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace VentasApp.Desktop.ViewModels.DTOs;

public partial class PagoDto : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private DateTime _fecha = DateTime.Today;

    [ObservableProperty]
    private decimal _monto;

    [ObservableProperty]
    private int _medioPagoId;

    [ObservableProperty]
    private string _medioPago = "";

    [ObservableProperty]
    private bool _verificado;

}
