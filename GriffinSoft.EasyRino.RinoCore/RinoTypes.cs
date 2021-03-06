﻿/*
 *  RINO specific type definition class
 *  Copyright (C) 2016 - 2020 Dusan Misic <promisic@gmail.com>
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

namespace GriffinSoft.EasyRino.RinoCore
{ 
    /// <summary>
    /// Obligation type enumeration.
    /// </summary>
    public enum RinoVrstaPoverioca
    {
        /// <summary>
        /// Normal business.
        /// </summary>
        PravnaLica = 0,

        /// <summary>
        /// Public sector.
        /// </summary>
        JavniSektor = 1,

        /// <summary>
        /// Agriculture and farming sector.
        /// </summary>
        PoljoprivrednaGazdinstva = 8,

        /// <summary>
        /// Compensation.
        /// </summary>
        Kompenzacija = 9
    }
}