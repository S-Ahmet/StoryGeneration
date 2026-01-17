using Microsoft.Extensions.Configuration;

namespace GNA.API.AppSettings
{
    public class Settings
    {
        // Şimdilik boş. İleride Story/Model ayarları eklenecek.

        public static Settings LoadFromConfiguration(IConfiguration configuration)
        {
            return new Settings();
        }
    }
}
