namespace major_data.Models
{
    public enum FileStatus : byte
    {
        Open,
        NaPodpis,
        Podpisan,
        ReOpen,
        Close,
        Bad,
        Soglasie,
        Otkaz,
        ErrorSoglasie
    }
}
