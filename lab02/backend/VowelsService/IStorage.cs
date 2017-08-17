namespace VowelsService
{
    public interface IStorage
    {
        void Save(string id, string value);
        string Get(string id);
    }
}
