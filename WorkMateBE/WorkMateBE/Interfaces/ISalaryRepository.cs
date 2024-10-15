namespace WorkMateBE.Interfaces
{
    public interface ISalaryRepository
    {
        bool CreateSalarySheet(int employeeId, int month, int year);
    }
}
