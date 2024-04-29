using Civica.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.ViewModels
{
    public static class Helper
    {
        public static Dictionary<Phase, string> Phases { get; set; } = new Dictionary<Phase, string>
        {
            { Phase.IDENTIFIED, "Identificeret" },
            { Phase.PLANNING, "Planlægning" },
            { Phase.IMPLEMENTATION, "Gennemførsel" },
            { Phase.OPERATION, "Drift" },
            { Phase.FOLLOW_UP, "Opfølgning" },
            { Phase.DONE, "Afsluttet" }
        };

        public static Dictionary<Status, string> Statuses { get; set; } = new Dictionary<Status, string>
        {
            { Status.NONE, "Ingen vurdering" },
            { Status.CRITICAL, "Kritisk" },
            { Status.DELAYED, "Forsinket" },
            { Status.ON_TRACK, "Planmæssigt" }
        };
    }
}
