using Microsoft.AspNetCore.Mvc;
using Someren_Case.Models;
using Someren_Case.Repositories;
namespace Someren_Case.Controllers
{
    public class ActivityController : Controller
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IActivityParticipantRepository _activityParticipantRepository;

        public ActivityController(IActivityRepository activityRepository, IStudentRepository studentRepository, IActivityParticipantRepository activityParticipantRepository)
        {
            _activityRepository = activityRepository;
            _studentRepository = studentRepository;
            _activityParticipantRepository = activityParticipantRepository;
        }

        // Get all activities
        public IActionResult Index()
        {
            var activities = _activityRepository.GetAll();
            return View(activities);
        }

        // View details of a specific activity
        public IActionResult Details(int id)
        {
            var activity = _activityRepository.GetById(id);
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }

        // Show the form to create a new activity
        public IActionResult Create()
        {
            return View();
        }

        // Handle the creation of an activity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Activity activity)
        {
            if (ModelState.IsValid)
            {
                _activityRepository.Add(activity);
                return RedirectToAction(nameof(Index));
            }
            return View(activity);
        }

        // Show the form to edit an activity
        public IActionResult Edit(int id)
        {
            var activity = _activityRepository.GetById(id);
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }

        // Handle the updating of an activity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Activity activity)
        {
            if (id != activity.ActivityID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _activityRepository.Update(activity);
                return RedirectToAction(nameof(Index));
            }
            return View(activity);
        }

        // Show the form to confirm deletion of an activity
        public IActionResult Delete(int id)
        {
            var activity = _activityRepository.GetById(id);
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);  // Pass the activity to the view
        }

        // POST: Activity/DeleteConfirmed/{id}
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int activityId)
        {
            try
            {
                var activity = _activityRepository.GetById(activityId);
                if (activity != null)
                {
                    _activityRepository.Delete(activity);  // Delete the activity
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");  // In case of an error, redirect to Index
            }
        }

        // Manage activity participants
        public IActionResult ManageParticipants(int activityId)
        {
            var activity = _activityRepository.GetById(activityId);
            if (activity == null)
            {
                return NotFound();
            }

            var participants = _activityParticipantRepository.GetParticipantsByActivityId(activityId);
            var nonParticipants = _activityParticipantRepository.GetNonParticipantsByActivityId(activityId);

            ViewBag.Activity = activity;
            ViewBag.Participants = participants;
            ViewBag.NonParticipants = nonParticipants;

            return View();
        }

        // Add participant to an activity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddParticipant(int activityId, int studentId)
        {
            try
            {
                // Make sure to add participant using your repository method
                _activityParticipantRepository.AddParticipant(activityId, studentId);
                return RedirectToAction("ManageParticipants", new { activityId });
            }
            catch (Exception ex)
            {
                // Log error and inform user
                ViewBag.ErrorMessage = "There was an error adding the participant.";
                return View("Error");
            }
        }

        // Remove participant from an activity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveParticipant(int activityId, int studentId)
        {
            try
            {
                // Remove participant using the repository method
                _activityParticipantRepository.RemoveParticipant(activityId, studentId);
                return RedirectToAction("ManageParticipants", new { activityId });
            }
            catch (Exception ex)
            {
                // Log error and inform user
                ViewBag.ErrorMessage = "There was an error removing the participant.";
                return View("Error");
            }
        }
        }
    }

