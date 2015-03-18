using System;
using System.Drawing;

namespace OctoGWT.Reports
{
    internal class WebDriverStepReport
    {
        private string categoryName;
        private string driverName;
        private string methodName;

        private int offset;

        private Exception exception;
        private Bitmap screenshot;

        public bool HasFailed
        {
            get { return exception != null; }
        }

        public Exception Exception
        {
            get
            {
                return exception;
            }
        }

        public Bitmap Screenshot
        {
            get
            {
                return screenshot;
            }
        }

        public string CategoryName
        {
            get
            {
                return categoryName;
            }
        }

        public string DriverName
        {
            get
            {
                return driverName;
            }
        }

        /// <summary>
        /// Returns the name of the method that triggered a step (for instance, IAmOnPage).
        /// </summary>
        public string MethodName
        {
            get
            {
                return methodName;
            }
        }

        public int Offset
        {
            get
            {
                return offset;
            }
        }

        public WebDriverStepReport(int offset, string categoryName, string methodName, string driverName, Bitmap screenshot, Exception exception)
        {
            this.offset = offset;
            this.categoryName = categoryName;
            this.methodName = methodName;
            this.driverName = driverName;
            this.screenshot = screenshot;
            this.exception = exception;
        }
    }
}
