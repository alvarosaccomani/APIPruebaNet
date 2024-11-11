namespace APIPruebaNet.Models
{
    public class Equipo
    {
        public int eq_Id { get; set; }
        public string eq_Nombre { get; set; }
        public double eq_Precio { get; set; }
        public string eq_Documentacion { get; set; }
        public byte[] eq_Contenido { get; set; }
    }
}
