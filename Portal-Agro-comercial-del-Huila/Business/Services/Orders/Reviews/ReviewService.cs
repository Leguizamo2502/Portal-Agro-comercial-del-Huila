using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.Implements.Orders.Reviews;
using Business.Repository;
using Data.Interfaces.Implements.Orders.Reviews;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Orders;
using Entity.DTOs.Order.Reviews;
using MapsterMapper;
using Utilities.Exceptions;

namespace Business.Services.Orders.Reviews
{
    public class ReviewService : BusinessGeneric<ReviewCreateDto, ReviewSelectDto, Review>, IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        public ReviewService(IDataGeneric<Review> data, IMapper mapper, IReviewRepository reviewRepository) : base(data, mapper)
        {
            _reviewRepository = reviewRepository;
        }


        public override async Task<IEnumerable<ReviewSelectDto>> GetAllAsync()
        {
            try
            {
                var entities = await _reviewRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<ReviewSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros de reviews.", ex);
            }
        }
    }
}
