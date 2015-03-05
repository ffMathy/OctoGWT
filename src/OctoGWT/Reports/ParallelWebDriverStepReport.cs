using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace OctoGWT.Reports
{
    internal class WebDriverStepReport
    {
        private string clauseName;
        private string driverName;
        private string methodName;

        private int offset;

        private Exception exception;
        private Screenshot screenshot;

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

        public Screenshot Screenshot
        {
            get
            {
                return screenshot;
            }
        }

        public string ClauseName
        {
            get
            {
                return clauseName;
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

        public WebDriverStepReport(int offset, string clauseName, string methodName, string driverName, Screenshot screenshot, Exception exception)
        {
            this.offset = offset;
            this.clauseName = clauseName;
            this.methodName = methodName;
            this.driverName = driverName;
            this.screenshot = screenshot;
            this.exception = exception;
        }
    }
}
