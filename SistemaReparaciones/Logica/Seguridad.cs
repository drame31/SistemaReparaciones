using System.Security.Cryptography;
using System.Text;

namespace SistemaReparaciones.Logica
{
    public static class Seguridad
    {
        /// <summary>
        /// Convierte la contrasena a SHA-256 en hexadecimal mayusculas.
        /// Tiene que dar el mismo resultado que
        /// CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', ...), 2) en SQL Server,
        /// que es como se guardaron las contrasenas del script 03.
        /// </summary>
        public static string Cifrar(string texto)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(texto));

                StringBuilder resultado = new StringBuilder();
                foreach (byte b in bytes)
                {
                    resultado.Append(b.ToString("X2"));
                }

                return resultado.ToString();
            }
        }
    }
}
