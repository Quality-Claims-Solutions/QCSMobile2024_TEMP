using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System.Text;

namespace QCSMobile2024.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncryptionController : ControllerBase
    {
        Byte[] IV = System.Text.ASCIIEncoding.ASCII.GetBytes("B320B7CB59814aca8392775B67CD8BE8");
        string key = "DDBDF52E79BB415290726F205C986062";

        [HttpGet("decrypt/{stringToDecrypt}")]
        public async Task<ActionResult> Decrypt(string stringToDecrypt)
        {
            // Replace characters to remove URL safety.
            stringToDecrypt = stringToDecrypt.Replace('-', '+').Replace('_', '/');
            byte[] cipherTextBytes = Convert.FromBase64String(stringToDecrypt);
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);

            IBufferedCipher cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new RijndaelEngine(256)));
            KeyParameter keyParam = new KeyParameter(keyBytes);
            ParametersWithIV keyParamWithIV = new ParametersWithIV(keyParam, IV, 0, 32);
            cipher.Init(false, keyParamWithIV);

            byte[] comparisonBytes = new byte[cipher.GetOutputSize(cipherTextBytes.Length)];
            int length = cipher.ProcessBytes(cipherTextBytes, 0, cipherTextBytes.Length, comparisonBytes, 0);
            cipher.DoFinal(comparisonBytes, length);

            return Ok(Encoding.ASCII.GetString(comparisonBytes).TrimEnd('\0'));
        }

        [HttpGet("encrypt")]
        public async Task<ActionResult> Encrypt(string stringToEncrypt)
        {
            var keyBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
            var engine = new RijndaelEngine(256);
            var blockCipher = new CbcBlockCipher(engine);
            var cipher = new PaddedBufferedBlockCipher(blockCipher, new Pkcs7Padding());
            var keyParam = new KeyParameter(keyBytes);
            var keyParamWithIV = new ParametersWithIV(keyParam, IV, 0, 32);

            cipher.Init(true, keyParamWithIV);
            var cipherTextBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(stringToEncrypt);
            var comparisonBytes = new byte[cipher.GetOutputSize(cipherTextBytes.Length)];
            var length = cipher.ProcessBytes(cipherTextBytes, comparisonBytes, 0);
            cipher.DoFinal(comparisonBytes, length);

            // Replace characters to make string URL safe.
            var base64UrlSafeString = Convert.ToBase64String(comparisonBytes)
                                  .Replace('+', '-')
                                  .Replace('/', '_');

            return Ok(base64UrlSafeString);
        }
    }
}
