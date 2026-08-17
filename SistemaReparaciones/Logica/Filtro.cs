namespace SistemaReparaciones.Logica
{
    /// <summary>
    /// Prepara el texto que el usuario escribio en el buscador antes de
    /// mandarselo al procedimiento almacenado. Esta aparte porque las seis
    /// pantallas de mantenimiento hacen exactamente lo mismo.
    /// </summary>
    public static class Filtro
    {
        public static string Preparar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                // null le dice al procedimiento que traiga la lista completa
                return null;
            }

            // Los comodines del LIKE se encierran entre corchetes para que
            // se busquen como texto normal. Si alguien escribe "100%" tiene
            // que buscar eso y no cualquier cosa que empiece con 100.
            return texto.Trim()
                        .Replace("[", "[[]")
                        .Replace("%", "[%]")
                        .Replace("_", "[_]");
        }
    }
}
