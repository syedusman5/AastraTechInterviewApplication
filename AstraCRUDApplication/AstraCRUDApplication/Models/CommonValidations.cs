namespace AstraCRUDApplication.Models
{
    public class CommonValidations
    {
        public static bool ValidateAccess(string apiKey)
        {
            //TODO : Validate from appsettings
            return "AstraTechR4yp5vBKeHZAcRJa7DGG6LhH29ZJ6wJe" == apiKey;
        }
    }
}
