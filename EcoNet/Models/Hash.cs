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
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(storedHash))
            {
                throw new ArgumentNullException(nameof(storedHash), "Stored hash cannot be null or empty.");
            }

            try
            {
                // Decodifica el hash almacenado desde base64
                byte[] hashBytes = Convert.FromBase64String(storedHash);

                if (hashBytes.Length < 16)
                {
                    throw new InvalidOperationException("Hash bytes length is less than the expected salt size.");
                }

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
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                    byte[] enteredHash = sha256.ComputeHash(Combine(passwordBytes, salt));

                    // Compara el hash generado con el hash almacenado
                    return CompareHashes(originalHash, enteredHash);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en VerifySSHA256Hash: {ex.Message}");
                throw; // Rethrow exception after logging
            }
        }

        private static byte[] Combine(byte[] passwordBytes, byte[] salt)
        {
            byte[] combined = new byte[passwordBytes.Length + salt.Length];
            Array.Copy(passwordBytes, 0, combined, 0, passwordBytes.Length);
            Array.Copy(salt, 0, combined, passwordBytes.Length, salt.Length);
            return combined;
        }

        private static bool CompareHashes(byte[] originalHash, byte[] enteredHash)
        {
            if (originalHash.Length != enteredHash.Length)
            {
                return false;
            }

            for (int i = 0; i < originalHash.Length; i++)
            {
                if (originalHash[i] != enteredHash[i])
                {
                    return false;
                }
            }

            return true;
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
       


    }
}
