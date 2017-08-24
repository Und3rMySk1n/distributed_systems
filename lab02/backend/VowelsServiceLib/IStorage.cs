namespace VowelsServiceLib
{
    public interface IStorage
    {
        void Save(string id, string value);
        void Delete(string id);
        string Get(string id);
    }
}
