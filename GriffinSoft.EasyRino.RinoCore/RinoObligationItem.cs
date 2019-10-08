/*
 *  RINO obligation item class
 *  Copyright (C) 2016-2019  Dusan Misic <promisic@outlook.com>
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
    public class RinoObligationItem
    {
        #region Constructor region

        /// <summary>
        ///     Constructor.
        /// </summary>
        public RinoObligationItem()
        {
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="action">Action type</param>
        /// <param name="iznos">Amount</param>
        /// <param name="nazivPoverioca">Obligation holders name</param>
        /// <param name="pibPoverioca">PIB ID</param>
        /// <param name="mbPoverioca">MB ID</param>
        /// <param name="vrstaPoverioca">Obligation type</param>
        /// <param name="nazivDokumenta">Name of the document</param>
        /// <param name="brojDokumenta">Document number</param>
        /// <param name="datumDokumenta">Date of document creation</param>
        /// <param name="datumNastanka">Date of obligation creation</param>
        /// <param name="datumRokaZaIzmirenje">Obligation payment due date</param>
        /// <param name="razlogIzmene">Reason for change</param>
        public RinoObligationItem(
            RinoActionType action,
            decimal iznos,
            string nazivPoverioca,
            string pibPoverioca,
            string mbPoverioca,
            RinoVrstaPoverioca vrstaPoverioca,
            string nazivDokumenta,
            string brojDokumenta,
            DateTime datumDokumenta,
            DateTime datumNastanka,
            DateTime datumRokaZaIzmirenje,
            string razlogIzmene)
        {
            // Passing all values from arguments to internal fields
            Action = action;
            Iznos = iznos;
            NazivPoverioca = nazivPoverioca;
            PibPoverioca = pibPoverioca;
            MbPoverioca = mbPoverioca;
            VrstaPoverioca = vrstaPoverioca;
            NazivDokumenta = nazivDokumenta;
            BrojDokumenta = brojDokumenta;
            DatumDokumenta = datumDokumenta;
            DatumNastanka = datumNastanka;
            DatumRokaZaIzmirenje = datumRokaZaIzmirenje;
            RazlogIzmene = razlogIzmene;
        }

        #endregion

        #region Property holder region

        /// <summary>
        ///     Property holds requested action operation.
        /// </summary>
        public RinoActionType Action { get; set; }

        /// <summary>
        ///     Property holds ammount.
        /// </summary>
        public decimal Iznos { get; set; }

        /// <summary>
        ///     Property holds unique obligator holders name.
        /// </summary>
        public string NazivPoverioca { get; set; }

        /// <summary>
        ///     Property holds PIBPoverioca.
        /// </summary>
        public string PibPoverioca { get; set; }

        /// <summary>
        ///     Property holds MBPoverioca.
        /// </summary>
        public string MbPoverioca { get; set; }

        /// <summary>
        ///     Property holds VrstaPoverioca.
        /// </summary>
        public RinoVrstaPoverioca VrstaPoverioca { get; set; }

        /// <summary>
        ///     Property holds NazivDokumenta.
        /// </summary>
        public string NazivDokumenta { get; set; }

        /// <summary>
        ///     Property holds ID of the document.
        /// </summary>
        public string BrojDokumenta { get; set; }

        /// <summary>
        ///     Property holds date of document creation.
        /// </summary>
        public DateTime DatumDokumenta { get; set; }

        /// <summary>
        ///     Property holds date when the obligation began.
        /// </summary>
        public DateTime DatumNastanka { get; set; }

        /// <summary>
        ///     Property holds date when obligation is due for payment.
        /// </summary>
        public DateTime DatumRokaZaIzmirenje { get; set; }

        /// <summary>
        ///     Property holds reason for change.
        /// </summary>
        public string RazlogIzmene { get; set; }

        #endregion

        #region Validity checks region

        /// <summary>
        ///     Checks if PIB is valid.
        /// </summary>
        /// <returns>True if PIB is 9 characters long, false if otherwise.</returns>
        public bool IsPibValid()
        {
            // Checking if PIB is 9 characters long
            if (PibPoverioca.Length == 9)
                return true;
            return false;
        }

        /// <summary>
        ///     Checks if MB is valid.
        /// </summary>
        /// <returns>True if valid, false if otherwise.</returns>
        public bool IsMbValid()
        {
            if (MbPoverioca.Length == 8 || MbPoverioca.Length == 5 || MbPoverioca.Length == 13) return true;

            return false;
        }

        /// <summary>
        ///     Checks if RazlogIzmene is valid in given context.
        /// </summary>
        /// <returns>True if valid, false if otherwise.</returns>
        public bool ReasonForChangeValid()
        {
            // Result holding variable
            var result = false;

            if (Action == RinoActionType.Izmena || Action == RinoActionType.Otkazivanje)
                if (RazlogIzmene.Length > 3)
                    result = true;

            return result;
        }

        /// <summary>
        ///     Checks if all fields and properties are "filled".
        /// </summary>
        /// <returns>True if valid, false if not.</returns>
        public bool CheckForGeneralValidity()
        {
            // Result holding variable
            var result = Iznos > 0 && NazivPoverioca.Length > 2 &&
                         IsPibValid() && IsMbValid() && NazivDokumenta.Length > 2 &&
                         BrojDokumenta.Length > 2;

            return result;
        }

        #endregion
    }
}