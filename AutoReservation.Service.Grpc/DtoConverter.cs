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
        #region Car
        private static Car GetAutoInstance(CarDto dto)
        {
            if (dto.CarClass == CarClass.Standard) { return new StandardCar(); }
            if (dto.CarClass == CarClass.Midclass) { return new MidClassCar(); }
            if (dto.CarClass == CarClass.Luxuryclass) { return new LuxuryClassCar(); }
            throw new ArgumentException("Unknown AutoDto implementation.", nameof(dto));
        }
        public static Car ConvertToEntity(this CarDto dto)
        {
            if (dto == null) { return null; }

            Car car = GetAutoInstance(dto);
            car.Id = dto.Id;
            car.Make = dto.Make;
            car.DailyRate = dto.DailyRate;
            car.RowVersion = dto.RowVersion.Length == 0
                ? null
                : dto.RowVersion.ToByteArray();

            if (car is LuxuryClassCar luxuryclassCar)
            {
                luxuryclassCar.BaseRate = dto.BaseRate;
            }
            return car;
        }
        public static async Task<CarDto> ConvertToDto(this Task<Car> entityTask) => (await entityTask).ConvertToDto();
        public static CarDto ConvertToDto(this Car entity)
        {
            if (entity == null) { return null; }

            CarDto dto = new CarDto
            {
                Id = entity.Id,
                Make = entity.Make,
                DailyRate = entity.DailyRate,
                RowVersion = ByteString.CopyFrom(entity.RowVersion ?? new byte[0]),
            };

            if (entity is StandardCar) { dto.CarClass = CarClass.Standard; }
            if (entity is MidClassCar) { dto.CarClass = CarClass.Midclass; }
            if (entity is LuxuryClassCar car)
            {
                dto.CarClass = CarClass.Luxuryclass;
                dto.BaseRate = car.BaseRate;
            }

            return dto;
        }
        public static List<Car> ConvertToEntities(this IEnumerable<CarDto> dtos)
        {
            return ConvertGenericList(dtos, ConvertToEntity);
        }
        public static async Task<List<CarDto>> ConvertToDtos(this Task<List<Car>> entitiesTask) => (await entitiesTask).ConvertToDtos();
        public static List<CarDto> ConvertToDtos(this IEnumerable<Car> entities)
        {
            return ConvertGenericList(entities, ConvertToDto);
        }
        #endregion
        #region Customer
        public static Customer ConvertToEntity(this CustomerDto dto)
        {
            if (dto == null) { return null; }

            return new Customer
            {
                Id = dto.Id,
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                BirthDate = dto.BirthDate.ToDateTime(),
                RowVersion = dto.RowVersion.Length == 0
                    ? null
                    : dto.RowVersion.ToByteArray()
            };
        }
        public static async Task<CustomerDto> ConvertToDto(this Task<Customer> entityTask) => (await entityTask).ConvertToDto();
        public static CustomerDto ConvertToDto(this Customer entity)
        {
            if (entity == null) { return null; }

            return new CustomerDto
            {
                Id = entity.Id,
                LastName = entity.LastName,
                FirstName = entity.FirstName,
                BirthDate = entity.BirthDate.ToTimestampUtcFaked(),
                RowVersion = ByteString.CopyFrom(entity.RowVersion ?? new byte[0]),
            };
        }
        public static List<Customer> ConvertToEntities(this IEnumerable<CustomerDto> dtos)
        {
            return ConvertGenericList(dtos, ConvertToEntity);
        }
        public static async Task<List<CustomerDto>> ConvertToDtos(this Task<List<Customer>> entitiesTask) => (await entitiesTask).ConvertToDtos();
        public static List<CustomerDto> ConvertToDtos(this IEnumerable<Customer> entities)
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
                ReservationNr = dto.ReservationNr,
                From = dto.From.ToDateTime(),
                To = dto.To.ToDateTime(),
                CarId = dto.Car.Id,
                CustomerId = dto.Customer.Id,
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
                ReservationNr = entity.ReservationNr,
                From = entity.From.ToTimestampUtcFaked(),
                To = entity.To.ToTimestampUtcFaked(),
                RowVersion = ByteString.CopyFrom(entity.RowVersion ?? new byte[0]),
                Car = ConvertToDto(entity.Car),
                Customer= ConvertToDto(entity.Customer)
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
