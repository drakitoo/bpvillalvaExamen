using System.Globalization;

namespace bpvillalvaExamen.Views;

public partial class ResumenPage : ContentPage
{
    public ResumenPage(string usuario, string nombre, string apellido, int edad,
        DateTime fecha, string ciudad, string pais, decimal montoInicial,
        decimal pagoMensual, decimal pagoTotal)
    {
        InitializeComponent();
        UsuarioConectadoLabel.Text = $"Usuario conectado: {usuario}";
        NombreLabel.Text = nombre;
        ApellidoLabel.Text = apellido;
        EdadLabel.Text = edad.ToString(CultureInfo.CurrentCulture);
        FechaLabel.Text = fecha.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture);
        CiudadLabel.Text = ciudad;
        PaisLabel.Text = pais;
        MontoInicialLabel.Text = FormatearUsd(montoInicial);
        PagoMensualLabel.Text = FormatearUsd(pagoMensual);
        PagoTotalLabel.Text = FormatearUsd(pagoTotal);
    }

    private async void Volver_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private static string FormatearUsd(decimal value)
    {
        return $"${value.ToString("0.00", CultureInfo.InvariantCulture)}";
    }
}