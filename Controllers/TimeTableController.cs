using DynamicTimeTableGenerator.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamicTimeTableGenerator.Controllers
{
    public class TimeTableController : Controller
    {
        private static int TotalHoursForWeek;
        private static List<SubjectAllocationModel> SubjectAllocations = new List<SubjectAllocationModel>();

      
        public IActionResult Index()
        {
            return View(new UserInputModel());
        }

        [HttpPost]
        public IActionResult Index(UserInputModel model)
        {
            if (ModelState.IsValid)
            {
                TotalHoursForWeek = model.WorkingDays * model.SubjectsPerDay;
                TempData["TotalHoursForWeek"] = TotalHoursForWeek;
                TempData["TotalSubjects"] = model.TotalSubjects;

                return RedirectToAction("AllocateSubjects");
            }

            TempData["ErrorMessage"] = "Please fill all fields correctly.";
            return View(model);
        }

       
        [HttpGet]
        public IActionResult AllocateSubjects()
        {
            ViewBag.TotalHoursForWeek = TempData["TotalHoursForWeek"] ?? 0;
            ViewBag.TotalSubjects = TempData["TotalSubjects"] ?? 0;

            int totalSubjects = ViewBag.TotalSubjects != null ? (int)ViewBag.TotalSubjects : 0;

       
            var model = new List<SubjectAllocationModel>();
            for (int i = 0; i < totalSubjects; i++)
            {
                model.Add(new SubjectAllocationModel());
            }

            return View(model);
        }


        [HttpPost]
        public IActionResult AllocateSubjects(List<SubjectAllocationModel> allocations)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors and try again.";
                return View(allocations);
            }
            int totalAllocatedHours = allocations.Sum(a => a.Hours);

            if (totalAllocatedHours != TotalHoursForWeek)
            {
                TempData["ErrorMessage"] = $"Total hours must equal {TotalHoursForWeek}. You entered {totalAllocatedHours} hours.";
                return View(allocations);
            }

            SubjectAllocations = allocations;
            return RedirectToAction("GenerateTimeTable");
        }

    
        public IActionResult GenerateTimeTable()
        {
            if (SubjectAllocations.Count == 0)
            {
                TempData["ErrorMessage"] = "No subjects allocated. Please allocate subjects first.";
                return RedirectToAction("AllocateSubjects");
            }

            var timetable = new List<List<string>>();
            var subjects = SubjectAllocations.SelectMany(a => Enumerable.Repeat(a.SubjectName, a.Hours)).ToList();

            int subjectsPerDay = TotalHoursForWeek / SubjectAllocations.Count;
            for (int i = 0; i < TotalHoursForWeek / subjectsPerDay; i++)
            {
                timetable.Add(subjects.Skip(i * subjectsPerDay).Take(subjectsPerDay).ToList());
            }

            return View(new TimeTableModel
            {
                Schedule = timetable,
                WorkingDays = TotalHoursForWeek / subjectsPerDay,
                SubjectsPerDay = subjectsPerDay
            });
        }
    }
}
