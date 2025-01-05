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

            // Loop through the table data and create a JSON object for each row
            foreach (var row in tableData)
            {
                var jsonRow = new Dictionary<string, string>();
                for (int i = 0; i < headers.Count; i++)
                {
                    // Add the cell value to the JSON object
                    jsonRow[headers[i]] = row.Length > i ? row[i] : string.Empty;
                }
                jsonData.Add(jsonRow);
            }

            // Serialize the JSON data with indentation
            return JsonSerializer.Serialize(jsonData, new JsonSerializerOptions { WriteIndented = true });
        }

        // Fetch course data from the QC course schedule website
        public static string FetchCoursesData(string year, string semester, string department)
        {
            // Set up ChromeOptions for headless mode
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--window-size=1920,1080");

            // Initialize the WebDriver with headless options
            IWebDriver driver = new ChromeDriver(options);

            try
            {
                // Navigate to the webpage
                driver.Navigate().GoToUrl("https://apps.qc.cuny.edu/Courses/Default.aspx");

                // Maximize the browser window (virtual in headless mode)
                driver.Manage().Window.Maximize();

                // Wait for the page to load
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                // Click on the Course Schedule tab
                IWebElement tabElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("__tab_MainContent_tcMainSearch_tbCourseSchd")));
                tabElement.Click();

                // Select the year
                IWebElement yearDropdown = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("MainContent_tcMainSearch_tbCourseSchd_ddlTermYear")));
                var selectYear = new SelectElement(yearDropdown);
                selectYear.SelectByValue(year);

                // Select the semester
                IWebElement semesterDropdown = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("MainContent_tcMainSearch_tbCourseSchd_ddlSemester")));
                var selectSemester = new SelectElement(semesterDropdown);
                selectSemester.SelectByValue(semester);

                // Select the department
                IWebElement departmentDropdown = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("MainContent_tcMainSearch_tbCourseSchd_ddlDeptList")));
                var selectDepartment = new SelectElement(departmentDropdown);
                selectDepartment.SelectByValue(department);

                // Click the submit button
                IWebElement submitButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("ctl00$MainContent$tcMainSearch$tbCourseSchd$btnBringSchedule")));
                submitButton.Click();

                // Wait for the table to load
                wait.Until(ExpectedConditions.ElementExists(By.Id("gvCourseSchd")));
                IWebElement table = driver.FindElement(By.Id("gvCourseSchd"));

                // Get the rows from the table
                IList<IWebElement> rows = table.FindElements(By.TagName("tr"));
                List<string[]> tableData = new List<string[]>();
                List<string> headers = new List<string>();

                // Loop through the rows and cells to extract the data
                for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
                {
                    IList<IWebElement> cells = rows[rowIndex].FindElements(By.TagName(rowIndex == 0 ? "th" : "td"));

                    string[] rowData = new string[cells.Count];
                    for (int i = 0; i < cells.Count; i++)
                    {
                        rowData[i] = cells[i].Text.Trim();
                    }

                    if (rowIndex == 0)
                    {
                        headers.AddRange(rowData);
                    }
                    else
                    {
                        // Reorder instructor name to First Name, Last Name format
                        if (rowData.Length > 6)
                        {
                            string instructorName = rowData[6];
                            if (!string.IsNullOrEmpty(instructorName) && instructorName.Contains(","))
                            {
                                var nameParts = instructorName.Split(',');
                                if (nameParts.Length == 2)
                                {
                                    string lastName = nameParts[0].Trim();
                                    string firstName = nameParts[1].Trim();
                                    rowData[6] = $"{firstName} {lastName}";
                                }
                            }
                        }

                        tableData.Add(rowData);
                    }
                }

                // Convert the table data to JSON
                return ConvertToJson(tableData, headers);
            }
            // Catch any exceptions and return an empty string
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return string.Empty;
            }
            // Close the browser window and quit the driver
            finally
            {
                Thread.Sleep(500);
                driver.Quit();
            }
        }
    }
}
