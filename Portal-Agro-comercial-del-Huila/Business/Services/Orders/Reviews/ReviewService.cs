using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.Implements.Orders.Reviews;
using Business.Repository;
using Data.Interfaces.Implements.Auth;
using Data.Interfaces.Implements.Orders.Reviews;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Orders;
using Entity.DTOs.Order.Reviews;
using MapsterMapper;
using Utilities.Exceptions;

namespace Business.Services.Orders.Reviews
{
    public class ReviewService : BusinessGeneric<ReviewCreateDto, ReviewSelectDto, Review>, IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;
        public ReviewService(IDataGeneric<Review> data, IMapper mapper, IReviewRepository reviewRepository, IUserRepository userRepository) : base(data, mapper)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
        }

        public async Task<ReviewSelectDto> CreateReviewAsync(ReviewCreateDto dto, int userId)
        {
            try
            {
                var entity = new Review
                {
                    ProductId = dto.ProductId,
                    UserId = userId,
                    Rating = dto.Rating,
                    Comment = dto.Comment,
                    IsDeleted = false,
                    Active = true,
                    CreateAt = DateTime.Now
                };

                var created = await _reviewRepository.AddAsync(entity);

                var result = await _reviewRepository.GetByIdAsync(created.Id);
                return _mapper.Map<ReviewSelectDto>(result!);
            }
            catch (Exception ex)
            {
                // Aquí podrías lanzar una excepción custom si ya usas tu capa de Utilities
                throw new BusinessException("Error al crear la reseña", ex);
            }
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

        public async Task<IEnumerable<ReviewSelectDto>> GetAllByProductId(int productId)
        {
            try
            {
                var entities = await _reviewRepository.GetAllByProductId(productId);
                return _mapper.Map<IEnumerable<ReviewSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros de reviews.", ex);
            }
        }
    }
}
