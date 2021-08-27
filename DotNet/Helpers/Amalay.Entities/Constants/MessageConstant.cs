using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Entities
{
    public partial class Constant
    {
        public const string TestMessage = "Test Message!";

        public struct Message
        {
            public const string SaveSuccess = "Data saved successfully!";
            public const string SaveError = "Error occurred while saving the data!";

            public const string UpdateSuccess = "Data updated successfully!";
            public const string UpdateError = "Error occurred while updating the data!";

            public const string SendSuccess = "Message send successfully!";
            public const string SendError = "Error occurred while sending message!";
        }
    }
}
