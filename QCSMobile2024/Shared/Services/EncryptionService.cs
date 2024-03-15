namespace QCSMobile2024.Shared.Services
{
    public class EncryptionService
    {
        HttpClient _http;

        public EncryptionService(HttpClient http)
        {
            _http = http;    
        }

        public async Task<string> Decrypt(string stringToDecrypt)
        {
            //Log.Information($"Beginning to decrypt {stringToDecrypt}.");

            var response = await _http.GetAsync($"api/encryption/decrypt/{stringToDecrypt}");
            string result = await response.Content.ReadAsStringAsync();

            //Log.Information($"Decrypted {stringToDecrypt}, result {result}.");

            return result;
        }
    }
}
