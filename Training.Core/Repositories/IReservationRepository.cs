﻿using System;
using System.Collections.Generic;
using Training.Core.Models;

namespace Training.Core.Repositories
{
    public interface IReservationRepository
    {
        IEnumerable<Reservation> GetByUser(Guid userId);
        Reservation GetById(Guid reservationId);
        void Update(Reservation reservation);
        void Create(Reservation reservation);
    }
}
