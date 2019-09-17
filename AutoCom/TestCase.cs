using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCom
{ 

    class TestCase
    {
        private string testCommand = string.Empty;
        public TestCase()
        {
            this.testCommand = string.Empty;
        }

        public TestCase(string testCommand)
        {
            this.testCommand = testCommand;
        }

        public string GetTestCommand()
        {
            return this.testCommand;
        }

        public void SetTestCommand(string testCommand)
        {
            this.testCommand = testCommand;
        }
    }
}
