using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Text.Json;

namespace QC_FetchAPI.Services
{
    public class FetchCourses
    {
        // Convert the table data to JSON
        private static string ConvertToJson(List<string[]> tableData, List<string> headers)
        {
            var jsonData = new List<Dictionary<string, string>>();

            foreach (var row in tableData)
            {
                var jsonRow = new Dictionary<string, string>();
                for (int i = 0; i < headers.Count; i++)
                {
                    jsonRow[headers[i]] = row.Length > i ? row[i] : string.Empty;
                }
                jsonData.Add(jsonRow);
            }

            return JsonSerializer.Serialize(jsonData, new JsonSerializerOptions { WriteIndented = true });
        }

        // Fetch course data from the QC course schedule website
        public static string FetchCoursesData(string year, string semester, string department)
        {
            // Set up ChromeOptions for headless mode
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--remote-allow-origins=*");

            // Initialize the WebDriver
            using (IWebDriver driver = new ChromeDriver(options))
            {
                try
                {
                    driver.Navigate().GoToUrl("https://apps.qc.cuny.edu/Courses/Default.aspx");

                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                    // Click on the Course Schedule tab
                    IWebElement tabElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("__tab_MainContent_tcMainSearch_tbCourseSchd")));
                    tabElement.Click();

                    // Select the year
                    IWebElement yearDropdown = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("MainContent_tcMainSearch_tbCourseSchd_ddlTermYear")));
                    new SelectElement(yearDropdown).SelectByValue(year);

                    // Select the semester
                    IWebElement semesterDropdown = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("MainContent_tcMainSearch_tbCourseSchd_ddlSemester")));
                    new SelectElement(semesterDropdown).SelectByValue(semester);

                    // Select the department
                    IWebElement departmentDropdown = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("MainContent_tcMainSearch_tbCourseSchd_ddlDeptList")));
                    new SelectElement(departmentDropdown).SelectByValue(department);

                    // Click the submit button
                    IWebElement submitButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("ctl00$MainContent$tcMainSearch$tbCourseSchd$btnBringSchedule")));
                    submitButton.Click();

                    // Wait for the table to load
                    wait.Until(ExpectedConditions.ElementExists(By.Id("gvCourseSchd")));
                    IWebElement table = driver.FindElement(By.Id("gvCourseSchd"));

                    IList<IWebElement> rows = table.FindElements(By.TagName("tr"));
                    List<string[]> tableData = new List<string[]>();
                    List<string> headers = new List<string>();

                    for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
                    {
                        IList<IWebElement> cells = rows[rowIndex].FindElements(By.TagName(rowIndex == 0 ? "th" : "td"));
                        string[] rowData = cells.Select(cell => cell.Text.Trim()).ToArray();

                        if (rowIndex == 0)
                        {
                            headers.AddRange(rowData);
                        }
                        else
                        {
                            if (rowData.Length > 6 && rowData[6].Contains(","))
                            {
                                var nameParts = rowData[6].Split(',');
                                if (nameParts.Length == 2)
                                {
                                    rowData[6] = $"{nameParts[1].Trim()} {nameParts[0].Trim()}";
                                }
                            }

                            tableData.Add(rowData);
                        }
                    }

                    return ConvertToJson(tableData, headers);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return JsonSerializer.Serialize(new { error = ex.Message }, new JsonSerializerOptions { WriteIndented = true });
                }
            }
        }
    }
}
