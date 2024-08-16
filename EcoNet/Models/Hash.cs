using System.Security.Cryptography;
using System.Text;

namespace EcoNet.Models
{
    public class Hash
    {
        public Hash()
        {

        }
        public bool VerifySSHA256Hash(string password, string storedHash)
        {
            // Decodifica el hash almacenado desde base64
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            // Asumimos que los últimos 16 bytes son el salt
            int saltSize = 16;
            byte[] salt = new byte[saltSize];
            Array.Copy(hashBytes, hashBytes.Length - saltSize, salt, 0, saltSize);

            // Los primeros bytes son el hash original
            byte[] originalHash = new byte[hashBytes.Length - saltSize];
            Array.Copy(hashBytes, 0, originalHash, 0, originalHash.Length);

            // Hashea la contraseña ingresada por el usuario con el mismo salt
            using (var sha256 = SHA256.Create())
            {
                byte[] enteredHash = sha256.ComputeHash(Combine(Encoding.UTF8.GetBytes(password), salt));

                // Compara el hash generado con el hash almacenado
                return CompareHashes(originalHash, enteredHash);
            }
        }
        public string CreateSSHA256Hash(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] passwordWithSaltBytes = Combine(passwordBytes, salt);
                byte[] hashBytes = sha256.ComputeHash(passwordWithSaltBytes);
                byte[] hashWithSaltBytes = Combine(hashBytes, salt);

                // Devuelve el hash con el salt en formato base64
                return Convert.ToBase64String(hashWithSaltBytes);
            }
        }

        public byte[] GenerateSalt(int size = 16)
        {
            byte[] salt = new byte[size];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private byte[] Combine(byte[] first, byte[] second)
        {
            byte[] combined = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, combined, 0, first.Length);
            Buffer.BlockCopy(second, 0, combined, first.Length, second.Length);
            return combined;
        }


        private bool CompareHashes(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length != hash2.Length) return false;

            for (int i = 0; i < hash1.Length; i++)
            {
                if (hash1[i] != hash2[i]) return false;
            }

            return true;
        }
    }
}
