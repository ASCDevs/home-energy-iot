namespace home_energy_iot_core.Helpers.Interfaces
{
    public interface IHasher
    {
        string GenerateHash(string input, string salt);
        string CreateSalt(int saltSize);
    }
}
