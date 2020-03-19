namespace Deadlock.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index() => this.View();

        public async Task<ActionResult> AsyncAction()
        {
            await WaitASecond();

            return this.Content("Success");
        }

        public ActionResult SyncAction()
        {
            WaitASecond().Wait();

            return this.Content("Success");
        }

        public static async Task WaitASecond()
        {
            await Task.Delay(1000); // Use .ConfigureAwait(false) to prevent deadlock.
        }

        // The SyncAction method calls WaitASecond (within the UI/ASP.NET context).
        // WaitASecond starts delaying by calling Task.Delay (still within the context).
        // Task.Delay returns an uncompleted Task, indicating the delay is not complete.
        // WaitASecond awaits the Task returned by the delay. The context is captured and will be used to continue running the WaitASecond method later. WaitASecond returns an uncompleted Task, indicating that the WaitASecond method is not complete.
        // The top-level method synchronously blocks on the Task returned by WaitASecond. This blocks the context thread.
        // Eventually, the delay will complete. This completes the Task that was returned by Task.Delay.
        // The continuation for WaitASecond is now ready to run, and it waits for the context to be available so it can execute in the context.
        // Deadlock. The top-level method is blocking the context thread, waiting for WaitASecond to complete, and WaitASecond is waiting for the context to be free so it can complete.
    }
}