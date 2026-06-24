namespace Domain.Entities
{
    public class Client
    {
        public string Nombre { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string EmailContacto { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;

        public decimal SumaFacturas { get; set; }

        public int NumeroFacturas { get; set; }
    }
}