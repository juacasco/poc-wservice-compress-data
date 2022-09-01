using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WService_ConceptTest_Host
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IExecQuery" in both code and config file together.
    [ServiceContract]
    public interface IExecQuery
    {
        [OperationContract]
        byte[] answerTest(string querySelectCmd, out long notZipLength, out long zipLength, out List<DateTime> timeCaptures, out List<String> strTimeCaptures);
    }
}
