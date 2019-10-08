/*
 *  RINO reconcilement item class
 *  Copyright (C) 2016 - 2019  Dusan Misic <promisic@outlook.com>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;

namespace GriffinSoft.EasyRino.RinoCore
{
    public class RinoReconcilementItem
    {
        #region Constructor region

        /// <summary>
        /// Constructor.
        /// </summary>
        public RinoReconcilementItem()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="action">Action type</param>
        /// <param name="rinoId">Unuque RINO portal ID</param>
        /// <param name="brojDokumenta">Document number</param>
        /// <param name="pibPoverioca">PIB ID</param>
        /// <param name="banka">Bank info</param>
        /// <param name="reklPodZaRek">Reclamation ID</param>
        /// <param name="datumIzmirenja">Date of payment</param>
        /// <param name="iznos">Amount</param>
        /// <param name="razlogIzmene">Reason for change</param>
        public RinoReconcilementItem(
            RinoActionType action,
            long rinoId,
            string brojDokumenta,
            string pibPoverioca,
            string banka,
            string reklPodZaRek,
            DateTime datumIzmirenja,
            decimal iznos,
            string razlogIzmene)
        {
            Action = action;
            RinoId = rinoId;
            BrojDokumenta = brojDokumenta;
            PibPoverioca = pibPoverioca;
            Banka = banka;
            ReklPodZaRek = reklPodZaRek;
            DatumIzmirenja = datumIzmirenja;
            Iznos = iznos;
            RazlogIzmene = razlogIzmene;
        }

        #endregion

        #region Property holder region

        /// <summary>
        /// Property holds requested action operation.
        /// </summary>
        public RinoActionType Action { get; set; }

        /// <summary>
        /// Property holds unique RINO ID.
        /// </summary>
        public long RinoId { get; set; }

        /// <summary>
        /// Property holds ID of the document.
        /// </summary>
        public string BrojDokumenta { get; set; }

        /// <summary>
        /// Property holds PIBPoverioca.
        /// </summary>
        public string PibPoverioca { get; set; }

        /// <summary>
        /// Property holds bank information.
        /// </summary>
        public string Banka { get; set; }

        /// <summary>
        /// Property holds ReklPodZaRek.
        /// </summary>
        public string ReklPodZaRek { get; set; }

        /// <summary>
        /// Property holds date of payment.
        /// </summary>
        public DateTime DatumIzmirenja { get; set; }

        /// <summary>
        /// Property holds ammount.
        /// </summary>
        public decimal Iznos { get; set; }

        /// <summary>
        /// Property holds reason for change.
        /// </summary>
        public string RazlogIzmene { get; set; }

        #endregion

        #region Validity checks region

        /// <summary>
        /// Checks if PIB is valid.
        /// </summary>
        /// <returns>True if PIB is 9 characters long, false if otherwise.</returns>
        public bool IsPibValid()
        {
            // Checking if PIB is 9 characters long
            if (this.PibPoverioca.Length == 9)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if ReklPodZaRek is valid.
        /// </summary>
        /// <returns>True if ReklPodZaRek is 16 character long, false if otherwise.</returns>
        public bool IsReklPodZaRekValid()
        {
            // Checking if ReklPodZaRek is 16 character long
            if (this.ReklPodZaRek.Length == 16)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if RazlogIzmene is valid in given context.
        /// </summary>
        /// <returns>True if valid, false if otherwise.</returns>
        public bool ReasonForChangeValid()
        {
            if (Action == RinoActionType.Izmena || Action == RinoActionType.Otkazivanje)
            {
                if (RazlogIzmene.Length > 3)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if all fields and properties are "filled".
        /// </summary>
        /// <returns>True if valid, false if not.</returns>
        public bool CheckForGeneralValidity()
        {
            if (RinoId > 0 && BrojDokumenta.Length > 2 && IsPibValid() &&
                Banka.Length > 2 && IsReklPodZaRekValid() && Iznos > 0)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
