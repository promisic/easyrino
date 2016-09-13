/*
  RINO Razduzene item class
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GriffinSoft.EasyRino.RinoCore
{
    public class RinoRazduzenjeItem
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        internal RinoRazduzenjeItem()
        {

        }

        /// <summary>
        /// Action type enumeration.
        /// </summary>
        internal enum ActionType
        {
            Unos,
            Izmena,
            Obrisi
        }

        /// <summary>
        /// Property holds requested action operation.
        /// </summary>
        internal ActionType Action { get; set; }

        /// <summary>
        /// Property holds unique RINO ID.
        /// </summary>
        internal long RinoId { get; set; }

        /// <summary>
        /// Property holds ID of the document.
        /// </summary>
        internal string BrojDokumenta { get; set; }

        /// <summary>
        /// Property holds PIBPoverioca.
        /// </summary>
        internal string PIBPoverioca { get; set; }

        /// <summary>
        /// Property holds bank information.
        /// </summary>
        internal string Banka { get; set; }

        /// <summary>
        /// Property holds ReklPodZaRek.
        /// </summary>
        internal string ReklPodZaRek { get; set; }

        /// <summary>
        /// Property holds date of payment.
        /// </summary>
        internal DateTime DatumIzmirenja { get; set; }

        /// <summary>
        /// Property holds ammount.
        /// </summary>
        internal decimal Iznos { get; set; }

        /// <summary>
        /// Property holds reason for change.
        /// </summary>
        internal string RazlogIzmene { get; set; }

        /// <summary>
        /// Checks if PIB is valid.
        /// </summary>
        /// <returns>True if PIB is 9 characters long, false if otherwise.</returns>
        internal bool IsPibValid()
        {
            // Checking if PIB is 9 characters long
            if (this.PIBPoverioca.Length == 9)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
