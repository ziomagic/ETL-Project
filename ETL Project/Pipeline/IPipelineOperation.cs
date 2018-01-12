using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_Project.Pipeline
{
    /// <summary>
    /// Interfejs operacji na danych
    /// </summary>
    interface IPipelineOperation
    {
        object HandleData(object input);
    }
}
