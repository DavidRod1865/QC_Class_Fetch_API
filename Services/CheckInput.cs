namespace QC_ClassFetch.Services
{
    public class CheckInput
    {
        var myCollection = new List<int> { 1, 2, 3 };
        private static readonly string[] validSemesters = ["Spring", "Summer 1", "Summer 2", "Fall", "Winter"];
        private static readonly string[] validDepartments = ["ADMIN", "MUSIC", "ACCT", "ACE", "AFST", "AMST", "ANTH", "ART", "BASS", "BIOL", "BALA", "GRKST", "CHEM", "CMAL", "CESL", "CMLIT", "CSCI", "CO-OP", "DRAM", "ECON", "ECP", "EECE", "ENGL", "ELL", "FNES", "LBSCI", "HLL", "HIST", "HNRS", "HTH", "HMNS", "HSS", "IRST", "ITAST", "JEWST", "JOURN", "LABST", "LBLST", "LIBR", "LCD", "MATH", "MEDST", "LEAP", "PHIL", "PHYS", "PSCI", "PSYCH", "RLGST", "SEEK", "SEES", "SEYS", "SOC", "SPST", "STPER", "URBST", "WOMST", "WLDST"];

        public static string CheckYear(ref string year)
        {

            if (year.Length == 4 && int.TryParse(year, out _))
            {
                return year;
            }

            Console.Write("Invalid year. Please enter a valid year (e.g., 2025): ");
            return string.Empty;
        }

        public static string CheckSemester(ref string semester)
        {

            Dictionary<string, string> semesterCodes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Spring", "02N" },
                { "Summer 1", "06N" },
                { "Summer 2", "06Y" },
                { "Fall", "09N" },
                { "Winter", "02Y" }
            };

            if (validSemesters.Contains(semester, StringComparer.OrdinalIgnoreCase))
            {
                return semesterCodes[semester];
            }

            Console.Write("Invalid semester. Please enter a valid semester (e.g., Spring, Summer 1, Summer 2, Fall, Winter): ");
            return string.Empty;
        }

        public static string CheckDepartment(ref string department)
        {
            department = department.ToUpper();

            if (validDepartments.Contains(department, StringComparer.OrdinalIgnoreCase))
            {
                return department;
            }

            Console.Write("Invalid department code. Please enter a valid department code.: ");
            return string.Empty;
        }
    }
}
