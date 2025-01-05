# QC Class Fetch API

🚀 **QC Class Fetch API** is a web-based application designed to streamline the process of retrieving course schedules for departments at Queens College. It provides an API endpoint to scrape the QC course schedule website, processes the data, and returns it as JSON for seamless integration with other applications.

---

## Features

- 🎓 **Department-Specific Schedule**: Fetches course schedules based on user-provided year, semester, and department.
- 📄 **JSON Output**: Returns structured course data in JSON format.
- 🖥️ **Headless Browser Automation**: Uses Selenium WebDriver in headless mode for efficient web scraping.
- 🔍 **Instructor Name Formatting**: Automatically reorders instructor names to "First Name Last Name" format for easier processing.

---

## API Endpoints

### `GET /api/Courses/GetCourses`

#### Query Parameters:
| Parameter    | Description                                  | Example           |
|--------------|----------------------------------------------|-------------------|
| `year`       | The academic year (e.g., 2025).              | `2025`            |
| `semester`   | The semester (`Spring`, `Summer 1`, etc.).   | `Spring`          |
| `department` | The department code (e.g., `CSCI`, `MATH`).  | `CSCI`            |

### Example Request:
```bash
curl -X 'GET' \
  'http://localhost:5004/api/Courses/GetCourses?year=2025&semester=Spring&department=CSCI' \
  -H 'accept: */*'
```

### Example Request:
```json
[
  {
    "Sec": "999A",
    "Code": "99999",
    "Course (hr, crd)": "CSCI 012 (4, 3)",
    "Description": "Intro Computers & Computation",
    "Day": "SU",
    "Time": "10:00 AM - 11:50 AM",
    "Instructor": "John Smith",
    "Location": "SB A999",
    "Enrolled": "14",
    "Limit": "39",
    "Mode of Instruction": "In-Person"
  }
]
```

### Build With:

- C# .NET 6
- Selenium WebDriver
- ASP.NET Core Web API
- Google Chrome & ChromeDriver

### Setup Instructions

Prerequisites
- .NET SDK: Install the latest version from dotnet.microsoft.com.
- Google Chrome: Ensure Google Chrome is installed.
- ChromeDriver: Install the compatible version of ChromeDriver.

#### Installation

1. Clone the repository:
```bash 
git clone https://github.com/DavidRod1865/QC_Class_Fetch_API.git
cd QC_ClassFetch
```

2. Install dependencies:
```bash
dotnet add package DotNetSeleniumExtras.WaitHelpers
dotnet add package Selenium.Support
dotnet add package Selenium.WebDriver
dotnet add package Selenium.WebDriver.ChromeDriver
```

3. Build the project:
```bash
dotnet build
```

4. Run the API:
```bash
dotnet run --project QC_FetchAPI
```

5. Access Swagger for API documentation at:
```bash
http://localhost:5004/swagger
```

### Project Structure
```bash
QC-Class-Fetch/
├── QC_FetchAPI/
│   ├── Program.cs                 # API configuration
│   ├── Controllers/
│   │   └── CoursesController.cs   # API endpoint logic
│   ├── Services/
│   │   ├── FetchCourses.cs        # Web scraping logic
│   │   └── CheckInput.cs          # Input validation
└── README.md                      # Project documentation
```

### Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are greatly appreciated.

1. Fork the Project
2. Create your Feature Branch (git checkout -b feature/AmazingFeature)
3. Commit your Changes (git commit -m 'Add some AmazingFeature')
4. Push to the Branch (git push origin feature/AmazingFeature)
5. Open a Pull Request

### License

Distributed under the MIT License.

### Contact

- David Rodriguez - [LinkedIn](d.rodriguez.1865@gmail.com)
- Project Link: https://github.com/DavidRod1865/QC_ClassFetch

### Acknowledgments

🏫 Queens College for the course schedule portal
🛠️ Selenium WebDriver for powerful web automation

### 🌟 Star this repository if you find it helpful! 🌟