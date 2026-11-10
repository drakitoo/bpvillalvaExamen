using System.Globalization;

namespace bpvillalvaExamen.Views;

public partial class RegistroPage : ContentPage
{
    private const decimal PrecioCurso = 1500m;
    private const int NumeroCuotas = 4;
    private readonly string usuario;

    private readonly Dictionary<string, string[]> ciudadesPorPais = new()
    {
        ["Ecuador"] = ["Quito", "Guayaquil", "Cuenca"],
        ["Colombia"] = ["Bogotá", "Medellín", "Cali"],
        ["Perú"] = ["Lima", "Arequipa", "Cusco"]
    };

    public RegistroPage(string usuario)
    {
        InitializeComponent();
        this.usuario = usuario;
        UsuarioConectadoLabel.Text = $"Usuario conectado: {usuario}";
        PaisPicker.ItemsSource = ciudadesPorPais.Keys.ToList();
        FechaPicker.Date = DateTime.Today;
    }

    private void PaisPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        CiudadPicker.ItemsSource = PaisPicker.SelectedItem is string pais
            ? ciudadesPorPais[pais]
            : null;
        CiudadPicker.SelectedItem = null;
    }

    private void MontoInicialEntry_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (TryParseDecimal(e.NewTextValue, out decimal monto) && monto <= PrecioCurso)
        {
            PagoMensualEntry.Text = FormatearUsd(CalcularPagoMensual(monto));
        }
        else
        {
            PagoMensualEntry.Text = string.Empty;
        }
    }

    private async void VerResumen_Clicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NombreEntry.Text) ||
            string.IsNullOrWhiteSpace(ApellidoEntry.Text) ||
            PaisPicker.SelectedItem is not string pais ||
            CiudadPicker.SelectedItem is not string ciudad ||
            !TryParseDecimal(MontoInicialEntry.Text, out decimal monto) ||
            monto < 0 || monto > PrecioCurso ||
            !int.TryParse(EdadEntry.Text, out int edad) || edad <= 0 || edad > 120)
        {
            await DisplayAlert("Datos incompletos", "Completa todos los campos con valores válidos.", "Aceptar");
            return;
        }

        decimal pagoMensual = CalcularPagoMensual(monto);
        decimal pagoTotal = monto + (pagoMensual * NumeroCuotas);
        await Navigation.PushAsync(new ResumenPage(
            usuario, NombreEntry.Text.Trim(), ApellidoEntry.Text.Trim(), edad,
            FechaPicker.Date ?? DateTime.Today, ciudad, pais, monto, pagoMensual, pagoTotal));
    }

    private static decimal CalcularPagoMensual(decimal montoInicial)
    {
        decimal saldo = PrecioCurso - montoInicial;
        decimal cuotaBase = saldo / NumeroCuotas;
        decimal recargo = cuotaBase * 0.04m;
        return cuotaBase + recargo;
    }

    private static string FormatearUsd(decimal value)
    {
        return $"${value.ToString("0.00", CultureInfo.InvariantCulture)}";
    }

    private static bool TryParseDecimal(string? text, out decimal value)
    {
        value = 0;
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        string normalized = text.Trim().Replace("$", string.Empty).Replace(" ", string.Empty);
        int lastComma = normalized.LastIndexOf(',');
        int lastDot = normalized.LastIndexOf('.');

        if (lastComma >= 0 && lastDot >= 0)
        {
            char decimalSeparator = lastComma > lastDot ? ',' : '.';
            char thousandsSeparator = decimalSeparator == ',' ? '.' : ',';
            normalized = normalized.Replace(thousandsSeparator.ToString(), string.Empty)
                                   .Replace(decimalSeparator, '.');
        }
        else if (lastComma >= 0)
        {
            normalized = normalized.Replace(',', '.');
        }

        return decimal.TryParse(normalized, NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture, out value);
    }
}