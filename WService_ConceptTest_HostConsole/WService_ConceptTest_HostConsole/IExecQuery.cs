using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace WService_ConceptTest_HostConsole
{
    [ServiceContract]
    public interface IExecQuery
    {
        [OperationContract]
        byte[] answerTest(string querySelectCmd, out long notZipLength, out long zipLength, out List<DateTime> timeCaptures, out List<String> strTimeCaptures);

        [OperationContract]
        DateTime GetServerDate(double addHours);

        [OperationContract]
        string GetData(int value);

    }
}
