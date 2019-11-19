using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoReservation.Dal.Entities;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

namespace AutoReservation.Service.Grpc
{
    internal static class DtoConverter
    {
        #region Auto
        private static Auto GetAutoInstance(AutoDto dto)
        {
            if (dto.AutoKlasse == AutoKlasse.Standard) { return new StandardAuto(); }
            if (dto.AutoKlasse == AutoKlasse.Mittelklasse) { return new MittelklasseAuto(); }
            if (dto.AutoKlasse == AutoKlasse.Luxusklasse) { return new LuxusklasseAuto(); }
            throw new ArgumentException("Unknown AutoDto implementation.", nameof(dto));
        }
        public static Auto ConvertToEntity(this AutoDto dto)
        {
            if (dto == null) { return null; }

            Auto auto = GetAutoInstance(dto);
            auto.Id = dto.Id;
            auto.Marke = dto.Marke;
            auto.Tagestarif = dto.Tagestarif;
            auto.RowVersion = dto.RowVersion.Length == 0
                ? null
                : dto.RowVersion.ToByteArray();

            if (auto is LuxusklasseAuto luxusklasseAuto)
            {
                luxusklasseAuto.Basistarif = dto.Basistarif;
            }
            return auto;
        }
        public static async Task<AutoDto> ConvertToDto(this Task<Auto> entityTask) => (await entityTask).ConvertToDto();
        public static AutoDto ConvertToDto(this Auto entity)
        {
            if (entity == null) { return null; }

            AutoDto dto = new AutoDto
            {
                Id = entity.Id,
                Marke = entity.Marke,
                Tagestarif = entity.Tagestarif,
                RowVersion = ByteString.CopyFrom(entity.RowVersion ?? new byte[0]),
            };

            if (entity is StandardAuto) { dto.AutoKlasse = AutoKlasse.Standard; }
            if (entity is MittelklasseAuto) { dto.AutoKlasse = AutoKlasse.Mittelklasse; }
            if (entity is LuxusklasseAuto auto)
            {
                dto.AutoKlasse = AutoKlasse.Luxusklasse;
                dto.Basistarif = auto.Basistarif;
            }

            return dto;
        }
        public static List<Auto> ConvertToEntities(this IEnumerable<AutoDto> dtos)
        {
            return ConvertGenericList(dtos, ConvertToEntity);
        }
        public static async Task<List<AutoDto>> ConvertToDtos(this Task<List<Auto>> entitiesTask) => (await entitiesTask).ConvertToDtos();
        public static List<AutoDto> ConvertToDtos(this IEnumerable<Auto> entities)
        {
            return ConvertGenericList(entities, ConvertToDto);
        }
        #endregion
        #region Kunde
        public static Kunde ConvertToEntity(this KundeDto dto)
        {
            if (dto == null) { return null; }

            return new Kunde
            {
                Id = dto.Id,
                Nachname = dto.Nachname,
                Vorname = dto.Vorname,
                Geburtsdatum = dto.Geburtsdatum.ToDateTime(),
                RowVersion = dto.RowVersion.Length == 0
                    ? null
                    : dto.RowVersion.ToByteArray()
            };
        }
        public static async Task<KundeDto> ConvertToDto(this Task<Kunde> entityTask) => (await entityTask).ConvertToDto();
        public static KundeDto ConvertToDto(this Kunde entity)
        {
            if (entity == null) { return null; }

            return new KundeDto
            {
                Id = entity.Id,
                Nachname = entity.Nachname,
                Vorname = entity.Vorname,
                Geburtsdatum = entity.Geburtsdatum.ToTimestampUtcFaked(),
                RowVersion = ByteString.CopyFrom(entity.RowVersion ?? new byte[0]),
            };
        }
        public static List<Kunde> ConvertToEntities(this IEnumerable<KundeDto> dtos)
        {
            return ConvertGenericList(dtos, ConvertToEntity);
        }
        public static async Task<List<KundeDto>> ConvertToDtos(this Task<List<Kunde>> entitiesTask) => (await entitiesTask).ConvertToDtos();
        public static List<KundeDto> ConvertToDtos(this IEnumerable<Kunde> entities)
        {
            return ConvertGenericList(entities, ConvertToDto);
        }
        #endregion
        #region Reservation
        public static Reservation ConvertToEntity(this ReservationDto dto)
        {
            if (dto == null) { return null; }

            Reservation reservation = new Reservation
            {
                ReservationsNr = dto.ReservationsNr,
                Von = dto.Von.ToDateTime(),
                Bis = dto.Bis.ToDateTime(),
                AutoId = dto.Auto.Id,
                KundeId = dto.Kunde.Id,
                RowVersion = dto.RowVersion.Length == 0
                    ? null
                    : dto.RowVersion.ToByteArray()
            };

            return reservation;
        }
        public static async Task<ReservationDto> ConvertToDto(this Task<Reservation> entityTask) => (await entityTask).ConvertToDto();
        public static ReservationDto ConvertToDto(this Reservation entity)
        {
            if (entity == null) { return null; }

            return new ReservationDto
            {
                ReservationsNr = entity.ReservationsNr,
                Von = entity.Von.ToTimestampUtcFaked(),
                Bis = entity.Bis.ToTimestampUtcFaked(),
                RowVersion = ByteString.CopyFrom(entity.RowVersion ?? new byte[0]),
                Auto = ConvertToDto(entity.Auto),
                Kunde = ConvertToDto(entity.Kunde)
            };
        }
        public static List<Reservation> ConvertToEntities(this IEnumerable<ReservationDto> dtos)
        {
            return ConvertGenericList(dtos, ConvertToEntity);
        }
        public static async Task<List<ReservationDto>> ConvertToDtos(this Task<List<Reservation>> entitiesTask) => (await entitiesTask).ConvertToDtos();
        public static List<ReservationDto> ConvertToDtos(this IEnumerable<Reservation> entities)
        {
            return ConvertGenericList(entities, ConvertToDto);
        }
        #endregion

        private static List<TTarget> ConvertGenericList<TSource, TTarget>(this IEnumerable<TSource> source, Func<TSource, TTarget> converter)
        {
            if (source == null) { return null; }
            if (converter == null) { return null; }

            return source.Select(converter).ToList();
        }
        /// <summary>
        /// Don't try this at home!
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static Timestamp ToTimestampUtcFaked(this DateTime source)
            => new DateTime(source.Ticks, DateTimeKind.Utc).ToTimestamp();
    }
}
